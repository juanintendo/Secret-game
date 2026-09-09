# Milestone 004 — tactical windows and Relay Yard harness

## Purpose

Turn the spatial command pipeline into a legal, queryable round whose forecast exposes action cost and tactical conditions before commitment.

## Implemented

- Two action points per activation, spent through resolved events.
- Explicit activation ownership; out-of-order activation and actions by inactive entities are rejected.
- Deterministic initiative advancement with stable-ID tie breaking.
- Applied conditions with activation duration and expiration.
- Derived `Isolated`, `Surrounded` and `Elevated` conditions recomputed from state and never stored.
- Forecast `ConditionsOpened` and `ConditionsClosed` from the same speculative resolution used by execution.
- LOS enforcement in damaging and condition-applying commands.
- Text renderer for AP, movement paths, damage, statuses, deployment mode and tactical windows.
- A 12×12 Relay Yard scenario with the fixed trio, separate pilot and `2×2` mech, three enemies, a two-cell opening, cover edges and elevation.
- A complete scripted initiative round with forecast/result parity at every command.

## Evidence

The harness passes 33 named tests. Relay Yard resolves 21 commands into 28 ordered events and terminates at deterministic state hash:

`0402E828A8891245921561C4E6C031D7457468410501581F69F4B0F3AF3A335B`

The run visibly reports windows such as `OPENS Human:Isolated`, `OPENS Warden:Marked`, and later closures caused by the mech and Synthetic repositioning.

## Honest boundary

This is a scripted representative round, not yet the complete Gate A encounter. Damage does not yet apply Guard or cover mitigation. Forced movement, reactions, Sync, typed abilities, data-authored gear, objective resolution, AI and bounded fuzz play remain absent. Replay is deterministic locally but still needs the required second-machine confirmation.

## Next milestone

Milestone 005 builds the minimal typed effect/content layer and validators, then runs the anti-loop fuzz corpus. The human forecast-legibility test occurs only after the five Gate A mechanical rows can be exercised rather than narrated.
