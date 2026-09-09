# Gate A — Windows handoff

From PowerShell in the repository root:

```powershell
powershell -ExecutionPolicy Bypass -File .\scripts\run-gate-a-windows.ps1
```

The script runs Release benchmarks, compares 1×1 and 2×2 Relay Yard, reproduces both replay hashes, then runs five informed legibility questions.

Expected hashes:

```text
REPLAY 1x1 716BE2E159FBB184B422C33910A6A0513FEF8B034D53E531F76E8A2BAACAF0B4
REPLAY 2x2 533CEDD457740A8604C19265EFEF864E361325152A10777E32C1C72D62260689
```

The harness now teaches the tested rules before scoring, prints zero-based X/Y coordinates, distinguishes all three enemies, and explains that the player predicts a tactical-window change rather than a movement route.

Answer format:

- Read the four descriptions shown below each question.
- Type only `1`, `2`, `3`, or `4`.
- Invalid prose or commands are rejected and reprompted instead of being counted as wrong.

Do not inspect `tools/SecretGame.GateA/Program.cs` while taking the test. A result of `4/5` or `5/5` passes.

## First-run evidence and correction

Juan's 2026-09-09 Windows run reproduced both expected hashes and passed both performance budgets. The original legibility protocol scored 0/5 because it asked a first-time participant to emit internal strings such as `+Human:Isolated` without teaching the coordinate convention, entity identities, condition rules, or Docked semantics. Natural-language route descriptions were therefore compared against machine tokens. That result is evidence that the original harness was illegible; it is not evidence that the combat forecast is inherently illegible.

Milestone 008 keeps the same five commands and kernel-derived expected deltas but repairs only the teaching and answer layer. The retest remains meaningful: the correct choice is still computed from the real forecast resolver, and no answer is displayed until after each prediction.

If PowerShell reports that `dotnet` is unavailable, stop and report that exact message. Do not install an SDK silently.
