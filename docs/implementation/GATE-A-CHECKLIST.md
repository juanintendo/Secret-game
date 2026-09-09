# Gate A — live checklist

No Unity/Astra production work begins until this table is honestly green or an explicit Juan + Mina ruling changes the gate.

| Criterion | Required | Current evidence | Status |
| --- | --- | --- | --- |
| Forecast parity | 100% across at least 500 pairs | 500/500 base pairs, every Relay Yard command, and 320,000 commands across 10,000 effect/reaction/displacement encounters | `PASS — LOCAL` |
| Determinism | identical replay on two machines | Juan's Windows run reproduced both expected hashes exactly on 2026-09-09 | `PASS — TWO MACHINES` |
| Anti-loop | zero violations in 10,000 runs | 10,000 bounded encounters pass with depth-zero reactions and forced movement; extra activations and AP refunds are explicitly cut from the Gate A encounter | `PASS — SPIKE SCOPE` |
| Turn resolution | at most 8 ms pass; over 16 ms fail | isolated Release benchmark p99 150 µs, max 2,179 µs | `PASS — LOCAL` |
| Forecast recompute | at most 4 ms pass; over 16 ms fail | isolated Release benchmark p99 92 µs, max 1,158 µs | `PASS — LOCAL` |
| Action vocabulary | at most 8 archetypes cover at least 90% | nine representative specialty abilities use eight motion archetypes (100% coverage of authored sample) | `PASS — REPRESENTATIVE SET` |
| Mech footprint tests 1, 2, 4 | all pass at `2×2` | deliberate 1-tile/2-tile doors, four 14×14 pathfinding variants, rectangular collision and four mass classes pass | `PASS — HEADLESS` |
| Encounter at both footprints | `1×1` and `2×2` characterized | same 21 commands legal; 35 event types each; mech paths 3 vs 4 steps; hashes recorded | `PASS — LOCAL` |
| Legibility | outside player predicts created conditions in at least 4 of 5 plays | First Windows attempt scored 0/5 because the harness required internal token syntax without teaching coordinates, entity names, Docked semantics, or window rules. This is recorded as a harness/UX failure, not player failure. Milestone 008 replaces it with an informed numbered-choice test. | `RETEST REQUIRED` |

## Current transition rule

All technical/headless rows pass, including cross-machine determinism. The transition to Unity/Astra now requires one external confirmation: at least 4/5 on the repaired informed legibility test. Camera framing, animation clearance and remote-control readability are the first Unity/Astra visual-spike gates, not claims made by the headless kernel.
