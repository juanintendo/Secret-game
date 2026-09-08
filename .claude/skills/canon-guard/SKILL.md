---
name: canon-guard
description: "Load FIRST for any work on this project (the 90s-anime tactical RPG / Secret-game): narrative, character, combat design, art, animation, UI, or code. Holds the authority order, the binding canon, the deliberately-open questions, the known art-vs-text conflicts, and the character quick-reference. Trigger on any task touching this repo, or on mentions of the trio, the mech, Operator, the prologue, Specialties, or the canon docs."
---

# Canon Guard

You are working on a premium story-driven tactical RPG about three women rebuilding a destroyed combat unit. This skill is the guardrail. Read it before you touch anything.

## 1. Authority order — never average conflicting sources

1. `docs/canon/STORY-CANON-001.md` — narrative truth, chronology, secrets, character knowledge.
2. `docs/canon/90S-ANIME-XCOM-PREPRODUCTION-001.md` — founding product, combat, visual, technical, production direction.
3. `docs/canon/90S-ANIME-XCOM-CODEX-CLAUDE-BRIEF-001.md` — combat progression, Specialties, Personal talents, recovery philosophy, cinematic-consistency pipeline.
4. `docs/canon/CANON-DECISIONS.md` — accepted rulings with evidence and status.
5. `docs/consultation/` — final consultation and handoff. Recommendations are non-canon until accepted in the decision ledger.
6. Approved art indexed by `art/ART-MANIFEST.md`.

**When sources conflict: report the conflict, cite both, follow the authority order, and keep going. Never silently pick a middle value.**

## 2. Hard rules

- **Do not import** characters, systems, lore, or assumptions from Juan's other projects (beat'em-up, House, Ozymandias, Spell Out, Arcadio). They are unrelated.
- **Do not rename** characters, Specialties, or tactical verbs unless you are reporting a concrete terminology collision (see §6).
- **Do not resolve open questions** (§4) on your own. Propose; do not decide.
- **Do not redesign the project into a generic tactics game.** Every rule must describe *these* people.
- **Do not add a permanent healer.** The synthetic is not a healer. Recovery is manufactured by play.
- **Do not treat the trio as interchangeable.** Fixed cast, fixed identities, no roster.
- Label every new suggestion explicitly as a recommendation.

## 3. Binding canon (short form)

**Product**
- Premium story-driven tactical RPG, three fixed protagonists, **no permanent leader**.
- Each woman is already individually capable. **The unit is level one, not the characters.**
- Progression has two axes: individual power, and earned team synchronization.
- Synchronization fills from **cooperation**, not from ordinary damage.
- Combat must be legible before commitment. Animation celebrates a resolved decision; it never conceals the rules.
- 60 fps responsive input/camera/UI/simulation. Stepped character acting is allowed; **input latency and camera judder are not**.
- Clean high-definition 1990s OVA visual language. Analog artifacts are authored punctuation, never a global VHS filter.
- Gameplay and cinematics share one character truth (same proportions, face construction, hair silhouette, costume/material IDs, VFX colors).
- Narrative decisions alter tactical state; tactical outcomes alter narrative state.

**Story spine**
- The original trio = green-haired human leader + cyborg mech pilot + luminous synthetic, coordinated remotely by **Operator** from a hidden HQ whose location nobody on the field team knows.
- The prologue is a showcase mission at near-peak power. A prepared intrusion exploits the **second synchronized Limit Break**. Operator apparently dies at HQ (a separate location, same coordinated operation). The leader rejects her AI copilot's machine-optimal recommendation, creates an escape path by human judgment, and is presumed dead. No body.
- Cyborg and synthetic survive, go underground for years, separately.
- Operator returns (restored/continued, now a hologram) and recontacts each survivor through a personalized, security-conscious operation. She then maneuvers them toward one specific human recruit.
- **Authorially true, not player-known:** the original leader survives in some form and becomes the unknown protective intruder who has been quietly patching the team's systems.
- The eight mystery questions must share causal machinery. They must not resolve as eight unrelated twists.

**Combat chassis (provisional but current)**
- Compact grid with discrete elevation; interleaved initiative; two AP per activation; facing, flank, LOS, elevation, contextual cover; deterministic base damage with uncertainty surfaced pre-commitment; reactions/assists/rescues/interrupts with explicit triggers; objectives beyond elimination; seeded replayable simulation.

## 4. Deliberately open — propose, never decide

Narrative: the leader's surviving form; the attacker's identity and objective; why the white-haired recruit is uniquely required; whether restored Operator is continuous/forked/compromised; HQ's nature and location; the leader's name/age/class; the gap duration.

Production: working title; character names/codenames; final Specialty and verb names; the initiative clock math; the AP/reaction economy; the Remote Arsenal command model; tree size and respec rules; gear-slot names; ordinary gameplay gear visibility; Heartless's cost to the synthetic; enemy families; spike target hardware; the 2D/3D balance for S-tier; platform scope beyond PC.

## 5. Resolved visual rulings

The former three art/text conflicts are closed. Use the manifest's current approved sheets: the white-haired human uses the buildable black field costume; the Synthetic inherits the red jacket as a controlled optional garment with cinematic override; the cyborg has two human upper arms and two mechanical forearms/hands. Do not reopen these from superseded hero art.

## 6. Accepted terminology rulings

- **`Integrity`** exclusively names the damage/health pool. It is not a visible attribute and is not renamed to `Frame`.
- The visible attributes are Strength, Agility, Intellect and Resolve. Con Girl scales through Agility plus authored mechanics; `Charisma` is not added as a sixth attribute.

## 7. Character quick reference

**White-haired human — Agility — verb: Exploit / Redirect**
Only fully human member of the definitive trio. Recruited years after the prologue for a concealed, specific reason; Operator requires *her*, not an equivalent operative. Elusive, irreverent, sensually confident, self-interested, improvisational. Faye Valentine energy; Spike Spiegel physical looseness. **She is not the missing leader and must never become a cosmetic recreation of her.** She rejects the empty role the survivors project onto her.
Specialties: **Dispatch** (agile melee assassination), **Con Girl** (charisma manipulation/CC at a DPS tradeoff), **Gunslinger** (mobile ranged execution).
Art: short/tousled pale white-lavender hair. Costume unresolved — see A1.

**Cyborg mech pilot — Strength — verb: Break / Anchor**
Survivor of the original team. The **only** protagonist with a mech; it is *repaired and incrementally upgraded*, never replaced by a different machine. Long layered blonde hair, mechanical arms (see A3), practical blue-gray field clothing. Spent years underground; her body and mech make hiding, maintenance, and trust uniquely hard.
Specialties: **Bulwark** (tank/interception/protection, support through protection not healing), **Remote Arsenal** (operate the mech remotely as a tank while she uses heavy ranged weapons), **Redline** (mech stance dance: Assault / Pursuit / Disengage-Recovery).
The mech magnifies her role. It must never erase her as an individually playable character on the field.

**Luminous synthetic — Intellect — verb: Rewrite / Conduct**
Survivor of the original team. Fully synthetic. Black bob; **literal iridescent luminous skeletal chassis** (not an X-ray effect over a human); black lacquer panels. Traumatized specifically about agency, internal systems, and remote access. Wears the leader's short fitted red bomber jacket in the definitive timeline (see A2).
Specialties: **Heartless** (invasive lightning damage grounded in her conduction anatomy), **Ghost Surgery** (hacking/biohacking CC against machines, augmented organisms, nerves, sensors, command protocols), **Spell Slinger** (conditional battlefield buffs inspired by the original leader's judgment).
**She is not a healer.** Her support is conditional, earned, and never an upkeep spell.
Orthographic sheet exists: `art/SHEET-synthetic-orthographic-001.png` (front/profile/back, nude base body).

**Original human leader (prologue + hidden protector) — green hair**
Fully human, not low-tech. Removable external white/gray AI-assisted armor; short fitted red bomber jacket. Uses a local AI copilot that *advises*; she retains final field authority. Her defining trait is **judgment** — recognizing when the machine-optimal answer is contextually wrong and accepting the cost of overriding it. Distinct entity from Operator.

**Operator**
Female-presenting, dry, composed, observant, psychologically imposing. "GLaDOS-like" = dramatic function and pressure, **not** GLaDOS's voice or comedy. Prologue era: physically present at HQ; ethereal white hair, white clothing with purple details, right arm bare below shoulder armor, left arm sleeved/gloved. Definitive era: a coherent human-shaped hologram.

## 8. Routing to the other project skills

- Combat rules, initiative, effects, determinism, AI → **`combat-kernel`**
- Ability/talent/gear/condition data authoring → **`content-schema`**
- Any art, shader, rig, animation, cinematic, or asset gate → **`visual-gate`**
- Unity project layout, asmdefs, presentation boundary, perf budgets → **`unity-pipeline`**
- Pressure-testing a proposal, adversarial review → **`adversarial-review`**
