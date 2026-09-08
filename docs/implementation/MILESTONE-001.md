# Milestone 001 — deterministic simulation foundation

## Status

Implementation started from preproduction checkpoint `2d1fd60a79f4a509320a2a5b5a4e9a3eeac8b723` on branch `implementation/kernel-milestone-01`.

## Rulings applied

- `Integrity` is exclusively a bounded damage pool, not a fifth visible attribute.
- Combat resolution has no hit roll in this milestone. Damage, conditions and displacement will be deterministic.
- Entity footprints are generic integer rectangles. The mech's `2×2` footprint remains a spike hypothesis, configurable without changing the model.
- Pilot and mech retain distinct stable IDs in every deployment mode. A docked pilot remains in authoritative state with all four battlefield flags disabled.
- The simulation uses integer logical cells and has no dependency on engine or physical-scale types.

## Implemented proof

The first executable harness verifies rectangular occupancy, docked and remote state transitions, stable entity identity, insertion-order-independent state hashing, and bounded Integrity damage.

## Explicitly deferred

Movement policy, line of sight, cover, initiative, commands, event streams, forecast UX, Unity integration and visual assets are outside this foundation commit. They enter only through subsequent falsifiable milestones.
