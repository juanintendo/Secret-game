# CLAUDE QUESTIONS 001 — unresolved decisions

Companion to `CLAUDE-TECHNICAL-CONSULTATION-001.md`.
Date opened: 2026-09-08
Working source: `C:\Users\colom\Documents\DD90s`

**This is the single home for open questions.** When new ones arise, update this file rather than scattering them through conversation. When one is answered, mark it `RESOLVED`, record the ruling and the date, and do not delete it — the history is what stops the same question being reopened.

Questions already answered by the documents or the character sheets are **not** here. Notably: the cyborg's arm topology (resolved by her sheet — bilateral, symmetric, mechanical from the elbow), the synthetic's red jacket (present and confirmed), and the white-haired human's costume (canon as drawn, no prohibited trim).

Deadlines are expressed relative to the work they block, with a calendar date assuming the technical spike begins **Monday 2026-09-14**.

---

## A. Blocking before architecture

### Q1 — Operator's arms — **RESOLVED 2026-09-08. Not a blocker.**

**Ruling (Juan, via `MINA-CONSULTATION-ADDENDUM-001.md` §1):** settled visual canon is the asymmetry — anatomical **right** arm exposed below the shoulder armor with no sleeve, glove, bracer or wrist covering; **left** arm with the white sleeve and white glove with purple detailing. A left-facing profile shows only the near left sleeved arm; the far right arm must never appear through the torso.

**There was never a contradiction.** Revision 1 of the consultation reported that `Characters/…09_54_53 AM (5).png` showed both arms sleeved. That finding was wrong. Re-inspected at magnification, view by view, the sheet depicts the canonical asymmetry correctly in front, back and profile. See consultation §5 C1 for the correction and its cause.

**Retained rather than deleted, per this file's own rule** — the history is what stops the question being reopened, and the methodological lesson is worth keeping: no visual fact enters a document unless it was read at magnification on the region in question. It is now gate 10 in consultation §11.9.

**The remaining specification gap is now CLOSED (revision 5).** `operator-arm-asymmetry-reference.png` has been inspected directly. Its `LEFT — SLEEVED` callout shows the construction explicitly: purple shoulder cap → **a short section of bare upper arm** → a purple band finishing the sleeve's top edge → white sleeve → purple forearm band → purple wrist cuff → white glove. The `RIGHT — BARE` callout shows the cap over bare skin the entire length to a bare hand. The `LEFT SIDE` view draws one arm only, with no far arm through the torso. **The shoulder caps are identical on both sides; only what happens below them differs.**

The sleeve deliberately begins below the cap. That was construction, not inconsistency. **Nothing on Operator's arms remains open.**

---

### Q2 — `Integrity`: attribute or pool?

**Owner: Mina** · **Deadline: before the kernel is written — 2026-09-12**

**Why it matters.** `CODEX-BRIEF` §5 lists Integrity in the primary stat family as "health/structural durability". §10 uses "Integrity/Health" as the damage pool. One name, two objects, in the same document.

**What changes technically or financially.** It blocks the ability schema's scaling expressions, the UI's stat panel, the balance model, and the forecast's damage display. It is also a source of sustained ambiguity in every design conversation, which is a slow cost that never shows up on a schedule.

**Options.**
1. Attribute becomes `Frame`; the pool keeps `Integrity`.
2. The pool becomes `Structure`; the attribute keeps `Integrity`.
3. Drop the attribute entirely; durability derives from Strength and gear.

**Provisional recommendation.** Option 1. The pool is the word players see constantly, so it should keep the more evocative name; `Frame` reads correctly for a cyborg, a human in armor and a synthetic chassis alike. Option 3 is worth considering separately — five attributes for three characters may be one too many — but it is a larger design change and should not be bundled into a naming fix.

---

### Q3 — Are there to-hit rolls?

**Owner: Mina** · **Deadline: before the kernel is written — 2026-09-12**

**Why it matters.** `PREPRODUCTION` §4 and `CODEX-BRIEF` §4 both say "deterministic base damage with clearly surfaced hit or mitigation uncertainty", while also requiring "seeded, replayable simulation". These are compatible readings of two different games.

**What changes technically or financially.** It determines the forecast UI, the AI's evaluation function, the balance model, and whether the PRNG is load-bearing. Retrofitting determinism onto a probabilistic kernel means rewriting the resolver, the AI and every test.

**Options.**
1. **Fully deterministic.** Uncertainty lives in hidden enemy intent and reinforcement timing.
2. **To-hit rolls with a displayed percentage.** Genre-conventional.
3. **Deterministic damage, probabilistic status application.** A hybrid.

**Provisional recommendation.** Option 1. A forecast that says "83 %" is not legibility, it is a slot machine with a tooltip — and legibility before commitment is a stated pillar (`CODEX-BRIEF` §2.4). Determinism also makes the player's forecast and the AI's evaluation the same computation, which is the only cheap way to keep them honest. Option 3 is the compromise to fall back to if playtesting finds combat feels solvable. Keep the seeded PRNG for AI tie-breaking regardless.

**Note.** This one has a real risk attached: deterministic tactics can feel like a puzzle rather than a battle. The §16 encounter is designed to detect that. Treat the ruling as revisitable *after* that test, and only then.

---

### Q4 — How large is the mech, and how many tiles does it occupy?

**Owner: Juan and Mina jointly** · **Deadline: before the grid is written — 2026-09-12**

**Why it matters.** The mech's footprint **in logical cells** is the most consequential undefined value in the kernel. Per Mina's revision-6 ruling this is a question about **cells, not metres** — the kernel is dimensionless and `cellSizeMeters` is a separate Unity concern (Q32).

**What changes technically or financially.** It determines: whether Remote Arsenal places a 1×1 and a 2×2 body on a compact grid simultaneously; whether Bulwark body-blocking covers one lane or three; minimum corridor and doorway width in cells on every map ever built; what "adjacent" means for the synthetic's rescue condition; and whether knockback needs one mass class or two. **It is upstream of the grid, and the grid is upstream of everything.**

**Status update, 2026-09-08 — a provisional hypothesis now exists. The question remains blocking.**

`MINA-CONSULTATION-ADDENDUM-001.md` §2 proposes, **explicitly as a hypothesis and not as canon**: mech ≈ **3.6 m**, pilot ≈ **1.75 m**, ratio ≈ **2.05×**, human **1×1**, mech **2×2**, rear-upper-torso / top-entry hatch fitted to the existing silhouette, single mech identity.

**Two cautions before the spike runs** (consultation §4.2):

1. **Height ratio and footprint are separate claims.** Occupancy is governed by width and depth, not height. A 3.6 m humanoid of pilot width would ordinarily occupy **1×1** and simply be tall. The 2×2 claim rests on the mech being roughly **twice as wide** as a person — which its broad silhouette supports visually but no measurement confirms.
2. **The study drawing has now been inspected and measured (revision 5).** It supports the rationale — the mech is genuinely broad in plan view, not merely tall — and it shows a working rear-upper-torso cockpit with a seated pilot volume. **It validates nothing tactical.** It also exposes an inconsistency: measured against its own drawn grid, the `MECH 2×2` bracket spans **4** squares and the `PILOT 1×1` box spans **1.4**, against a legend declaring 1 m squares; the front elevation gives the mech ≈2.4 m wide where the top-down would give 3.8 m. The sheet is coherent only if the drawn squares are **0.5 m**. See consultation §4.2 and **Q32**.

**It becomes canon only after the spike passes the seven validations** in consultation §7.6: doors and traversal · narrow-map pathfinding · cover interaction · occupancy and forced movement · camera framing · encounter-space cost · remote-control readability.

**Options if validation fails.**
1. **2×2 holds.** Multi-tile occupancy is a first-class kernel feature from day 1, not a retrofit.
2. **1×1 with the same visual mech**, accepting that a large machine occupies a person's tile. Cheapest; costs the spatial meaning that makes Bulwark and Remote Arsenal interesting.
3. **Escalate the conflict** between visual scale and tactical footprint as a design decision for Juan and Mina — the addendum's explicit instruction, and the correct path.

**Provisional recommendation.** Prototype at 2×2 as the addendum directs, and **run the §16 encounter twice, at 1×1 and 2×2, on the same map** — an hour's work and the most informative comparison in the spike. **If 2×2 fails, do not silently compress the mech and do not silently enlarge the map.** Both are answers to a question that has been reserved.

**Partly done.** The study now provides the cyborg beside the mech on one shared ruler — the project's first cross-character scale reference, and it partially answers Q11 for that pair. The other four principals still have no shared height reference.

**Settle Q32 before pathfinding is tested.** Tile size is upstream of every result test 2 could produce.

---

## B. Blocking before the combat prototype

### Q7 — What is the representative PC target? — **SUPERSEDED by Q30**

**Owner: Mina** · **Deadline: spike day 1 — 2026-09-14**

Every performance budget in consultation §13 is meaningless without a named machine. I assumed a GTX 1660 / RX 5600-class at 1080p. **What changes:** whether 60 fps with the full post stack is comfortable or marginal, and therefore how much of the analog treatment survives. **Recommendation:** name the actual minimum-spec machine you intend to support and profile on that hardware, not on a development workstation.

### Q8 — Which mech command model, after testing?

**Owner: Mina** · **Deadline: end of spike week 1 — 2026-09-19** · **Cross-references Q31**

`CODEX-BRIEF` §8.2 asks for shared AP, queued directives, and separate initiative with command bandwidth to all be tested. **What changes:** the cyborg's entire action economy and roughly a third of her ability set. **Recommendation:** queued directives with command bandwidth, tested in the headless kernel where each model is a day rather than a week. Pass criterion: across ten AI-vs-AI simulations she contributes 30–45 % of team actions. This is `REQUIRES PROTOTYPE` — do not choose by fantasy.

**Cheaper to test since Mina's entity ruling (Q31).** All three command models operate on the same stable two-entity representation, so switching between them changes initiative and AP rules rather than the object graph. Test them as configuration, not as three implementations.

### Q9 — What are the three enemy families for the first encounter?

**Owner: Juan and Mina** · **Deadline: spike day 3 — 2026-09-16**

Enemy design appears once across ~92 KB of documentation, as an open item. With three fixed protagonists, tactical variance comes from the opposition and the map — this is the main content axis and it is empty. **What changes:** whether the §16 encounter can falsify anything at all. **Recommendation:** the three in consultation §16 — an anchored heavy, networked drones, and ranged suppressors. One per protagonist's verb, so that each character has something the others handle badly.

### Q10 — Does the mech get its own `Integrity` pool, or share the pilot's?

**Owner: Mina** · **Deadline: spike day 3 — 2026-09-16**

**What changes:** whether destroying the mech is a mission-level consequence or a resource cost, and whether Redline's disengage-and-repair has anything to repair. **Recommendation:** separate pools. The mech is repaired and upgraded, never replaced (`CODEX-BRIEF` §3.2) — a separate pool with persistent damage between missions makes that canon mechanically visible, and gives Redline's recovery stance a purpose.

### Q31 — Which Specialties permit which deployment mode? — **entity model RULED; product decision remains**

**Owner: Juan and Mina** · **Deadline: before the combat prototype — 2026-09-26** · **Not an architecture blocker**

**Ruled by Mina, 2026-09-08 — the architecture half is closed.** Pilot and mech retain **separate, stable logical entity IDs in combat state regardless of deployment mode**:

- **Embarked / docked** — the pilot remains in authoritative state but is **non-spatial, non-selectable, non-targetable, and has no independent initiative slot**. The mech owns battlefield occupancy and is the active selectable unit.
- **Remote operation** — pilot and mech may **both** be spatial and addressable.
- Initiative, AP sharing, command bandwidth and queued directives **remain prototype questions** — see Q8.
- The ruling **does not** determine which Specialties permit each deployment mode.

**Why this stops being an architecture blocker.** The unit model is now determined. `spatial`, `selectable`, `targetable` and `hasInitiativeSlot` become per-entity state flags that deployment mode sets and every other system reads; the turn-order rail, targeting, LOS and the forecast filter on those flags rather than special-casing a docked pilot. Entity IDs are allocated once at encounter setup and never on a mode change, so embark/disembark are state transitions on existing entities rather than despawn/spawn events — which is what keeps replay determinism intact across a mode change. Consultation §9.6.1.

**What actually remains open, and it is a product question.** Which of the cyborg's three Specialties permit which mode:

- **Redline** — mech stance dance. Presumably embarked. Does disengage ever eject her?
- **Remote Arsenal** — explicitly remote by design (`CODEX-BRIEF` §8.2): pilot at range with heavy weapons, mech operating separately.
- **Bulwark** — unstated in every document. Embarked, remote, or either?

**What changes if this is answered differently.** Nothing in the data structures. It changes encounter-space cost (a remote Bulwark puts five tiles of team footprint on a compact grid), the tactical meaning of separation, and how many bodies the player is tracking per Specialty. Those are balance and feel, tested in the prototype.

**Provisional recommendation.** Bulwark embarked, Redline embarked, Remote Arsenal remote — one mode per Specialty, so the deployment state is a legible consequence of the build rather than a per-turn toggle. Revisit only if the prototype shows a reason to allow switching mid-encounter, which would add a transition cost and an animation.

**Cross-reference: Q8** is the other half of this subject — the command model in remote state. Q8 is the prototype question; Q31 is the product question. **Neither blocks kernel architecture.**

---

### Q32 — What `cellSizeMeters` does Unity use for visual scale? — **reframed, revision 6**

**Owner: Juan and Mina** · **Deadline: spike week 2, decided by comparison not in advance** · **Sub-decision of Q4. Not a fourth architecture blocker.**

**Reframed by Mina's ruling.** The earlier framing — "what is the tactical tile size in metres" — was wrong, and my recommendation to derive it from the study's drawn grid is retracted. **A generated image is construction evidence, not metrology.**

**Four concepts, kept separate:**

| # | Concept | Lives in | Unit |
|---|---|---|---|
| 1 | Logical kernel cell | `Game.Tactics` | **Integer coordinates. No metres.** |
| 2 | Entity footprint | Content data | Logical cells — human **1×1**, mech **2×2** (candidate) |
| 3 | `cellSizeMeters` | Unity presentation | Metres. **Configurable.** |
| 4 | Decorative / subdivided grid lines in studies | Reference art | Nothing |

**Why this stops being a pre-decision.** The kernel uses integer cell coordinates and is independent of metres, so **there is nothing to rule before the grid is written.** Footprints are counts of cells; `cellSizeMeters` scales the world and changes no combat rule.

**What the spike does instead.** Render **1.0 m, 1.25 m and 1.5 m** against **identical combat rules** and compare: camera framing, animation clearance, how large characters read at the tactical camera, and how a 2×2 mech feels beside a 1×1 human. **Combat behaviour must be provably unchanged across all three** — if it is not, the kernel has leaked metres and the §9.1 boundary is broken.

**Validate independently**, per the ruling: traversal · doors · cover · forced movement · camera · **animation clearance** · encounter-space cost.

**Provisional recommendation.** None, deliberately. This is a comparison to run, not a number to guess. The only advance commitment worth making is the CI assertion that no metre, world-space unit or float distance appears in the simulation assemblies — which is what makes the comparison meaningful.

---

### Q30 — What exact hardware defines the lowest supported preset?

**Owner: Mina** · **Deadline: spike day 1 — 2026-09-14**

**Why it matters.** The addendum settles that the minimum preset must be a *separately tuned art target* that never looks abandoned. That is a quality commitment with no number attached. Every budget in consultation §13 and the entire minimum column of §13.2 is unmeasurable until a machine is named. Supersedes and subsumes Q7.

**What changes technically or financially.** It determines how much of the analog post stack survives at minimum, whether upscaling is required, what the texture memory ceiling is, and how much separate art tuning the floor actually needs. Naming a weak machine is a real production cost; naming an optimistic one silently shrinks the addressable market.

**Options.**
1. **Steam Deck class.** Sets a hard, well-understood, widely-owned bar; excellent for a turn-based game; forces genuine discipline.
2. **GTX 1050 Ti / integrated-graphics class.** Broadest reach, hardest floor to hold at this art bar.
3. **GTX 1660 / RX 5600 class as minimum**, with the Standard tier above it.

**Provisional recommendation.** Option 1 or a machine of similar class. A turn-based tactical game is an unusually good fit for handheld-class hardware, the 60 fps target is achievable there with upscaling, and it gives the art team a concrete, testable object rather than an abstraction. Whatever is chosen, **profile on that machine, not on a development workstation** — this is the single most common way a minimum preset silently becomes unshippable.

---

---

## C. Blocking before art production

### Q11 — What is the shared proportion chart?

**Owner: Juan** · **Deadline: before modelling — 2026-09-19**

The six sheets were produced at three different canvas sizes with three different guide spacings. **No height relationship between any two characters is currently established** — including between the mech and the humans, which is why this and Q4 should be answered together. `CODEX-BRIEF` §13.1 requires this chart and it does not exist. **What changes:** the rig, the camera, the tile size, cover heights, and every cinematic composition. **Recommendation:** composite all six existing sheets onto one height reference. No new illustration required; one afternoon.

### Q12 — What does the leader look like without her armor?

**Owner: Juan** · **Deadline: before prologue production, and before her rig — 2026-09-26**

`STORY-CANON-001.md` §2.1 makes the armor "removable external"; `PREPRODUCTION` §13 requires a costume state change during the prologue. Only the armored state has a sheet, and in it her arms and hands are entirely enclosed. **What changes:** it is a second silhouette, a second material set and a transition — effectively a second character to produce. There is also a narrative risk: a modeller working from the armored sheet alone will produce someone who reads as a cyborg, which destroys the prologue's point that a *human* overrides the machine. **Recommendation:** a second sheet in the unarmored state before her rig begins. If that is out of scope now, the answer to Q13 makes it moot for a while.

### Q13 — Which two characters are art-complete for the vertical slice?

**Owner: Juan and Mina** · **Deadline: before modelling — 2026-09-26**

**What changes:** roughly 3–5 person-months of animation. **Recommendation:** the synthetic and the cyborg — the two survivors, the pair the main timeline opens with (`STORY-CANON-001.md` §7), and the cheapest and most expensive rigs in the cast, so the slice proves the range. This also defers Q12 and the human's expensive costume rig past the slice.

### Q14 — Is the human's coat rigged as authored bone chains, and is the heel budgeted?

**Owner: Mina** · **Deadline: before her rig — 2026-10-03**

Her sheet confirms a coat worn off both shoulders with the arms through the sleeves — so the garment hangs from the elbows, not the shoulders — plus garter straps crossing bare thigh skin, and thigh-high boots on a block heel. None of this is prohibited trim; all of it costs. **What changes:** cloth-versus-bone-chain decisions, a corrective shape at the hip so straps do not pinch through skin, and a dedicated foot-IK profile for the heel on discrete elevation. **Recommendation:** authored bone chains only, never simulation; validate against a 128 px silhouette plate *before* modelling. See consultation §11.5.

### Q26 — What are the names of the four gear slots? — **structure settled; naming only**

**Owner: Juan and Mina** · **Deadline: before any gear is authored — 2026-09-26**

**Settled by Juan (revision 3).** The shape is **`1 weapon + 4 gear`**:

- **four gear slots** per protagonist, plus **one separate character-specific weapon slot**;
- **only the four gear pieces participate in set bonuses**; the weapon **never** counts toward a set threshold, at any tier;
- two different 2-piece bonuses may be combined; a complete 4-piece set gives its full-set bonus;
- **`mech-hardpoint`, if retained, occupies one of the cyborg's four gear slots** — not a fifth gear slot, not a sixth equip position.

**The `CODEX-BRIEF` §12 conflict is resolved.** Its five categories — `signature-weapon`, `frame-component`, `utility-module`, `character-signature`, `mech-hardpoint` — reconcile as **`signature-weapon` = the weapon slot**, with the other four as gear. **It must not become `5 gear + weapon`.**

Note that this **overturns my revision-2 reading**, which proposed treating `mech-hardpoint` as the cyborg's instance of `character-signature`. That would leave only three gear categories. `mech-hardpoint` is a distinct fourth gear slot that the cyborg fills with a hardpoint.

**What actually remains open.** One naming question, and it is small: **what do the human and the synthetic put in the slot the cyborg fills with `mech-hardpoint`?** Either that slot is generically named and each character fills it differently, or each character has her own name for her fourth slot.

**Options.**
1. **Three shared gear categories plus one personalized fourth.** Cyborg: `mech-hardpoint`. Human: concealed equipment. Synthetic: a conduction module. One shared UI panel with one character-specific position.
2. **Four shared categories**, with `mech-hardpoint` as one valid *item type* within a generic slot rather than a slot name.
3. **Four fully character-specific slot names.** Maximum identity, triple the UI work.

**Provisional recommendation.** Option 1. It satisfies "must respect each character's identity" without tripling the interface, and it is the natural home for `CODEX-BRIEF` §12's existing `mech-hardpoint` idea. Option 2 is nearly as good and slightly simpler to author.

**Why this is no longer blocking.** The schema can be written today against a **fixed four-entry gear slot enum plus one weapon slot**; only the enum's string values are pending. Consultation §10.5 defines gear and weapon as **separate record types**, with the weapon record carrying no `setId` field at all — so the "weapon never counts toward a set threshold" ruling is enforced structurally rather than by memory. Validator rules §10.3 #15, #15a and #15b guard the count, the weapon's exclusion, and the `5 gear + weapon` misreading.

**Whatever is chosen: name the slots once and never rename them.** Ids are permanent; player-facing labels live in the localization table where they can change freely.

---

### Q27 — Does ordinary gameplay visibly display equipped gear?

**Owner: Juan** · **Deadline: before modelling — 2026-09-26**

**Why it matters.** This is **the single largest cost fork in the equipment system**, and the addendum explicitly reserves it.

**What changes technically or financially.** Option 1 creates a mesh and material matrix of four slots × three protagonists × *n* sets, every visible variant of which must pass the 128 px silhouette gate (consultation §11.9 gate 8) and must not break the canonical silhouettes that `CODEX-BRIEF` §2.1 declares sacred. Option 3 costs almost nothing. The difference is plausibly a person-month or more of modelling and validation, and it interacts directly with the §11.6 animation estimate.

**Options.**
1. **All equipped gear visible.** Maximum player expression, maximum asset cost, maximum silhouette risk.
2. **Weapons and selected approved overlays visible; armour abstracted.** Weapons already differ by character class (settled), so they carry the visual read; a small approved overlay set covers signature pieces.
3. **Canonical clothing always; gear entirely statistical.** Cheapest, strongest identity guarantee, weakest expression.

**Provisional recommendation.** **Option 2.** Weapons are what players look at, they are already character-specific by settled direction, and they change silhouette in ways that read at gameplay distance. Armour variants at 128 px mostly do not read at all — you pay full modelling and validation cost for a difference the tactical camera cannot resolve. Option 2 also protects the sacred-silhouette pillar by construction rather than by review.

This pairs naturally with the cinematic override (§12.1): if gameplay shows weapons and cinematics show the canonical loadout, the visual gap between them is small enough that players will not experience the override as the game ignoring them.

---

### Q15 — Who is the final approver per asset class?

**Owner: Juan and Mina** · **Deadline: before outsourcing anything — 2026-10-03**

`PREPRODUCTION` §16 states shared creative direction. Shared authority is fine for direction and a deadlock risk at approval gates, which is where a strict anti-slop pipeline generates the most friction. **What changes:** throughput, and whether external contributors can be given a clear accept/reject signal. **Recommendation:** one named approver per asset class — characters, environments, UI, VFX, cinematics — with Mina holding technical veto on all of them.

---

## D. Blocking before the vertical slice

### Q16 — How does the two-survivor chapter work mechanically?

**Owner: Juan and Mina** · **Deadline: before slice content lock — 2026-10-17**

`PREPRODUCTION` §6.2 and §13 and `CODEX-BRIEF` §16 Gate 5 all require a main-timeline chapter played by the cyborg and synthetic **alone**. Every combat proposal in all three documents assumes three units. With two: nobody creates `Isolated`, the synthetic's conditional windows have exactly one possible target, and the three-character handoff — the thesis — is structurally impossible. **What changes:** either a bespoke two-unit balance pass, or a fiction-level solution. **Recommendation:** give the pair a temporary third presence — the mech operating semi-autonomously under Remote Arsenal is the obvious candidate and is already canon. That preserves three tactical bodies while keeping two characters, and it introduces Remote Arsenal at exactly the point in the story where it means something.

### Q17 — Working title and stable character codenames?

**Owner: Juan** · **Deadline: before the first public-facing material — 2026-10-17**

Currently TBD across all three documents. **What changes:** every id in the content schema, every localization key, every filename. **Recommendation:** stable *codenames* immediately even if the final names come later — `human`, `cyborg`, `synthetic`, `leader`, `operator`, `mech` are already in use informally and are fine as ids. Ids should never be renamed once content exists; player-facing names live in the localization table where they can change freely.

### Q18 — What is the first public-facing material, and what must it prove?

**Owner: Juan** · **Deadline: before slice content lock — 2026-10-17**

**What changes:** what the slice must contain, as opposed to what would be nice. **Recommendation:** one 30-second capture of the §16 line — lure, condition badge, pulse, displacement into the water, chain, commit zoom. It is the only artefact that demonstrates the tactical thesis and the visual thesis at the same time. Build the slice so that clip exists.

---

## E. Non-blocking, future

### Q19 — Does the synthetic wear the jacket in every main-timeline scene, or situationally?
**Owner: Juan.** Affects whether the jacket is a costume state or part of the base asset. Recommendation: base asset, with a jacketless variant reserved for a specific dramatic beat.

### Q20 — Are the cyborg's prosthetics integrated or removable?
**Owner: Juan.** Her sheet establishes the topology but not this. Affects `CODEX-BRIEF` §8.4's "emergency actions when separated from the mech" and any scene showing maintenance.

### Q21 — What are Operator's hologram rules?
**Owner: Juan and Mina.** Fixed scale, opacity, glitch behaviour and anchor point, or scene-dependent? Affects the shader and whether she is a character asset or a presentation effect.

### Q22 — Are the cyborg's asymmetric thigh rigs deliberate?
**Owner: Juan.** Her sheet shows different pouch assemblies left and right. Natural for a soldier's loadout, but a turnaround is what a modeller builds from — record it as intentional or it will be "corrected", and then it will drift back.

### Q23 — Does chassis exposure become a mechanic?
**Owner: Juan and Mina.** `CODEX-BRIEF` §9.4 raises visibility of the luminous chassis as a Personal-talent theme. Affects the shader's parameter surface. Not needed until her Personal tree is designed.

### Q24 — Platform scope beyond PC?
**Owner: Juan and Mina.** Affects input abstraction and performance budgets. Recommendation: build input controller-first from day one regardless — it is nearly free now and expensive later — but make no platform commitment until after the slice.

### Q29 — Does the Synthetic's jacket preference persist per save, per loadout, or globally?
**Owner: Juan and Mina.** Cosmetic scope only. **What changes:** where the flag lives — campaign save, loadout record, or a user-profile setting outside the save entirely. **Recommendation:** per loadout, matching the addendum's own suggestion, with a global convenience default only if user testing asks for it. Per loadout keeps it beside the equipment UI where players will look for it, and it lets the choice differ between a stealth build and a frontline build. Whichever is chosen, the flag must live in the presentation/profile store, never in `Game.Core` (consultation §10.5).

### Q28 — Are set bonuses tied to named authored sets only, or may compatible pieces share a family tag?
**Owner: Mina.** **What changes:** whether the 2+2 combination space is enumerable. Named sets give `n × (n−1) / 2` pairings — 15 at six sets, testable exhaustively. Family tags make the space open-ended and effectively untestable, which is the road to loot soup that the addendum forbids. **Recommendation:** **named authored sets only**, with a `family` tag used for *flavour and filtering in the UI*, never for bonus eligibility. Revisit only if authored sets prove too rigid in playtesting.

### Q25 — Is `Inspo/` excluded from version control?
**Owner: Mina.** The folder is published third-party comic art. Committing it to a public repository would be a redistribution. Recommendation: add to `.gitignore` before the first push, and never place these images in a generation context that touches a canonical character.

---

---

## F. Not blockers — settled work and source governance

Reclassified in revision 3. These are real tasks with real deadlines. **None of them is a decision anyone is waiting on, and none blocks the combat kernel.** They were previously counted as architecture blockers, which overstated how stuck the project is.

### Q5 — Transcribe the settled Con Girl / Agility ruling into the canon documents — **EDITING TASK, not a blocker**

**Owner: Juan** · **Deadline: before any content authoring — 2026-09-12**

**Settled, not open.** All three of the white-haired human's Specialties — Dispatch, Con Girl, Gunslinger — scale from **Agility**. "Charisma" describes Con Girl's fantasy and behaviour: deception, provocation, leverage, behavioural manipulation, and the animation, dialogue, targeting rules and crowd-control effects that express them. **It is not an attribute and is not added to the stat sheet.**

**Why this is still work.** The ruling exists in conversation and in these reports. It does **not** exist in `DD90s`. `CODEX-BRIEF` §7.2 is still titled "Con Girl — Charisma control and manipulation" and `STORY-CANON-001.md` §3.1 still says "Charisma-driven". Anyone reading the canon documents today would implement a Charisma stat, and content authored against the wrong axis has to be re-authored.

**What to do:** edit `CODEX-BRIEF` §5 and §7.2 and `STORY-CANON-001.md` §3.1 in place, and log the ruling so it is not reopened by someone who was not in the conversation. Consultation validator rule §10.3 #7 already enforces the ruling mechanically — an ability that scales from an attribute other than its owner's primary axis fails CI — so the code side is protected even before the documents catch up.

**Related, still genuinely open and non-blocking:** a character-specific `Leverage` resource is a recommendation only and must not be treated as canon. Consultation §8.1 proposes a way to get the leverage *fantasy* without a new resource or stat.

---

### Q6 — Sheet hygiene: duplicate, naming, provenance — **SOURCE GOVERNANCE, not a blocker**

**Owner: Juan** · **Deadline: before modelling — 2026-09-19**

**Why it matters.** `Characters/…05_51_52 AM.png` and `Characters/…06_38_03 AM.png` are byte-identical (SHA-256 `90f11662…`). All eight sheets carry generator-default filenames that are unsortable, unversionable and silent about subject and status. No sheet has a provenance or approval record.

**Why it is not an architecture blocker.** It affects how reliably assets are identified. It does not affect a single line of kernel code, and the spike can run without it.

**Why it is nonetheless urgent.** Without a manifest, the error documented in consultation §5 C6 recurs — an obsolete image treated as canonical, producing confidently wrong downstream work. That has already happened once on this project, and `Team Concept 1st draft.png` still sits in `Art/` beside two canonical images.

**What to do:** record every image's SHA-256, status label, what it establishes and what it **cannot** settle, in an art manifest. Adopt `SHEET-<subject>-<state>-<revision>.png`.

**Do not delete the historical sheets yet.** Supersession is recorded by **status**, not by deletion — that is the whole point of the `SUPERSEDED` label. The duplicate is the one exception worth considering later, and only once the manifest exists to record that it was a duplicate rather than a lost revision.


---

## Status summary — recalculated for revision 3

| Group | Open | Change | Note |
|---|---|---|---|
| **A — blocking before architecture** | **3** | 5 → 3 | Q2, Q3, Q4. Q1 closed; Q5 and Q6 reclassified to group F |
| B — blocking before the combat prototype | 5 | — | Q7 superseded by Q30 |
| C — blocking before art production | 6 | 7 → 6 | Q26 reduced to a naming task, still listed here |
| D — blocking before the vertical slice | 3 | — | — |
| E — non-blocking, future | 9 | — | — |
| **F — not blockers: settled work and governance** | 2 | — | Q5 editing · Q6 source governance |
| **Total open** | **29** | 28 → 29 | Q32 added; 1 resolved (Q1), 1 superseded (Q7) |

### The three genuine architecture blockers

| # | Decision | Owner | Deadline |
|---|---|---|---|
| **Q2** | `Integrity` — attribute or pool | Mina | 2026-09-12 |
| **Q3** | To-hit rolls — yes or no | Mina | 2026-09-12 |
| **Q4** | Mech footprint — is it 2×2 | Juan + Mina | Spike Gate A, 2026-09-19 |

A question belongs in group A only if **the combat kernel cannot be written correctly without the answer.** Q2 and Q3 are rulings that can be made in an afternoon. Q4 is now a spike outcome rather than an armchair ruling — progress, but it stays blocking until the seven validations in consultation §7.6 pass, and it must not be closed by adopting the hypothesis as canon.

**Revision 5 note.** The mech study has been inspected and measured. It strengthens the *rationale* for Q4's 2×2 hypothesis and it exposes **Q32**, tile size — which is a sub-decision *inside* Q4, not a fourth blocker. **The count remains exactly three.**

### What was reclassified, and why

- **Q1 — closed.** Operator's arms were never in conflict; the revision-1 finding was my error.
- **Q5 — editing task.** Con Girl's scaling is settled: all three of the white-haired human's Specialties scale from Agility, and "Charisma" names her fantasy, not an attribute. The kernel can be written today against that ruling, and validator rule §10.3 #7 enforces it mechanically. What remains is transcription into the canon documents — real work with a real drift risk, but not a decision anyone is waiting on.
- **Q6 — source governance.** Duplicate cleanup and filename conventions affect asset identification, not kernel code. **Historical sheets are not deleted**; supersession is recorded by status.

### The conditional fourth — **closed**

**Q31, the pilot/mech entity model**, was the only remaining candidate that could have made the count four. **Mina ruled it on 2026-09-08:** pilot and mech retain separate, stable logical entity IDs regardless of deployment mode; embarked, the pilot stays in authoritative state but is non-spatial, non-selectable, non-targetable and holds no initiative slot; in remote operation both may be spatial.

The unit model is therefore determined. What remains under Q31 — which Specialties permit which deployment mode — is a **product decision that changes no data structure**. Q31 stays a combat-prototype and product question, cross-referenced to Q8. **The count is exactly three.**

### Verified as *not* architecture blockers

Named explicitly, because "three" is only credible if the near-misses are listed: **Q8** mech command model (prototype-decidable; all three models run on the same entity representation) · **Q10** mech `Integrity` pool (downstream of Q2, not independent) · **Q26** gear slot names (structure settled by Juan; naming only) · **Q27** gear visibility (presentation layer; blocks modelling, touches no kernel code) · **Q30** minimum hardware (blocks measurement; a spike day-1 prerequisite) · **Q31** deployment modes by Specialty (entity model ruled; the remainder sets existing flags).

### Still on the critical path despite not blocking architecture

**Q30** blocks spike day 1 — every performance budget is unmeasurable without a named machine. **Q27** is the largest cost fork in the equipment system and blocks modelling. **Q31** is decided by the same spike as Q4.
