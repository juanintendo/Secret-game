# Milestone 005 — typed content contract and bounded fuzz

## Purpose

Make representative combat content inspectable before runtime ability execution expands, and put the existing command pipeline through a repeatable large corpus.

## Implemented

- Engine-free `SecretGame.Content` assembly with typed definitions for abilities, effects, reactions, gear and weapons.
- Twelve allowed effect types; all authored numeric values are integer-only.
- Nine representative abilities: one for each approved protagonist specialty.
- Three representative reactions with a hard once-per-event authoring limit.
- Eight motion archetypes across nine abilities, proving at least one reuse in the representative set.
- Four provisional gear indices, two combinable two-piece sets, one loose item and one separate cyborg weapon.
- `WeaponDefinition` has no `SetId`; raw weapon JSON containing `setId` is rejected before deserialization.
- Four JSON schemas with `additionalProperties: false` at each root record.
- Catalog validation for IDs, versions, AP costs, approved motion archetypes, control degradation, displacement collision policy, reaction references and loadout ownership.
- Deterministic fuzz executable covering 10,000 bounded encounters.
- Corrected the stale five-slot rule in the content-schema skill.

## Evidence

- Simulation tests: `33/33 PASS`.
- Content tests: `9/9 PASS`.
- Boundary scan: `PASS`.
- Relay Yard: 28 events and unchanged final hash `0402E828A8891245921561C4E6C031D7457468410501581F69F4B0F3AF3A335B`.
- Fuzz corpus: 10,000 encounters, 320,000 commands and 514,883 events in 12,141 ms on the current container.
- Every generated fuzz command matched forecast events and resulting hash; replay reproduced every final hash and ordered event stream.

## Honest boundary

The JSON definitions are validated authoring contracts, not yet executable abilities. Runtime reactions, forced movement, Guard mitigation, typed effect execution, objectives and AI policy remain absent. Consequently the 10,000-run corpus proves that the current activation/AP/damage/condition subset is bounded; it does **not** yet satisfy the full anti-loop gate for reaction chains, forced movement or extra activations.

The four gear indices are intentionally unnamed pending Q26. They are structural positions, not canon slot names.

## Next milestone

Milestone 006 compiles the effect grammar into commands/events, adds bounded reaction-chain processing and displacement collision behavior, then reruns the 10,000 corpus against those mechanics. That is the last kernel-heavy expansion before the remaining mech comparison and human legibility gates determine whether the Unity/Astra visual spike may begin.
