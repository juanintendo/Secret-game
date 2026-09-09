# Gate A closeout — 2026-09-09

## Verdict

**PASS. Unity/Astra week 2 is authorized.**

Gate A tested whether the engine-free combat thesis could survive deterministic execution, forecast parity, multi-cell occupancy, bounded resolution, a second machine, and a person who did not design the rules. It passed after one required iteration to the human-test protocol.

## Final evidence

| Criterion | Result |
| --- | --- |
| Forecast parity | 500/500 base pairs, all Relay Yard commands, and 320,000 fuzz commands |
| Anti-loop | 10,000 bounded encounters, zero invariant violations |
| Windows turn benchmark | p50 59 µs · p95 89 µs · p99 111 µs · max 681 µs |
| Windows forecast benchmark | p50 18 µs · p95 22 µs · p99 34 µs · max 246 µs |
| 1×1 replay | `716BE2E159FBB184B422C33910A6A0513FEF8B034D53E531F76E8A2BAACAF0B4` on both machines |
| 2×2 replay | `533CEDD457740A8604C19265EFEF864E361325152A10777E32C1C72D62260689` on both machines |
| Footprint comparison | both legal; 35 events each; event-stream shape equal; mech path 3 vs 4 steps |
| External legibility | Juan 4/5 on the repaired informed test |

## What the failed first protocol taught us

The original 0/5 run did not expose a combat-rule failure. It exposed an interface that asked for undocumented machine tokens, hid coordinates, collapsed three enemies into `E`, and did not teach `Docked`. Milestone 008 corrected the protocol while keeping the commands, resolver and expected deltas unchanged. The repaired test then passed.

This is the first project claim to move from declared direction or hypothesis into experimentally validated fact: the same kernel replays identically on two machines, and a minimally taught player can predict its tactical windows before commitment.

## Carry-forward UX finding

`Docked` is a valid internal state name but an ambiguous player-facing action. Juan initially read it as a mech stance or separation state. The Unity UI should present the actions as **Board Mech** and **Deploy Remotely**, then preview the explicit consequence: which entity leaves or enters spatial occupancy, selection, targeting and initiative. The kernel enum remains unchanged.

## Next gate

The Unity 6.3 LTS + URP technical-art spike now begins. Its first proof is narrow: the existing simulation source runs unchanged inside Unity, reproduces both hashes in-editor, and presentation code consumes resolved events without obtaining authority over `CombatState`.
