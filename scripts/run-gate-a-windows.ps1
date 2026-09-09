$ErrorActionPreference = "Stop"

Write-Host "Gate A: isolated benchmarks"
dotnet run --configuration Release --project tools/SecretGame.Benchmarks/SecretGame.Benchmarks.csproj

Write-Host "Gate A: footprint comparison"
dotnet run --configuration Release --project tools/SecretGame.FootprintComparison/SecretGame.FootprintComparison.csproj

Write-Host "Gate A: second-machine replay and blinded legibility"
dotnet run --configuration Release --project tools/SecretGame.GateA/SecretGame.GateA.csproj
