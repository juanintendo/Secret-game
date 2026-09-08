# CANON DECISIONS — append-only ledger

Document ID: `CANON-DECISIONS`
Location: `docs/canon/CANON-DECISIONS.md`
Authority: entry 6 in the order defined by `docs/canon/CANON-INDEX.md` §2.

> **Append only.** Never edit or delete a past entry. A decision that changes is superseded by a new entry that names it.
>
> This ledger records **that** a ruling exists, when, by whom, and what it touches. It does not restate the prose — the fact lives in its home document (`CANON-INDEX.md` §12).
>
> Status values: `SETTLED CANON` · `CURRENT DESIGN DIRECTION` · `PROVISIONAL HYPOTHESIS` · `OPEN QUESTION` · `SUPERSEDED` (defined in `CANON-INDEX.md` §4).

---

### CD-0001 — The repository is the canonical source

- **Date:** 2026-09-08 · **Status:** `SETTLED CANON` · **Authority:** Juan
- **Decision:** This repository is the active source of truth. Copies outside it, notably `~/Documents/DD90s/`, are historical snapshots — readable, never authoritative, never edited or deleted, promoted only by an explicit instruction recorded here.
- **Affects:** `CANON-INDEX.md` §8; all external copies
- **Supersedes:** the prior ambiguity in which `DD90s` and the repository held equal-standing copies

### CD-0002 — `CANON-INDEX.md` is the mandatory entry point

- **Date:** 2026-09-08 · **Status:** `SETTLED CANON` · **Authority:** Juan
- **Decision:** Every agent and every session reads `docs/canon/CANON-INDEX.md` before reading, citing or changing anything else.
- **Affects:** `CANON-INDEX.md`; `CLAUDE.md`; all skills — **`CLAUDE.md` is not yet updated; see CD-0016**
- **Supersedes:** —

### CD-0003 — Artwork canon resolves through the art manifest

- **Date:** 2026-09-08 · **Status:** `SETTLED CANON` · **Authority:** Juan
- **Decision:** `art/ART-MANIFEST.md` is the single authority on visual truth. Where prose conflicts with an image labelled `CANONICAL`, the image governs and the prose is corrected. An image has no status until the manifest assigns one.
- **Affects:** `art/ART-MANIFEST.md`; `CANON-INDEX.md` §6; all three canon documents; `visual-gate` and `canon-guard` skills
- **Supersedes:** —

### CD-0004 — Charisma is not an attribute; Con Girl scales through Agility

- **Date:** 2026-09-08 · **Status:** `SETTLED CANON` · **Authority:** Juan
- **Decision:** "Charisma" describes Con Girl's character quality and combat fantasy — deception, provocation, leverage, behavioural manipulation, and the animation, dialogue, targeting rules and crowd-control effects that express them. It is **not** a stat and is not added to the global stat sheet. Agility governs Con Girl's numerical scaling.
- **Affects:** `CODEX-BRIEF` §5 and §7.2; `STORY-CANON-001.md` §3.1; `content-schema` skill
- **Supersedes:** the description of Con Girl as "Charisma-driven"

### CD-0005 — One primary axis per character, across all her Specialties

- **Date:** 2026-09-08 · **Status:** `SETTLED CANON` · **Authority:** Juan
- **Decision:** White-haired human = **Agility** (Dispatch, Con Girl, Gunslinger). Cyborg = **Strength** (Bulwark, Remote Arsenal, Redline). Synthetic = **Intellect** (Heartless, Ghost Surgery, Spell Slinger). No Specialty scales off another character's axis or a new attribute.
- **Affects:** `CODEX-BRIEF` §5, §7–§9; `content-schema` validator rules
- **Supersedes:** —

### CD-0006 — The synthetic's inherited red bomber jacket is definitive-era canon

- **Date:** 2026-09-08 · **Status:** `SETTLED CANON` · **Authority:** Juan; verified against current artwork
- **Decision:** In the main timeline the luminous synthetic wears the original leader's short fitted red bomber jacket. Confirmed in `art/HERO-definitive-trio-002.png` and `art/SHEET-synthetic-jacket-001.png`, and **design-identical** to the leader's — same cut, ribbed cuffs and hem, black rectangular sleeve patch, cyan-dotted zipper tape. Inherited clothing with narrative weight, not a similar garment.
- **Affects:** manifest entries 2, 6, 7; `STORY-CANON-001.md` §2.3; `CODEX-BRIEF` §3.2
- **Supersedes:** the consultation finding that the jacket was canon in text but absent from art — drawn from `art/superseded/HERO-definitive-trio-001.png`

### CD-0007 — The white-haired human's black costume is canon and is not a trim violation

- **Date:** 2026-09-08 · **Status:** `SETTLED CANON` · **Authority:** Juan; verified against current artwork
- **Decision:** Her costume as shown in `art/HERO-definitive-trio-002.png` and `art/SHEET-human-field-001.png` is canonical. The production prohibition targets fur, feathers, fuzzy trim and uncontrolled particulate trim — none of which it contains. Straps, an open jacket, fitted clothing and heeled boots are **not** automatically non-canonical; their production costs are assigned in the manifest rather than treated as grounds for simplification.
- **Affects:** manifest entries 2, 3; `STORY-CANON-001.md` §3.1; `PREPRODUCTION` §3 Reference 01; `CODEX-BRIEF` §3.2; `visual-gate` skill
- **Supersedes:** the consultation finding that her costume contradicted the production brief; and the Reference 01 reading describing a "feathered or particulate outer texture", which described an early moodboard image

### CD-0008 — Cyborg arm topology: bilateral, symmetric, mechanical from the elbow

- **Date:** 2026-09-08 · **Status:** `SETTLED CANON` · **Authority:** Juan; established by `art/SHEET-cyborg-field-001.png`
- **Decision:** Both arms are mechanical from the elbow — dark articulated elbow assembly, white plated forearm, dark wrist cuff with a cyan indicator, five-digit hand. Construction is symmetric left and right. `STORY-CANON-001.md` §2.2's "mechanical forearms/hands" is the precise phrasing.
- **Affects:** manifest entries 1, 2, 4; `STORY-CANON-001.md` §2.2; `CODEX-BRIEF` §3.1
- **Supersedes:** **CD-0008 replaces the earlier `OPEN QUESTION` status of this topic.** It also supersedes the consultation claim that the art showed a full right arm plus a left forearm — unsupported; her right arm was simply not visible in the superseded image.

### CD-0009 — Scenario-first and prologue-first are two tracks, not a contradiction

- **Date:** 2026-09-08 · **Status:** `CURRENT DESIGN DIRECTION` · **Authority:** Juan
- **Decision:** The small representative scenario is the **first internal engineering and combat proof**. The prologue is the **first major narrative production target**. Both remain on the plan; prove the scenario first.
- **Affects:** `CANON-INDEX.md` §10; `PREPRODUCTION` §13; `CODEX-BRIEF` §20
- **Supersedes:** the reading of these sections as a contradiction requiring one to be discarded

### CD-0010 — Operator's arm asymmetry is settled visual canon

- **Date:** 2026-09-08 · **Status:** `SETTLED CANON` · **Authority:** Juan, via `MINA-CONSULTATION-ADDENDUM-001.md` §1
- **Decision:** The anatomical **right** arm is exposed below the shoulder armor with no sleeve, glove, bracer or wrist covering. The **left** arm carries the white sleeve and white glove with purple detailing. A left-facing profile shows only the near left sleeved arm; the far right arm must never appear through the torso. The sleeve begins **below** the shoulder cap over a short section of bare upper arm, finished with a purple band; shoulder caps are identical on both sides.
- **Affects:** manifest entries 1, 2, 8, 10; `STORY-CANON-001.md` §2.4; `CODEX-BRIEF` §3.1
- **Supersedes:** the consultation claim that the Operator sheet showed two sleeved arms — a consultant error from reading a low-contrast region at reduced scale. There was never a conflict.

### CD-0011 — Equipment structure: `1 weapon + 4 gear`

- **Date:** 2026-09-08 · **Status:** `SETTLED CANON` · **Authority:** Juan, via `MINA-CONSULTATION-ADDENDUM-001.md` §3
- **Decision:** Exactly **four gear slots** per protagonist plus **one separate character-specific weapon slot**. **Only the four gear pieces participate in set bonuses; the weapon never counts toward a set threshold.** Two different two-piece bonuses may combine, or one complete four-piece set. Weapons are not interchangeable between protagonists. `mech-hardpoint`, if retained, occupies one of the cyborg's four gear slots — not a fifth gear slot or sixth equip position. Cinematics always use the authored canonical loadout.
- **Affects:** `CODEX-BRIEF` §5, §12; `content-schema` skill
- **Supersedes:** the reading of `CODEX-BRIEF` §12's five categories as five gear slots. They reconcile only as `1 weapon + 4 gear` — **never `5 gear + weapon`.**

### CD-0012 — The Synthetic's red-jacket visibility toggle

- **Date:** 2026-09-08 · **Status:** `SETTLED CANON` · **Authority:** Juan, via `MINA-CONSULTATION-ADDENDUM-001.md` §4
- **Decision:** Only the Synthetic receives a character-equipment UI option to show or hide the inherited red jacket. It is a **cosmetic visibility toggle** — not a stat, skill, slot, item or alternate body. Both states use the **same canonical model and rig**. Authored cinematics override the toggle. Portraits, UI renders, shadows, VFX anchors, hitboxes, targeting, animation timing and simulation must not diverge. Implemented as **one controlled optional garment layer**, never a duplicated character.
- **Affects:** manifest entries 5, 6; presentation architecture; asset validation
- **Supersedes:** —

### CD-0013 — Graphics and performance policy

- **Date:** 2026-09-08 · **Status:** `SETTLED CANON` · **Authority:** Juan, via `MINA-CONSULTATION-ADDENDUM-001.md` §5
- **Decision:** Maximum visual quality at the highest preset, plus industry-standard graphics and accessibility settings. **The lowest supported preset is a separately tuned art target** — intentionally art-directed, stable, legible and polished. It may be simpler; it must never look broken, muddy, generic or abandoned. **A low preset is not created by globally disabling the visual system.** The addendum enumerates what may scale and what must be preserved.
- **Affects:** technical-art pipeline; performance budgets; spike acceptance criteria
- **Supersedes:** —

### CD-0014 — Pilot and mech keep separate, stable logical entity IDs

- **Date:** 2026-09-08 · **Status:** `SETTLED CANON` · **Authority:** Mina (technical direction)
- **Decision:** Pilot and mech retain **separate, stable logical entity IDs in combat state regardless of deployment mode**. Embarked or docked: the pilot remains in authoritative state but is **non-spatial, non-selectable, non-targetable, with no independent initiative slot**; the mech owns battlefield occupancy and is the active selectable unit. In remote operation both may be spatial and addressable. Initiative, AP sharing, command bandwidth and queued directives remain prototype questions. **The ruling does not determine which Specialties permit each mode.**
- **Affects:** combat architecture; unit model; save and replay format
- **Supersedes:** the treatment of the pilot/mech entity model as an unresolved architecture blocker

### CD-0015 — The kernel is dimensionless; a generated study is not metrology

- **Date:** 2026-09-08 · **Status:** `SETTLED CANON` · **Authority:** Mina (technical direction)
- **Decision:** Four concepts stay separate — **logical kernel cell** (integer coordinates, no metres) · **entity footprint in logical cells** · **Unity `cellSizeMeters`** (configurable presentation) · **decorative or subdivided grid lines in visual studies** (no dimensional meaning). **The headless kernel uses integer cell coordinates and remains independent of metres.** Do not adopt a tile size derived from a generated study's drawn grid: **the image is construction evidence, not authoritative metrology.**
- **Affects:** combat architecture; `art/ART-MANIFEST.md` entry 11; visual-scale comparison
- **Supersedes:** the recommendation to adopt a 0.5 m tile because it reconciled the study's drawn grid — **retracted**

### CD-0016 — Provenance checkpoint; skills and consultation deliberately not updated

- **Date:** 2026-09-08 · **Status:** `SETTLED CANON` (scope of this checkpoint) · **Authority:** Juan
- **Decision:** This checkpoint transfers the provenance layer only — `CANON-INDEX.md`, `CANON-DECISIONS.md`, `art/ART-MANIFEST.md`, the approved art at canonical paths, the addendum, and the corrected canon documents. **`README.md`, `CLAUDE.md`, the six skills and `docs/consult/CONSULT-CLAUDE-001.md` are deliberately untouched** and are known stale: they still describe the three art-versus-text conflicts resolved by CD-0006, CD-0007 and CD-0008. Correcting them is the next checkpoint.
- **Affects:** `CANON-INDEX.md` §13; `CLAUDE.md`; `.claude/skills/*`; `docs/consult/CONSULT-CLAUDE-001.md`
- **Supersedes:** —

### CD-0017 — Mech scale and footprint remain a provisional hypothesis

- **Date:** 2026-09-08 · **Status:** `PROVISIONAL HYPOTHESIS` · **Authority:** Mina; **explicitly not canon**
- **Decision:** Prototype the mech at ≈**3.6 m** beside a ≈**1.75 m** pilot (ratio ≈2.05×), with a **2×2** logical footprint against a human **1×1**, and a rear-upper-torso top-entry hatch. **It becomes binding only if the technical spike validates** doors and traversal, narrow-map pathfinding, cover interaction, occupancy and forced movement, camera framing, animation clearance, encounter-space cost and remote-control readability. **If 2×2 fails, do not silently compress the same broad mech into 1×1** — escalate the conflict between visual scale and tactical footprint.
- **Affects:** `art/ART-MANIFEST.md` entry 11; grid design; `CANON-INDEX.md` §11 blocker 3
- **Supersedes:** —

---

## How to add an entry

Append at the bottom. Next free ID. Six fields, nothing else:

```
### CD-00NN — one-line decision title

- **Date:** YYYY-MM-DD · **Status:** one of the five labels · **Authority:** who ruled
- **Decision:** what was decided, in two or three sentences at most
- **Affects:** documents and assets that must change or already changed
- **Supersedes:** the decision or claim this replaces, or —
```

Then correct the fact in its **home document** (`CANON-INDEX.md` §12) — once, not everywhere.
