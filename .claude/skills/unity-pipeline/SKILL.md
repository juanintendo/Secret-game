---
name: unity-pipeline
description: "Unity 6 / URP project conventions for this game: assembly-definition map, the simulation-presentation boundary, Timeline and Cinemachine rules, addressables and content generation, performance budgets, editor tooling, and what must stay engine-independent. Load before creating or modifying Unity project structure, C# assemblies, shaders, Timeline sequences, prefabs, editor tools, or performance work."
---

# Unity Pipeline

Read `canon-guard`, `combat-kernel`, and `visual-gate` first.

**Engine status: provisional.** Unity 6 (pinned 6000.x LTS) + URP is the recommendation, binding only after the two-week technical-art spike passes (see `docs/consult/CONSULT-CLAUDE-001.md` §12). Write code that survives being wrong about this.

## 1. Assembly map — the boundary is the architecture

| Assembly | Unity refs | Owns |
| --- | --- | --- |
| `Game.Core` | **none** | Deterministic primitives, fixed-point math, tags, seeded PRNG, `BattleState`, event log |
| `Game.Tactics` | **none** | Grid, nav, LOS, cover, initiative, AP, reactions, forced movement, objectives. **Only writer to `BattleState`** |
| `Game.Abilities` | **none** | Definitions, targeting, conditions, costs, typed effects, upgrades |
| `Game.Forecast` | **none** | Dry-run resolution. Consumed by UI, AI, and tests alike |
| `Game.Progression` | none | Attributes, trees, gear, respec; emits an immutable `LoadoutSnapshot` at battle start |
| `Game.Synchronization` | none | Handoffs, assists, rescues, chains, Limit resource |
| `Game.AI` | none | Utility evaluation **through `Game.Forecast`** — the AI sees exactly what the player sees |
| `Game.Narrative` | Unity ok | Ink bridge, story facts, relationships, save migration. Issues typed `TacticalCommand`s only |
| `Game.Presentation` | Unity | Animation requests, Cinemachine, VFX, audio, cut-ins, cinematics, post |
| `Game.UI` | Unity | Input-independent view models + display implementations |
| `Game.Content` | Unity | Generated ScriptableObject mirrors, addressable groups |
| `Game.Editor` | Unity | Validators, importers, preview scenes, balance tools, screenshot harness |
| `Game.Tests` | none for core | Deterministic fixtures, property/fuzz tests, content validation, golden images |

**The five assemblies marked "none" are a plain .NET class library.** They must build and run headless outside Unity. If an engine change happens, they survive it untouched. Any PR that adds a `UnityEngine` reference to one of them is rejected.

**`Game.Presentation` may not reference `Game.Tactics` types other than the event stream.** Enforce with the asmdef reference list *and* a test that scans for forbidden type usage — asmdefs alone are too easy to widen.

## 2. Simulation ↔ presentation contract

- The sim resolves an entire action **instantly** and emits an ordered `BattleEvent` list with authored millisecond offsets.
- Presentation plays that list over wall-clock time. It may **accelerate or skip safely at any point** — skipping must be a no-op on truth.
- Animation events never decide anything. They may only *request* a VFX or audio cue.
- **Test:** run every fixture twice, once with presentation disabled and once at 10× speed; assert identical final state hashes.

## 3. Content generation, not content clicking

- JSON under `content/` is the source of truth (see `content-schema`).
- An editor importer generates ScriptableObject mirrors. **Never hand-author a definition SO** — it will drift from JSON and win silently.
- Generated assets go in a folder marked as generated and are regenerable from scratch; deleting them must be harmless.
- Prefer editor scripting (validators, importers, test-scene generators, repeatable asset setup) over manual editor clicking. This project is agent-led; production truth must stay diffable.
- Keep binary editor assets thin. Mirror or generate important values from reviewable source data.

## 4. Timeline and Cinemachine

- Timeline is used for **cinematics only** (Tier A/S). Never for combat truth.
- A Timeline may read state; it may never write to `BattleState`.
- Combat "cinematic" beats (the commit zoom, ability camera pushes) are **Cinemachine blends driven by the event stream**, not Timeline clips. They must be interruptible and skippable.
- Every Timeline has a `shotId` in the shot metadata registry so it can be regression-screenshotted.

## 5. Rendering

- URP, pinned Unity 6000.x LTS. Forward+ unless the spike shows otherwise.
- Toon shading, outlines, and the analog post stack: see `visual-gate` §2–3.
- Post passes are **separate Renderer Features**, each independently toggleable, each with a golden-image test.
- UI renders on a separate overlay camera, excluded from post. Always.
- Character outlines are inverted hull; environment edges are screen-space. Do not mix.

## 6. Performance budgets — set during the spike, not after content

Capture on **both** a named low-bar and high-bar target machine. Record in `docs/pipeline/perf-budget.md` and re-measure on every milestone.

- CPU frame time / GPU frame time
- Draw calls and visible materials
- Skeletal bones and skinned meshes (per character and total)
- Texture residency
- Transparent VFX overdraw
- Dynamic lights and shadow casters
- Animation and AI evaluation cost
- **Tactical simulation turn-resolution time** — must be well under one frame; the sim is not allowed to hitch the camera

Stress-scene target: 3 protagonists + 1 mech + 6 enemies + tactical UI + 2 simultaneous VFX + full post stack, **60 fps with ≥ 30% headroom on the high-bar target.**

## 7. Input, camera, and the 60 fps promise

- Input acknowledgment on frame 1, always (`visual-gate` §5).
- Camera, cursor, path preview, turn transitions, and UI are never quantized and never inherit animation cadence.
- Grid movement interpolates continuously even when the body pose is stepped.
- Animation cancel/skip rules must be able to fire at any time without corrupting the simulation — which is free, because the simulation already resolved.
- The tactical camera must survive keyboard/mouse **and** controller from the first real implementation, not as a retrofit.

## 8. Testing

- Deterministic combat fixtures with state-hash assertions.
- Property/fuzz tests for the `combat-kernel` §13 invariants (10k random-legal-action encounters, bounded termination).
- Forecast-parity test: `forecast(s, a) == apply(s, a)` over a large sampled action space.
- Content validation (see `content-schema` §7) in CI.
- Golden-image tests per shader, per post pass, per UI screen, per cinematic `shotId`.
- Replay regression: every recorded playtest replays to the same final hash.
- Every mission boots directly into a developer test harness — no menu path required.

## 9. Repository hygiene

- Pin the Unity version in `ProjectSettings/ProjectVersion.txt` and state it in the README.
- Git LFS for binary art; keep source art out of the repo or in a clearly separated path.
- `.meta` files are always committed.
- Seeded, logged randomness only — a bug report is a seed plus a command list.
