# Milestone 019 — verified Unity 6.3.23f1 Windows baseline

## Outcome

The managed Unity bootstrap passed on Juan's Windows machine:

- total: 3;
- passed: 3;
- failed: 0;
- skipped: 0;
- result: `Passed`;
- Unity exit code: 0;
- leaked weak pointers: none reported.

The tests cover both exact Gate A replay hashes, the absence of a UnityEngine reference from the Simulation assembly and the presentation inbox boundary that excludes authoritative `CombatState`.

## Adopted baseline

- Unity 6.3 LTS `6000.3.23f1` (`09d2ecc7fb28`);
- Universal Render Pipeline `17.3.0`;
- Unity Test Framework `1.6.0`;
- resolved dependency graph recorded in `Packages/packages-lock.json`;
- generated initial `ProjectSettings` and URP global/default volume assets committed as the reproducible project baseline.

This supersedes the unexecuted `6000.3.0f1` bootstrap hypothesis. Future editor or package upgrades require an explicit commit and clean replay tests.

## Security and compilation evidence

- Smart App Control remained enabled.
- Code Integrity events proved unsigned Burst JIT cache DLLs were initially blocked.
- The successful run disabled Burst only for the batch bootstrap process.
- The 31 authoritative kernel sources were copied byte-for-byte and verified by SHA-256.
- Unity compiled that mirror with C# 10, nullable enabled and a generated `IsExternalInit` marker.
- The simulation source contains no UnityEngine dependency.

## Repository hygiene

Unity `Library`, `Temp`, generated kernel mirror and test results remain ignored. .NET `bin` directories are now ignored explicitly. The committed baseline contains only package declarations/lock, project settings and required URP assets with their metadata.

## Remaining verification

The Simulation harness passed 33/33 under .NET SDK 8.0.425. The Tactics harness result after restoring the solution's C# 11 boundary remains pending external confirmation and must not be inferred from the Unity result.

## Next implementation gate

The next system experiment is the engine-free cyborg/mech deployment and Charge economy described as a non-canon proposal in `docs/design/CYBORG-MECH-SYSTEM-PROPOSAL-001.md`. Visual production remains gated behind that falsifying experiment; the next Unity visual work is a bounded Relay Yard blockout, not character modelling.
