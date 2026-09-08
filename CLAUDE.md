# Secret-game — agent instructions

A premium story-driven tactical RPG: three women rebuild a destroyed combat unit. 1990s techno-anime presentation at modern fidelity. Fixed trio, no leader, earned synchronization.

## Read this first, every session

**Invoke the `canon-guard` skill before doing anything on this project.** It holds the authority order, the binding canon, the deliberately-open questions, and the three unresolved art-vs-text conflicts. Working without it produces confidently wrong output.

## Skill routing

| Task | Skill |
| --- | --- |
| Anything at all on this project — start here | `canon-guard` |
| Combat rules, initiative, effects, determinism, AI, forecast | `combat-kernel` |
| Abilities, talents, gear, conditions, enemies — authoring or editing data | `content-schema` |
| Art, shaders, rigs, animation, VFX, cinematics, asset approval | `visual-gate` |
| Unity project structure, asmdefs, Timeline, perf budgets | `unity-pipeline` |
| Reviewing or pressure-testing a proposal | `adversarial-review` |

## Authority order

1. `docs/canon/STORY-CANON-001.md` — narrative truth
2. `docs/canon/90S-ANIME-XCOM-PREPRODUCTION-001.md` — founding product, combat, visual, production
3. `docs/canon/90S-ANIME-XCOM-CODEX-CLAUDE-BRIEF-001.md` — progression, Specialties, recovery, pipeline
4. `docs/consult/CONSULT-CLAUDE-001.md` — **recommendations only**, not canon, until Juan or Mina accepts them

Conflicts get **reported and cited**, never silently averaged.

## The rules that get violated most

- **Do not import** from Juan's other projects (beat'em-up, House, Ozymandias, Spell Out, Arcadio).
- **Do not resolve open questions.** Propose; do not decide.
- **Do not rename** characters, Specialties, or verbs except to report a real terminology collision.
- **Label every suggestion as a recommendation.**
- **The simulation is engine-free and deterministic.** `Game.Core`, `Game.Tactics`, `Game.Abilities`, `Game.Forecast`, and the AI's evaluation path must compile with no `UnityEngine` reference. No floats in the sim.
- **The forecast is produced by the same resolver as the result.** UI, AI, and tests all consume `Game.Forecast`. Never reimplement a formula.
- **An ability costs animation; a talent must not.** A talent that needs a new clip is an ability.
- **The synthetic is not a healer.** Recovery is manufactured by play.
- **No generated pixels become canonical character assets**, ever.

## Current state

Pre-production. No engine code yet. The repo currently holds canon documents, the consultant analysis, and these skills.

Immediate objective (per `docs/canon/90S-ANIME-XCOM-CODEX-CLAUDE-BRIEF-001.md` §20 and the consult's §14): **one compact combat scenario** that demonstrates the whole thesis — forecast, three distinct verbs, a deliberately constructed conditional rescue, forced movement, stance choice, earned recovery, a three-character handoff, and character identity preserved at gameplay distance. Do not start by building talent trees.

## Working conventions

- Documents are versioned by name (`-001`), not overwritten. Bump the number for a new revision.
- Content data is JSON under `content/`; ScriptableObjects are generated mirrors, never hand-authored.
- Every playtest writes `{seed, initialState, commandList}` and becomes a replay regression test.
- Commit messages end with the standard co-author trailer.
