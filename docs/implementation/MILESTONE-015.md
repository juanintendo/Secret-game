# Milestone 015 — deterministic .NET SDK and LINQ binding

## Observed failure

After Milestone 014, Juan's machine selected the newly installed .NET SDK `10.0.401` because the repository did not contain `global.json` and set `LangVersion` to `latest`.

The simulation test expression `first.Reverse()` then bound to the newer in-place array/span API returning `void` instead of LINQ's sequence-returning `Enumerable.Reverse`. Compilation failed with `CS0815` before any test executed.

This was an SDK-dependent test binding, not a deterministic-hash failure.

## Correction

- `global.json` selects the .NET 8 feature band, starting at `8.0.100`, and rolls forward only to the latest installed .NET 8 patch. Juan's verified `8.0.425` therefore satisfies it while .NET 10 is not selected accidentally.
- `Directory.Build.props` pins C# `10.0` instead of `latest`, matching the explicit Unity compiler response.
- The test now calls `first.AsEnumerable().Reverse()`, making its non-mutating LINQ intent unambiguous across SDK API growth.

## Invariants

- Production simulation behavior is unchanged.
- The test still proves entity insertion order cannot change the state hash.
- The repository and Unity now compile against the same C# language level.
- SDK major-version changes require an explicit commit and full replay verification.

## Required evidence

Run both .NET harnesses first. They must reproduce their previous results under SDK 8. Then rerun the Unity bootstrap and require both Gate A hashes in NUnit results.
