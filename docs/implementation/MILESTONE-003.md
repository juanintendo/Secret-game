# Milestone 003 — deterministic spatial kernel and initiative query

## Purpose

Move the command/forecast/replay contract onto a real logical battlefield while keeping topology and mech footprint testable rather than silently settled design conclusions.

## Implemented

- Immutable logical map dimensions, terrain overrides, integer elevation and blocked cells.
- Cardinal-edge half/full cover records and deterministic attack-edge lookup.
- Configurable `CardinalFour` and `EightConnected` movement through one pathfinder.
- Corner-cut prevention for diagonal movement.
- Rectangular multi-cell pathfinding with bounds, terrain, elevation, occupancy and movement-allowance validation.
- Exact resolved movement path carried by the forecast and event stream.
- Integer line tracing with terrain and elevation blocking.
- Queryable future initiative rail ordered by tick and stable entity ID.
- Initial-state rejection for invalid initiative, blocked occupancy, out-of-map occupancy and entity overlap.
- Map, movement rules and initiative values included in the deterministic state hash.

## Evidence

The executable harness now covers 22 named properties. New spike evidence includes:

- a `1×1` entity crosses the one-cell test door while a `2×2` entity cannot;
- movement topology changes path length through configuration, not rewritten logic;
- diagonals cannot pass through blocked corners;
- terrain blocks line of sight;
- cover is read from the target edge facing the attack;
- a docked pilot is absent from the initiative rail while retaining its stable ID;
- movement forecast and execution carry an identical path and resulting state hash.

## Provisional boundaries

The LOS elevation rule and topology choice remain spike policies, not canon. Cover mitigation, action costs and initiative mutation are not implemented yet. The mech's `2×2` result remains unvalidated until all named footprint experiments run in the representative encounter.

## Next gate

Milestone 004 adds applied and derived conditions, explicit `conditionsOpened` / `conditionsClosed`, action economy, and a text-mode representative encounter. That is the final headless engineering block before Juan's forecast-legibility playtest decides whether work enters Unity/Astra.
