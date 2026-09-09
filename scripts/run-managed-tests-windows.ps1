$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

$repoRoot = Split-Path -Parent $PSScriptRoot
$projects = @(
    "tests\SecretGame.Simulation.Tests\SecretGame.Simulation.Tests.csproj",
    "tests\SecretGame.Tactics.Tests\SecretGame.Tactics.Tests.csproj",
    "tools\SecretGame.SpecialtyPressureExperiment\SecretGame.SpecialtyPressureExperiment.csproj"
)

foreach ($relativeProject in $projects) {
    $project = Join-Path $repoRoot $relativeProject
    Write-Host "Building $relativeProject without a native apphost..."
    dotnet build $project --nologo -p:UseAppHost=false
    if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

    $projectDirectory = Split-Path -Parent $project
    $assemblyName = [System.IO.Path]::GetFileNameWithoutExtension($project)
    $assembly = Join-Path $projectDirectory "bin\Debug\net8.0\$assemblyName.dll"
    Write-Host "Running $assemblyName through the signed dotnet host..."
    dotnet $assembly
    if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
}
