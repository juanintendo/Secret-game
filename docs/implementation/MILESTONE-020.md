# Milestone 020 — cyborg/mech shared-resource experiment

**Status:** implemented in source; awaiting Windows .NET and Unity verification.

**Design authority:** `docs/design/CYBORG-MECH-SYSTEM-PROPOSAL-001.md` remains `RECOMMENDATION — NOT CANON`.

**Parent baseline:** `465eaa79ddd1a68224cce2d2d5d609733fa48084`.

## Outcome under test

This milestone makes the first cyborg/mech resource loop executable without choosing final balance values or presentation names:

1. one authoritative `SharedResourcePool` is owned by the pilot/mech identity and stored once in `CombatState`;
2. **Board Mech** spends AP, requires physical adjacency, removes the pilot from map authority, and never refills the pool;
3. **Recharge** is a separate AP action legal only while the pilot is aboard;
4. **Deploy Remotely** atomically spends AP and resource before restoring the pilot to the map;
5. the remotely controlled mech remains spatial/selectable/targetable but has no independent initiative slot;
6. every AP, resource and deployment change is emitted through the same resolver used by forecast and execution.

The working experiment values are `Charge 3/8`, `Recharge +3 for 1 AP`, and `Deploy Remotely -2 for 1 AP`. They are test inputs, not canon or balance commitments.

## New executable evidence

Run:

```powershell
dotnet run --project tools/SecretGame.CyborgMechExperiment
```

The harness prints the authoritative state and exact forecast before each command. It fails if forecast events or resulting hashes diverge from execution.

## Automated properties

The Simulation harness now verifies:

- resource state survives clone and participates in deterministic hashing;
- boarding cannot recharge and cannot occur from across the map;
- recharge clamps at the pool maximum and reports the actual delta;
- remote deployment spends the single shared pool;
- the remote mech cannot receive a free initiative slot;
- insufficient resource rejects the whole transition without mutation;
- both accepted Gate A hashes remain byte-for-byte unchanged.

Unity Edit Mode adds the complete scripted experiment as a fourth cross-host forecast-parity test.

## Deliberate boundaries

This milestone does not decide:

- the final UI name for `Charge`;
- final pool sizes, generation or costs;
- Specialty policy values;
- queued-directive behavior for the remote mech;
- projected weapon content, defensive fields, alternator recovery, art or animation.

The remote mech has no independent initiative slot now. A later bounded experiment must compare queued directives against another shared-budget model before remote attacks or movement are implemented.

## Windows acceptance

From a clean checkout at this milestone:

```powershell
dotnet run --project tests/SecretGame.Simulation.Tests
dotnet run --project tools/SecretGame.CyborgMechExperiment
powershell -ExecutionPolicy Bypass -File .\scripts\run-unity-bootstrap-windows.ps1 `
  -UnityEditorPath "C:\Program Files\Unity\Hub\Editor\6000.3.23f1\Editor\Unity.exe"
```

Accept only when Simulation passes **40/40**, the experiment ends in `PASS`, Unity passes **4/4**, the historical Gate A hashes are unchanged, and `git status -sb` is clean after generated output is removed or ignored.
