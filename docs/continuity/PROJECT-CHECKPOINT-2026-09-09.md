# Project checkpoint — 2026-09-09

**Use this file to resume in a new conversation.** Canon still follows the authority order in the root `README.md`; this is an operational checkpoint, not a replacement canon document.

## Current verified state

- Working branch: `implementation/kernel-milestone-01`.
- Gate A is closed: deterministic cross-machine replay, performance budgets and external forecast legibility passed.
- Juan's repaired informed legibility result was **4/5 PASS**.
- The single miss established a presentation requirement: show **Board Mech** / **Deploy Remotely** and preview occupancy consequences; keep `Docked` internal.
- Gate A replay hashes:
  - 1×1: `716BE2E159FBB184B422C33910A6A0513FEF8B034D53E531F76E8A2BAACAF0B4`
  - 2×2: `533CEDD457740A8604C19265EFEF864E361325152A10777E32C1C72D62260689`
- Unity baseline is verified on Windows and pinned to Unity 6.3 LTS `6000.3.23f1` + URP `17.3.0`.
- The source-controlled Unity bootstrap exists at `unity/SecretGame`; the authoritative kernel remains `src/SecretGame.Simulation`.
- Windows Unity batch bootstrap passed 3/3 Edit Mode tests with exit code 0, Smart App Control enabled and Burst disabled only for the managed test process. Milestone 019 records the accepted generated baseline.
- Windows subsequently confirmed Tactics **17/17** and a clean synchronized branch at `465eaa79ddd1a68224cce2d2d5d609733fa48084`. All baseline harnesses are closed.
- Milestone 020 then passed on Windows at `787870695b889d2288677d93c668fd78cd254062`: Simulation **40/40**, the shared Charge/Board/Recharge/Deploy harness `PASS`, Unity **4/4**, clean synchronized branch. The data-only resource loop is accepted; its numeric inputs remain provisional.
- Milestone 021 passed on Windows at `a82a7a45971d1ffe5621ed36652599635860ce07`: Simulation **46/46**, immediate remote-directive harness `PASS`, Unity **5/5**, clean synchronized branch. Option A is the retained prototype because no evidence yet justifies a temporal queue.
- Milestone 022 core evidence passed at `7b4d3bec6c0a98d84a650d450a17c591ade3a291`: Simulation **50/50** and the three-policy comparison `PASS`. Smart App Control blocked only the generated Tactics apphost; the transcript ended before Unity returned. Milestone 023 carries a security-preserving managed-DLL runner and keeps both observations pending.
- The first Windows attempt synchronized 31/31 kernel files, then exposed a PowerShell runner defect: direct invocation of the GUI editor left `$LASTEXITCODE` unset. Milestone 011 replaces that mechanism with an explicit waited process handle. This is not yet Unity test evidence.
- The next waited run reached compilation and exposed one compatibility mismatch: Unity selected C# 9 while the kernel uses C# 10 syntax. Milestone 012 adds a pinned `-langversion:10.0` compiler-response probe; Unity tests remain pending until that probe runs on Windows.
- Windows evidence confirmed the C# 10 probe works. The next failure was the absent framework marker `IsExternalInit`, not rejected language syntax. Milestone 013 generates that marker only inside Unity's ignored kernel mirror and enables nullable annotations; tests remain pending.
- The following Windows run resolved records and reduced compilation to four unavailable modern .NET APIs. Milestone 014 replaces them in the authoritative source with behavior-equivalent portable null checking, SHA-256 and uppercase hexadecimal encoding. Expected replay hashes are unchanged; Unity execution remains pending.
- The next .NET verification selected SDK 10 because the repo used `latest`, changing `array.Reverse()` overload binding in one test. Milestone 015 pins the .NET 8 patch band and C# 10, and makes the intended LINQ call explicit. No test executed in the failed run; both .NET harnesses and Unity remain to be rerun.
- Windows Code Integrity then proved Smart App Control was blocking unsigned DLLs created by Unity Burst JIT under `Library/BurstCache/JIT`. Milestone 016 disables Burst only for the managed bootstrap test process; global Windows security remains enabled. Burst production use remains a later explicit decision.
- The first SDK pin used `8.0.100 + latestPatch`, which cannot roll across feature bands to the installed `8.0.425`. Milestone 017 corrects the request to `8.0.400 + latestPatch`. The failed commands never compiled or ran tests.
- With SDK 8.0.425 selected, Simulation passed 33/33. Tactics then revealed that the repository-wide C# 10 pin rejected Content's established C# 11 `required` members. Milestone 018 pins the .NET solution to C# 11 while keeping only the Unity Simulation boundary on C# 10. Unity Edit Mode already passed 3/3 independently.
- The generated Windows baseline was imported from a 27-file archive and reviewed: Unity `6000.3.23f1`, Test Framework `1.6.0`, package lock, ProjectSettings and two GUID-linked URP settings assets. Tactics after Milestone 018 remains the only pending harness result.

## Accepted architecture

- Engine-free deterministic simulation; presentation consumes resolved events and never owns combat truth.
- Square integer grid. World meters and Unity vectors do not enter simulation assemblies.
- Stable pilot and mech entity IDs in every deployment mode.
- A boarded pilot exists authoritatively but is non-spatial, non-selectable, non-targetable and has no initiative slot.
- Mech footprint remains configurable and tested as 1×1 and 2×2; visual/world scale is independent.
- Separate Guard and Integrity pools; no probabilistic to-hit in v0.
- Exactly four gear pieces plus one character-specific weapon; weapons never contribute to gear sets.

## New design proposal

Juan proposed a battery-driven cyborg/mech identity with meaningful boarded and separated play, glove-emitted weapon programs, and radically different Specialty silhouettes.

The organized proposal is `docs/design/CYBORG-MECH-SYSTEM-PROPOSAL-001.md` and remains **NOT CANON** until reviewed. Its central reconciliation is:

- one historical mech, never three replacements;
- Bulwark = modular fortress configuration;
- Remote Arsenal = synthetic-derived link retrofit on the same chassis;
- Redline = stripped prototype configuration;
- one shared pilot/mech `Charge` pool (working name);
- Remote shares a bounded command economy instead of granting two free turns;
- chips are the cyborg's weapon records, not extra gear slots;
- Redline has no visible projection but retains a valid unarmed weapon record.

## Guardrails for the next implementation

Do not:

- promote the proposal to canon without Juan's explicit approval;
- create three unrelated mech prefabs, rigs or identities;
- implement a hex grid;
- duplicate Charge on pilot and mech;
- give Remote pilot and mech two unrestricted AP economies;
- allow action-generated Charge to pay for or recursively regenerate the same action;
- build character models before the data-only mode/resource experiment passes.

## Immediate next objective

1. Import and verify Milestone 023 on Juan's Windows checkout with the consolidated runner.
2. Confirm all six stay/transition scripts remain forecast-identical across .NET and Unity.
3. Retain the measured damage/prevention/exposure/Charge tradeoffs only if the table passes.
4. Add one reusable attack-range definition before measuring threatened cells.
5. Approve, revise or reject each policy only after spatial reach is measured too.

## Resume prompt

> Continue Secret-game from `docs/continuity/PROJECT-CHECKPOINT-2026-09-09.md`. Load canon-guard, combat-kernel, content-schema and adversarial-review. Verify branch and HEAD first. Do not treat `docs/design/CYBORG-MECH-SYSTEM-PROPOSAL-001.md` as canon. Milestone 023 compares stay-versus-transition pressure outcomes and includes a Smart App Control-safe managed test runner. Verify it on Windows, then add one reusable attack-range definition before measuring threatened cells.
