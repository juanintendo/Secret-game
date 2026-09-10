param(
    [ValidateSet("All", "Validate", "Capture", "BuildPlayer", "Profile")]
    [string]$Phase = "All",
    [string]$UnityEditorPath = "C:\Program Files\Unity\Hub\Editor\6000.3.23f1\Editor\Unity.exe"
)
$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest
$repoRoot = Split-Path -Parent $PSScriptRoot
$projectPath = Join-Path $repoRoot "unity\SecretGame"
$resultRoot = Join-Path $repoRoot "TestResults\VisualSpike001"
New-Item -ItemType Directory -Force -Path $resultRoot | Out-Null
if (-not (Test-Path -LiteralPath $UnityEditorPath)) { throw "Unity 6000.3.23f1 is required." }

function Invoke-VisualEditor([string]$Method, [string]$Name) {
    $log = Join-Path $resultRoot ($Name + ".log")
    $arguments = @("-batchmode", "-projectPath", ('"' + $projectPath + '"'),
        "-executeMethod", $Method, "-quit", "-logFile", ('"' + $log + '"'))
    $process = Start-Process -FilePath $UnityEditorPath -ArgumentList $arguments -WindowStyle Hidden -PassThru -Wait
    if ($process.ExitCode -ne 0) { throw "Unity $Name failed ($($process.ExitCode)): $log" }
}

if ($Phase -in @("All", "Validate")) {
    # Existing suite also discovers the isolated VisualSpike tests.
    & (Join-Path $PSScriptRoot "sync-unity-kernel.ps1")
    $xmlPath = Join-Path $resultRoot "editmode-results.xml"
    $logPath = Join-Path $resultRoot "editmode.log"
    $arguments = @("-batchmode", "-projectPath", ('"' + $projectPath + '"'), "-runTests",
        "-testPlatform", "editmode", "-testResults", ('"' + $xmlPath + '"'), "-logFile", ('"' + $logPath + '"'))
    $process = Start-Process -FilePath $UnityEditorPath -ArgumentList $arguments -WindowStyle Hidden -PassThru -Wait
    if ($process.ExitCode -ne 0 -or -not (Test-Path -LiteralPath $xmlPath)) { throw "Edit Mode validation failed: $logPath" }
    [xml]$results = Get-Content -LiteralPath $xmlPath
    $run = $results.'test-run'
    if ([int]$run.total -lt 11 -or [int]$run.failed -gt 0 -or [int]$run.skipped -gt 0) {
        throw "Unexpected tests: total=$($run.total), failed=$($run.failed), skipped=$($run.skipped)"
    }
    Write-Host "Unity tests: $($run.passed)/$($run.total)"
}
if ($Phase -in @("All", "Capture")) {
    Invoke-VisualEditor "SecretGame.VisualSpike.Editor.VisualSpikeBuilder.Build" "scene"
    Invoke-VisualEditor "SecretGame.VisualSpike.Editor.VisualSpikeCapture.Capture" "captures"
}
if ($Phase -in @("All", "BuildPlayer")) {
    Invoke-VisualEditor "SecretGame.VisualSpike.Editor.VisualSpikeBuilder.BuildPlayer" "player"
}
if ($Phase -in @("All", "Profile")) {
    $playerPath = Join-Path $repoRoot "Build\VisualSpike001\VisualSpike001.exe"
    if (-not (Test-Path -LiteralPath $playerPath)) { throw "Build the player first." }
    foreach ($preset in @("High", "Low")) {
        $output = Join-Path $resultRoot ("profile-" + $preset + ".json")
        $log = Join-Path $resultRoot ("player-" + $preset + ".log")
        $arguments = @("-screen-fullscreen", "0", "-screen-width", "1920", "-screen-height", "1080",
            "--visual-profile", "--profile-output", ('"' + $output + '"'), "-logFile", ('"' + $log + '"'))
        if ($preset -eq "Low") { $arguments += "--visual-low" }
        $process = Start-Process -FilePath $playerPath -ArgumentList $arguments -WindowStyle Hidden -PassThru -Wait
        if ($process.ExitCode -ne 0 -or -not (Test-Path -LiteralPath $output)) { throw "Profile failed: $log" }
        Get-Content -LiteralPath $output
    }
}
