# Tactical Foundation Checkpoint 001

**Status:** `ACCEPTED`.

**Accepted implementation commit:** `e44c69a906027106d97d9d0116d07aa46ca0654f`.

**Acceptance evidence:** Windows Simulation **54/54**, Tactics **18/18**, Specialty Pressure `PASS`, Unity Edit Mode **7/7**, **36/36** synchronized kernel sources, zero failed or skipped Unity tests.

## What this checkpoint proves

The project now has an engine-free, deterministic tactical foundation with:

- stable entity identity and integer logical cells;
- forecast and execution parity through the same resolver;
- reproducible command logs and state hashes;
- movement, rectangular footprints, cover, line of sight and elevation;
- initiative, action points, applied and derived conditions;
- typed effects, reactions, displacement, Guard and Integrity;
- a provisional 2×2 mech supported without engine leakage;
- pilot/mech Docked and Remote transitions without despawn/respawn;
- one shared Charge pool and no free remote initiative slot;
- measurable stay-versus-transition tradeoffs for three provisional Specialty policies.

Windows evidence already received for Milestone 023:

- Simulation **54/54**;
- Tactics **18/18** through the signed `dotnet` host;
- Specialty Pressure experiment `PASS` with all six expected outcome rows;
- Unity synchronized **36/36** kernel source files and reported `total=7 passed=7 failed=0 skipped=0 result=Passed`.

## What this checkpoint does not prove

- final combat balance or final Specialty kits;
- final attack-range and threatened-cell rules;
- final mech footprint or presentation scale;
- fun across a complete encounter;
- shipping UI, animation, art quality or performance;
- campaign, progression, enemy roster or content volume.

This is a foundation checkpoint, not a vertical slice.

## Freeze boundary

Visual work may consume:

- immutable state snapshots;
- resolved event streams;
- forecast results;
- stable entity IDs, logical anchors and footprints.

Visual work must not:

- become combat authority;
- calculate alternative damage, targeting, range or occupancy;
- mutate authoritative state through animation callbacks;
- introduce metres, `Vector3`, frame time or Unity types into simulation;
- rename provisional design into canon.

Any visual requirement that appears to need one of those violations must be returned as an architecture finding, not implemented as a shortcut.

## Exit condition

Satisfied. Begin Visual Spike 001 from the accepted foundation plus its documentation-only handoff. No merge into `main` is required to begin the isolated visual branch.
