# Gate A — Windows handoff

From PowerShell in the repository root:

```powershell
powershell -ExecutionPolicy Bypass -File .\scripts\run-gate-a-windows.ps1
```

The script runs Release benchmarks, compares 1×1 and 2×2 Relay Yard, reproduces both replay hashes, then asks five blinded questions.

Expected hashes:

```text
REPLAY 1x1 716BE2E159FBB184B422C33910A6A0513FEF8B034D53E531F76E8A2BAACAF0B4
REPLAY 2x2 533CEDD457740A8604C19265EFEF864E361325152A10777E32C1C72D62260689
```

Answer format:

- `+Name:Condition` when a window opens.
- `-Name:Condition` when a window closes.
- Separate multiple predictions with commas.
- Type `none` if no window changes.

Do not inspect `tools/SecretGame.GateA/Program.cs` while taking the test. A result of `4/5` or `5/5` passes.

If PowerShell reports that `dotnet` is unavailable, stop and report that exact message. Do not install an SDK silently.
