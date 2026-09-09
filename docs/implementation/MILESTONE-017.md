# Milestone 017 — corrected .NET 8 feature-band pin

## Observed failure

Milestone 015 requested SDK `8.0.100` with `rollForward: latestPatch`. The installed SDK was `8.0.425`, but .NET correctly rejected it: `latestPatch` stays within the requested `8.0.1xx` feature band and does not cross into `8.0.4xx`.

No project compilation or test ran during this failure.

## Correction

`global.json` now requests `8.0.400` with `latestPatch`. This selects Juan's verified installed SDK `8.0.425`, excludes the independently installed .NET 10 SDK and keeps future resolution inside the approved .NET 8.0.4xx feature band.

## Required evidence

From the repository root:

1. `dotnet --version` returns `8.0.425`.
2. Simulation and Tactics harnesses pass with their established replay evidence.
3. Unity bootstrap is evaluated independently; its batch process does not use the system SDK resolution governed by this file.
