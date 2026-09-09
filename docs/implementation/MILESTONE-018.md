# Milestone 018 — separate .NET and Unity language boundaries

## Evidence

After the corrected SDK pin selected .NET `8.0.425`, the complete Simulation harness passed **33/33**. Tactics compilation then exposed that `SecretGame.Content` already uses C# 11 required members. The repository-wide C# 10 pin from Milestone 015 rejected those declarations before any Tactics test ran.

This was a configuration regression, not a Content or Tactics behavior failure.

## Correction

- The .NET solution pins C# `11.0`, the minimum language level required by its established authoring models.
- The synchronized Unity Simulation assembly remains explicitly pinned to C# `10.0` through `Assets/csc.rsp` because the kernel itself requires no C# 11 feature.
- The two values are intentionally different and represent different compilation boundaries, not toolchain drift.

Removing `required` from Content would weaken compile-time completeness guarantees merely to satisfy a Unity boundary that Content has not crossed. That conversion is rejected.

## Verified and pending evidence

- Verified: .NET 8.0.425 selected.
- Verified: Simulation 33/33 passed after the portable hash implementation.
- Verified independently: Unity Edit Mode 3/3 passed with Burst disabled and Smart App Control enabled.
- Pending after this correction: Tactics harness execution under C# 11.

If Content later crosses into Unity, it requires its own adapter or explicit C# 11 compatibility experiment. This milestone does not silently authorize that migration.
