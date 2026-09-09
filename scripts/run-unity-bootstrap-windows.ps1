param(
    [string]$UnityEditorPath = "C:\Program Files\Unity\Hub\Editor\6000.3.0f1\Editor\Unity.exe"
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

$repoRoot = Split-Path -Parent $PSScriptRoot
$projectPath = Join-Path $repoRoot "unity\SecretGame"
$resultRoot = Join-Path $repoRoot "TestResults\UnityBootstrap"
$resultPath = Join-Path $resultRoot "editmode-results.xml"
$logPath = Join-Path $resultRoot "unity-editor.log"

if (-not (Test-Path -LiteralPath $UnityEditorPath)) {
    throw @"
Unity 6000.3.0f1 was not found at:
$UnityEditorPath

Install Unity 6.3 LTS 6000.3.0f1 through Unity Hub, or rerun with:
-UnityEditorPath "C:\full\path\to\Unity.exe"
"@
}

& (Join-Path $PSScriptRoot "sync-unity-kernel.ps1")

New-Item -ItemType Directory -Force -Path $resultRoot | Out-Null
Write-Host "Running Unity 6.3 LTS bootstrap Edit Mode tests..."
& $UnityEditorPath `
    -batchmode `
    -nographics `
    -projectPath $projectPath `
    -runTests `
    -testPlatform editmode `
    -testResults $resultPath `
    -logFile $logPath

$unityExitCode = $LASTEXITCODE
if ($unityExitCode -ne 0) {
    Write-Error "Unity bootstrap failed with exit code $unityExitCode. Log: $logPath"
    exit $unityExitCode
}

if (-not (Test-Path -LiteralPath $resultPath)) {
    throw "Unity exited successfully but did not write test results to $resultPath"
}

[xml]$results = Get-Content -LiteralPath $resultPath
$run = $results.'test-run'
Write-Host "Unity bootstrap: total=$($run.total) passed=$($run.passed) failed=$($run.failed) result=$($run.result)"
Write-Host "Results: $resultPath"
Write-Host "Log: $logPath"

if ([int]$run.failed -gt 0) { exit 1 }
exit 0
