param(
    [string]$Configuration = "Debug"
)

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

foreach ($proj in $projects) {
    Write-Host "Processing $proj..."

    $xmlPath = Get-ChildItem -Recurse -Filter "$proj.xml" | Select-Object -First 1

    if (-not $xmlPath) {
        Write-Warning "No XML doc found for $proj"
        continue
    }

    $output = "docs/PhotoArchive/generated/$proj"
    New-Item -ItemType Directory -Force -Path $output | Out-Null

    Write-Host "Generating docs for $proj"
    xml2doc --input "$($xmlPath.FullName)" --output "$output"
}

Write-Host "Documentation generation complete."