# CLAUDE TWO-WEEK SPIKE 001

Companion to `CLAUDE-TECHNICAL-CONSULTATION-001.md`.
Date: 2026-09-08
Working source: `C:\Users\colom\Documents\DD90s`
Assumed start: **Monday 2026-09-14**. Implementation lead: Mina, primarily via OpenAI Astra.

`RECOMMENDATION` throughout. Nothing here is canon.

---

## Purpose

To determine whether **Unity 6 URP becomes a binding decision**, and — more importantly — whether the combat thesis survives contact with a player.

The spike answers four questions in this order:

1. Can a player read the forecast? (R2)
2. Is the animation budget survivable? (R1)
3. Does the simulation stay deterministic and exploit-free? (R7, R10)
4. Can Unity URP deliver stepped 90s cadence at 60 fps without shimmer, latency or a muddied UI? (R16, R17, R18)

**Week 1 is deliberately not in Unity.** The two highest-cost-of-learning-late assumptions are testable in a terminal for a fraction of the price, and they produce the replay corpus that every later regression test consumes. Building the renderer first would mean discovering a broken thesis after paying for it.

---

## Prerequisites — must be settled before day 1

These are not spike work. If they are open on Monday morning, the spike stalls.

| Item | Source | Owner |
|---|---|---|
| Q2 — `Integrity`: attribute or pool | `CLAUDE-QUESTIONS-001.md` A/Q2 | Mina |
| Q3 — to-hit rolls: yes or no | A/Q3 | Mina |
| **Q30 — the minimum supported machine, named** | B/Q30 | Mina |
| ~~Q31 — pilot/mech entity model~~ — **RULED**, build to it; deployment-mode-by-Specialty stays a product question | B/Q31 | — |
| Q27 — does gameplay display equipped gear *(blocks nothing in the spike; blocks modelling after it)* | C/Q27 | Juan |

**Resolved, no longer prerequisites:** ~~Q1 Operator's arms~~ — settled canon, never a conflict. ~~Q7~~ — superseded by Q30.

**Q4 (mech footprint) is no longer a prerequisite ruling — it is now spike work.** The addendum supplies a provisional hypothesis (≈3.6 m, 2×2); the spike validates or falsifies it. Build day 2 to test it, not to assume it.

**Revision 3 — the prerequisite list is now three rulings, not five.** Con Girl's Agility scaling is **settled**; transcribing it into the canon documents is an editing task that does not gate the kernel, and validator rule §10.3 #7 enforces it in CI regardless. Sheet hygiene is source governance. Neither belongs on this list. See consultation §21.

**Revision 4 — the pilot/mech entity model is now ruled, not a spike decision.** Mina's ruling (consultation §9.6.1): pilot and mech keep **separate, stable logical entity IDs regardless of deployment mode**. Embarked, the pilot stays in authoritative state but is **non-spatial, non-selectable, non-targetable, with no independent initiative slot**; the mech owns occupancy and selection. In remote operation both may be spatial and addressable. **Build to this on day 2** — it is no longer a choice to make. The conditional fourth blocker is closed and the count is exactly three.

---

## Week 1 — headless kernel and forecast. No engine.

### Day 1 — Skeleton, determinism harness, and the census begins

**Build.** Solution scaffold with the assembly boundaries from consultation §9.1. `Game.Core`: fixed-point arithmetic, tags, a seeded PRNG owned in one place, an append-only event log. Two CI assertions from the very first commit: **no `UnityEngine` reference in any simulation assembly**, and **no float literal in any content numeric field**.

**In parallel (Juan, not Mina).** Start the **Action Vocabulary Census** — enumerate every distinct authored action implied by the nine signature abilities plus locomotion and reactions, and map each to one of eight motion archetypes per character. This is the R1 validation and it needs no code.

**Instrumentation.** Wall-clock timing hooks on command application and forecast evaluation, from day 1. Retrofitting measurement is how budgets get missed.

**Done when:** an empty simulation compiles with zero engine references and the CI assertions run.

---

### Day 2 — Grid, LOS, cover, elevation

**Build.** `Game.Tactics`: square grid, 8-connected movement, integer elevation. LOS tile-centre to tile-centre with corner rules; elevation grants LOS over one intervening level. Binary half/full cover derived from the tile edge crossed, **not** from a separate object list — deriving it is what keeps forecast and result identical.

**The kernel is dimensionless — build it that way from the first line.** Per Mina's ruling: **integer cell coordinates, no metres anywhere in the simulation.** Footprints are counts of cells — human **1×1**, mech **2×2** (candidate). `cellSizeMeters` is a Unity field that does not exist in week 1 and never gates anything here. Add the CI assertion today alongside the float-literal rule: **no `Meters`, `worldPos`, `Vector3` or float distance in any sim assembly.** Q32 is a week-2 visual comparison, not a prerequisite.

**Mech occupancy — build for the 2×2 hypothesis, and make the footprint a constant.** Implement multi-tile occupancy **today** as a first-class feature: multi-tile pathing, multi-tile LOS, and per-edge cover contribution. Do not retrofit it later, and do not hard-code `1` anywhere — footprint is a unit property so that running the encounter at 1×1 and 2×2 is a config change, not a rewrite.

Build the §16 map as specified: **one 2-tile doorway, one 1-tile doorway, 1-tile gantry ramps, irregular pillar scatter.** That geometry is what makes validations 1, 2 and 3 testable rather than theoretical.

**Done when:** a text renderer prints the §16 map with elevation bands, cover edges and LOS from any tile, **and a 2×2 unit can be placed, pathed and blocked correctly.**

**Mech footprint validations run today** — consultation §7.6, tests 1, 2 and 4:

| Test | Pass | Fail |
|---|---|---|
| 1 Doors and traversal | Mech routes through the 2-tile door; the 1-tile door is impassable *by design* and the map still works | Mech cannot leave the yard at all, or every door must be widened |
| 2 Narrow-map pathfinding | 2×2 pathing resolves with no oscillation and no illegal diagonal squeeze, on 4 map variants | Mech cannot reach the objective on ≥ 1 variant |
| 4 Occupancy and forced movement | `Displace` on a 2×2 unit has defined collision, legality and mass class | Knockback on the mech is undefined or illegal everywhere |

**If any of these fail, that is a finding, not a problem to design around.** Record it and escalate per R30 — do not compress the mech to 1×1, and do not widen every door.

---

### Day 3 — Initiative, AP, commands

**Build.** Tick-clock initiative: each unit has `nextAt`, lowest acts, acting adds `speedCost`. 2 AP per activation. `Command` objects — serializable, validated separately from application, appended to the log. Three enemy families stubbed with published intent (icon plus threatened tiles).

**Entity flags, per Mina's ruling (consultation §9.6.1).** Allocate pilot and mech entity IDs **once, at encounter setup** — never on a deployment change. Give every entity four state flags: `spatial`, `selectable`, `targetable`, `hasInitiativeSlot`. Deployment mode sets them; **the turn-order rail, targeting validation, LOS and the forecast filter on the flags and never special-case "is this a docked pilot".** Assert in a test that embarking and disembarking are state transitions on existing IDs, and that a replay crossing a mode change reproduces bit-identically — that is the property the stable-ID ruling buys, and it is cheap to protect on day 3 and expensive to retrofit.

**Benchmark today, not later:** turn resolution against the §13 target of ≤ 8 ms.

**Done when:** a full round of stub units resolves, the turn-order rail is queryable, and the replay log round-trips.

---

### Day 4 — `Game.Forecast` and the parity test

**The most important day of the spike.**

**Build.** `Forecast(state, command)` running the **same resolver** against a speculative state copy, returning legality, cost, damage and guard before/after, displacement paths, resulting turn order, `reactionsProvoked`, `syncDelta`, and — the signature output — `conditionsOpened` and `conditionsClosed`.

**Build the parity test in the same commit.** For a corpus of (state, command) pairs, applying the command must produce *exactly* the forecast. Any divergence fails the build. If this test is deferred, it never gets written and R6 becomes real.

**Benchmark:** forecast recompute against ≤ 4 ms.

**Done when:** parity holds across ≥ 500 pairs and hovering a tile is instantaneous.

---

### Day 5 — Effects, conditions, the nine abilities, and the fuzz harness

**Build.** The typed effect grammar (consultation §9.4). Conditions split into two categories, and keep them split: **derived** predicates (`Surrounded`, `Flanked`, `Isolated`, `Elevated`) recomputed from board state and never stored; **applied** statuses (`Hasted`, `Shocked`, `Staggered`) with durations, living on units. Conflating these is the most common source of forecast desync.

Author the nine signature abilities — one per Specialty — in JSON. Three reactions. The eight anti-loop invariants from consultation §7.3, each as an assertion.

**Gear, minimally — and to the `1 weapon + 4 gear` shape.** Author **two sets and one loose piece, plus one weapon, for the cyborg only** (consultation §10.4, §10.5). Enough to exercise the 2+2 combination rule and the identity-preservation validators (§10.3 rules 13–18). **Do not author gear for the other two protagonists**; the schema either works on one character or it does not.

**Build gear and weapon as separate record types from the first commit**, with **no `setId` field on the weapon record at all** — that is what makes Juan's ruling structurally unbreakable rather than a rule someone must remember. Assert rules #15, #15a and #15b today: exactly four gear slots, exactly one weapon slot, no weapon contributing to a set threshold, and a gear enum that cannot grow to five (R31).

Enumerate the resulting 2+2 pairings and check none is degenerate (R26).

**Fuzz harness:** 10,000 AI-vs-AI simulations asserting every invariant and the bounded-resolution iteration cap.

**Done when:** the §16 encounter is playable end to end in ASCII, and the fuzz run is clean.

---

### End of week 1 — Gate A

| Criterion | Pass | Fail |
|---|---|---|
| Forecast parity | 100 % across ≥ 500 pairs | any divergence |
| Determinism | identical replay on two machines | any non-reproducible replay |
| Anti-loop | 0 violations / 10,000 runs | any violation |
| Turn resolution | ≤ 8 ms | > 16 ms |
| Forecast recompute | ≤ 4 ms | > 16 ms |
| Action Vocabulary Census | ≤ 8 archetypes cover ≥ 90 % of actions | > 12 archetypes needed |
| **Mech footprint 1, 2, 4** | all three pass at 2×2 | any fails → escalate the scale/footprint conflict (R30), do not compress |
| **Encounter at both footprints** | §16 run at 1×1 and 2×2; the difference is characterised | only one footprint was tried |
| **Legibility** | a player who did not design it predicts the conditions a move creates, ≥ 4 of 5 plays | cannot predict, or ignores the badge |

**The legibility row is the one that matters.** Run it with a real person who has not seen the code. If it fails, stop and redesign the forecast presentation before entering week 2 — no amount of Unity work fixes an illegible mechanic, and continuing would spend the engine budget on a thesis known to be broken.

---

## Week 2 — Unity 6 URP. One character.

**Spike character: the luminous synthetic.** She is the cheapest asset in the cast — her body is a shader, the red bomber jacket is her only cloth — while exercising the hardest shading problem in the project. If the pipeline works on her it generalises down; if it fails on her, nothing else will pass.

**Representative assets required:** one blockout synthetic mesh matching her orthographic sheet proportions, one rigged skeleton at LOD1, one toon material set with named material IDs, **the red bomber jacket as a separate submesh on the same skeleton** (consultation §11.8), one small environment slice with three elevation bands.

**Why the jacket is in the spike rather than deferred:** she is the only character with the toggle, she is the spike character anyway, and the garment-layer approach either works structurally or it does not. Discovering that it needs a duplicated character is a week-2 finding, not a month-6 one.

**Temporary assets permitted:** grey-box environment; placeholder enemies as capsules; a single unpolished VFX per ability; untextured mech proxy for scale reference only; programmer UI.

**Not permitted, even temporarily:** a second character model; any cinematic beyond the day-10 test shot; final textures; hair simulation.

---

### Day 6 — Project setup and the sim/presentation boundary

**Build.** Unity 6 pinned to a supported 6000.x, URP, assembly definitions mirroring week 1 exactly — the simulation assemblies are referenced, not rewritten. `Game.Presentation` consumes the resolved event stream and **may not write simulation state**. Animation events consume resolved events; they never decide truth.

**Done when:** the week-1 kernel runs unchanged inside Unity and a headless replay produces identical results in-editor.

---

### Day 7 — Toon shading and outlines (R17)

**Build.** Flat ramp with a hard terminator, one shadow-shape parameter per material, rim light as a separate pass. **Inverted-hull outlines on characters. Screen-space depth/normal edge on the environment. Never the screen-space pass on characters.** Palette swatch file that both the shader and the artists sample.

**Test:** full camera orbit at gameplay distance, 1080p, in motion.

**Pass:** no visible shimmer. **Fail:** shimmer that cannot be removed without disabling outlines — **an engine-reconsideration trigger.**

---

### Day 8 — Analog post stack (R18)

**Build.** Seven independently toggleable Renderer Features in order: line-weight stabilisation (before any blur) → halation masked to emissive IDs → luma/chroma separation → scanline/phosphor → palette-aware grain → gate weave (cinematics only) → optional tape dropout. **UI composites last on a separate overlay camera, entirely outside the stack.**

**Test:** per-pass profiling; a close face shot with the stack at full strength.

**Pass:** total ≤ 2.5 ms, UI pixel-crisp, faces unmuddied, per-pass goldens captured. **Fail:** > 5 ms, or UI inherits the stack.

---

### Day 9 — Stepped cadence at 60 fps (R16) — the engine-binding test

**Build.** A custom Playable / AnimationJob that quantises clip time **per pose layer**: `t_step = floor(t * rate) / rate`, rate 12 for 2s or 8 for 3s. Not a global timescale hack. Not baked 12 fps clips — baking destroys blending and locks the cadence.

**Sampled every frame, never quantised:** root and pelvis transform, IK targets, attach points, camera targets, grid movement interpolation, VFX, UI, projectiles, hit timing.

**Input acknowledgment is always on frame 1** — cursor response, an audio click and a camera micro-move fire immediately; the stepped pose may begin on the next step boundary (≤ 83 ms at 12 Hz) without reading as late. **Hit timing lives in the simulation**; presentation places the impact at an authored millisecond offset. A held pose can never delay damage.

**Instrumentation:** input-to-first-response latency measured in frames; frame-time capture on the named target machine.

**Pass:** input acknowledged within 1 frame; camera and root continuous; hit timing sim-driven; stepped body pose slides smoothly across a camera pan. **Fail:** cadence cannot be decoupled from camera or hit timing without fighting Unity's animation system — **an engine-reconsideration trigger.**

---

### Day 10 — Tactical camera, commit zoom, and the transition

**Build.** Final-intent tactical camera; grid, cover and elevation overlays; the **commit zoom** — a hard switcher-style cut to a closer camera for the action-resolution beat, using the same asset at cinematic LOD, with hair and face LOD switching at the same threshold. One transition from a static B-tier narrative frame into playable combat with no visible asset swap.

**Pass:** protagonists ≥ 140 px tall at default zoom on 1080p; grid and elevation readable; the transition shows no pop.

---

### Day 10b — `cellSizeMeters` visual-scale comparison (Q32)

Added in revision 6. **This is a comparison, not a decision made in advance.**

**Build.** Expose `cellSizeMeters` as a configurable Unity field. Render the §16 map and the spike character at **1.0 m, 1.25 m and 1.5 m** against **identical combat rules and the identical replay log.**

**Validate independently at each scale:** traversal · doors · cover · forced movement · camera framing · **animation clearance** · encounter-space cost.

| Gate | Pass | Fail |
|---|---|---|
| **Rule invariance** | Event streams **bit-identical** across all three scales | Any behavioural difference — the kernel has leaked metres and §9.1 is violated |
| Camera framing | Protagonists ≥ 140 px at default zoom, and mech + pilot readable in one frame, at the chosen scale | Camera must pull back past the 96 px readability floor |
| Animation clearance | A 2×2 mech's attack and stance-change arcs do not intersect adjacent occupied cells | Arcs overlap neighbours, forcing either bigger cells or smaller animations |
| Encounter-space cost | The 14×14 map still reads as a place rather than a corridor | The map must grow, raising art cost on every encounter |

**Report which scale you would ship and why — but do not write it into canon.** It is a presentation parameter and it can change after the vertical slice without touching a rule.

---

### Day 11 — Quality presets and the minimum-quality floor

Added in revision 2, implementing the settled graphics policy (`MINA-CONSULTATION-ADDENDUM-001.md` §5, consultation §13.2).

**Build.** Three URP Assets — Minimum / Standard / Ultra — plus a Renderer Feature enable-mask per tier. **The minimum preset is authored as a tuned target, not produced by turning things off.** Analog passes degrade by whole pass in a fixed priority order: gate weave, tape dropout and grain go first; **line-weight stabilisation and halation-on-emissive go last.** Inverted-hull outlines are never scaled out; their width scales with resolution so the perceptual weight stays constant. The Synthetic's luminous chassis is **explicitly exempt** from VFX-density scaling — it is a shader, not an effect. Cloth and hair degrade to fully skinned, never to absent. Keep a cheap grounding shadow at minimum; a character with no contact shadow floats and the player misjudges elevation.

**Measure on the machine named in Q30 — not on a development workstation.** This is the single most common way a minimum preset silently becomes unshippable.

| Gate | Pass | Fail |
|---|---|---|
| Frame time at minimum | ≤ 16.6 ms, **stable pacing**, 1 % low ≥ 50 fps | frame time exceeded, or visible hitching |
| Silhouette at minimum | gate 2 (128 px / 64 px IoU) passes | fails at minimum though it passes at Ultra |
| Material identity at minimum | gate 3 (ΔE00 ≤ 3) passes; luminous chassis reads | palette drift, or the chassis stops reading |
| Screen-space readability at minimum | gate 6 passes | telegraphs, outlines or forecast text illegible |
| Memory at minimum | ≤ VRAM budget for the named machine | texture thrashing |
| Cold load at minimum | ≤ 6 s | exceeded |
| **Art-direction judgement** | a person looks at minimum and Ultra side by side and calls minimum *simpler*, not *broken* | minimum reads as muddy, generic or abandoned |

**That last row is not automatable and it is the one the policy is actually about.** Run it with a human, side by side, and write down what they said.

---

### Day 12 — Integration, measurement, and the honest write-up

Play the §16 encounter in-engine with the synthetic real and the other two as placeholders. Capture the full budget table from consultation §13 on the named target machine, **at all three presets**. Run the asset-validation gates (§11.9) against the blockout and record every failure — **those failures are the specification for real modelling** (R8). Run **gate 7, jacket-state parity**, and report it explicitly (R28).

Write the results up plainly, including what did not work. A spike that reports only successes has not been run properly. State, for the mech, **which of the seven validations passed and which did not** — a footprint conclusion without that breakdown is the failure R30 describes.

---

### End of week 2 — Gate B

| Criterion | Pass | Fail |
|---|---|---|
| Outline stability | no shimmer at gameplay distance in motion | shimmer requiring outlines off |
| Post stack | ≤ 2.5 ms, UI excluded, faces clean | > 5 ms or UI inside the stack |
| Stepped cadence | input ≤ 1 frame; camera and root continuous | cadence coupled to camera or hit timing |
| Frame time | ≤ 16.6 ms on the named target | > 16.6 ms with only one real character |
| Readability | protagonists ≥ 140 px; grid legible | characters < 96 px at default zoom |
| Determinism in-engine | replays identical to headless | any divergence |
| **Jacket-state parity** *(new)* | hitbox bounds, skeleton pose, VFX anchors and event timings bit-identical between states; only garment pixels differ | any divergence, or a second character prefab exists |
| **Minimum preset floor** *(new)* | all day-11 gates pass, including the human side-by-side judgement | minimum fails a gate, or reads as broken rather than simpler |
| **Mech footprint 5, 7** *(new)* | camera frames mech and pilot together with protagonists ≥ 96 px; player can tell which body acts next and what each threatens | camera must pull back past the readability floor, or players lose track of which body they command |
| **Rule invariance across `cellSizeMeters`** *(new)* | event streams bit-identical at 1.0 / 1.25 / 1.5 m | any behavioural difference between visual scales |
| **Animation clearance** *(new)* | 2×2 mech arcs clear adjacent occupied cells at the chosen scale | arcs intersect neighbours |

---

## What must NOT be built during the spike

Talent trees. **Gear beyond the two cyborg sets, one loose piece and one weapon** — no gear for the other two protagonists, and **no visible gear meshes at all until Q27 is ruled.** More than three enemy families. The mech's full stance system. Any part of the prologue. Any cinematic beyond the day-10 transition. Ink integration. Save/load beyond the replay log. A second character model — **including a second Synthetic prefab for the jacket-hidden state; that is precisely what §11.8 forbids.** Final textures. Hair simulation. UI beyond programmer art, **except the jacket toggle itself, which is one checkbox.** Audio beyond one input click. **The minimum preset as finished art** — define its rules, wire its gates, measure it; tune it after the slice.

Every item on this list is something a spike naturally drifts into and none of them answers a spike question.

---

## Results that would change the architecture but not the engine

- Forecast recompute > 4 ms → fix the resolver's data structures, not the renderer.
- Turn resolution > 8 ms → profile the grid and LOS, likely a caching problem.
- Anti-loop violations → the invariant set is incomplete; add rules before adding content.
- Census needs > 12 archetypes → cut Specialties, not quality. **This is the cut that protects the project**, and it is much cheaper to make now than after the abilities are designed.
- Multi-tile mech occupancy proves expensive but workable → keep 2×2 and budget it as a first-class kernel feature.
- **Any of the seven mech validations fails** → this is **not** licence to compress the mech to 1×1. Report which validations failed and escalate the visual-scale-versus-tactical-footprint conflict to Juan and Mina as a decision (addendum §2, R30).
- The jacket cannot be a garment layer without a duplicated character → escalate; do not ship two Synthetics.
- The minimum preset cannot hold the silhouette or material-ID gates → raise the minimum hardware (Q30) rather than lowering the art floor. The floor is settled policy; the machine is not.

## Results that would justify reconsidering the engine

- Outline shimmer that cannot be removed without disabling outlines (day 7).
- Post stack that cannot get under ~5 ms with only one real character on screen (day 8).
- Stepped cadence that cannot be decoupled from camera and hit timing (day 9).

Only these three. **Performance problems caused by unoptimised blockout assets are not engine problems**, and a spike that concludes "Unity is too slow" from an unoptimised mesh has failed rather than proven anything. If a reconsideration trigger fires, the correct next step is a **three-day** comparison of the same shot in Unreal — not a migration, and not an essay.

## After the spike

Do not begin character modelling until the §16 encounter has been played and judged by someone who did not design it (R5). The spike's job is to make that judgement possible, not to start production.
