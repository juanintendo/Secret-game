# Milestone 021 — immediate remote directives

**Status:** accepted on Windows.

**Design authority:** Option A is an experiment under `docs/design/CYBORG-MECH-SYSTEM-PROPOSAL-001.md`; it is not canon or final balance.

**Parent baseline:** `787870695b889d2288677d93c668fd78cd254062`.

**Accepted commit:** `a82a7a45971d1ffe5621ed36652599635860ce07`.

Windows evidence: Simulation **46/46**, immediate remote-directive harness `PASS`, Unity Edit Mode **5/5**, 34 synchronized kernel sources, clean branch synchronized with origin.

## Question under test

Can the Remote configuration make both bodies tactically useful without creating a free fourth protagonist or hiding its command economy?

Option A uses immediate directives:

- the Cyborg must own the current activation;
- `Remote Move` and `Remote Attack` spend her AP;
- each directive spends the one shared Charge pool;
- movement and line of sight originate from the mech's footprint;
- the mech emits the physical movement/damage event;
- the mech's own AP never refreshes or pays for the directive;
- the mech remains without an initiative slot throughout Remote mode.

This is intentionally smaller than a queued-command system. A queue adds latency, persistence, cancellation and prediction states. We only build it if immediate control proves legible but strategically too flexible or thematically too direct.

## Prototype inputs

- starting Charge: `4/8`;
- remote move: `1 Cyborg AP + 1 Charge`, maximum two cells;
- remote attack: `1 Cyborg AP + 1 Charge`, six damage;
- target: the Warden fixture at `18 Integrity`;
- expected finish: Cyborg `0 AP`, mech `2 AP` untouched, Charge `2/8`, Warden `12 Integrity`.

These values are scenario inputs, not approved balance.

## Executable evidence

```powershell
dotnet run --project tools/SecretGame.RemoteDirectiveExperiment
```

The harness prints coordinates, state, and the exact forecast before each action. The expected trace shows:

1. Cyborg activation begins;
2. she spends one AP and one Charge to move the 2×2 mech;
3. she spends her remaining AP and one Charge to order its attack;
4. mech AP remains untouched and direct mech activation remains illegal.

## Automated properties

Simulation adds six checks:

- complete script forecast/execution parity;
- remote movement spends pilot AP and shared Charge;
- remote attack resolves from the mech and does not spend mech AP;
- remote mech cannot begin an activation;
- insufficient Charge rejects without partial mutation;
- directives reject unless pilot and mech are in the Remote flag configuration.

Unity runs the complete script as a fifth cross-host forecast-parity test. The existing Gate A hash assertions remain in the Simulation harness.

## Deliberate boundaries

This milestone does not implement:

- queued or delayed directives;
- autonomous mech AI;
- signal range, disruption or latency;
- Specialty bonuses;
- final Charge/AP costs;
- weapon chips or animation requests;
- a second remote activation.

## Acceptance

```powershell
dotnet run --project tests/SecretGame.Simulation.Tests
dotnet run --project tools/SecretGame.RemoteDirectiveExperiment
powershell -ExecutionPolicy Bypass -File .\scripts\run-unity-bootstrap-windows.ps1 `
  -UnityEditorPath "C:\Program Files\Unity\Hub\Editor\6000.3.23f1\Editor\Unity.exe"
git status -sb
git rev-parse HEAD
```

Accept only when Simulation passes **46/46**, the remote harness ends in `PASS`, Unity passes **5/5**, Gate A hashes remain unchanged and the branch is clean/synchronized.

## Product decision after evidence

Do not build a queue automatically. Juan should first answer one concrete question after reading the harness: does immediate remote control feel like commanding a linked machine, or merely like moving a fourth unit through the Cyborg's menu? If it feels like a fourth unit, compare a one-slot queued directive in Milestone 022. If it reads as one coordinated character, keep the simpler model and proceed to Specialty policy tests.
