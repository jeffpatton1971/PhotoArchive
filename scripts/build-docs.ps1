param(
    [string]$Configuration = "Debug"
)

$ErrorActionPreference = "Stop"
$PSNativeCommandUseErrorActionPreference = $true

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

    $xmlPath = Get-ChildItem -Recurse -Filter "$proj.xml" | Where-Object {
        $_.FullName -like "*bin*$Configuration*"
    } | Select-Object -First 1

    if (-not $xmlPath) {
        Write-Warning "No XML doc found for $proj in configuration '$Configuration'"
        continue
    }

    $output = Join-Path $generatedRoot "$proj.md"

    Write-Host "Generating docs for $proj -> $output"

    dotnet tool run xml2doc -- `
        --xml "$($xmlPath.FullName)" `
        --out "$output" `
        --single `
        --file-names clean
}

Write-Host "Documentation generation complete."