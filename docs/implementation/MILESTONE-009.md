# Milestone 009 — Gate A closure and Unity bootstrap

## Outcome

Gate A passed on Juan's Windows machine at 4/5 external legibility with exact cross-machine replay hashes. Week 2 is authorized and the first Unity host is now source-controlled.

## Engine baseline

The spike pins **Unity 6.3 LTS `6000.3.0f1`** and URP `17.3.0`. This is a test baseline, not a permanent engine commitment. Unity 6.3 is the current LTS family and is supported through December 2027. Patch upgrades require an explicit commit and a clean replay/golden-test run.

## Single-source kernel integration

The authoritative kernel remains `src/SecretGame.Simulation`. `scripts/sync-unity-kernel.ps1` creates an ignored Unity compilation mirror under `unity/SecretGame/Assets/Generated/Kernel`, verifies each copied file by SHA-256, and writes a sync manifest. Generated files are never edited or committed.

This deliberately avoids symlinks, which are fragile on Windows, and avoids maintaining a second hand-edited copy of the simulation.

## Assembly boundary

- Generated `SecretGame.Simulation.asmdef`: `noEngineReferences: true`.
- `SecretGame.Presentation`: accepts only `IReadOnlyList<ResolvedEvent>` through `CombatEventInbox`; it has no `CombatState` API.
- `SecretGame.Editor`: owns the editor-only replay verifier and may construct fixtures.
- Edit Mode tests assert both Gate A hashes, the absence of a UnityEngine reference from Simulation, and the absence of CombatState from the presentation inbox API.

## First Unity action

1. Run `scripts/sync-unity-kernel.ps1` from the repo root.
2. Open `unity/SecretGame` in Unity 6.3 LTS.
3. Allow Package Manager to resolve URP and the Unity Test Framework.
4. Run Edit Mode tests or choose `Secret Game > Verify > Gate A Replay Hashes`.

On Windows, `scripts/run-unity-bootstrap-windows.ps1` performs steps 1, 3 and 4 in batch mode using the pinned editor path, emitting both a Unity log and NUnit XML under `TestResults/UnityBootstrap/`.

The milestone passes when Unity compiles without source changes and both replay hashes match. It does not authorize character modelling yet; the next technical-art step is a bounded Relay Yard blockout and the luminous Synthetic shader/garment experiment.
