# CLAUDE CONSULTATION HANDOFF 001

**Consulting phase: closed, 2026-09-08.**
Working source: `C:\Users\colom\Documents\DD90s`
Implementation lead: Mina, primarily via OpenAI Astra. Creative direction: Juan + Mina.

This is a router and a starting gun. It adds no scope. Everything it points at is already written.

---

## 1. The four documents

| File | What it is |
|---|---|
| `CLAUDE-TECHNICAL-CONSULTATION-001.md` | The analysis. 22 sections. Read §1, §21, §22 first. |
| `CLAUDE-QUESTIONS-001.md` | Every open question, grouped by what it blocks. **The single home for questions** — update it, don't scatter answers through chat. |
| `CLAUDE-RISK-REGISTER-001.md` | 32 entries, ranked by cost of learning late. Review at each spike gate. |
| `CLAUDE-TWO-WEEK-SPIKE-001.md` | Day-by-day plan with pass/fail thresholds and two gates. |

Nothing in any of them is canon. Every claim is labelled `CANON OBSERVED`, `RECOMMENDATION`, `OPEN QUESTION`, `RISK` or `REQUIRES PROTOTYPE`.

---

## 2. Verdict, in one paragraph

The thesis is coherent and the visual canon is strong — six orthographic sheets, two team hero images, a scale study and a dedicated construction reference. **The mechanical canon has not moved, and that gap is the project's central risk.** The art is now developed enough that starting character production feels reasonable, which is precisely the trap `CODEX-BRIEF` §20 warns against. Three rulings and one two-week spike separate the project from a proven thesis. **Do not start modelling before the first encounter has been played and judged.**

---

## 3. What is settled

`CANON OBSERVED`. Not to be relitigated.

- **Attribute axes.** Human = Agility (all three Specialties, Con Girl included) · Cyborg = Strength · Synthetic = Intellect. **Charisma is not an attribute.**
- **Equipment.** `1 weapon + 4 gear`. Only the four gear pieces count toward set thresholds; **the weapon never does**. Combinable 2+2, or one full 4-piece. Character-specific weapon classes. `mech-hardpoint` occupies one of the cyborg's four. **Never `5 gear + weapon`.**
- **Pilot/mech entity model.** Separate, stable logical entity IDs regardless of deployment mode. Embarked: pilot authoritative but non-spatial, non-selectable, non-targetable, no initiative slot; mech owns occupancy. Remote: both may be spatial.
- **The kernel is dimensionless.** Integer cell coordinates. No metres in the simulation. `cellSizeMeters` is a Unity presentation field.
- **Synthetic's jacket toggle.** Cosmetic visibility only, one rig, one garment layer. Cinematics override it.
- **Graphics floor.** The minimum preset is a separately tuned art target, never a disabled visual system.
- **Operator's arm asymmetry.** Right bare and ungloved, left sleeved and gloved; sleeve begins below the shoulder cap over a bare section of upper arm. Fully specified, nothing open.
- **Cyborg's arms.** Bilateral, symmetric, mechanical from the elbow.
- **Sequencing.** Small representative scenario = first engineering proof. Prologue = first major narrative production target. Prove the scenario first.

---

## 4. What remains open

### The three architecture blockers

Nothing else gates the kernel.

| # | Decision | Owner | By |
|---|---|---|---|
| **Q2** | `Integrity` — attribute or pool? | Mina | Before day 1 |
| **Q3** | To-hit rolls — yes or no? | Mina | Before day 1 |
| **Q4** | Mech footprint in **logical cells** — is it 2×2? | Juan + Mina | Spike Gate A |

Q2 and Q3 are afternoon rulings. **Q4 is a spike outcome, not an armchair ruling** — it closes when the seven validations in consultation §7.6 pass, and **not** by adopting the hypothesis as canon. Q32 (`cellSizeMeters`) sits inside Q4 as a week-2 visual comparison.

### Also needed before day 1

**Q30 — name the minimum supported machine.** Every performance budget is unmeasurable without it, and profiling must happen on that machine, not a workstation.

### Blocks modelling, not the kernel

**Q27** — does gameplay display equipped gear? The largest cost fork in the project. **Q26** — name the four gear slots (structure settled; naming only). **Q11** — one shared proportion chart across all six principals.

### Work that is settled but not yet done

**Q5** — transcribe the Con Girl / Agility ruling into `CODEX-BRIEF` §5 and §7.2 and `STORY-CANON-001.md` §3.1. Anyone reading `DD90s` today would still implement a Charisma stat. **Q6** — art manifest with SHA-256, status labels and revision names. **Do not delete historical sheets**; supersession is recorded by status.

### Marked for removal

The `FOR SCIENTIFIC EDUCATIONAL USE ONLY` text on `mech-scale-footprint-study-v1.png` is a generation artifact. Remove it in a **new revision** (`-v2`), together with the misleading `GRID = 1 m × 1 m` legend and the `2×2` / `1×1` brackets, which do not match the drawn squares. Keep the height ruler — that part was sound.

---

## 5. First implementation milestone

**Build the text-mode forecast harness. One week. Zero art. Not in Unity.**

A headless implementation of consultation §7's ruleset in which `Game.Forecast` produces `conditionsOpened` / `conditionsClosed`, rendered as ASCII, playing the §16 "Relay Yard" encounter, replayable from `{seed, initialState, commandLog}`.

**Non-negotiable properties, asserted in CI from the first commit:**

1. No `UnityEngine` reference in any simulation assembly.
2. No float literal in any content numeric field.
3. **No metre, world-space unit or float distance anywhere in the simulation.**
4. Forecast parity — applying a command produces exactly what the forecast promised, across ≥ 500 recorded pairs.
5. `initialState + commandLog → identical finalState`, on two different machines.

**It is done when** a person who did not design it can say, before committing a move, what conditions that move will create — in ≥ 4 of 5 plays.

**If that fails, stop.** Redesign the forecast presentation before entering week 2. No amount of Unity work rescues an illegible mechanic, and continuing would spend the engine budget on a thesis already known to be broken.

---

## 6. The one thing worth repeating

The consultation separates evidence into four classes: **visual construction evidence · declared project direction · provisional hypothesis · experimentally validated fact.**

**The fourth class is empty.**

Everything this project believes today is drawn, declared or hypothesised. Nothing has yet been put to a test that could have failed and survived it. That is not a criticism of the work — preproduction is supposed to look like this. It is the reason the spike exists, and the reason its first week deliberately produces no pixels.

The first entry in that fourth column should be the forecast harness.

---

## 7. Standing cautions

- **Do not start character modelling** until the §16 encounter has been played and judged by someone who did not design it.
- **If 2×2 fails validation, escalate.** Do not compress the mech to 1×1, and do not enlarge every map to rescue it.
- **Do not treat a generated image as metrology.** It is construction evidence. This has already cost the project once.
- **Read visual details at magnification.** Two wrong findings in this project came from reading sheets at reduced scale — one of them mine.
- **A reclassified question keeps its risk.** Moving something out of the blocker list does not retire the register entry behind it.

---

**No GitHub operations were performed at any point in this consultation. No file outside `DD90s` was modified. Nothing was implemented, committed or pushed.**
