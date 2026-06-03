<#
.SYNOPSIS
    Validate or discover symbols for docs/ai/ai-index.json (STAF.Selenium.Tests).

.DESCRIPTION
    -Default: validates that each symbol's file exists and declares the class.
    -Discover: prints public/internal classes found under STAFTests/Pages, Actions, Tests, Requests
               (quick aid when updating the index after adding types).

.EXAMPLE
    pwsh tools/UpdateAiIndex.ps1
    pwsh tools/UpdateAiIndex.ps1 -Discover
#>
param(
    [switch]$Discover
)

$ErrorActionPreference = 'Stop'
$repoRoot = Resolve-Path (Join-Path $PSScriptRoot '..')
$indexPath = Join-Path $repoRoot 'docs/ai/ai-index.json'

if (-not (Test-Path $indexPath)) {
    Write-Error "Missing $indexPath"
}

if ($Discover) {
    $dirs = @(
        (Join-Path $repoRoot 'STAFTests/Pages'),
        (Join-Path $repoRoot 'STAFTests/Actions'),
        (Join-Path $repoRoot 'STAFTests/Tests'),
        (Join-Path $repoRoot 'STAFTests/Requests'),
        (Join-Path $repoRoot 'STAFTests/APIData')
    )
    $rx = '(?m)^\s*(?:public\s+|internal\s+)?class\s+(\w+)(?:\s*:\s*(\w+))?'
    $rows = New-Object System.Collections.Generic.List[object]
    foreach ($dir in $dirs) {
        if (-not (Test-Path $dir)) { continue }
        Get-ChildItem -Path $dir -Filter '*.cs' -File | ForEach-Object {
            $text = Get-Content $_.FullName -Raw
            foreach ($m in [regex]::Matches($text, $rx)) {
                $rel = $_.FullName.Substring($repoRoot.Path.Length).TrimStart('\', '/')
                $rows.Add([PSCustomObject]@{
                    File  = $rel
                    Class = $m.Groups[1].Value
                    Base  = $m.Groups[2].Value
                })
            }
        }
    }
    $rows | Format-Table -AutoSize
    exit 0
}

# Validate index
$jsonText = Get-Content $indexPath -Raw -Encoding UTF8
$index = $jsonText | ConvertFrom-Json
$failures = New-Object System.Collections.Generic.List[string]

foreach ($prop in $index.symbols.PSObject.Properties) {
    $name = $prop.Name
    $sym = $prop.Value
    $rel = $sym.file
    if (-not $rel) {
        $failures.Add("$name : missing 'file'")
        continue
    }
    $full = Join-Path $repoRoot $rel
    if (-not (Test-Path $full)) {
        $failures.Add("$name : file not found: $rel")
        continue
    }
    $content = Get-Content $full -Raw -Encoding UTF8
    $classPattern = "(?m)^\s*public\s+class\s+$([regex]::Escape($name))\b"
    $altPattern = "(?m)^\s*class\s+$([regex]::Escape($name))\b"
    if ($content -notmatch $classPattern -and $content -notmatch $altPattern) {
        $failures.Add("$name : class '$name' not found in $rel")
    }
}

if ($failures.Count -gt 0) {
    Write-Host "Validation failed:" -ForegroundColor Red
    $failures | ForEach-Object { Write-Host "  $_" }
    exit 1
}

$n = @($index.symbols.PSObject.Properties).Count
Write-Host "docs/ai/ai-index.json: all $n symbols validated OK." -ForegroundColor Green
exit 0
