# Release Notes

## Project upgraded to .NET 10

The project has been upgraded from **.NET 8** to **.NET 10**.

### Details

- **Target framework:** `net8.0` → `net10.0`
- **Prerequisites:** .NET 10 SDK is required to build and run the project. See [Prerequisites](README.md#prerequisites) in the README.

### Dependency updates (aligned with .NET 10)

| Package | Previous | Updated |
|---------|----------|---------|
| Microsoft.NET.Test.Sdk | 18.0.1 | 18.3.0 |
| MSTest.TestAdapter | 4.0.2 | 4.1.0 |
| RestSharp | 113.0.0 | 113.1.0 |
| HtmlAgilityPack | 1.12.4 | 1.12.4 (unchanged) |
| STAF.UI.API | 4.3.3 | 4.3.3 (unchanged) |

Build and test execution have been verified on .NET 10.
