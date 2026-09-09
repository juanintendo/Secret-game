# Milestone 007 — Guard, concrete reactions and Gate A closeout harness

## Purpose

Complete the headless mechanics and measurements required before the Unity/Astra visual spike.

## Implemented

- Separate `GuardPool` in authoritative entity state and deterministic state hashes.
- Non-piercing damage depletes Guard before Integrity; Piercing bypasses Guard.
- Guard grants and damage absorption emit explicit forecastable events.
- Before/after reaction timing and pre-damage target redirection.
- Concrete tactical qualifiers:
  - Guard requires a declared `Guarded` condition, available Guard and a reaction charge.
  - Bulwark Intercept requires an adjacent ally, grants the authored Guard response and redirects damage before resolution.
  - Gunslinger Reaction Shot requires a displacement from outside to inside a configured band and LOS in the resolver-produced resulting state.
- Stable priority uses Intercept before Guard when both could protect the same hit. This is a spike recommendation, not canon.
- Light, Standard, Heavy and Anchored mass classes reduce displacement by tier; Anchored converts displacement into one activation of Staggered.
- Four 14×14 narrow-map variants for 2×2 pathfinding.
- Isolated turn and forecast benchmark executable.
- Relay Yard comparison executable using identical commands at 1×1 and 2×2.
- Interactive five-question Gate A legibility and second-machine replay executable.
- Windows PowerShell runner at `scripts/run-gate-a-windows.ps1`.

## Final local evidence

- Simulation: `33/33 PASS`.
- Content: `9/9 PASS`.
- Tactics: `17/17 PASS`.
- Fuzz: 10,000 encounters, 320,000 commands, 780,560 events, 23,676 ms.
- Turn benchmark: p50 52 µs, p95 87 µs, p99 150 µs, max 2,179 µs — pass against 8 ms p99 target.
- Forecast benchmark: p50 27 µs, p95 47 µs, p99 92 µs, max 1,158 µs — pass against 4 ms p99 target.
- Relay Yard 1×1: legal, 35 events, mech path 3 steps.
- Relay Yard 2×2: legal, 35 events, mech path 4 steps.
- Both footprints produce the same ordered event-type shape.
- 2×2 pathfinding reaches the objective on all four 14×14 narrow-map variants.
- Simulation boundary scan: pass.

## Cross-machine hashes expected

- 1×1: `716BE2E159FBB184B422C33910A6A0513FEF8B034D53E531F76E8A2BAACAF0B4`
- 2×2: `533CEDD457740A8604C19265EFEF864E361325152A10777E32C1C72D62260689`

These remain local evidence until reproduced on Juan's Windows machine.

## Gate interpretation

Extra activations and AP refunds are deliberately excluded from the representative Gate A encounter. They remain future mechanics and must receive their own invariant tests before entering a playable build. Their absence does not masquerade as fuzz coverage.

Headless mech validations 1, 2 and 4 pass. Camera framing, animation clearance and remote-control readability intentionally move into the Unity/Astra visual spike because they cannot be measured by an engine-free kernel.

## Transition

Run `scripts/run-gate-a-windows.ps1`. If both replay hashes match and the blinded legibility result is at least 4/5, Gate A closes and the next commit bootstraps the Unity 6 URP/Astra visual spike. A failure is evidence and stops the transition.
