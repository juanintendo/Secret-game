# Milestone 022 — cyborg Specialty policy comparison

**Status:** core comparison passed on Windows; one host-policy gate and the final Unity result remain to be captured.

**Design authority:** all three policies and every number below remain `RECOMMENDATION — NOT CANON`.

**Parent baseline:** `a82a7a45971d1ffe5621ed36652599635860ce07`.

## Question under test

Can Bulwark, Remote Arsenal and Redline produce visibly different tactical economies while sharing one mech, one kernel and one typed effect grammar?

This milestone does not attempt balance. It creates one representative action signature per policy:

| Policy | Mode | AP authority | Prototype action | Expected signature |
|---|---|---|---|---|
| Bulwark | Boarded | Mech | Spend 1 AP + 2 Charge, gain 6 Guard | resource becomes defense |
| Remote Arsenal | Remote | Cyborg | Spend 2 AP + 2 Charge across move and attack | resource becomes distributed reach and damage |
| Redline | Boarded | Mech | Spend 1 AP, deal 7 damage, preserve Charge | low-projection pressure |

The same historical mech and shared `Charge` pool are used throughout. No visual configuration, final ability, talent or public name is approved here.

## Architecture added

- `SpendResourceEffect` joins the reusable typed effect grammar.
- `AbilityCompiler` can compile an authored `spendResource` effect with an explicit integer resource ID and amount.
- `BoardedMechEffectStackCommand` adds the deployment precondition while delegating actual effects to the existing atomic `EffectStackCommand` resolver.
- Bulwark therefore composes `SpendResource + GrantGuard`; Redline uses ordinary `Damage` without a parallel resolver.
- Remote Arsenal reuses the immediate directives accepted in Milestone 021.

This avoids three bespoke Specialty kernels. Deployment restrictions are command validation; damage, Guard, resource events, cloning, forecast and replay remain shared.

## Executable comparison

```powershell
dotnet run --project tools/SecretGame.SpecialtyPolicyExperiment
```

Expected final table:

| Policy | AP spent | Charge | Damage | Guard | Free mech turn |
|---|---:|---:|---:|---:|---|
| Bulwark | 1 | 2/8 | 0 | 6 | No |
| Remote Arsenal | 2 | 2/8 | 6 | 0 | No |
| Redline | 1 | 4/8 | 7 | 0 | No |

The tool fails if any forecast differs from execution.

## Automated properties

Simulation adds four tests:

- boarded Bulwark and Redline scripts preserve forecast/event/hash parity;
- Bulwark spends exact Charge and grants exact Guard;
- Redline deals damage without silently spending or generating Charge;
- wrong deployment mode and insufficient Charge reject atomically.

Tactics adds one compiler test for authored resource spending. Unity runs both boarded policies as a sixth cross-host parity test. Historical Gate A hashes remain asserted by the existing suite.

## Evidence this milestone does not provide

Distinct output columns are necessary but not sufficient. They do not prove:

- that any policy is fun or balanced;
- that changing deployment beats staying put in real encounters;
- that Bulwark's defense is preferable to damage;
- that Remote Arsenal's board coverage is worth two AP and exposed pilot risk;
- that Redline needs an alternator recovery rule;
- that three modular visual configurations fit the art budget.

Do not implement Redline alternator recovery yet. Its per-activation cap needs evidence that Redline exhausts Charge too quickly once real abilities spend it; the current representative strike deliberately costs zero Charge to establish a clean baseline.

## Acceptance

```powershell
dotnet run --project tests/SecretGame.Simulation.Tests
dotnet run --project tests/SecretGame.Tactics.Tests
dotnet run --project tools/SecretGame.SpecialtyPolicyExperiment
powershell -ExecutionPolicy Bypass -File .\scripts\run-unity-bootstrap-windows.ps1 `
  -UnityEditorPath "C:\Program Files\Unity\Hub\Editor\6000.3.23f1\Editor\Unity.exe"
git status -sb
git rev-parse HEAD
```

Accept the implementation only when Simulation passes **50/50**, Tactics **18/18**, the comparison ends in `PASS`, Unity passes **6/6**, Gate A hashes remain unchanged and the branch is clean/synchronized.

## Next falsifying experiment

If accepted, build one compact encounter where each policy must choose between staying in its current deployment and changing it. Measure prevented damage, dealt damage, threatened cells, Charge, AP and stalled turns. That encounter—not this table—decides whether the three policies have genuine tactical identities.

## Windows evidence received

At commit `7b4d3bec6c0a98d84a650d450a17c591ade3a291`:

- Simulation passed **50/50** and retained the historical Gate A hashes.
- The three-policy executable comparison ended in `PASS` with the expected table.
- Unity synchronized **35/35** source files, but the captured transcript ended while the editor was still running.
- Tactics did not report a code failure: Windows Smart App Control blocked the generated native apphost before the managed test assembly started. Milestone 023 adds a signed-`dotnet`-host runner for this environment.

The missing host observations remain explicit; they are not silently promoted to passes.
