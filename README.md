# Secret-game

Working title TBD. A premium story-driven tactical RPG about three women rebuilding a combat unit after the destruction of a near-perfect one. Fixed trio, no appointed leader, earned synchronization. 1990s techno-anime design language at modern fidelity.

Creative direction: Juan + Mina. Technical direction: Mina.

## Status

Implementation spike in progress. The engine-free deterministic simulation began on 2026-09-09; Unity 6 LTS + URP remains **provisional** until the technical-art spike passes.

## Layout

```
docs/canon/      Source-of-truth documents (authority order below) + approved art
docs/consult/    External consultant analysis — recommendations, not canon
docs/consultation/ Final preproduction consultation and implementation handoff
docs/implementation/ Implementation milestones and accepted technical rulings
src/             Engine-free simulation source
tests/           Executable simulation verification harnesses
docs/pipeline/   Production pipeline artifacts (perf budgets, swatches, shot registry)
.claude/skills/  Project skills that load the canon and the working rules into any agent session
CLAUDE.md        Agent entry point and skill routing
```

## Authority order

1. `docs/canon/STORY-CANON-001.md`
2. `docs/canon/90S-ANIME-XCOM-PREPRODUCTION-001.md`
3. `docs/canon/90S-ANIME-XCOM-CODEX-CLAUDE-BRIEF-001.md`
4. `docs/canon/CANON-DECISIONS.md`
5. `docs/consultation/` (recommendations and handoff; accepted rulings are recorded separately)

Conflicts are reported and cited, never silently averaged.

## Working with an agent here

Any Claude Code session should invoke the `canon-guard` skill first. See `CLAUDE.md`.

## First executable milestone

With .NET 8 installed:

```powershell
dotnet run --project tests/SecretGame.Simulation.Tests
```

The harness verifies deterministic spatial rules, rectangular footprints, stable pilot/mech identities, initiative queries, command validation, ordered events, replay, and exact forecast/result parity across a 500-pair corpus. The simulation boundary check is `scripts/check-simulation-boundary.sh` on Bash-capable CI runners.

Run the current text-mode encounter with:

```powershell
dotnet run --project tools/SecretGame.RelayYard
```

The live transition criteria for Unity/Astra are tracked in `docs/implementation/GATE-A-CHECKLIST.md`.

## Canon home

The repository is now the maintained source of truth. `DD90s` was the preproduction staging location and must not be edited as a competing canon home.
