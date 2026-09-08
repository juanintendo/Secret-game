# 90s Anime × Tactical RPG — External Consultation 001

Consultant: Claude (Opus 5), acting as senior game systems architect, technical art director, animation-pipeline consultant, and adversarial production reviewer.
Date: 2026-09-08
Requested by: Juan + Mina
Sources read in full: `STORY-CANON-001.md`, `90S-ANIME-XCOM-PREPRODUCTION-001.md`, `90S-ANIME-XCOM-CODEX-CLAUDE-BRIEF-001.md`, plus the approved art in `docs/canon/art/`.

**Status of this document: recommendations, not canon.** Nothing here is binding until Juan or Mina accepts it. Where I propose a rule, it is labelled. Where I report a conflict, it is cited.

---

## 1. Executive Verdict

**The product thesis is coherent. The production plan attached to it is not yet.**

The thesis — three fixed, individually lethal women whose progression is *unit synchronization* rather than personal competence, expressed through a legible tactical ruleset and a 1990s OVA visual language — is internally consistent, mechanically expressible, and genuinely differentiated. The conditional-support idea (support abilities that are *unlocked by battlefield state the player deliberately constructs*) is the strongest single design idea in the three documents. It is the thing a player would describe to a friend without naming FFT, XCOM, or Persona. It is worth protecting at the cost of almost everything else.

**The single greatest risk is authored animation volume, and it is not close.**

Nine Specialty trees plus three Personal trees, at a quality bar defined as "faces, proportions, hair masses, costume topology, signature materials, palette, mech silhouette and synthetic anatomy may not drift shot to shot" (`CODEX-BRIEF` §2.1), with a mech that has three stances, a pilot who can operate remotely, and a cast whose costumes include layered long hair and — in the current hero art — garter straps and heeled thigh boots, is a AA-to-AAA animation bill. Every other risk in this project is recoverable. This one compounds silently: you discover it after the trees are designed, after some are animated, and the fix is a progression redesign, which is the most expensive thing to redo because it invalidates content, UI, balance, and tutorialization simultaneously.

The fix is structural and cheap to adopt *now*, and it is the central recommendation of this document:

> **An ability costs animation. A talent must not.** Every ability maps to one of ~8 approved **motion archetypes** per character; abilities are `archetype + timing variant + VFX set + data row`. Talents change fields, never clips. Enforce it in the content validator.

This is also, not incidentally, how 1990s television anime was actually produced — a small library of signature actions, re-timed and re-cut. Adopting it makes the production model and the art model the *same* model, which is exactly the kind of alignment this project keeps asking for.

**The second risk is under-discussed in the documents and I want it on the record:** with only three protagonists, **tactical variance must come from the opposition and the map, not from the roster.** In XCOM, depth comes from squad composition and permadeath pressure; in FFT, from unit count and the job system. You have neither. Enemy design is therefore your primary content axis — and across three documents totalling ~90KB, enemy families appear once, in a list of open decisions (`CODEX-BRIEF` §19). That is an inverted priority.

**The single most important thing to prototype first is the forecast, not the combat.**

Not the shader. Not the trees. Not even the rules. The entire design rests on the premise that a player will *intentionally construct* a condition — walk the cyborg into a position where she becomes surrounded, in order to enable the synthetic's rescue. If the interface cannot show, *before commitment*, that this move produces `SURROUNDED 3` and that this enables `Rescue Pulse`, then Spell Slinger is a lottery, "buffs feel like intelligent answers to battlefield state" (`CODEX-BRIEF` §9.3) is false, and the differentiating idea collapses into an ordinary buff button with an annoying gate.

That is testable in **text, in about a week, with zero art**, in a headless prototype with an ASCII forecast panel. It is the cheapest high-information experiment available to this project and it should precede the Unity spike or run beside it.

---

## 2. Preserved Canon

Treated as binding, not re-litigated:

**Product and pillars**
- Premium story-driven tactical RPG. Three fixed protagonists. No appointed leader in the definitive trio.
- Each woman is already individually capable. **The unit is level one, not the characters.**
- Two progression axes: individual power and collective synchronization.
- The synchronization resource fills from genuine cooperation, not ordinary damage.
- Combat legible before commitment; animation celebrates a resolved decision and never conceals rules.
- 60 fps responsive input, cursor, camera, UI, path preview, turn transitions, and simulation. Stepped/held character acting permitted on 2s/3s.
- Characters must look as close as technically practical in combat to how they look in cinematics; shared character truth (proportions, face construction, hair silhouette, costume/material IDs, signature VFX colours), scaled by LOD rather than by unrelated models.
- Clean high-definition 1990s OVA language. Analog artifacts are authored punctuation, never a global VHS filter, never obscuring silhouettes, UI, or tactical information.
- Approved character art is canonical reference. No generated frame becomes canon because it looks impressive once. No uncontrolled text-to-video pipeline redefines canonical characters frame by frame.
- Narrative decisions alter tactical state; tactical outcomes alter narrative state. Stateful episodes, not an exponential branch tree. Ink as the authoring layer.
- Deterministic simulation, seeded and logged randomness, inspectable content, gameplay logic never dependent on Timeline.

**Combat and progression**
- Compact grid with discrete elevation; interleaved initiative; two AP per activation; movement / primary / utility / guard-overwatch / character ability; facing, flank, LOS, elevation, contextual cover; reactions, assists, rescues, interrupts with explicit triggers; environmental interaction; objectives beyond elimination.
- Three Specialties per character, each an interpretation of that character's central verb — not nine unrelated classes. One Personal tree per character as connective tissue.
- Healing, protection, haste, and major buffs are **earned by satisfying mechanical conditions**, not dispensed by a permanent healer. The synthetic is not a healer.
- Gear expands builds without erasing identity: few named components with visible consequences and drawbacks; no random affix soup; no equipment granting another protagonist's unique verb.
- Do not implement nine trees before the central combat thesis is proven.

**Characters** (as specified in `canon-guard` §7; not restated here in full)
- White-haired human — Agility — Exploit/Redirect — Dispatch, Con Girl, Gunslinger. Only fully human member of the definitive trio; recruited for a concealed specific reason; must never become a cosmetic recreation of the missing leader.
- Cyborg mech pilot — Strength — Break/Anchor — Bulwark, Remote Arsenal, Redline. The only protagonist with a mech; the mech is repaired and incrementally upgraded, never replaced; it magnifies her without erasing her on the field.
- Luminous synthetic — Intellect — Rewrite/Conduct — Heartless, Ghost Surgery, Spell Slinger. Fully synthetic; the luminous skeleton is literal design, not an effect; specific trauma around agency, internal systems, and remote access.
- Original green-haired leader: fully human, AI copilot advises, she retains final field authority, defined by judgment and by accepting the cost of overriding the machine-optimal answer. Survives in some form; becomes the unknown protective intruder. Operator is a separate entity, apparently killed at HQ, restored, now a hologram.

**Story spine**
- Prologue at near-peak power; prepared intrusion exploits the second synchronized Limit Break; HQ and field site are distinct locations struck as one coordinated act of payback; the leader's human override creates the escape path; no body; underground years; individualized recontacts; the recruit joins later.
- The eight mystery questions share causal machinery and must not resolve as unrelated twists.
- The definitive trio only succeeds when all three stop trying to recreate the missing woman.

**Not reopened:** the fixed trio, the absence of a leader, the prologue's dramatic shape, the synthetic's non-healer identity, the mech being unique to the cyborg, Ink, or the anti-slop gate.

---

## 3. Contradictions and Ambiguities

Distinguishing real conflicts from intentionally open questions.

### 3.1 Real contradictions

**C1 — The white-haired human's costume is described three incompatible ways, and the current art is the most expensive one.**
- `CODEX-BRIEF` §3.2 and `STORY-CANON` §3.1: "clean black tactical outfit **without fur, feathers, or production-hostile trim**."
- `PREPRODUCTION` §3 Reference 01: "black reflective dress, **feathered or particulate outer texture**."
- `docs/canon/art/HERO-definitive-trio-001.png`: a black glamour bodysuit/dress with **garter straps**, an **open off-shoulder jacket**, and **heeled thigh-high boots**.

These are three different garments. The art configuration is, by the brief's own definition, production-hostile trim: garter straps are thin self-intersecting geometry that reads as noise below ~128 px; a separate open jacket is two more bone chains; heeled boots change foot IK, ground contact, and every locomotion cycle, and they are the worst possible footwear for a character who traverses discrete elevation on a grid. She is also your **most animated character** — Dispatch is high-mobility melee, Gunslinger is "escalating payoff from relocating between shots" (`CODEX-BRIEF` §7.3). This is the most expensive unresolved item in the project.

There is also a soft motion contradiction: heeled thigh boots against "extreme agility, improvisation, and fluid close-quarters movement with Spike Spiegel-like looseness" (`PREPRODUCTION` §3). Anime does this constantly and it is not fatal — but it is not free either, and it should be a decision rather than an accident.

**C2 — The synthetic's red bomber jacket is canon in text and absent in art.**
`STORY-CANON` §2.3 and §3.2 of the brief make the original leader's short fitted red bomber jacket definitive-timeline canon for her — it is a significant story object, the inherited garment. `HERO-definitive-trio-001.png` shows the definitive trio (the white-haired recruit is present, so this cannot be prologue-era) with the synthetic **not wearing it**.

This is worth resolving quickly because the answer is unusually consequential and unusually good: her orthographic sheet shows that her body *is* her costume — lacquer panels and luminous chassis, no cloth. If the jacket is the only fabric she wears, she becomes the cheapest character in the cast to produce and the most spectacular to light. That is a production gift; take it deliberately rather than by accident.

**C3 — The cyborg's arms: symmetric forearms in text, asymmetric in art.**
`STORY-CANON` §2.2 says "mechanical **forearms**/hands"; `CODEX-BRIEF` §3.1 says "mechanical arms." The hero art shows an apparently **full right arm plated to the shoulder** and a **left mechanical hand/forearm** — asymmetric. This is not cosmetic. `CODEX-BRIEF` §8.4 calls for "mechanical-arm counters and emergency actions when separated from the mech." One heavy signature arm gives you a characterful, memorable, single-clip counter. Two symmetric arms give you a mirrored system, twice the animation, and less identity.

**C4 — `Integrity` is simultaneously an attribute and a resource.**
`CODEX-BRIEF` §5 lists **Integrity** in the primary stat family ("health/structural durability"). §10 uses **Integrity/Health** as the damage pool. One name, two objects, in the same document. This will produce sustained confusion in the schema, the UI, the balance spreadsheet, and in conversation. *This is the concrete terminology problem I am permitted to flag.* Recommendation: attribute = `Frame`, pool = `Integrity`.

**C5 — Con Girl runs on an attribute that does not exist.**
`CODEX-BRIEF` §7.2 and `STORY-CANON` §3.1 both describe Con Girl as "**Charisma**-driven." The stat family in `CODEX-BRIEF` §5 is Strength / Agility / Intellect / Resolve / Integrity. No Charisma. Either it is a sixth attribute — which adds a whole balance axis and gear dimension for the benefit of one of nine Specialties — or Con Girl scales off Agility/Resolve and "Charisma" is fiction. This blocks the ability schema's `scalingAttribute` enum and it blocks gear design. It is cheap to decide and expensive to leave open.

**C6 — The prologue is simultaneously "first major playable production target" and a violation of the project's own smallest-first doctrine.**
`PREPRODUCTION` §13 states the prologue "becomes the first major playable production target." `CODEX-BRIEF` §20 states, in bold effect, that the immediate objective is *one small combat scenario* and warns "do not begin by implementing three full talent trees."

These pull in opposite directions, and the prologue is the more expensive path by a wide margin. Building it first requires:
- a **fourth** fully canonical character (the green-haired leader) with her own model sheet, kit, and AI-copilot UI;
- her **removable external white/gray armor** — a costume *state change*, which is a second silhouette, a second set of materials, and a transition;
- Operator's **physically present body** at HQ (a fifth canonical character, since the definitive-era hologram is a different asset);
- prologue-only enemies, a prologue-only map, and the HQ interior;
- a **working Limit Break system**, twice, one of which must fail dramatically;
- and a scripted collapse sequence in which "the player retains limited control" (`PREPRODUCTION` §13) — the hardest kind of content to build and tune.

That is the most expensive possible first deliverable, and it demonstrates the *old* team, not the game you are shipping. Recommendation: build the main-timeline synergy scenario first; treat the prologue as vertical-slice content, not as first content.

**C7 — "Deterministic base damage" and "hit uncertainty" are asserted together without resolution.**
`PREPRODUCTION` §4 and `CODEX-BRIEF` §4 both say "deterministic base damage with clearly surfaced hit or mitigation uncertainty," while §4 of the brief also requires "seeded, replayable simulation," implying live randomness. Is there a to-hit roll or not? This is ambiguity rather than contradiction, but it must be resolved **before the kernel is written**, because it determines the forecast UI, the AI's evaluation function, the balance model, and whether the seeded PRNG is load-bearing or decorative. My recommendation is in §5.

### 3.2 Gaps — things the documents assume but never specify

**G1 — The two-survivor chapter has no combat design.**
`PREPRODUCTION` §6.2, §13 and `CODEX-BRIEF` §16 Gate 5 all require a main-timeline chapter played by the cyborg and the synthetic **alone**, before the recruit joins. Every combat proposal in all three documents assumes three units. Two-unit encounters are a materially different balance problem: no Con Girl means nobody creates `Isolated`, nobody lures enemies into position, and the synthetic's conditional windows have exactly one possible target. The Gate 2 synergy chain — which is the game's thesis — is structurally impossible with two characters. This needs a deliberate answer, not a difficulty tuning pass.

**G2 — Enemy design is absent.** Discussed in §1 and §4 (R2). With three protagonists, this is the main content axis and it currently appears only as an open decision.

**G3 — "No leader" versus the player's existence.** The fiction insists nobody commands; the interface makes the player an omniscient commander who chooses everyone's turn. The documents never address this. It is not a bug — it is the best available *opportunity*: make **initiative exchange, ceded turns, and handoffs** the literal mechanical expression of distributed authority. The trio passes authority around; the player arbitrates. Recommend stating that as intent so the UI and the ability set can serve it.

**G4 — No named final approver.** `PREPRODUCTION` §16 states "Juan and Mina share creative direction; Mina owns technical direction." Shared authority is fine for direction and a deadlock risk at *approval gates*, which is where a strict anti-slop pipeline generates the most friction. Recommend a single named final approver per asset class.

### 3.3 Intentionally open — correctly left unresolved, not touched here

The leader's surviving form and embodiment; the attacker's identity and objective; why the recruit is uniquely required; whether restored Operator is continuous, forked, or compromised; HQ's nature; the underground-years duration; working title and character names; final Specialty and verb names; platform scope.

---

## 4. Risk Register

Ranked by cost of discovering late, which is the number that should drive sequencing.

| # | Risk | Prob. | Impact | Cost of learning late | Cheapest early test | Decision deadline |
|---|---|---|---|---|---|---|
| **R1** | Authored animation volume across 9 Specialties + 3 Personal trees exceeds capacity at the canonical quality bar | High | Fatal to scope | **Very high** — discovered after trees are designed and partly animated; the fix is a progression redesign that invalidates content, UI, balance, and tutorialization at once | **Action Vocabulary Census**: take the §14 scenario plus one complete Specialty (Redline), enumerate every distinct authored character clip incl. transitions and hit reacts, then multiply by 12 trees. 1 day, a spreadsheet | Before any tree is authored past representative examples |
| **R2** | Three units generate insufficient tactical variance; enemy design (the actual variance source) is unspecified | Med-High | High — the core loop is dull | High — discovered in playtest after maps and abilities exist | Headless: same 3 kits vs. 3 different enemy compositions; count distinct viable plans a human finds in 5 minutes. Target ≥3 per composition | End of Gate 1 |
| **R3** | Conditional windows are not forecastable; Spell Slinger becomes accidental rather than constructed | Medium | High — kills the differentiating idea | Med-High — UI rework plus ability redesign | Text/ASCII forecast panel in the headless prototype; 3 external testers try to *deliberately* create a `Surrounded` window | Gate 2 |
| **R4** | Prologue-first plan requires 2 extra canonical characters + a costume state change + a Limit Break system before anything is proven | High (already written into the docs) | High — months of schedule | High | None needed — count the assets. It is a planning decision, not an unknown | **Now** |
| **R5** | Team capacity: two named people against a premium tactical RPG with anime cinematics, branching narrative, and 12 trees | High | Existential | Very high | Honest scope math against R1's census output | Before any external commitment |
| **R6** | Toon shader + analog post + 3 protagonists + mech + VFX cannot hold hero-image quality at 60 fps at gameplay camera distance | Medium | High | Medium — recoverable, but re-authoring art to a lower bar is demoralizing and slow | Two-week spike, Day 6 stress scene on named low- and high-bar hardware | End of spike (this is the engine gate) |
| **R7** | Costume complexity (garter straps, open jacket, heeled boots, long layered hair) blows the deformation/cloth budget and dies at gameplay scale | Med-High | Med-High | Medium | Build **one** character to final quality — the cyborg's hair is the hardest element; count bones, cloth, and cost. Then run the §9.6 screen-space readability test | Before costume lock (see C1/C2/C3) |
| **R8** | Degenerate action economy: AP refunds + initiative theft + haste + reactions compound into loops or a single dominant character | Med-High (near-certain if unguarded) | Medium | Medium | Fuzz test: random-legal-action agent, 10k encounters, assert bounded termination and that no invariant fires | Gate 1 |
| **R9** | Remote Arsenal is either a free second unit (broken) or a tax (unfun) | Medium | Medium | Medium | Headless A/B of all three command models on one encounter; measure turn length, damage+mitigation share, and win-rate delta | Gate 2 |
| **R10** | Generative-tool cleanup cost for canonical animation exceeds labour saved | High **if attempted on characters** | Medium (wasted weeks) | Medium | One 4-second shot; measure cleanup hours against authored hours honestly | Before any generative work is scheduled against canonical frames |
| **R11** | Stepped animation reads as judder or feels like latency against a 60 fps moving camera | Medium | Med-High (pillar violation) | Medium — but if content was authored on 2s, everything is re-timed | Day 4 of the spike: three cadence configs, measured input→response latency | Day 5 of the spike |
| **R12** | Ink ↔ tactical state coupling produces save-migration pain | Medium | Medium | Medium-High (late saves are hard to fix) | Gate 4 bridge with a deliberate schema version bump and a migration test | Before the slice |
| **R13** | The two-survivor chapter (G1) is unbalanceable because the thesis needs three units | Medium | Medium | Medium | Run the §14 scenario with the human removed; see what breaks | Before the slice's mission list is fixed |
| **R14** | Terminology collisions (C4, C5) cause schema churn | High | Low | Low | Decide them. 10 minutes | Before the ability schema is written |

**Reading of the register:** R1, R4, and R5 are the same risk viewed from three angles — the project's ambition currently exceeds its stated production model. None of them requires an experiment to discover. They require an afternoon of counting. Do that afternoon first.

---

## 5. Minimal Combat Proposal

*All of this is a recommendation.* The goal is the **smallest deterministic ruleset that can prove three-unit tactical depth** — small enough to build headless in two weeks, complete enough that a failure means the thesis is wrong rather than that the prototype was under-built.

### 5.1 Grid and positioning subset

- **Square grid. 4-directional movement, cost 1.** FFT precedent. Diagonals create ambiguity in LOS, cover, knockback direction, and — critically — in counting "surrounded," which is the game's signature condition. Do not pay that cost for a marginal movement benefit.
- **4 facings.** Flank = attacker in the target's side or rear arc.
- **Integer elevation, three bands (0/1/2).** One `Vertical` stat covers climb and jump.
- **Cover on tile edges**, half or full — not derived from "adjacent to a wall." Edge cover is deterministic, authorable, and drawable.
- **LOS:** tile-center to tile-center with elevation blocking. Height grants extended LOS over half cover plus a flat damage or guard-break bonus (not accuracy, see below).
- **Adjacency-8 for spatial-pressure concepts only** — `Surrounded`, knockback direction snapping. Movement stays 4-way. Document the asymmetry; it is intentional and it reads correctly to players.
- **Map size 12×12 to 16×16.** Small enough to reason about; large enough that a 3-tile knockback changes the situation.

### 5.2 Initiative model — tick clock with hard invariants

A shared tick clock. Each unit has `nextActAt`; on acting, `nextActAt += max(MIN_INTERVAL, BASE − Speed)`.

Why a tick clock rather than round-based speed ordering: the design explicitly wants haste, delay, ceded initiative, and initiative exchange as *tactical objects* (`CODEX-BRIEF` §9.1, §11). A tick clock makes those first-class and forecastable. It also produces a **turn-order rail** — a visible queue of the next 8 activations — which is both a required UI element and a perfect fit for the broadcast-switcher interface direction (`PREPRODUCTION` §8): the rail *is* a channel queue.

The honest cost: tick clocks are harder to forecast and harder to balance than round order. The mitigation is mandatory, not optional — **the rail must be visible at rest, and hovering any ability must show a ghost rail of the reordering it would cause.**

**Anti-loop invariants (all four are load-bearing):**

- **A — Monotone clock.** No effect may set a unit's `nextActAt` earlier than `now`. Haste reduces the *next* interval; it never rewinds the clock. This single rule eliminates the classic haste-act-haste-act loop.
- **B — Haste is boolean + duration, never a stacking magnitude.** A flat reduction to the next interval, once. Forecastable and unexploitable.
- **C — Extra activations are budgeted.** Any effect granting an off-turn activation (AP refunds, initiative exchange, assists) draws from a per-unit, per-round budget of **1**, tracked in the sim and shown in the UI.
- **D — `MIN_INTERVAL > 0`.** Every activation advances that unit's clock. This bounds any loop by construction.

### 5.3 AP economy

- **2 AP per activation.** Move = 1, Primary = 1, Utility = 1, Guard/Overwatch = 1 (ends activation), Ability = 1 or 2.
- **Move + Move is legal, but the second Move costs your Guard**: you end with `Exposed` until your next activation. This preserves the mobility fantasy for Dispatch and Gunslinger, prices it honestly, and — the important part — *emits a condition other characters can exploit*, feeding the tag grammar instead of just being a dash button.
- **Reactions do not cost AP.** Each unit has 1 reaction charge, refreshed at its own activation. Keeping reactions out of the AP economy stops them from competing with the main turn and bounds reaction chains structurally.

### 5.4 Reactions — exactly three in v0

1. **Guard / Overwatch** — universal, declared, fires once.
2. **Intercept** — Bulwark; when an adjacent ally would be hit, move up to 1 and take it. The cyborg's signature and the game's main Guard engine.
3. **Reaction Shot** — Gunslinger; an enemy in LOS enters a band, fire once.

One implementation: `ReactionTrigger { event, condition, response }`.

**Anti-loop:** reaction depth 0 — a reaction can never trigger a reaction. At most 1 reaction per unit per event; at most 2 reactions total per event.

### 5.5 Forced movement

- `Displace(target, direction, distance)`; direction snaps to the 8 compass headings.
- **Mass classes:** `Light` (full distance) / `Standard` (−1) / `Heavy` (−2) / `Anchored` (immune, but see §9 boss degradation).
- **Collision:** into an occupied tile → displaced unit stops one short, **both** take collision damage scaled by remaining distance. Into a wall or blocked tile → stop + impact damage. Off a ledge with drop ≥2 → fall damage + `Prone`. Uphill by ≥2 → blocked.
- **Depth 0** — a displaced unit colliding with another does **not** push it. This is the rule that keeps displacement deterministic, replayable, and — most importantly — *drawable in the forecast*. Chain-pushes look cool once and destroy forecastability forever.

### 5.6 Conditions — keep two categories separate

This distinction matters more than it looks:

**Statuses** live on units, have durations, are visible on the unit:
`Exposed` · `Prone` · `Staggered` (loses reaction and guard) · `Pinned` (movement 0) · `Shocked` · `Hacked` · `Marked` · `Hasted` · `Slowed` · `Guarded`

**Event tags** are emitted by resolved events and live for a **one-activation window**:
`Expose` · `Displace` · `Conductive` · `Guarded` · `Hacked` · `Isolated` · `Intercepted` · `Rescued`

Merging them is the most likely early implementation mistake. The window lifetime on tags is precisely what makes a handoff *feel* like a handoff — a fleeting opening someone has to take — rather than a buff sitting on a unit. `CODEX-BRIEF` §11 already names the right tags; this adds the lifetime rule that makes them work.

Conditions carry a `cause` field (`EnemyAdvance` | `AllyAction` | `SelfMove`) — needed for the exploit rules in §9.

### 5.7 Damage forecasting

**Recommendation: no to-hit roll for the protagonists.** Damage is a computed integer, displayed exactly, before commitment.

Reasoning: with three units and a design pillar that says "information required for a decision is legible before commitment" (`CODEX-BRIEF` §2.4), percentage-to-hit is actively hostile. XCOM's dice work because you field six soldiers and losses are the drama; here, a missed 85% on one of three protagonists is not drama, it is noise, and it poisons the forecast that the whole conditional system depends on. Deterministic damage makes this a *puzzle-forward* tactics game, which is what a three-unit game should be.

Relocate uncertainty to things the player can work on:
- **enemy intent** — every enemy publishes what it will do and which tiles it threatens, before its activation;
- **hidden information** — unscanned enemies, unrevealed reinforcements;
- **reaction state** — whether an enemy still holds its reaction charge.

Keep the seeded PRNG for AI tie-breaks and flavour only, so replays stay exact.

**The forecast must display, pre-commitment:** exact damage · resulting Guard and Integrity · full displacement path with stop tiles and collision markers · the resulting turn-order rail · and **which conditional windows this action opens or closes**. That last line is this game's signature UI element and it does not exist in any comparable game.

### 5.8 Recovery

See §8 in full. Summary: `Guard` (renewable, never passive) → `Integrity` (rare in-mission restoration) → `Faults` (persistent typed modifiers). Downed, not dead. A `Cornered` threshold turns losing into a comeback opportunity.

### 5.9 Synchronization generation

**Sync is granted only when a unit consumes an event tag emitted by a different unit.** Damage grants zero Sync. A resolved chain with three distinct contributors grants a bonus tier.

This is a one-line rule that exactly encodes `CODEX-BRIEF` §11's intent, is trivially testable, and cannot be farmed by grinding damage.

### 5.10 Anti-loop rules, collected

Assert every one of these in tests; a violation is a **test failure**, not a silent clamp.

1. Monotone clock (§5.2 A).
2. `MIN_INTERVAL > 0` floor per activation.
3. Haste is boolean + duration.
4. ≤1 extra activation per unit per round.
5. Reaction depth 0; ≤1 per unit per event; ≤2 per event.
6. ≤1 refunded AP per activation, and refunded AP may not pay for the ability that refunded it (`NoRefundChain` tag).
7. Forced-movement depth 0.
8. `MaxEffectApplicationsPerStack = 8`.
9. Global resolution-stack depth cap with a logged error.

**Required test:** a random-legal-action fuzz agent plays 10,000 encounters; assert bounded termination and zero invariant violations. This costs about two days and it is the difference between a system you can add talents to and one you cannot.

---

## 6. Character Progression Proposal

### 6.0 The structure that makes twelve trees producible

*Recommendation.* Three linked rules:

**(1) An ability costs animation. A talent must not.** A talent may change numbers, targeting shape, cost, tags, triggers, conditions, and VFX *parameters*. It may never require a new character animation clip. If it needs one, it is an ability and is charged against the ability budget. Enforce in the content validator.

**(2) Motion archetypes.** Each character has ~8 approved archetypes. Every ability is `archetype + timing variant + VFX set + data row`. For the human: *Dash-Strike, Pivot-Shot, Vault, Feint/Taunt, Finisher, Reload-Flourish, Hit-React, Evade*. New abilities cost VFX and timing, not a keyframe session.

**(3) Focus, not filling.** A character picks one Specialty as **Focus**: it unlocks that tree's signature and capstone and grants an extra talent tier. Non-capstone talents from the other two trees cost double points. Hybrids exist; identity does not dissolve.

Proposed caps until the Action Vocabulary Census (R1) produces real numbers: **8 motion archetypes, 22 abilities (6 per Specialty + 4 Personal), ~48 talent nodes, 5 named gear items across the whole game.**

**Respec:** talents are **free** to respec between missions — experimentation costs nothing and produces better players. **Focus** changes cost a scarce resource or a narrative beat, because Focus changes the ability set the player has learned to read.

---

### 6.1 White-haired human — Agility — *Exploit / Redirect*

**Coherence of the three Specialties: weakest of the three, and fixable.**
Dispatch and Gunslinger are both "damage + mobility," differing mainly in range. Con Girl is a different kind of thing — control at a DPS cost. Read plainly, the set is *melee / control / ranged*: a class triangle, not "three interpretations of one verb" (`CODEX-BRIEF` §6).

**Recommendation:** make `CODEX-BRIEF` §7.4's "leverage/observation system shared across her three Specialties" into a literal, first-class resource — **Leverage**. She gains it from observing, flanking, first strikes, and enemy mistakes. Each Specialty *spends* it differently:
- **Dispatch** → convert to a Finisher or a bounded AP refund;
- **Con Girl** → convert to control strength and duration;
- **Gunslinger** → convert to reaction shots and initiative theft.

Now all three are one character doing one thing: acquiring an edge and cashing it. It also gives you a single limiter for the balance failure below.

**Central verb, mechanically:** she is the only character who can **create and consume `Isolated` and `Exposed` at will.**

**Personal tree function:** the Leverage economy plus escape. It also carries the arc — `CODEX-BRIEF` §7.4's "selfish actions that can be upgraded into handoffs as trust grows" should be literal talent upgrades that convert a self-only effect into a tag emission. That is the character arc expressed as a talent path, which is exactly what this project keeps promising to do.

**Cross-tree access:** naturally coherent through Leverage. Cap outside-Focus spending at roughly a third of points.

**Gear interaction:** her signature weapon slot determines her Primary archetype (blade / pistol brace / wire). Gear should force commitment — a blade loadout should not also be the best ranged loadout.

**Animation/content cost: highest in the cast.** Most acrobatic, most locomotion states, and — under the current hero art — the most complex costume. Mitigated by archetypes; **not** mitigated if C1 resolves toward the glamour dress.

**Likely balance failure: action-economy compounding.** Dispatch offers "AP or initiative refunds tied to clean execution," Gunslinger offers "quick-draw initiative contests," Con Girl offers "causing enemies to waste overwatch." Those are three separate action-economy multipliers on one character. Unchecked she becomes the only protagonist worth playing. Leverage as a single shared currency plus anti-loop rule 6 is the containment.

*Representative talents (to show the schema pattern, not a tree):*
- `Read the Room` — passive: gain 1 Leverage the first time each enemy activates within LOS. *(Patch: none. Adds a trigger.)*
- `Cut and Run` — patch `dispatch.finisher`: on kill, `+1 AP` once per activation, tagged `NoRefundChain`. *(Field patch only, no new clip.)*
- `Somebody Else's Problem` — patch `congirl.lure`: `emits.eventTags += ["Isolated"]`. *(One field. Converts a selfish taunt into a handoff — the arc, in one line of data.)*

---

### 6.2 Cyborg mech pilot — Strength — *Break / Anchor*

**Coherence: the strongest trio of the three.** Bulwark, Remote Arsenal, and Redline are all answers to "where does force get applied, and through which body." Nothing to fix.

**Central verb, mechanically:** she is the only character who can **change tile ownership** — body-block, create cover, destroy cover, and compel enemies to interact with her.

**A real architectural finding: Remote Arsenal and Redline are structurally incompatible, not merely different.** Remote Arsenal splits pilot and mech into two positions; Redline requires her *inside* the mech doing stance work; Bulwark works either way. This is not a talent-level difference. It requires a first-class simulation state:

> `MechMode ∈ { Piloted, Remote, Docked }`, with an AP cost to change, and every ability tagged with its required mode.

Get that into the kernel early. Retrofitting a mode enum after abilities exist is unpleasant.

**Personal tree function:** the pilot/mech bond as an actual stat — link integrity and command range, read by all three Specialties — plus the repair economy and the answer to "what can she do without the mech."

**Gear interaction:** mech hardpoints. Constraint from canon: the mech is *repaired and incrementally upgraded*, never replaced, so hardpoints must be visible upgrades to a recognizable silhouette, drawn from an approved variant set.

**Animation/content cost: highest total in the project.** A full human set *and* a full mech set, plus mount/dismount/remote transitions, plus three Redline stances.

> **Recommendation, and it saves months:** Redline's stances must **share one locomotion set**, differentiated by an additive upper-body layer, ability availability, stat profile, and heat/vent VFX. Three distinct base locomotion sets for one character's three stances is where the animation budget dies. `CODEX-BRIEF` §8.3's own anti-pattern warning ("three colored passive buffs toggled every turn") is about the design; this is the production version of the same warning.

**Likely balance failure — two of them.** First, Remote Arsenal is either a free second unit or a tax (see §7). Second, and less obvious: **Bulwark's interception is the team's main Guard engine, so if interception is too cheap, the team never takes Integrity damage at all** and the entire "recovery is earned" model collapses into "always intercept." Intercept must cost her something real — her reaction charge, position, and exposure — and enemies must have `Piercing` and `Area` answers to it.

*Representative talents:*
- `Weight of the Machine` — patch all `cyb.*` abilities: `massClass = Anchored` while in `Piloted` mode. *(One field, enormous tactical consequence.)*
- `Vent to Spite` — patch `redline.disengage`: `effects += GrantGuard(target: allies within 1)`. *(Turns a selfish recovery into a team act; emits `Guarded`.)*
- `Dead Man's Grip` — new trigger on `Intercepted`: gain 1 Sync. *(No clip; reuses the interception animation.)*

---

### 6.3 Luminous synthetic — Intellect — *Rewrite / Conduct*

**Coherence: good, and it becomes excellent with one addition.** Heartless and Ghost Surgery already share "invade a system." Spell Slinger is ally-facing and reads as a different discipline. `CODEX-BRIEF` §9.4 names the unifier — "conductivity and system-access grammar shared across all three Specialties" — but leaves it abstract.

**Recommendation: make it literal. A resource called `Conduction`, expressed as `Link` objects placed on units, machines, and terrain.**
- **Heartless** spends Links for damage cascades and chaining.
- **Ghost Surgery** spends Links for control and subsystem disabling.
- **Spell Slinger** requires a Link to reach an ally and to power the buff.

All three become one verb: *establish links, then rewrite what flows through them.* And critically, **Links are visible objects on the map**, which is a large part of the answer to "how do players forecast conditional abilities" (§9). Links must decay, or she becomes a passive accumulator.

**Central verb, mechanically:** she is the only character who can create `Hacked` and `Conductive`, and the only one who can directly manipulate the initiative rail.

**Personal tree function:** chassis exposure as a real risk/reward toggle (more conduction, more vulnerability — this is where `CODEX-BRIEF` §19's open "synthetic vulnerability/cost for Heartless" gets its answer); intrusion resistance, which is her trauma made mechanical; and the inherited-judgment conditions from the original leader — expressed as *condition unlocks*, never as a leadership aura.

**Cross-tree access:** naturally strong via Links. She is the character where hybrid builds should feel best.

**Animation/content cost: lowest in the cast, by a wide margin.** Her body is her costume; her spectacle is emissive shader parameters and 2D overlays, not skinned geometry. **Recommendation: spend the VFX budget disproportionately here.** She is where you buy the most visual impact per hour, and she is the character most likely to appear in a trailer.

**Likely balance failure — three:**
1. **Spell Slinger + the highest-damage ally is degenerate** if haste stacks or the window is farmable. See §9.5.
2. **Ghost Surgery against bosses** is the classic control problem: either it trivializes them or it is dead weight. See §5 / §9.7.
3. **Link accumulation** turns her into a setup-then-delete engine if Links persist. They must decay on a short clock.

*Representative talents:*
- `Standing Current` — patch `heartless.arc`: chain targets `+1` when the path crosses a `Conductive` tile. *(Field patch, existing VFX.)*
- `Second Opinion` — patch `spellslinger.rescue-pulse`: `requires.conditions` accepts `cond.intercepted` as an alternative trigger. *(Adds a second door to the same room — a new decision, no new clip.)*
- `She Would Have Waited` — passive: if you decline to act on the first legal activation of an encounter, your next conditional ability costs 1 AP. *(The leader's judgment as a mechanic: patience as a resource. No animation at all.)*

---

## 7. Remote Mech Recommendation

`CODEX-BRIEF` §8.2 correctly refuses to choose by fantasy. Here is the comparison and a recommendation.

| | **(a) Shared pilot AP** | **(b) Queued directives** | **(c) Separate initiative + bandwidth** |
|---|---|---|---|
| Action economy | Trivially safe | Capped — a directive costs AP now, pays later | **Effectively a fourth unit** |
| Fantasy delivered | Weak — the mech is a summon that eats her turn | Strong — genuine split presence | Strongest |
| Sim complexity | Lowest | Medium (directive object + behavior) | High (extra clock entry, extra UI, extra AI) |
| Forecast difficulty | Low | **Medium-high** — must show what the directive will do | High |
| Can the mech tank on its own timeline? | No | Yes, within its directive | Yes, fully |
| Thematic fit | Neutral | **Excellent** — delegation, remote agency, and predictability are this story's themes | Good |
| Balance risk | Remote is strictly worse than piloting; nobody picks it | Directives too dumb = punishing; too smart = free unit | Remote becomes the objectively correct Specialty |

**Recommendation: prototype (b), queued directives, first.**

It is the only model that preserves both the fantasy and the action-economy invariant, and it is thematically load-bearing in a way the other two are not. This is a story about a woman whose agency was hijacked through a remote connection, working with a synthetic who is traumatized by exactly the same thing. A mech that executes *your prior instructions* rather than your live intent — that can be predicted, exploited, and cut off — is the story in mechanical form. (a) says nothing. (c) says "you have four units."

**Directive set to prototype (recommendation):** `Hold` · `Advance and Block <tile>` · `Suppress <tile>` · `Escort <ally>`. Four is enough to test whether directives are legible.

**Implementation note that makes this cheap:** (b) and (c) differ by one thing — whether the mech has its own clock entry. Build `MechMode` + a `Directive` object + an *optional* clock entry behind a config flag, then A/B all three models in the headless kernel on the same encounter. That converts an argument into an experiment for about a day of extra work.

**Explicit pass/fail for the A/B:**
- Turn resolution wall-clock per round does not exceed the Piloted baseline by more than 40% (if it does, the model is fiddly).
- The cyborg's combined damage + mitigation share stays within 15% of her Piloted baseline (outside that, it is a free unit or a tax).
- Testers find ≥2 distinct viable plans per encounter under the model.
- **Fail condition:** Remote wins ≥15% more often than Bulwark or Redline on the same encounter.

**Failure cases to watch for in (b) specifically:** directives that are ignored because the battlefield moved (feels like the mech is broken); directives whose forecast cannot be drawn (violates the legibility pillar); and the "set and forget" trap where one directive is correct in every encounter — which would mean the directive set is too coarse, not that the model is wrong.

---

## 8. Recovery Without a Healer

*Recommendation.* Three layers with sharply different economies.

**1. `Guard` — renewable, never passive.**
The absorbing layer. Depleted before Integrity. Regenerates **only** from mechanical acts:
- ending an activation in cover — small;
- the Guard action — moderate, costs your activation;
- a Bulwark **interception** — grants Guard to the *protected ally*, not the interceptor;
- a Redline **Disengage** — vents Guard to self at the cost of pressure and damage;
- the synthetic's **Rescue Pulse** — grants Guard as part of the conditional package;
- completing a **handoff or rescue chain** — a team-wide stabilization threshold.

No passive regeneration, ever. Guard is the primary answer to `CODEX-BRIEF` §10's "recovery emerges from mechanical success."

**2. `Integrity` — the consequential pool.**
Reduced only when Guard is exhausted, or by **`Piercing`** attacks that bypass Guard entirely. Piercing is the main enemy-design lever against turtling and should be common enough that "always hold Guard" is not a strategy.

In-mission Integrity restoration is **rare and always manufactured**: a scarce repair/stim resource that is earned (a completed three-character chain, a mission objective) or placed on the map in limited quantity. Never a spell.

**3. `Faults` — persistent, typed, characterful.**
Gained when Integrity crosses thresholds (e.g. 50% and 0). A Fault is **not a number** — it is a typed modifier that changes how the character plays:
- `Servo Lag` — −1 Move, reactions cost 2
- `Optic Damage` — −1 range band
- `Intrusion Scar` — −Resolve against `Hacked` *(and it is the synthetic's trauma made mechanical)*
- `Hairline Fracture` (mech) — Redline stance changes cost +1 AP

Faults survive the mission. They are cleared between missions with a scarce resource **or a narrative choice**, which is exactly the narrative↔tactical write-back the design keeps asking for (`PREPRODUCTION` §2.2, §6).

**Downed, not dead.**
At Integrity 0: `Prone`, no actions, an immediate Fault, and a bleed/cascade timer. A teammate can **Rescue** — move plus 1 AP, adjacent — restoring 1 Integrity and a small Guard, clearing the timer, keeping the Fault, emitting the `Rescued` tag, and granting Sync. Timer expiry produces a **mission-level fail-forward consequence** (forced extraction, capture, a narrative cost), never permadeath for the fixed trio. *(If Juan and Mina want permadeath on the table, say so — it changes this entire section and the narrative state store. See §15 Q10.)*

**The anti-death-spiral device: `Cornered`.**
When team aggregate Integrity drops below ~40%, a team state activates: Sync generation increases (×1.5) and one specific class of recovery ability becomes available. This is the anime third act as a rule. It converts losing into a **comeback opportunity** rather than a slow slide, without granting free healing, and it makes the tensest moment of an encounter the most mechanically interesting one.

**Why this satisfies the brief:** healing exists but its supply is manufactured by play; the synthetic gives Guard, haste, and shock — never Integrity — so she is structurally incapable of becoming a heal-bot; the cyborg's interception is the main engine, which makes protection the support role, exactly as `CODEX-BRIEF` §8.1 intends; and Faults keep damage consequential across missions.

**The balance knife-edge:** Guard must be *cheap to gain and expensive to hold*, or turtling wins. That balance is enforced almost entirely by enemy design — `Piercing`, `GuardBreak`, and `Area` profiles. Which is the third time in this document that enemy design turns out to be doing the real work. Please staff it.

---

## 9. Conditional Synthetic Support Prototype

Full analysis of the canonical example. This is the ability the whole thesis rests on.

### 9.1 Activation rule

```
Target: one ally, not self.
Requires: cond.surrounded { min: 2, radius: 1, adjacency: 8 } on target
Requires: LOS to target, range ≤ 6
        (a talent may substitute an existing Link for LOS)
Cost:    2 AP — her entire activation
Cooldown: 2 of her own activations
```

Adjacency-8 for "surrounded" even though movement is 4-way: surrounding is a spatial-pressure concept and 8 reads correctly to a player looking at the board. Document the asymmetry.

**Multi-tile enemies:** a 2×2 unit counts as **1** hostile for `Surrounded`. Otherwise a single boss standing next to an ally trivially satisfies the window, and the ability's whole meaning — *you are outnumbered and closing in* — evaporates. A large unit instead grants a separate `PinnedByMass` condition that opens the same window. Decide this explicitly; it is the kind of edge case that silently breaks a signature system.

### 9.2 Player forecast — the part that must be built first

Four layers, all pre-commitment. This is the actual first prototype (§1).

1. **Persistent condition badges.** Every ally permanently displays which conditional windows they currently satisfy: `SURROUNDED 2/2 ✓`. Not hidden, not on hover, not in a submenu.
2. **Predictive badges during planning.** While previewing *any* action by *any* character — including a plain move — affected units show *projected* badges: `SURROUNDED 1/2 → 3/2`. **This is the mechanism by which players intentionally construct conditions.** Without it, the design's central verb is unavailable to the player.
3. **A Windows panel.** A persistent list of every currently-enabled conditional ability across the team, lit or greyed, with the reason: *"Rescue Pulse — enabled: Cyborg is Surrounded(3). Range OK. 2 AP."* And when greyed: *"needs 2 hostiles adjacent to an ally."*
4. **Full effect forecast on target hover.** Per-enemy knockback path with stop tiles and collision markers; the before/after turn-order rail; exact Guard and haste values; shock duration; and clear marking of which enemies are mass-reduced or immune.

**Test for the prototype:** three testers, no instruction. If fewer than two of them *deliberately* set up a Pulse on their second attempt, the forecast has failed and the ability must be redesigned — not the tutorial.

### 9.3 Cost and timing

- **2 AP — her whole activation.** The Pulse must cost her tempo. This is what makes it "accepting a cost for another character" (`CODEX-BRIEF` §11) and what stops Pulse-plus-Heartless in one turn.
- **Cast on her own activation in v0. Not a reaction.** Reaction-casting is tempting and dangerous; if it is ever added, it should be a capstone that fires once per encounter and consumes her next activation entirely.
- **Resolution order is fixed and documented:**
  1. Re-validate the condition at resolution.
  2. Emit `Displace`; resolve knockbacks **simultaneously in a deterministic order** — by distance, then by clockwise index from the ally — so collisions are replayable.
  3. Apply collision and fall damage.
  4. Apply `Guarded` (Guard points) to the ally.
  5. Apply `Hasted` to the ally.
  6. Apply `Shock Coating`.
  7. Emit `Rescued` on the ally, `Displace` on the affected tiles, and Sync.

### 9.4 Initiative consequences

`Hasted` reduces the ally's **next** interval by a flat amount, once. It never rewinds the clock (Invariant A). **Duration: one activation, not "3 turns."** A single stolen tempo beat is more legible, more dramatic, and far less exploitable than a stacking window. The rail must visibly reorder the moment the ability resolves — that visual is the payoff.

### 9.5 Knockback and collision

- **Base distance 3**, reduced by mass: Light 3 · Standard 2 · Heavy 1 · Anchored 0.
- Direction: radially away from the **ally**, snapped to 8 compass headings.
- Stops on: wall, blocked tile, occupied tile (both units take collision damage), ledge with drop ≥2 (fall damage + `Prone`), uphill ≥2 (blocked).
- **Depth 0** — a displaced unit that collides does not push the unit it hits. Non-negotiable for forecastability.

### 9.6 Boss and large-enemy behaviour

**The governing principle, applicable to the whole game: control degrades in kind against bosses; it never becomes null.**

Against `Anchored` targets the Pulse does not displace. Instead it applies `Staggered` + `Unbalanced` (loses its reaction; takes +25% forced-movement effect for 1 turn). **The ally-facing half — Guard, haste, shock coating — always fires at full strength.** So the ability is never a dead button, and the boss fight becomes a different puzzle rather than an off-limits one.

Make this a schema requirement: every control or displacement ability carries a `bossDegradation` field, and CI fails without it (see `content-schema` §7).

### 9.7 Duration

Guard: until spent. `Hasted`: 1 activation. `Shock Coating`: 2 of the ally's activations **or** 3 attacks, whichever comes first. Short durations are correct here — they keep the board readable and they keep the ability a *moment* rather than a maintenance task.

### 9.8 Synergy opportunities

- **The Gate 2 chain, exactly as written:** Con Girl lures enemies into surrounding the cyborg → the window opens → Pulse → the cyborg converts the opening. Three contributors, one Sync tier. The documents already have this right.
- **The tank's job becomes the enabler.** Bulwark *wants* to be surrounded — threat control is her function — so the Pulse rewards the cyborg for doing exactly what her Specialty says she should. That alignment is rare and worth protecting.
- **Cross-Specialty inside one character:** `Shock Coating` makes the ally apply `Shocked`, which feeds Heartless's chaining. Spell Slinger sets up Heartless. That is what "three interpretations of one verb" should feel like.
- **Displacement into hazards, off ledges, or into the Gunslinger's reaction line.**
- **Displacement creates `Isolated`,** which is Dispatch's food.

### 9.9 Exploit prevention

| Exploit | Prevention |
|---|---|
| **Surround farm** — push them away, they walk back, push again | 2 AP + 2-activation cooldown, plus a `Recently Rescued` status on the ally (2 turns) that blocks re-targeting |
| **Self-inflicted surround** — deliberately walk into enemies to farm Guard, haste, and Sync | The `Surrounded` condition carries a `cause`. The Pulse is legal in all cases, but **Sync is denied when `cause = SelfMove`**, and more importantly: **Guard and Sync scale with *threat*, not adjacency** — with the number of surrounding enemies that actually have an available action against the ally this turn. Standing next to two `Staggered` or `Hacked` enemies pays nothing. This is the precise mechanical expression of "reward intentionally creating or accepting tactical pressure, not accidental button availability" |
| **Haste chain** — hasted ally acts sooner, builds Sync, Pulse again | Invariant A + boolean haste + `Recently Rescued` |
| **Ledge instakill** — free kills by pushing enemies off everything | Cap fall damage. Lethal pits exist only as **rare, marked, authored** map features; enemies near them are `Anchored` or ledge-aware. A pit is a designed moment, not an ambient exploit |
| **Reaction-cast Pulse** (future talent) | If ever added: once per encounter, and it consumes her next activation |

### 9.10 Animation and VFX requirements

**Synthetic (1 archetype, reused by every Spell Slinger ability):** a two-pose stepped cast — anticipation held ~4 frames, release held ~2 — plus a chassis flare that is an **emissive shader parameter animation, not new geometry**, plus one 2D impact-frame insert on release.

**Ally (must work on all three protagonists *and* the mech):** one **additive** brace/blown-back layer plus a rise-out-of-crouch. One additive clip per rig, not one per ability. This is the difference between an affordable rescue system and an unaffordable one.

**Enemies:** **one shared knockback set per mass class** — launch, air, land, get-up — reused by every displacement source in the entire game. Three sets total. Authoring per-enemy knockback animation is the kind of decision that quietly costs a year.

**VFX:** a ground-projected radial force ring (**below units in draw order**, so the tiles it clears stay readable); persistent tile highlights on the projected stop tiles for ~0.4 s **so the player can verify the forecast matched the result** — this small touch is what teaches players to trust the forecast; an emissive haste sigil on the ally; a shock-coating shader parameter on the ally's weapon.

**Legibility budget:** total VFX ≤ ~0.9 s, never occluding the grid, and **the camera does not cut away.** A Tier-C cut-in portrait may play simultaneously in the corner.

**Timing contract:** the simulation resolves the entire thing instantly and emits an event list with authored millisecond offsets. Presentation plays it over ~1.2 s and is fully skippable. Never the reverse.

---

## 10. Canonical Visual Pipeline

### 10.1 Is shared canonical 3D plus selective authored 2D the right strategy? — Yes, unambiguously

For this team size, this quality bar, and this pillar set, it is not a close call. A fixed cast of three (plus Operator and the leader) is the ideal case for shared canonical 3D: the per-character investment is amortized across gameplay, Tier A cinematics, Tier C cut-ins, previs, and marketing. The alternative — separate 2D gameplay sprites and 2D cinematic art — guarantees the drift the brief forbids and multiplies every costume revision by the number of pipelines.

The one addition I would make to `CODEX-BRIEF` §13.2: the shared asset's value is only realized if the gameplay camera ever gets close enough to see it. See §10.6.

### 10.2 Character source package — the brief's list, plus five additions

`CODEX-BRIEF` §13.1 is good. Add:

1. **A turnaround at the gameplay camera angle (~45° down).** Orthographic sheets lie about how a design reads from the tactical camera. This is where you discover that a great front view is an unreadable blob from above.
2. **Silhouette-only plates at 128 px and 64 px height.** The gate that catches "beautiful in the viewport, mush in the game."
3. **A costume topology map that names each garment's rig strategy** — skinned / bone-chain / cloth / card / shader / 2D overlay. This single document is what prevents the C1-class budget surprise.
4. **A motion archetype sheet** — the ~8 signature actions. This is simultaneously an art document and the content budget (§6.0).
5. **Locked signature VFX colour IDs** — 2–3 emissive colours per character, treated as material IDs.

Plus a **swatch strip** exported from the palette that both the 2D artists and the toon shader read from, so the automated palette test has something to assert against.

### 10.3 Unity pipeline (canonical models → validation)

Detailed in the `unity-pipeline` skill. The load-bearing points:

- **LOD0 (cinematic) and LOD1 (gameplay) share mesh topology, UVs, material IDs, and skeleton root hierarchy.** They differ only by subdivision, hair bone count, face rig complexity, texture resolution, and shadow-shape resolution. **Never by design.** If they diverge structurally, drift is guaranteed and no amount of review will catch it.
- **Content is JSON; ScriptableObjects are generated mirrors.** Hand-authored definition SOs will drift from source and win silently.
- **Timeline is for cinematics only.** Combat "cinematic" beats are Cinemachine blends driven by the event stream — interruptible and skippable, because the simulation already resolved.
- **Character outlines: inverted hull.** Environment edges: screen-space depth/normal. Do not apply the screen-space pass to characters; it shimmers at gameplay distance and violates the no-shimmer pillar.
- **Post passes are separate Renderer Features**, individually toggleable, each with a golden-image test. UI renders on a separate overlay camera, excluded from the stack, always.
- Post order: line-weight stabilization (**before any blur**) → halation/bloom masked to emissive IDs → luma/chroma separation → scanline/phosphor → palette-aware grain → gate weave (cinematics only) → tape dropout as punctuation.

### 10.4 Stepped anime acting at responsive 60 fps — the actual technique

This is the question with the most misinformation around it, so here is the precise answer.

The common failure is implementing "on 2s" as *sampling the whole character at 30 Hz*, which judders against a moving camera and feels like input lag. Instead:

**Split the rig.**
- **Sampled every frame, continuously:** root/pelvis transform, anything that tracks the world (weapon muzzle, IK targets, attach points, camera targets), and grid movement interpolation.
- **Sampled stepped:** pose layers only — spine-up, limbs, face — by quantizing the clip's time cursor: `t_step = floor(t * rate) / rate`, with `rate = 12` for 2s or `8` for 3s.

**Implement as a custom Playable / AnimationJob that quantizes clip time per layer.** Not a global timescale hack. **Not baked 12 fps clips** — baking destroys blending and permanently locks the cadence, so you can never tune it.

**Never quantize:** camera, UI, VFX simulation, root motion, hit timing, projectiles, grid movement. A stepped body pose sliding continuously across a pan is exactly what good cel animation looks like; a stepped *root* is what jitter looks like.

**Input acknowledgment is always on frame 1.** Fluid cursor response, an audio click, and a camera micro-move fire immediately. The character's stepped pose may begin on the next step boundary — ≤83 ms at 12 Hz — without reading as late, because the player already received feedback. This is the whole trick, and it is why the brief's pillar ("low animation cadence must never become control latency") is achievable rather than contradictory.

**Hit timing lives in the simulation.** The sim resolved already; presentation places the impact at an authored millisecond offset. A held pose can never delay damage.

Impact frames and smears deliberately break cadence — per-clip override must be supported from the start.

### 10.5 Hair and clothing assignment

| Element | Strategy | Note |
|---|---|---|
| All primary silhouette masses | **Skinned geometry** | The silhouette is never simulated |
| Synthetic's bob | Skinned helmet shape, 3–5 tip bones | Cheap |
| Cyborg's long layered blonde hair | **3–4 authored bone chains × 4 bones (~16 bones), spring solver, hard angular limits** | **Not cloth sim.** Hardest hair in the cast — prototype it first (spike Day 5) |
| Human's tousled short hair | Fully skinned + 2 accent bones | Cheap |
| Jacket tails, open jacket, long straps | Bone chains + spring, 2 chains max per garment | |
| Belt pouches | Skinned | Never simulated |
| Synthetic's red bomber jacket | Controlled cloth, **cinematic LOD only** | Never cloth in gameplay LOD |
| Hair wisps and flyaways | **Alpha cards** attached to the mass | Reads as ink line, near-free |
| Synthetic's luminous chassis | **Shader** — emissive + fresnel + scrolling iridescence keyed to material IDs | **Her costume is a shader.** The cast's cheapest character |
| Lacquer specular, shock coating, mech heat/vent | Shader parameters | Free per-ability variation |
| Eye highlights, eye cuts, impact frames, speed lines | **2D overlays** | The 90s feeling lives here |
| Anime hair highlight band | Authored band texture in hair UV space | More art-directable than a specular model |

**Forbidden in gameplay LOD:** garter straps and other thin self-intersecting geometry; unsupported free cloth; individual-strand hair; heeled boots with separate ankle geometry that breaks foot IK on stairs and elevation. *(Directly implicates C1. This is not an aesthetic objection — it is a deformation and readability objection.)*

### 10.6 Making combat characters look like their cinematic versions

Beyond the shared asset, one specific and cheap lever:

- **Keep protagonists at ≥140 px tall at default zoom** on a 1080p screen. Below ~96 px, asset quality is invisible and you are paying for nothing.
- **Add a commit zoom.** A hard, switcher-style cut to a closer camera for the action-resolution beat, using the same asset at cinematic LOD, then cut back. Hair and face LOD switch at the same threshold.

This is the cheapest possible way to make combat look like the cinematics — you already have the asset; you simply need to look at it. And a hard cut to a close channel is the *exact* language of the broadcast-control UI direction (`PREPRODUCTION` §8), so the mechanism and the art direction reinforce each other instead of competing.

### 10.7 Generative tools — where they pay

**Permitted (net labour saved):** static background and matte-style backdrops for Tier B (single frames, no temporal problem, overpaintable) · storyboard and thumbnail exploration · disposable animatics for timing · non-hero prop and material variation with human cleanup · UI and typography *exploration*, never final text · enemy silhouette ideation · supervised upscaling of your own authored art · **code and tooling generation** (validators, importers, test harnesses, the screenshot-regression rig) — which is far and away the highest-value use for this team.

**Not permitted (cleanup exceeds labour saved):** any canonical character frame or animation · faces and hands at any scale · S-tier in-betweens (interpolation smears the line and mutates costume detail; supervised human in-betweening is cheaper than reviewing every frame) · anything that becomes a material or palette ID.

**The number:** budget roughly **1.5–3× authored cost** for temporally-consistent character video cleanup at this quality bar. That is not a reason to never try it; it is a reason to never *schedule* it as a saving.

**Entry rule to write into the pipeline document:** generated pixels may enter only as (a) a static background, (b) a reference/underlay that is overpainted, or (c) a non-hero texture — each with a provenance record. Nothing generated ever becomes a canonical character asset, and nothing generated is ever the source of a material ID.

### 10.8 Automated gates — production tests, run on every content or shader change

1. **Golden renders** — 6 fixed cameras per character (front / 3q / profile / back / gameplay-45° / cinematic-close) × 2 lighting rigs. Perceptual diff; a difference opens a review, never an auto-fail.
2. **Silhouette overlay** — alpha-only at 128 px and 64 px vs. the approved plate; assert IoU ≥ threshold.
3. **Palette / material-ID sampler** — fixed sample points; assert ΔE00 ≤ 3 against the swatch file.
4. **Proportion test** — head-height ratio, shoulder width, total height from the golden render vs. the proportion chart.
5. **Animation continuity scan** — step every frame of every clip; assert bone-length invariance, no hair/cloth proxy self-intersection, no frame where a tracked attach point leaves its envelope. *(This is the automated version of the brief's frame-stepping review and it catches drift no human will.)*
6. **Screen-space readability** — render the gameplay-45° camera at 1080p, downscale the character to real on-screen size, assert silhouette IoU still passes.
7. **UI regression** — screenshot each screen at min/max resolution; assert text is rendered by the text system (no rasterized text); contrast check.
8. **Post-stack per-pass goldens** — a shader change must not silently muddy faces.
9. **Colourblind simulation** on tactical overlays (protanopia/deuteranopia) with a legibility assertion. The UI direction is cyan/green/magenta/red — that is a red-green problem. Shape and pattern coding from day one. **This is not a cuttable feature.**

**Human gates, never automated:** final art-direction approval; "is this the same person"; cadence and feel. Automated tests are warning systems, never approval.

### 10.9 Cinematic tier budget for an indie slice

*Estimates.*

| Tier | Slice budget | Reality |
|---|---|---|
| **S** | 1 shot, 5–10 s | True hand-animated OVA quality is roughly **300–600 skilled hours**, or a 4–8 week funded outsource, for ~10 finished seconds. **Recommendation: do not attempt a true S for the vertical slice.** Deliver an exceptional hybrid — 3D layout, toon render, 2D impact frames, heavy compositing — that reads as S. `PREPRODUCTION` §9 already permits "exceptionally polished hybrid sequence." Save true 2D for the shipping opening, with a real budget. |
| **A** | 2 shots, 20–30 s total | In-engine, Cinemachine + Timeline, LOD0 rigs. **The pipeline is the cost, not the shot** — once built, additional A shots are comparatively cheap, which is the argument for building it early. |
| **B** | 3–4 minutes | Layered art, constrained motion, camera, light, particles, typography. **This is where the 90s feeling gets delivered per dollar. Spend here.** |
| **C** | 12–20 cut-in lines | Portraits + 3–5 expressions each. Cheap, enormously effective, and it is what makes the trio feel like people during combat. **Do more of these than feels necessary.** |

Rough slice ratio, S : A : B : C ≈ **1 : 25 : 200 : many** seconds. B and C carry the game's identity; A carries the promise; S is marketing.

---

## 11. Technical Architecture

The module list in `CODEX-BRIEF` §15 is sound. Four changes.

### 11.1 Add `Game.Forecast` as a separate module — the most important recommendation here

The forecast must be computed by **the same resolver that produces the result**: run the real resolver on a cloned state with `DryRun = true` and return the complete predicted delta.

- The **UI**, the **AI**, and the **tests** all consume `Game.Forecast`. None may reimplement damage, displacement, or condition math.
- If forecast logic ever lives in the UI, you will ship a game whose preview lies — and in a game where the player's core verb is *constructing conditions from the preview*, that is not a polish bug, it is a broken product.
- This constraint should drive the entire core design: **the resolver must be pure and the state must be cheaply clonable.** Everything else follows from that.
- Required test: over a large sampled action space, `forecast(s, a) == apply(s, a)`.

A second benefit: the AI now literally evaluates using the same surfaced rules available to the player, which `CODEX-BRIEF` §15 asks for but does not provide a mechanism for.

### 11.2 The engine-free boundary is the real architecture

`Game.Core`, `Game.Tactics`, `Game.Abilities`, `Game.Forecast`, and the AI's evaluation path must compile with **no `UnityEngine` reference** — a plain .NET class library that runs headless. This is what makes the engine decision reversible: if the Unity spike fails and you move to Unreal, the simulation survives untouched and you re-implement presentation only.

Enforce with asmdef reference lists **and** a test that scans for forbidden type usage. Asmdefs alone are too easy to widen in a hurry.

### 11.3 Data ownership

- `Game.Content` — immutable definitions. JSON is the source of truth; ScriptableObjects are *generated mirrors*, never hand-authored.
- `Game.Core` — owns `BattleState`: serializable, clonable, hashable, no Unity types.
- `Game.Tactics` — **the only writer to `BattleState`.**
- `Game.Progression` — produces an immutable `LoadoutSnapshot` at battle start; never mutates during battle.
- `Game.Presentation` — a pure consumer of `IReadOnlyList<BattleEvent>`; **may never read `BattleState`.**
- `Game.Narrative` — may only issue typed `TacticalCommand`s and read `StoryFacts`.

**Determinism rules:** no floats in the sim (int or Q16.16 fixed-point) · no ambient randomness (one seeded PRNG whose state lives in `BattleState`) · no hash-map iteration-order dependence · no wall-clock or frame count · every event appended to an ordered log · **a state hash after every event** so replay divergence is caught at the exact diverging event.

**Replay as a first-class test artifact:** every playtest writes `{seed, initialState, commandList}`; a CI test replays all of them and asserts final state hashes. Roughly two days of work; it pays for the entire project. A bug report becomes a seed and a command list.

### 11.4 Animation events consume resolved events, never decide

- The sim resolves an action **instantly** and emits an ordered event list with **authored millisecond offsets**.
- Presentation plays that list over wall-clock time and may **accelerate or skip safely at any point**. Skipping must be a no-op on truth.
- Animation events may only *request* a VFX or audio cue. They never gate, decide, or confirm anything.
- **Test:** run every fixture twice — once with presentation disabled, once at 10× speed — and assert identical final state hashes.

### 11.5 Content schema

Full schema in the `content-schema` skill. The shape:

```jsonc
Ability {
  id, version, owner, specialty, tags[],
  cost      { ap, charges, cooldown, resource },
  requires  { mode?, conditions[] },
  targeting { kind, range, shape, losRequired, validTargets[], facingRule },
  effects[],                    // ordered, typed
  bossDegradation { vsAnchored: Effect[] },   // REQUIRED for control/displacement
  emits     { eventTags[], sync { amount, requiresCrossCharacter, deniedWhenCause[] } },
  reaction? { trigger, window, limit },
  presentation { motionArchetype, vfxSet, cameraHint, durationMs, cutInId },
  ai        { intent, scoreHints[] },
  upgrades[]
}
Talent { id, requires{focus,tier,points}, patches[{target, op, path, value, onlyIf?}], addsEffects[], addsTriggers[] }
```

**Effect types — the entire typed grammar:** `Damage` · `ApplyStatus` · `RemoveStatus` · `Displace` · `Move` · `ModifyInitiative` · `GrantGuard` · `SpendResource` · `EmitTag` · `ConsumeTag` · `Link` · `Conditional{if,then,else}`.

If a designer needs an effect type not on this list, that is a **kernel change** — escalate it; do not write a bespoke script.

**Which effects deserve bespoke code (and only these four):**
1. the **Conduction/Link substrate** — a graph with decay and propagation; a data structure, not an effect;
2. **MechMode + Directive execution** — a small behavior system;
3. **Limit Break choreography** — set pieces: Timeline plus an explicit scripted sim macro that emits a documented event list;
4. the **forecast resolver** itself.

Everything else is data. For numbers, use a **tiny sandboxed expression DSL** (`"8 + intellect / 2"`) rather than either magic constants or per-talent C#. **Per-talent scripts are banned** — that is the road to `CODEX-BRIEF` §6.1's "opaque script spaghetti."

Validator rules that keep the system honest are listed in `content-schema` §7. The two that matter most: **a talent may never introduce a motion archetype**, and **every control ability must declare `bossDegradation`.**

---

## 12. Two-Week Spike

Scope note: this spike is a **technical-art, camera, and cadence** spike. The headless combat kernel is a *parallel, engine-independent track* and must not be absorbed into it — otherwise an engine failure takes the rules work down with it.

**Day 0 (before the clock starts).** Pin Unity 6000.x LTS. **Name the target hardware** — a low bar and a high bar, e.g. an integrated-graphics or GTX 1650-class laptop and a 4060-class desktop. Produce the swatch/colour-model file and one character's orthographic sheet. *(You already have the synthetic's.)* Without named hardware, every pass/fail below is unfalsifiable.

**Days 1–2 — Character asset round trip.**
Bring **one** character fully into the engine. Recommend the **synthetic**: her costume is her body (no cloth, no complex garments) while her luminous chassis is the hardest shader problem in the cast — so you test the hardest shader on the easiest costume.
Deliverable: rigged, toon-shaded, in a fixed-camera golden-render scene under neutral light.
**Pass:** silhouette overlay ≥95% area match against the sheet at 128 px; palette sampling within ΔE00 ≤ 3 on flats and shadows.
**Fail →** stop. The model or shader approach is wrong and nothing downstream is worth building.

**Day 3 — Gameplay camera readability.**
16×16 graybox, three elevation bands, half and full edge cover, final-intent camera (angle, FOV, distance).
**Pass:** from a still 1080p screenshot, three naive viewers each correctly identify which tiles are cover, which are elevated, and which character is which, in under 5 seconds.
**Fail →** camera and scale are wrong. Fix now; every art decision downstream depends on it.

**Day 4 — Stepped cadence versus responsiveness.**
Three builds: (a) all on-1s; (b) **body on-2s with continuous root, camera, and UI**; (c) on-2s including root. Measure input→visible-response latency with high-speed capture or an on-screen frame counter.
**Pass:** config (b) holds ≤2 frames (≈33 ms) of acknowledgment latency, shows no root-motion stutter at 60 fps, and reads as cel animation to Juan.
**Fail →** fall back to on-1s with *held poses* (hold the pose, sample the transform every frame). Decide by Day 5; do not carry this uncertainty into content.

**Day 5 — Hair and one complex garment.**
The **cyborg's long layered blonde hair** — the hardest hair in the cast — as a bone chain with authored constraints. Add a garment if time permits.
**Pass:** ≤24 hair bones; no interpenetration across a 3-second turn-and-run cycle; silhouette holds from four camera angles; <0.6 ms CPU on the low-bar target.
**Fail →** reduce to fewer, larger card-driven masses. Better to learn this before three characters are groomed.

**Day 6 — Stress scene and budgets. This is the binding engine gate.**
3 protagonists + mech + 6 enemies + tactical UI + 2 simultaneous VFX + full post stack, on both named targets. Capture CPU/GPU frame time, draw calls, skinned meshes, bones, overdraw, texture residency, animation and AI cost, and sim turn-resolution time.
**Pass:** stable 60 fps with ≥30% headroom on the high-bar target; ≥40 fps on the low-bar target (then explicitly decide whether the low bar is supported or dropped).
**Fail →** this is the signal to compare against Unreal (see Day 10).

**Day 7 — Analog post stack.**
Each pass as a separate, independently toggleable Renderer Feature; UI on a separate overlay camera excluded from post.
**Pass:** every pass toggles independently; faces stay clean at gameplay distance; UI text is pixel-crisp; a golden-image test exists per pass.
**Fail →** you built a preset, not a stack. Rebuild it now, because tuning it later across finished content is misery.

**Day 8 — A-tier close shot.**
Same asset at LOD0 with the face rig; one ~10-second in-engine cinematic (Cinemachine + Timeline).
**Pass:** frame-stepped review shows no identity drift, no costume mutation, no limb-length change — **and the same character in the Day 6 stress scene is recognizably the same person.** That comparison is the entire point of the shared-asset strategy; make it explicitly.

**Day 9 — B-tier to combat transition.**
One animated-graphic-novel beat (layered art, camera, typography) hard-cutting into the playable graybox map.
**Pass:** no loading seam >1 s, no visual reset, and it reads as one production rather than two glued together.

**Day 10 — Evidence package and decision.**
Assemble screenshots, capture data, latency measurements, golden renders, and a written go/no-go.
**Pass:** Days 1–9 all passed → **Unity becomes binding.**
**Fail on Day 1, 6, or 8** → rebuild *only those three tests* in Unreal for a focused 3-day comparison before committing. Do not rebuild the whole spike, and do not rebuild game systems.

**Explicit spike non-goals:** no talent trees · no Ink · no save system · no enemy AI beyond a scripted dummy · no audio design · no gear · no narrative content. If the spike grows any of these, it stops being a decision instrument.

---

## 13. Vertical-Slice Cuts

Deliberately not built yet. Each with the reason, because these will all be re-argued later.

1. **The prologue.** *(C6, R4.)* Two extra canonical characters, a costume state change, Operator's physical body, an HQ interior, a working Limit Break system used twice, and a scripted collapse with retained player control. It is the most expensive content in the project and it demonstrates the *old* team. Build it second.
2. **Six of the nine Specialties.** Ship **Con Girl + Bulwark + Spell Slinger** — the exact three that produce the Gate 2 chain and therefore prove the thesis. Add **one** Heartless ability and **one** Dispatch finisher as unlocked demo abilities so the slice does not feel starved of spectacle. *(Named tradeoff: the slice will be light on raw damage fantasy. Accept it; the differentiator is the chain, not the damage.)*
3. **Remote Arsenal and Redline's third stance.** Ship `Piloted` mode only. Prove the directive model in the headless kernel, where it costs days rather than months.
4. **Gear and the loot economy.** Fixed loadouts. Gear proves nothing about the thesis and costs UI, balance, art, and validation.
5. **Respec systems.** Fixed builds in the slice.
6. **Ink beyond the minimum.** One pre-mission choice, one mid-mission consequence, one post-mission callback. **No save migration yet** — but write the schema version field now, because retrofitting it is painful.
7. **Destruction as a system.** One authored destructible cover type.
8. **Fog of war and stealth.** Not core to the thesis; large AI and UI cost.
9. **Supporting recruits.** Already deferred in `PREPRODUCTION` §2.1 — keep it that way.
10. **A true hand-animated S-tier sequence.** Deliver an exceptional hybrid instead (§10.9). The brief permits it.
11. **Multiple enemy families.** Ship **3 archetypes + 1 mini-boss**, deliberately chosen to exercise `GuardBreak`, `Piercing`, and `Anchored` — the three things that make the recovery model and the boss-degradation rule testable.
12. **Full voice acting.** Temp voice for C-tier cut-ins; they need *something* to land, but not a cast.
13. **Console and platform work.** PC-first, as already decided.

**Do not cut:** the forecast UI (it *is* the game); the turn-order rail; deterministic replay and the state hash; colourblind-safe tactical overlays; and the golden-render gates. Each of these is far cheaper to build now than to retrofit, and the first two are the product.

---

## 14. Recommended Immediate Scenario

One compact encounter that exercises the entire thesis. Build it headless first, then in engine.

### "Relay Yard" — 14×14, three elevation bands

**Layout.** A sunken loading yard (elev 0) centre-left. A raised catwalk (elev 2) along the north edge. A mid-level dock platform (elev 1) east, with a 2-tile drop to the yard. Shipping containers providing edge cover. One **power terminal** on the catwalk which, when activated, energizes a 3×3 puddle in the yard into `Conductive` terrain.

**Objective — not elimination.** Extract the data core from the yard terminal and reach the east dock within 8 rounds, while a Warden holds the yard.

**Opposition.**
- 4 × **Sweeper** — Light mass, `Piercing` melee, no reaction.
- 2 × **Lancer** — Standard mass, ranged, holds Overwatch.
- 1 × **Warden** — mini-boss, Heavy/`Anchored`, occupies 2×2, `GuardBreak` slam, hackable subsystems `Optics` and `Slam Actuator`, `Firewall` pool.

Those three profiles are chosen to make `Piercing`, `GuardBreak`, and `Anchored` all matter, which is what makes the recovery model and the boss-degradation rule testable in a single encounter.

### The intended line (one of several — see the pass criteria)

1. **Human — Agility.** From the catwalk (elevation, LOS), `Con Girl: Bad Read` lures two Sweepers toward the cyborg in the yard. Side effect: a Lancer loses its flank cover and becomes `Isolated`.
   → *Tests: Agility, LOS/elevation, control-without-hard-stun, deliberate condition creation.*
2. **Cyborg — Strength.** Moves into the yard, takes `Anchor Stance`. She is now adjacent to 2 Sweepers plus the Warden's 2×2 face → `SURROUNDED 3`, with `cause = AllyAction`. Second AP on `Brace`.
   → *Tests: Strength, body-blocking, tile ownership, deliberately accepting pressure, the multi-tile counting rule.*
3. **Enemy activation.** The Warden slams (`GuardBreak`) and strips her Guard; a Sweeper's `Piercing` hit reaches Integrity. She is ~55%.
   → *Tests: real threat, the Guard→Integrity layering, and the pressure that makes recovery matter.*
4. **Synthetic — Intellect.** `Rescue Pulse` on the cyborg (2 AP). Sweepers (Light) fly 3 tiles — one collides with a container for impact damage, one goes over the ledge for fall damage + `Prone`. The Warden is `Anchored` → `Staggered + Unbalanced` instead. The cyborg receives Guard, `Hasted` (moving up the rail ahead of both Lancers), and `Shock Coating`. Emits `Rescued` and `Displace`.
   → *Tests: the entire conditional system, the forecast, mass/collision/fall rules, boss degradation, initiative manipulation, earned recovery.*
5. **Cyborg (hasted, now acting before the Lancers).** Either:
   - `Redline: Disengage` — vent for partial repair, break contact, move to the dock, and her shock-coated attack on the way applies `Shocked` to the Prone Sweeper; **or**
   - stay in `Assault` and use the freed space to armor-break the `Staggered` Warden.
   → *Tests: the stance decision. **The encounter must not have one right answer here** — that is the actual test.*
6. **Human.** `Dispatch` finisher on the still-`Isolated` Lancer (consuming her own step-1 tag through a different character's window), **or** a Gunslinger reaction-shot setup covering the dock approach.
   → *Tests: a second handoff, tag consumption, Agility conversion.*
7. **Synthetic.** `Heartless` arc through the `Shocked` target and the energized puddle (if the terminal was activated) → chains the remaining Sweepers.
   → *Tests: the Conduction substrate, environmental interaction, and the Spell-Slinger→Heartless cross-Specialty payoff.*
8. **Synchronization.** Three distinct contributors resolved in one linked chain (human created the condition → synthetic converted it → cyborg converted the opening) → first Sync tier, unlocking a basic joint attack that breaks the Warden's `Slam Actuator` and opens the extraction.

### Variants to run on the same map

- **Remote Arsenal variant** — the cyborg holds the dock with a heavy rifle and issues a `Hold Line` directive to the mech in the yard. Same map, same enemies. This is the §7 A/B.
- **Two-survivor variant** — remove the human entirely. This is the cheapest possible test of gap **G1**, and it will tell you within an hour whether the two-survivor chapter needs its own design.

### Embedded visual-consistency test

At the moment the Rescue Pulse resolves, trigger a **4-second A-tier in-engine insert**: camera push to the synthetic's face, chassis flare, one 2D impact frame, then cut back to the tactical camera. Same asset, higher LOD. Frame-step it against the model sheet, and compare it against a still of her from the tactical camera in the same encounter. That single comparison is the shared-asset strategy either working or not.

### Pass / fail

- Three external testers complete it without instruction in ≤15 minutes.
- **≥2 of 3 deliberately create the `Surrounded` window on a second attempt** rather than stumbling into it. *This is the single most important number in the whole prototype.*
- ≥2 distinct viable winning lines emerge across testers.
- The turn-order rail is observably used at least twice per playthrough.
- No tester reports the animation lagging their input.
- Frame time stays inside budget throughout.
- The Remote variant does not out-win the Piloted variant by ≥15%.

If this scenario is legible, tense, and visually faithful, the thesis is proven and more talents will make it bigger. If it is not, more talents will only make the failure larger — which `CODEX-BRIEF` §20 already says, and which is correct.

---

## 15. Questions for Juan and Mina

Only decisions that change architecture, production cost, or the first prototype.

**Blocking the kernel**

1. **To-hit rolls: yes or no?** (C7) Determines the forecast UI, the AI's evaluation function, the balance model, and whether the seeded PRNG is load-bearing. My recommendation is no rolls; I need your ruling before the kernel is written.
2. **Is `Charisma` a sixth attribute, or is Con Girl fiction-flavoured Agility/Resolve?** (C5) Blocks the ability schema's `scalingAttribute` enum and all gear design.
3. **`Integrity` — attribute or pool?** (C4) Pick one name for each. Proposed: attribute `Frame`, pool `Integrity`.
4. **Is permadeath ever on the table for a protagonist?** Changes the entire recovery model (§8) and the narrative state store.

**Blocking art production**

5. **The white-haired human's costume: tactical outfit, or the hero-image glamour dress?** (C1) And specifically: **heeled thigh boots — yes or no?** This is the most expensive open item in the project. It changes her rig, cloth budget, foot IK on elevation, and every animation she has.
6. **Does the synthetic wear the red bomber jacket in the definitive timeline?** (C2) Text says yes; current art says no. If yes, it is her only cloth and everything else is a shader — a large production win worth taking deliberately.
7. **The cyborg's arms: symmetric mechanical forearms, or the asymmetric full-right-arm the art shows?** (C3) Determines the rig, the IK, and whether her signature counter is one memorable clip or a mirrored system.

**Blocking the schedule**

8. **Prologue first, or the main-timeline scenario first?** (C6, R4) The prologue requires two extra canonical characters, a costume state change, and a working Limit Break system before anything is proven. My recommendation is main-timeline first — but it contradicts `PREPRODUCTION` §13, so it is your call, not mine.
9. **What is the actual team?** Two people, or is there budget for contract animation and VFX? This is the number that sets the ability budget in §6.0 and determines whether nine Specialties is a two-year plan or a six-year one. I would rather size the design to the team than discover the mismatch in year two.
10. **Name the spike's target hardware — a low bar and a high bar.** Without it, every pass/fail criterion in §12 is unfalsifiable.
11. **Who is the single final art approver per asset class?** (G4) Shared creative direction is right for direction and a deadlock risk at approval gates, which is where a strict anti-slop pipeline generates the most friction.

**Blocking level design**

12. **How does the two-survivor chapter work tactically?** (G1) Two units cannot produce the three-contributor chain that defines the game. Is it a deliberately different, tighter mode? Is the mech the third body? Is Operator a limited third actor? This changes encounter design, balance, and possibly the ability set.
13. **Does the mech ever appear on maps where it physically cannot fit?** If yes, `Docked` mode becomes a mandatory constraint on every map you ever build, which is a permanent level-design tax. Decide it before maps exist, not after.

---

## Appendix — What I would do Monday morning

In priority order, and none of it requires art:

1. **Answer Q1, Q2, Q3, and Q8** — an afternoon, and it unblocks everything.
2. **Run the Action Vocabulary Census (R1)** — one day with a spreadsheet. Count the clips implied by one full Specialty, multiply by twelve, and look at the number honestly. Every scope decision downstream depends on that number existing.
3. **Build the headless kernel with the ASCII forecast panel** — §5 rules only, the Relay Yard encounter, no art. One to two weeks. This is the experiment that can falsify the thesis.
4. **Start the two-week Unity spike in parallel** — it is an independent track and must stay independent.
5. **Resolve C1/C2/C3 before any character enters production.** Costume decisions made after rigging are made twice.

The goal of this consultation was not to approve the project. It was to find what must be true for it to work. The three things that must be true are: **that a player can see a condition before they create it**, **that three units against well-designed opposition is enough**, and **that the animation bill fits the team you actually have.** All three are testable within a month, and none of them requires a single finished asset.
