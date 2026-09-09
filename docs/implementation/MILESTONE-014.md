# Milestone 014 — portable framework APIs for the authoritative kernel

## Evidence from Milestone 013

Unity 6.3.23f1 compiled C# 10 syntax and resolved the generated `IsExternalInit` marker. Only four errors remained:

- two calls to `ArgumentNullException.ThrowIfNull`;
- one call to `SHA256.HashData`;
- one call to `Convert.ToHexString`.

These APIs belong to a newer .NET framework surface than Unity exposes. They are not language features and do not justify a generated Unity fork.

## Correction

The authoritative source now uses portable equivalents:

- explicit null checks still throw `ArgumentNullException` with the original parameter name;
- `SHA256.Create().ComputeHash` computes the same SHA-256 bytes;
- a local deterministic encoder emits two uppercase hexadecimal characters per byte, preserving the existing 64-character hash representation.

The same null-check form was applied to `SecretGame.Tactics` proactively because that assembly will cross the Unity boundary later.

## Invariants

- Deterministic serialization bytes are unchanged.
- Hash algorithm is unchanged: SHA-256.
- Hexadecimal output remains uppercase and culture-independent.
- Simulation boundary CI now rejects reintroduction of these three unavailable framework APIs.
- Gate A expected hashes remain unchanged and must be reproduced by both .NET and Unity tests.

## Gate

The Windows Unity bootstrap must compile and reproduce:

- 1×1: `716BE2E159FBB184B422C33910A6A0513FEF8B034D53E531F76E8A2BAACAF0B4`
- 2×2: `533CEDD457740A8604C19265EFEF864E361325152A10777E32C1C72D62260689`
