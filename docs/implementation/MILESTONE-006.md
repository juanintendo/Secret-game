# Milestone 006 — executable effect stack, displacement and reaction bounds

## Purpose

Connect validated content to the authoritative resolver without allowing content, UI or animation to write combat state.

## Implemented

- `SecretGame.Tactics` compiles supported authored effects into simulation-native `EffectStackCommand` values.
- One stack spends its declared AP cost exactly once, even when it contains multiple effects.
- Stack execution is preflighted on a clone, so an invalid later effect or reaction cannot partially mutate authoritative state.
- Maximum eight total effect applications, including reaction responses.
- Executable `Damage`, `ApplyStatus` and `Displace` effects. Unsupported authored types fail explicitly rather than silently doing nothing.
- Eight-direction forced movement with projected path and explicit `Completed`, `Blocked` or `Occupied` stop reason.
- Blocked collision damages the displaced target; occupied collision damages both bodies and never pushes the second body.
- Rectangular footprints participate in every displacement occupancy check.
- Reaction charges live in authoritative entity state, refresh on the owner's activation and participate in state hashing.
- Reaction invocations consume one charge, execute off-turn, allow at most one reaction per unit per triggering effect and at most two total reactions per effect.
- Reaction responses are structurally depth zero: response effects are applied but never scanned for more reactions.
- Text forecast exposes displacement path/stop, reaction charge consumption and reaction identity.

## Evidence

- Simulation regression suite: `33/33 PASS`.
- Tactics suite: `10/10 PASS`.
- Content suite from Milestone 005: `9/9 PASS`.
- Expanded fuzz: 10,000 encounters, 320,000 commands and 780,560 events in 21,816 ms on the final validation run in the current container.
- Every fuzz command matched forecast events and resulting hash. Every encounter replay reproduced its complete event stream and final hash.
- No fuzz resolution exceeded reaction-count invariants.
- Simulation boundary scan: `PASS`.
- Relay Yard regression: 35 events, final hash `63710B5D4B78BAB3A50CB3F69F6FC68244159FD26EEDD5A482EDE7AE3A552728`.

## Honest boundary

The kernel now enforces reaction mechanics; the tactical qualification rules for the three authored reactions are not complete. Guard still needs the Guard pool, Intercept needs pre-damage target substitution and adjacency qualification, and Reaction Shot needs band-entry qualification. Until those selectors exist, callers supply already-qualified reaction invocations.

Only three of the twelve authored effect types execute. `RemoveStatus`, initiative modification, Guard, resources, tags, links and conditionals remain explicit `NotSupportedException` paths in the compiler.

The fuzz corpus now exercises reaction responses and forced movement, but full Gate A anti-loop status remains partial until extra-activation and AP-refund paths either exist and pass or are explicitly cut from the representative encounter.

## Next milestone

Milestone 007 implements Guard as a separate pool and the three concrete reaction qualifiers. It then adds isolated turn/forecast benchmarks and the two-footprint Relay Yard comparison. Those results determine the final blinded legibility test and the transition into the Unity/Astra visual spike.
