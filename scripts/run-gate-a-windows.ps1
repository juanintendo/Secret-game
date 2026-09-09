$ErrorActionPreference = "Stop"

Write-Host "Gate A: isolated benchmarks"
dotnet run --configuration Release --project tools/SecretGame.Benchmarks/SecretGame.Benchmarks.csproj
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

Write-Host "Gate A: footprint comparison"
dotnet run --configuration Release --project tools/SecretGame.FootprintComparison/SecretGame.FootprintComparison.csproj
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

Write-Host "Gate A: second-machine replay and informed legibility"
dotnet run --configuration Release --project tools/SecretGame.GateA/SecretGame.GateA.csproj
exit $LASTEXITCODE
