# Visual Spike 001 — implementation evidence

Status: `EXPERIMENT IMPLEMENTED`. Baseline: `47d60960c8113fc664fb3a1414daacd0c0c25c82`. Unity: `6000.3.23f1 / URP 17.3`.

## Executable proof

`Assets/Game/VisualSpike/Reference/VisualSpike001.unity` is one relay-yard scene with the canonical-identity construction studies, neutral enemy proxy, tactical camera, resolved two-cell movement, rear/top Docked study, forecast, one resolved attack, immediate hit feedback, toon bands, inverted-hull outlines, reference lighting and High/Low assets. The same mech hierarchy is used throughout.

The visual layer consumes snapshots, exact movement paths, forecasts and resolved events from `CombatResolver`. It does not calculate targeting, damage, occupancy or deployment. Removing presentation or changing `cellSizeMeters` / preset leaves replay hash `5D2700392B4CCEDC15D17B1A0366DC4CB8D4D024D49EC9F41CB69FC3D2F761CD` unchanged.

## Validation

- Managed: Simulation 54/54; Tactics 18/18; Specialty Pressure PASS.
- Unity Edit Mode: 11/11, zero failed/skipped; synchronized kernel 36/36.
- Windows player build: PASS.
- Fixed evidence: 78 PNG + 78 JSON sidecars, covering 13 camera/time IDs × High//Low × 1.0/1.25/1.5 presentation scales.
- High at 1920×1080, i7-13700H / RTX 4060 Laptop: 1,801 samples; frame p95 16.69 ms; p99 16.79 ms; 6 frames over 17 ms.
- Low at 1920×1080, same machine: 1,800 samples; frame p95 16.68 ms; p99 16.81 ms; 4 frames over 17 ms.
- CPU FrameTimingManager p95: High 16.72 ms, Low 16.74 ms. GPU timing was unavailable from this D3D11 player (`-1`), so separate GPU budget is unresolved.

## Gate status

The small reference executable and local 60 fps frame-pacing target are credible. This is not a Visual Spike `PASS`: canonical visual approval, premium model quality, outline motion review and representative-hardware GPU timing remain open. Available sources cannot support an approved production character mesh, face or rig. The procedural studies are bounded construction evidence and must not enter character production as shipping assets.

Generated captures and player builds remain ignored build evidence. Reproduce with `scripts/run-visual-spike-windows.ps1`.
