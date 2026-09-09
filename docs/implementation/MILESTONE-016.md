# Milestone 016 — Smart App Control-safe Unity bootstrap

## Verified external blocker

Windows Code Integrity events `3077` and `3033` showed Unity 6.3.23f1 attempting to load unsigned native DLLs generated under:

`unity/SecretGame/Library/BurstCache/JIT/`

Smart App Control rejected those DLLs because they did not meet the enforced signing policy. This was not an inferred antivirus issue: the process, exact generated paths and policy rejection were captured from `Microsoft-Windows-CodeIntegrity/Operational`.

## Bounded correction

The bootstrap runner now passes `--burst-disable-compilation` to this specific batch process.

The Gate A Unity tests execute managed deterministic simulation code and do not depend on Burst jobs or Burst performance. Disabling Burst for this verification therefore removes an irrelevant unsigned JIT boundary without changing the tested rules.

## Security decision

Smart App Control remains enabled. No global Windows protection, policy, signing requirement or folder exclusion is weakened for the bootstrap.

This is not a permanent decision to exclude Burst from the game. If a later representative performance test needs Burst, its compatibility with the production/development environment must be evaluated separately with signed AOT output or an explicitly approved development-machine policy.

## Gate

The bootstrap still requires:

- successful compilation;
- NUnit Edit Mode results;
- exact 1×1 and 2×2 Gate A replay hashes;
- no new enforced Code Integrity event caused by the bootstrap process.
