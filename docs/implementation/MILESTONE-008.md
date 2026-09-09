# Milestone 008 — repair the Gate A human protocol

## Outcome

The first external Windows run supplied the missing technical evidence: Release performance passed and both replay hashes matched the Linux/container reference exactly. The same run also falsified the original human-test design. It scored prose about routes against undocumented internal condition tokens.

This milestone repairs the test protocol without changing simulation rules, Relay Yard commands, forecast calculations, expected deltas, or replay hashes.

## First-run evidence

- Turn benchmark: p50 55 µs, p95 96 µs, p99 130 µs, max 890 µs — pass.
- Forecast benchmark: p50 17 µs, p95 29 µs, p99 33 µs, max 721 µs — pass.
- Windows replay 1×1: `716BE2E159FBB184B422C33910A6A0513FEF8B034D53E531F76E8A2BAACAF0B4` — exact match.
- Windows replay 2×2: `533CEDD457740A8604C19265EFEF864E361325152A10777E32C1C72D62260689` — exact match.
- Footprint comparison: both legal, 35 events each, event-stream shape equal.
- Original legibility protocol: 0/5 — protocol failure caused by missing instruction and machine-token input.

## Repair

- Added a coordinate-labelled teaching renderer. `(X,Y)` begins at the upper-left; X increases right and Y increases down.
- Gave each Relay Yard entity a unique teaching marker, including Warden, Striker and RelayTech.
- Added an explicit legend for characters, enemies, mech footprint, walls and empty cells.
- Defined the exact concepts used by the five questions: adjacency, `Isolated`, `Marked`, and `Docked`.
- Replaced internal token entry with four numbered natural-language descriptions.
- Invalid input is reprompted and does not consume a question.
- Expected answers continue to come from `CombatForecast`; no presentation formula duplicates the resolver.
- The PowerShell runner now stops immediately when any native `dotnet` command fails instead of continuing after an SDK/runtime error.

## Gate rule

Gate A remains open until the repaired test scores at least 4/5. A passing retest closes the last Gate A row and authorizes the Unity 6 URP/Astra visual spike. A failing informed retest is genuine evidence that the rules or forecast presentation require another iteration.
