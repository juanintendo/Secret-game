# Milestone 013 — generated Unity record compatibility shim

## Evidence from Milestone 012

Unity 6.3.23f1 accepted `-langversion:10.0`: all previous `CS8773` language-feature failures disappeared. Compilation then reached record lowering and failed because Unity's framework surface does not define `System.Runtime.CompilerServices.IsExternalInit`. Nullable annotations also produced warnings because the Unity compilation context did not enable them.

This proves the C# 10 compiler probe works. Rewriting the authoritative kernel to C# 9 is not justified.

## Correction

The Unity sync boundary now generates `UnityIsExternalInit.cs` beside the copied kernel sources. It provides only the compiler marker required by init-only properties and records. It is ignored with the rest of the generated mirror and never enters the authoritative .NET project.

`Assets/csc.rsp` also pins `-nullable:enable`, matching the kernel's .NET build context.

## Boundary guarantees

- All 31 authoritative `.cs` files are still copied byte-for-byte and verified by SHA-256.
- The shim is generated, visibly named and contains no gameplay behavior.
- No Unity reference or conditional compilation symbol enters `src/SecretGame.Simulation`.
- Failure after this milestone must be diagnosed independently; it may not be attributed to the resolved C# 9 selection without new evidence.

## Gate

Unity bootstrap remains open until compilation completes and Edit Mode tests reproduce both Gate A hashes.
