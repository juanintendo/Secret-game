# Secret-game

Working title TBD. A premium story-driven tactical RPG about three women rebuilding a combat unit after the destruction of a near-perfect one. Fixed trio, no appointed leader, earned synchronization. 1990s techno-anime design language at modern fidelity.

Creative direction: Juan + Mina. Technical direction: Mina.

## Status

Pre-production. No engine code yet. Engine recommendation (Unity 6 LTS + URP) is **provisional** until the two-week technical-art spike passes.

## Layout

```
docs/canon/      Source-of-truth documents (authority order below) + approved art
docs/consult/    External consultant analysis — recommendations, not canon
docs/pipeline/   Production pipeline artifacts (perf budgets, swatches, shot registry)
.claude/skills/  Project skills that load the canon and the working rules into any agent session
CLAUDE.md        Agent entry point and skill routing
```

## Authority order

1. `docs/canon/STORY-CANON-001.md`
2. `docs/canon/90S-ANIME-XCOM-PREPRODUCTION-001.md`
3. `docs/canon/90S-ANIME-XCOM-CODEX-CLAUDE-BRIEF-001.md`
4. `docs/consult/CONSULT-CLAUDE-001.md` (recommendations only)

Conflicts are reported and cited, never silently averaged.

## Working with an agent here

Any Claude Code session should invoke the `canon-guard` skill first. See `CLAUDE.md`.

## Note on duplicated canon

`docs/canon/*.md` were copied from `~/Documents/DD90s/` on 2026-09-08 and were byte-identical at that time. **Pick one home.** The recommendation is that this repo becomes the single source of truth and the `DD90s` copies are retired, otherwise the two will diverge silently.
