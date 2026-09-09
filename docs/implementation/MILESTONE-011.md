# Milestone 011 — reliable Windows Unity process capture

## Outcome

The first Windows Unity bootstrap attempt synchronized all 31 kernel files with matching SHA-256 hashes, then failed in the PowerShell wrapper before it could report the Unity result. This was a runner defect, not a simulation or Unity test failure.

## Cause

Windows PowerShell does not reliably set `$LASTEXITCODE` when a Windows GUI executable such as `Unity.exe` is invoked directly. Under strict mode, reading the unset variable throws immediately. Unity may also continue independently, making the wrapper's apparent completion misleading.

## Correction

`scripts/run-unity-bootstrap-windows.ps1` now launches Unity through `Start-Process -Wait -PassThru` and reads the returned process object's `ExitCode`. The wrapper therefore:

- waits for the actual Unity process to finish;
- captures its explicit exit code;
- reports the existing log on failure;
- parses NUnit results only after a successful editor exit.

No simulation, gameplay, forecast, content or Unity project code changed.

## Remaining evidence

The bootstrap gate remains open until Juan's Windows machine produces the NUnit result file and confirms the replay tests under the installed Unity 6.3 LTS patch. The installed editor observed on that machine is `6000.3.23f1`; adopting it as the repository baseline requires a successful run and a separate explicit version update.
