param(
    [string]$Configuration = "Debug"
)

$ErrorActionPreference = "Stop"

Write-Host "Building solution..."
dotnet clean
dotnet restore
dotnet build -c $Configuration

Write-Host "Restoring tools..."
dotnet tool restore

$projects = @(
    "PhotoArchive.Api",
    "PhotoArchive.Core",
    "PhotoArchive.Data",
    "PhotoArchive.Tools"
)

$generatedRoot = "docs/PhotoArchive/generated"
New-Item -ItemType Directory -Force -Path $generatedRoot | Out-Null

foreach ($proj in $projects) {
    Write-Host "Processing $proj..."

    $xmlPath = Get-ChildItem -Recurse -Filter "$proj.xml" | Select-Object -First 1

    if (-not $xmlPath) {
        Write-Warning "No XML doc found for $proj"
        continue
    }

    $output = Join-Path $generatedRoot "$proj.md"

    Write-Host "Generating docs for $proj -> $output"
    Xml2Doc.Cli `
        --xml "$($xmlPath.FullName)" `
        --out "$output" `
        --single `
        --file-names clean
}

Write-Host "Documentation generation complete."
