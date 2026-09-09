# Gate A — live checklist

No Unity/Astra production work begins until this table is honestly green or an explicit Juan + Mina ruling changes the gate.

| Criterion | Required | Current evidence | Status |
| --- | --- | --- | --- |
| Forecast parity | 100% across at least 500 pairs | 500/500 base pairs, every Relay Yard command, and 320,000 commands across 10,000 effect/reaction/displacement encounters | `PASS — LOCAL` |
| Determinism | identical replay on two machines | repeated local replay produces identical trace and hash | `SECOND MACHINE PENDING` |
| Anti-loop | zero violations in 10,000 runs | 10,000 bounded encounters pass with depth-zero reactions and forced movement; extra activations and AP refunds remain absent | `PARTIAL` |
| Turn resolution | at most 8 ms pass; over 16 ms fail | not measured as an isolated benchmark | `PENDING` |
| Forecast recompute | at most 4 ms pass; over 16 ms fail | 500-evaluation aggregate budget passes locally | `PROVISIONAL PASS` |
| Action vocabulary | at most 8 archetypes cover at least 90% | nine representative specialty abilities use eight motion archetypes (100% coverage of authored sample) | `PASS — REPRESENTATIVE SET` |
| Mech footprint tests 1, 2, 4 | all pass at `2×2` | narrow-door pathfinding plus 2×2 displacement obstruction demonstrated; cover/camera/clearance set incomplete | `PARTIAL` |
| Encounter at both footprints | `1×1` and `2×2` characterized | only `2×2` Relay Yard currently scripted | `PENDING` |
| Legibility | outside player predicts created conditions in at least 4 of 5 plays | Juan test not run | `PENDING` |

## Current transition rule

Gate A does not pass merely because Relay Yard prints a board or the current subset survives fuzzing. Reaction/displacement bounds now have local evidence; the next valid transition to Unity/Astra still requires the concrete reaction qualifiers, isolated benchmarks, both-footprint comparison, second-machine replay, and Juan's blinded legibility result.
