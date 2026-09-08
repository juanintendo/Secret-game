# CANON INDEX — mandatory first read

Document ID: `CANON-INDEX`
Location: `docs/canon/CANON-INDEX.md`
Revision date: 2026-09-08
Creative direction: Juan + Mina (Mina owns technical direction)

> **Every agent and every session starts here.** Read this file before reading, citing, or changing anything else in this repository. It is a router, an authority map, a state summary, and a change protocol. It is deliberately **not** a copy of the project's facts — if you want a fact, this file tells you which document owns it.

---

## 1. Project identity

A premium, story-driven tactical RPG about three women rebuilding an elite combat unit destroyed years earlier. The definitive trio has no appointed leader and none of them is incompetent: each begins dangerous and experienced, and the progression the player earns is *synchronization* between them. **The unit is level one, not the characters.** 1990s techno-anime visual language at modern fidelity. Combat is legible before commitment — animation celebrates a decision, it never hides the rules.

## 2. Canon authority order

When two sources disagree, the higher entry governs. **Never average a conflict, and never silently pick a side — cite both and escalate it to §11.**

| # | Authority | Governs |
|---|---|---|
| 1 | `docs/canon/STORY-CANON-001.md` | Narrative truth, chronology, character knowledge, secrets |
| 2 | `docs/canon/90S-ANIME-XCOM-PREPRODUCTION-001.md` | Founding product, visual, combat and production direction |
| 3 | `docs/canon/90S-ANIME-XCOM-CODEX-CLAUDE-BRIEF-001.md` | Progression and combat-class direction, character kits |
| 4 | `art/ART-MANIFEST.md` | **Visual truth.** Which image is canonical, what each establishes, what it cannot settle |
| 5 | `docs/canon/MINA-CONSULTATION-ADDENDUM-001.md` | Juan-approved direction issued after the three documents above were written |
| 6 | `docs/canon/CANON-DECISIONS.md` | Append-only ledger of rulings. A later entry supersedes the specific statement it names |

Two clarifications that override a naive reading:

- **Artwork conflicts resolve at #4, not #1–#3.** See §6.
- **A ruling in #5 or #6 supersedes #1–#3** on the specific point it names, and only that point.

## 3. Canonical paths

```
docs/canon/CANON-INDEX.md                            <- you are here; read first
docs/canon/STORY-CANON-001.md                        <- narrative truth
docs/canon/90S-ANIME-XCOM-PREPRODUCTION-001.md       <- founding product/visual/production
docs/canon/90S-ANIME-XCOM-CODEX-CLAUDE-BRIEF-001.md  <- progression/combat classes
docs/canon/MINA-CONSULTATION-ADDENDUM-001.md         <- Juan-approved later direction
docs/canon/CANON-DECISIONS.md                        <- append-only ruling ledger
art/ART-MANIFEST.md                                  <- visual truth and asset status
art/                                                 <- the approved assets
art/superseded/                                      <- retained history; never cite as support
docs/consult/                                        <- recommendations, NOT canon (see §7)
CLAUDE.md · .claude/skills/                          <- agent routing and execution rules (see §12)
```

Anything not on this list is working material and carries no authority.

## 4. Knowledge status — the five labels

Use these words exactly. Do not invent synonyms, and do not let a statement drift up the list without a ledger entry.

**`SETTLED CANON`** — true in the project. Changed only by Juan and Mina, and recorded in `CANON-DECISIONS.md`. An agent may not soften or reinterpret it while doing other work.

**`CURRENT DESIGN DIRECTION`** — the active plan, strong enough to build against, not yet proven. Build on it, and say so when you do.

**`PROVISIONAL HYPOTHESIS`** — a proposal awaiting a test or a ruling. Prototype it, measure it, report the result. Never present it as decided.

**`OPEN QUESTION`** — deliberately unresolved. Do not resolve it by writing an answer into a document. Propose options; let Juan and Mina rule.

**`SUPERSEDED`** — was stated or shown, and is no longer true. Retained so the same wrong claim is not rediscovered. **Never cite as support.**

There is a sixth thing this project has almost none of: **experimentally validated fact** — something a test could have falsified and did not. Nothing has reached that bar yet. Everything currently believed is drawn, declared or hypothesised.

## 5. Required reading order by task type

Always `CANON-INDEX.md` first. Then:

| If your task is… | Read, in order |
|---|---|
| Story, character, chronology, mystery structure | `STORY-CANON-001`, then `CANON-DECISIONS` |
| Combat rules, initiative, AP, kernel | `CODEX-BRIEF` §4–§5, §10–§11, `PREPRODUCTION` §4–§5, then `CANON-DECISIONS` |
| Specialties, talents, progression, gear | `CODEX-BRIEF` §5–§12, then `MINA-CONSULTATION-ADDENDUM-001` §3 |
| **Anything visual** — art, model sheets, costume, animation, shaders | `art/ART-MANIFEST.md` **first**, then `PREPRODUCTION` §7 and §12, `CODEX-BRIEF` §2 and §13 |
| Engine, architecture, asmdefs, performance | `PREPRODUCTION` §10–§11, `CODEX-BRIEF` §14–§15 |
| Production sequencing, slices, gates | `PREPRODUCTION` §13–§14, `CODEX-BRIEF` §16, then §10 below |

If your task spans two rows, read both. If you find yourself reading everything for a small change, you are probably about to duplicate a fact — see §12.

## 6. Artwork resolves through the manifest

`art/ART-MANIFEST.md` is the single authority on visual truth.

- A prose description of a character anywhere in `docs/canon/` is a *summary* of canonical art. Where prose and a `CANONICAL` image disagree, **the image governs and the prose is corrected.**
- An image is not canonical because it exists in `art/`. Status is assigned explicitly, per image, in the manifest.
- The manifest records, per image, **what that image cannot settle.** Those are open questions; do not close one by inference from a single drawing.
- **A generated image is construction evidence, not metrology.** Never derive a measurement from a drawn grid, ruler or bracket and treat it as a dimension.

## 7. Consultation documents cannot override canon

Anything under `docs/consult/` is **analysis and recommendation**. It ranks below every entry in §2 and never becomes canon by being useful, detailed, confident or repeated. A recommendation becomes binding only when Juan approves it — with Mina on technical questions — and the approval is written into `CANON-DECISIONS.md`.

Consult documents may also contain claims that were correct when written and are now wrong, particularly visual claims made before current artwork existed. **Cross-check any consult claim about art against the manifest before repeating it.**

## 8. External and duplicated documents are snapshots

Copies of these documents exist outside the repository, notably in `~/Documents/DD90s/`.

**The repository is the source of truth. External copies are historical snapshots.** Read them for history; do not treat them as current; do not edit or delete them. A snapshot is promoted only by an explicit instruction recorded in `CANON-DECISIONS.md`. If a snapshot appears newer than the repository copy, that is a finding to report, not a licence to overwrite.

## 9. Current verified state

**Settled and verified against current art:**

- Three protagonists, one primary axis each: white-haired human = **Agility**, cyborg = **Strength**, synthetic = **Intellect**. All of a character's Specialties scale from her single axis.
- **Charisma is not an attribute.** It describes Con Girl's fantasy and behaviour. Con Girl scales through Agility.
- **Equipment: `1 weapon + 4 gear`.** Only the four gear pieces count toward set thresholds; the weapon never does. Combinable 2+2, or one full 4-piece. Character-specific weapon classes. `mech-hardpoint` occupies one of the cyborg's four. **Never `5 gear + weapon`.**
- **Pilot and mech keep separate, stable logical entity IDs** regardless of deployment mode. Embarked: pilot authoritative but non-spatial, non-selectable, non-targetable, no initiative slot; mech owns occupancy. Remote: both may be spatial.
- **The kernel is dimensionless** — integer cell coordinates, no metres. `cellSizeMeters` is a Unity presentation field.
- **The Synthetic's red-jacket toggle** is cosmetic visibility only: one model, one rig, one garment layer. Cinematics override it.
- **The minimum graphics preset is a separately tuned art target**, never a disabled visual system.
- **Operator:** right arm bare and ungloved, left sleeved and gloved; the sleeve begins below the shoulder cap over a bare section of upper arm. Fully specified.
- **Cyborg:** bilateral, symmetric, mechanical from the elbow.
- **Synthetic:** wears the leader's red bomber jacket in the definitive era, design-identical to the original.
- **White-haired human:** costume canon as drawn; contains no prohibited trim.
- No member of the definitive trio is the permanent leader.

**Current design direction:** the combat chassis in `CODEX-BRIEF` §4; nine Specialties and three Personal trees; recovery without a healer; synchronization; Unity 6 URP pending the technical-art spike.

**Provisional hypothesis, explicitly not canon:** the mech's **2×2** logical footprint, and the ≈3.6 m / ≈2.05× scale. Untested until the spike.

## 10. Immediate next objective

Two different "firsts" exist and must not be confused:

- **Engineering proof — first.** One small representative scenario: forecast legibility, one action per protagonist through her own axis and verb, a conditional synthetic rescue created intentionally by positioning, forced movement, and a three-character handoff generating Synchronization. Needs no finished art.
- **Narrative production target — first major.** The prologue. It depends on two extra canonical characters, a costume state change that does not yet exist, and a working Limit Break system.

**Sequencing: prove the scenario before building the prologue.** Both remain on the plan.

**Do not begin character modelling** until that scenario has been played and judged by someone who did not design it.

## 11. Blockers — rulings required from Juan and Mina

**Three, and only three, block the combat kernel.**

| # | Question | Owner |
|---|---|---|
| 1 | Is `Integrity` the **attribute** or the **health pool**? `CODEX-BRIEF` §5 and §10 use it as both. | Juan + Mina |
| 2 | Are there binary to-hit rolls, or is damage deterministic with uncertainty moved into enemy intent and hidden information? | Mina |
| 3 | Is the mech's logical footprint **2×2**? Provisional and untested; settled only by the spike, never by adopting the hypothesis. | Juan + Mina |

Everything else is work rather than a decision: naming the four gear slots, whether gameplay displays equipped gear, the minimum supported hardware, deployment modes by Specialty, and the visual-scale comparison for `cellSizeMeters`.

## 12. Change protocol

A fact lives in exactly one place and everything else points at it.

| Kind of fact | Home |
|---|---|
| Narrative truth, chronology, who knows what | `STORY-CANON-001.md` |
| Product, visual, combat and production foundations | `90S-ANIME-XCOM-PREPRODUCTION-001.md` |
| Progression, Specialties, kits | `90S-ANIME-XCOM-CODEX-CLAUDE-BRIEF-001.md` |
| What a character looks like; asset status | `art/ART-MANIFEST.md` |
| That a ruling was made, when, and by whom | `CANON-DECISIONS.md` |

**Recording a change:**

1. Write the fact **once**, in its home document, at the status label it has earned (§4).
2. Add one line to `CANON-DECISIONS.md`: date, decision, status, authority, documents or assets affected, and what it supersedes. The ledger records *that* the ruling exists — it does not restate the prose.
3. If the change makes an older statement wrong, correct it in place and mark the old claim `SUPERSEDED` where a reader might still meet it. Never leave two live versions.
4. Update §9 here if the verified state changed, and §11 if a blocker resolved or appeared.
5. Never overwrite a `-001` document with a differently-scoped rewrite. New scope gets a new versioned document and an authority-order entry here.

**Skills** (`.claude/skills/`) are execution aids. A skill may carry the compact invariants an agent needs while working, but it **must not become a competing source of truth.** When a canon fact changes, the skill's summary is corrected in the same commit or removed; a stale skill is worse than a thin one.

**When you find a contradiction:** do not resolve it by choosing. Record both citations, add the question to §11, and tell Juan and Mina. The only exception is prose-versus-art, which §6 already resolves in favour of the art.

## 13. Known pending at this checkpoint

This checkpoint is deliberately scoped to the provenance layer. The following are **known stale and are not yet corrected** — do not treat them as current:

- **`CLAUDE.md` and the six skills in `.claude/skills/`** still describe three art-versus-text conflicts (the human's costume, the synthetic's jacket, the cyborg's arms) as unresolved. **All three are resolved** — see §9 and the manifest. `canon-guard` §5 and §6 and `visual-gate` §1 and §4 are the affected sections.
- **`docs/consult/CONSULT-CLAUDE-001.md`** is partially obsolete for the same reason and is mid-correction.
- **`docs/canon/art/`** still holds two files that are byte-identical duplicates of assets now in `art/`. They should be removed from version control.
- **No shared proportion chart exists** across the cast, and no per-asset approval record exists in the manifest.

**Where this file and a skill disagree, this file governs.** Correcting the skills is the next checkpoint.
