# Milestone 012 — Unity C# 10 compatibility probe

## Observed failure

Unity 6.3.23f1 successfully resolved packages and began compiling the verified 31-file kernel mirror. Compilation then stopped before tests because Unity invoked Roslyn with C# 9 while the authoritative .NET kernel uses three C# 10 language features:

- file-scoped namespaces;
- record structs;
- global using directives.

Every reported `CS8773` is a consequence of this single language-version mismatch. No gameplay rule, replay hash or test failed, and no NUnit result file was produced.

## Bounded correction

`unity/SecretGame/Assets/csc.rsp` now requests `-langversion:10.0` for Unity compilation. This is a small, explicit and reversible compatibility probe that preserves the engine-free kernel byte-for-byte.

The repository check asserts that the response file exists, is tracked with Unity metadata and pins exactly C# 10 rather than an unstable `latest` setting.

## Decision rule

- **Pass:** Unity compiles the unchanged synchronized kernel and the Edit Mode replay tests reproduce both Gate A hashes. Keep the response file and record the installed Unity patch.
- **Fail because the compiler rejects C# 10 or runtime support is missing:** remove the response file and convert the authoritative kernel to an explicitly C# 9-compatible syntax in a separate milestone, preserving all .NET and replay tests.

No source conversion is authorized until this cheaper probe returns evidence.
