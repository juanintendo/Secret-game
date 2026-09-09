---
name: combat-kernel
description: "Rules and invariants for this project's deterministic tactical simulation: grid, initiative clock, AP economy, reactions, forced movement, conditions, event tags, synchronization, forecast parity, and the anti-loop invariants. Load before writing or changing any combat, ability resolution, initiative, effect, targeting, AI, or simulation code, and before proposing new combat rules."
---

# Combat Kernel

Read `canon-guard` first. This skill governs the simulation layer.

## 0. The one architectural law

**The simulation is a pure, clonable, engine-free state machine. Presentation is a downstream consumer of its event stream and has zero authority.**

- `Game.Core` and `Game.Tactics` must compile with **no `UnityEngine` reference**. They are a plain .NET class library. This is what makes the engine decision reversible.
- `Game.Tactics` is the **only** writer to `BattleState`.
- `Game.Presentation` may read `IReadOnlyList<BattleEvent>` and never `BattleState`. Enforce with an asmdef boundary plus a test that fails on the reference.
- Gameplay truth never waits on a Timeline clip, an animation event, or a coroutine.

## 1. Determinism rules (non-negotiable)

- **No floats in the simulation.** Use `int` or fixed-point Q16.16. Floats are allowed in presentation only.
- **No `UnityEngine.Random`, no `System.Random` ambient.** One seeded PRNG (PCG/xoshiro) whose state lives inside `BattleState`.
- **No dependence on hash-map iteration order.** Sorted keys or a deterministic ordered collection.
- **No wall-clock, no `DateTime`, no frame count** inside the sim.
- Every resolved event is appended to an ordered log. **Hash `BattleState` after every event** so a replay divergence is caught at the exact event that diverged.
- Every playtest writes `{seed, initialState, commandList}`. A CI test replays every recorded session and asserts the final state hash. Build this early; it pays for itself.

## 2. Forecast parity — the rule most likely to be violated

**The forecast must be produced by the same resolver that produces the result.**

- `Game.Forecast` is a separate module. It runs the real resolver on a cloned state with `DryRun = true` and returns the complete predicted delta.
- The **UI**, the **AI**, and the **tests** all consume `Game.Forecast`. None of them may reimplement damage, displacement, or condition math.
- If forecast logic ever lives inside the UI, you will ship a game whose preview lies. Treat any duplicated formula as a bug.
- A test asserts, for a large sample of legal actions: `forecast(state, action) == apply(state, action)`.

## 3. Grid and positioning (v0 subset)

- Square grid with **configurable movement topology** during the spike: `CardinalFour` and `EightConnected` run through the same pathfinder. Both cost 1 per logical step and forbid cutting blocked corners. Do not promote either policy to canon until the representative encounter compares them.
- **4 facings.** Flank = attacker in the target's side or rear arc.
- Integer elevation bands. A single `Vertical` stat covers climb/jump.
- **Cover lives on tile edges** (half / full), not on "adjacent to a wall" heuristics. Edge cover is deterministic and drawable.
- LOS: tile-center to tile-center with elevation blocking.
- Eight-direction adjacency remains available for spatial-pressure concepts (`Surrounded`, knockback direction snapping) independently of the selected movement topology.
- v0 map: 12×12 to 16×16, three elevation bands.

## 4. Initiative — tick clock with hard invariants

A shared tick clock. Each unit has `nextActAt`. On acting: `nextActAt += max(MIN_INTERVAL, BASE - Speed)`.

**Invariant A — Monotone clock.** No effect may ever set a unit's `nextActAt` earlier than `now`. Haste reduces the *next* interval; it never rewinds the clock. This single rule kills the classic haste-loop.

**Invariant B — Haste is boolean + duration, never a stacking magnitude.** `Hasted` reduces the next interval by a flat amount, once. Forecastable, unexploitable.

**Invariant C — Extra activations are budgeted.** Any effect granting an off-turn activation (AP refunds, initiative exchange, assists) draws from a per-unit, per-round budget of **1**. Tracked in the sim, displayed in the UI.

**Required UI consequence:** a visible **turn-order rail** showing the next 8 activations, plus a *ghost rail* on hover showing how the previewed action reorders it. Initiative manipulation is a headline mechanic; it is worthless if invisible.

## 5. AP economy

- 2 AP per activation. Move = 1, Primary = 1, Utility = 1, Guard/Overwatch = 1 (ends activation), Ability = 1 or 2.
- **Move + Move is legal but the second Move costs your Guard**: you end with `Exposed` until your next activation. Mobility stays available; it creates a condition other characters can exploit, feeding the tag grammar.
- **Reactions do not cost AP.** Each unit has 1 `Reaction` charge, refreshed at its own activation.

## 6. Reactions — exactly three in v0

1. **Guard / Overwatch** — universal; declared, fires once.
2. **Intercept** — Bulwark; when an adjacent ally would be hit, move up to 1 and take it.
3. **Reaction Shot** — Gunslinger; enemy in LOS enters a band, fire once.

All three are one implementation: `ReactionTrigger { event, condition, response }`.

**Reaction anti-loop:** depth 0 — a reaction can never trigger a reaction. At most **1 reaction per unit per event**, at most **2 reactions total per event**.

## 7. Forced movement

- `Displace(target, direction, distance)`. Direction snaps to the 8 compass headings.
- **Mass classes:** `Light` / `Standard` / `Heavy` / `Anchored`. Distance is reduced per tier; `Anchored` is immune (see §11 for what happens instead).
- **Collision:** entering an occupied tile → the displaced unit stops one tile short, **both** take collision damage. Entering a blocked tile/wall → stop + impact damage. Leaving a ledge with drop ≥ 2 → fall damage + `Prone`. Uphill by ≥ 2 → blocked.
- **Forced movement is depth 0.** A displaced unit colliding with another does **not** push it. This keeps resolution deterministic, drawable, and forecastable.
- The forecast must draw the projected path, the stop tile, and every collision marker before commitment.

## 8. Conditions vs. event tags — keep these separate

**Statuses** live on units, have durations, and are visible on the unit:
`Exposed`, `Prone`, `Staggered` (loses reaction and guard), `Pinned` (movement 0), `Shocked`, `Hacked`, `Marked`, `Hasted`, `Slowed`, `Guarded`.

**Event tags** are emitted by resolved events and live for a **1-activation window**:
`Expose`, `Displace`, `Conductive`, `Guarded`, `Hacked`, `Isolated`, `Intercepted`, `Rescued`.

This distinction is what makes a handoff feel like a handoff instead of a buff. Do not merge them.

Conditions carry a `cause` field (`EnemyAdvance` | `AllyAction` | `SelfMove`). Payouts may read it — see §10.

## 9. Damage and uncertainty

- **Accepted for v0: no to-hit roll.** Damage, conditions and displacement are deterministic and shown before commitment. Any future hybrid experiment belongs on an experimental branch and requires an explicit ruling before entering the kernel.
- Uncertainty is relocated to information the player can *work on*: enemy intent, threatened tiles, unscanned enemies, and whether an enemy still holds a reaction. Not dice.
- Damage depletes **Guard** first, then **Integrity**. `Piercing` attacks bypass Guard — this is the main enemy-design lever against turtling.
- The forecast must show: exact damage, resulting Guard/Integrity, displacement path, resulting turn-order rail, and **which conditional windows this action opens or closes**. That last line is the game's signature UI element.

## 10. Recovery model

Three layers — see the consult doc for full rationale.

- **Guard** — renewable, never passive. Gained only from mechanical acts: ending in cover, the Guard action, a Bulwark interception, a Redline vent, a synthetic Pulse.
- **Integrity** — the health pool. Restored in-mission only via an earned or map-placed resource. Rare.
- **Faults** — typed persistent modifiers gained at Integrity thresholds (e.g. `Servo Lag`: −1 Move, reactions cost 2). They survive the mission and are cleared between missions with a scarce resource or a narrative choice.

**Downed, not dead.** At Integrity 0: prone, no actions, immediate Fault, bleed/cascade timer. A teammate `Rescue` (move + 1 AP, adjacent) restores 1 Integrity + small Guard, clears the timer, keeps the Fault, emits `Rescued`, grants Sync. Timer expiry = mission-level fail-forward consequence, **never permadeath for the fixed trio** (unless Juan/Mina rule otherwise).

**Anti-death-spiral: `Cornered`.** When team aggregate Integrity drops below ~40%, Sync generation increases and a specific class of recovery ability unlocks. Losing becomes a comeback *opportunity*, not free healing.

## 11. Bosses and control — degrade, never null

Ghost Surgery and displacement must stay useful against every boss without deleting turns.

1. **Subsystems, not the unit.** A boss has 3–4 named subsystems (Optics, Actuator, Comms, Coolant). Hacks disable a specific *option*, never a turn.
2. **Firewall pool.** Hacks deplete `Firewall`; at 0 the boss becomes `Compromised` for N turns. Hacking is a second health bar the synthetic attacks.
3. **Documented degradation.** Every control or displacement ability must specify a `bossDegradation` alternative (e.g. `Anchored` targets are not pushed but become `Staggered + Unbalanced`).
4. **Multi-tile units:** a 2×2 enemy counts as **1** hostile for `Surrounded` (otherwise bosses trivially satisfy conditional windows). It instead grants a separate `PinnedByMass` condition that enables the same windows.

## 12. Synchronization

Sync is granted **only** when a unit consumes an event tag emitted by a *different* unit. Damage grants zero Sync. A chain with three distinct contributors grants a bonus tier.

Ladder: joint attacks → handoffs → assists during another turn → rescues that bend a rule → three-character chains → synchronized Limit Breaks.

## 13. Anti-loop invariants — assert all of these in tests

1. Monotone clock (§4 A).
2. `MIN_INTERVAL > 0` floor on every activation.
3. Haste is boolean + duration (§4 B).
4. ≤ 1 extra activation per unit per round (§4 C).
5. Reaction depth 0; ≤ 1 per unit per event; ≤ 2 per event (§6).
6. ≤ 1 refunded AP per activation, and refunded AP may not pay for the ability that refunded it (`NoRefundChain` tag).
7. Forced movement depth 0 (§7).
8. `MaxEffectApplicationsPerStack = 8` — a violation is a **test failure**, not a silent clamp.
9. Global resolution-stack depth cap with a logged error.

**Required test:** a random-legal-action fuzz agent plays 10,000 encounters and asserts every encounter terminates within a bounded tick count and no invariant fires.

## 14. Where bespoke code is allowed

Typed data grammar covers ~90% of content (see `content-schema`). Bespoke C# is permitted **only** for:

- the **Conduction / Link substrate** (a graph with decay and propagation — a data structure, not an effect);
- **MechMode + Directive execution** (a small behavior system);
- **Limit Break choreography** (set pieces: Timeline plus an explicit scripted sim macro that emits a documented event list);
- the **forecast resolver** itself.

A tiny sandboxed expression DSL for numbers (`threatCount * 2 + intellect / 4`) is preferred over both magic constants and per-talent C#. **Per-talent scripts are banned.**
