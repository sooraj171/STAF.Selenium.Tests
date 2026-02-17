# Rebuild mcp-sharp-staf-selenium and copy to MCPAgent/publish
# Requires mcp-sharp-staf-selenium repo at sibling path: ..\mcp-sharp-staf-selenium

$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$projectRoot = Split-Path -Parent $scriptDir
$mcpSource = Join-Path (Split-Path -Parent $projectRoot) "mcp-sharp-staf-selenium"
$csproj = Join-Path $mcpSource "mcp-sharp-staf-selenium\mcp-sharp-staf-selenium.csproj"
$publishTarget = Join-Path $scriptDir "publish"

if (-not (Test-Path $csproj)) {
    Write-Host "mcp-sharp-staf-selenium not found at: $mcpSource" -ForegroundColor Yellow
    Write-Host "Clone it with: git clone https://github.com/sooraj171/mcp-sharp-staf-selenium" -ForegroundColor Yellow
    Write-Host "Or run publish manually:" -ForegroundColor Yellow
    Write-Host "  dotnet publish <path-to>\mcp-sharp-staf-selenium.csproj -c Release -r win-x64 --self-contained true -o $publishTarget" -ForegroundColor Cyan
    exit 1
}

Write-Host "Building mcp-sharp-staf-selenium..." -ForegroundColor Green
dotnet publish $csproj -c Release -r win-x64 --self-contained true -o $publishTarget

if ($LASTEXITCODE -eq 0) {
    Write-Host "MCP Agent built successfully at: $publishTarget" -ForegroundColor Green
} else {
    Write-Host "Build failed." -ForegroundColor Red
    exit 1
}
