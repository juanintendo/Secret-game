# Secret Game — Unity technical-art spike

This is the provisional Unity 6.3 LTS + URP host for the engine-free combat kernel. Unity is not yet a binding engine decision.

## Before opening the project

From the repository root in PowerShell:

```powershell
powershell -ExecutionPolicy Bypass -File .\scripts\sync-unity-kernel.ps1
```

The script copies `src/SecretGame.Simulation/*.cs` into the ignored `Assets/Generated/Kernel/` directory and verifies every copy by SHA-256. The committed .NET source remains authoritative; generated Unity copies must never be edited.

Open `unity/SecretGame` with Unity `6000.3.0f1`. Let Package Manager resolve URP and the Test Framework, then run Edit Mode tests. On Windows, the repository runner performs synchronization and the first batch-mode test in one command:

```powershell
powershell -ExecutionPolicy Bypass -File .\scripts\run-unity-bootstrap-windows.ps1
```

The first acceptance criterion is that both Relay Yard replay hashes match Gate A inside the Unity editor. No character production starts in this bootstrap milestone.
