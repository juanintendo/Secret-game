# Milestone 023 — Specialty pressure: stay versus transition

**Status:** implemented in source; awaiting one consolidated Windows verification.

**Design authority:** experiment only. Policies, actions, costs and names remain `RECOMMENDATION — NOT CANON`.

**Parent baseline:** `7b4d3bec6c0a98d84a650d450a17c591ade3a291`.

## Question under test

Does deployment create a real costed choice under enemy pressure, or is one mode always cosmetic?

Each policy receives two player activations and one identical 8-damage Warden attack. The harness compares staying in the policy's starting mode with transitioning. It measures damage dealt, Guard actually consumed, pilot loss, mech loss, remaining Charge and final deployment mode.

## Expected signatures

| Policy | Choice | Damage | Prevented | Pilot loss | Mech loss | Charge | Final mode |
|---|---|---:|---:|---:|---:|---:|---|
| Bulwark | Stay | 0 | 6 | 0 | 2 | 0/8 | Boarded |
| Bulwark | Transition | 12 | 0 | 0 | 8 | 0/8 | Remote |
| Remote Arsenal | Stay | 0 | 0 | 0 | 8 | 8/8 | Boarded |
| Remote Arsenal | Transition | 12 | 0 | 0 | 8 | 0/8 | Remote |
| Redline | Stay | 24 | 0 | 8 | 0 | 0/8 | Remote |
| Redline | Transition | 7 | 0 | 0 | 8 | 4/8 | Boarded |

These deliberately expose different trades:

- Bulwark exchanges projected damage for absorbed pressure.
- Remote Arsenal exchanges stored Charge for immediate remote pressure.
- Redline exchanges maximum output and pilot exposure for boarding safety.

This does not establish balance. It does establish that deployment is observable in outcome metrics rather than merely changing a flag.

## Architecture

- `SpecialtyPressureExperimentScenario` composes only existing activation, deployment, damage, recharge, typed-effect and immediate-directive commands.
- No encounter-only resolver or privileged damage path was added.
- Every command is forecast before execution; event sequence and resulting hash must match.
- The Warden uses the ordinary initiative scheduler and ordinary `DamageCommand`.

Threatened-cell coverage is deliberately deferred. The kernel has no approved attack-range grammar yet; fabricating a range formula in the reporting tool would produce a persuasive but false metric.

## Automated evidence

Simulation adds four properties:

- all six policy/choice scripts preserve forecast parity;
- Bulwark's stay/transition outputs remain distinct;
- Remote Arsenal exposes stored-energy versus immediate-pressure tradeoff;
- Redline exposes pilot safety versus output tradeoff.

Unity replays all six scripts as a seventh cross-host test.

## Smart App Control runner

`scripts/run-managed-tests-windows.ps1` builds both managed test projects and the pressure harness with `UseAppHost=false`, then runs their DLLs through the installed signed `dotnet` host. This avoids launching unsigned generated `.exe` apphosts that Smart App Control may block. It does not disable or weaken Windows security policy.

## Acceptance

```powershell
powershell -ExecutionPolicy Bypass -File .\scripts\run-managed-tests-windows.ps1
powershell -ExecutionPolicy Bypass -File .\scripts\run-unity-bootstrap-windows.ps1 `
  -UnityEditorPath "C:\Program Files\Unity\Hub\Editor\6000.3.23f1\Editor\Unity.exe"
git status -sb
git rev-parse HEAD
```

Accept only when Simulation passes **54/54**, Tactics **18/18**, the pressure harness ends in `PASS`, Unity passes **7/7**, and the branch is clean and synchronized.

## Next decision

If the table holds, the next experiment should introduce one reusable attack-range definition and measure threatened cells. Only then can the project judge whether Remote Arsenal's spatial reach compensates for its Charge and pilot-exposure costs.
