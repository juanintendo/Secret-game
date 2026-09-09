$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

$repoRoot = Split-Path -Parent $PSScriptRoot
$sourceRoot = Join-Path $repoRoot "src\SecretGame.Simulation"
$unityProject = Join-Path $repoRoot "unity\SecretGame"
$generatedRoot = Join-Path $unityProject "Assets\Generated\Kernel\SecretGame.Simulation"

if (-not (Test-Path (Join-Path $sourceRoot "SecretGame.Simulation.csproj"))) {
    throw "Simulation source root was not found at $sourceRoot"
}
if (-not (Test-Path (Join-Path $unityProject "ProjectSettings\ProjectVersion.txt"))) {
    throw "Unity project root was not found at $unityProject"
}

$generatedParent = Split-Path -Parent $generatedRoot
if (Test-Path $generatedRoot) {
    $resolved = (Resolve-Path $generatedRoot).Path
    $expectedSuffix = "Assets\Generated\Kernel\SecretGame.Simulation"
    if (-not $resolved.EndsWith($expectedSuffix, [System.StringComparison]::OrdinalIgnoreCase)) {
        throw "Refusing to clear unexpected generated path: $resolved"
    }
    Remove-Item -Recurse -Force $resolved
}

New-Item -ItemType Directory -Force -Path $generatedRoot | Out-Null
$sources = Get-ChildItem -Path $sourceRoot -Filter "*.cs" -File | Sort-Object Name
if ($sources.Count -eq 0) { throw "No simulation C# sources were found." }

foreach ($source in $sources) {
    $destination = Join-Path $generatedRoot $source.Name
    Copy-Item -LiteralPath $source.FullName -Destination $destination
    $sourceHash = (Get-FileHash -Algorithm SHA256 -LiteralPath $source.FullName).Hash
    $destinationHash = (Get-FileHash -Algorithm SHA256 -LiteralPath $destination).Hash
    if ($sourceHash -ne $destinationHash) {
        throw "Unity kernel sync hash mismatch for $($source.Name)"
    }
}

$assemblyDefinition = @'
{
  "name": "SecretGame.Simulation",
  "rootNamespace": "SecretGame.Simulation",
  "references": [],
  "includePlatforms": [],
  "excludePlatforms": [],
  "allowUnsafeCode": false,
  "overrideReferences": false,
  "precompiledReferences": [],
  "autoReferenced": false,
  "defineConstraints": [],
  "versionDefines": [],
  "noEngineReferences": true
}
'@
$assemblyDefinition | Set-Content -Encoding UTF8 (Join-Path $generatedRoot "SecretGame.Simulation.asmdef")

$manifest = foreach ($source in $sources) {
    $hash = (Get-FileHash -Algorithm SHA256 -LiteralPath $source.FullName).Hash
    "$hash  $($source.Name)"
}
$manifest | Set-Content -Encoding UTF8 (Join-Path $generatedRoot "unity-kernel-sync-manifest.txt")

Write-Host "Unity kernel sync complete: $($sources.Count) source files, all SHA-256 matched."
Write-Host "Generated destination: $generatedRoot"
