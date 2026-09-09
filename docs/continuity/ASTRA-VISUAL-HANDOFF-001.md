# Astra Visual Handoff 001

## Mission

Lead Visual Spike 001 as the technical-art and presentation owner. Produce one premium, representative Unity combat proof for the canonical Cyborg and her single mech. This is the first visual evidence gate, not general content production.

## Mandatory reading

Read in this order:

1. `docs/canon/CANON-INDEX.md`
2. `art/ART-MANIFEST.md`
3. `docs/implementation/TACTICAL-FOUNDATION-CHECKPOINT-001.md`
4. `docs/implementation/VISUAL-SPIKE-001.md`
5. `docs/canon/90S-ANIME-XCOM-PREPRODUCTION-001.md` §§7, 10–13
6. `docs/canon/90S-ANIME-XCOM-CODEX-CLAUDE-BRIEF-001.md` §§2, 13–15

Inspect the four image sources named by Visual Spike 001 directly and at full resolution. Do not infer visual facts from filenames or prose summaries.

## Authority

- Canon and `ART-MANIFEST` govern identity.
- The simulation event stream governs combat truth.
- Visual Spike 001 governs scope and acceptance.
- `docs/design/` and Specialty names remain recommendations, not canon.

Do not redesign characters, invent final measurements, create multiple mech identities or alter combat rules to improve a shot.

## First deliverable

Before broad implementation, return a bounded technical-art plan containing:

- scene and asset inventory;
- modelling/rigging assumptions and explicit missing source material;
- URP renderer/shader strategy;
- outline method and failure modes;
- lighting strategy;
- camera and stepped-animation strategy;
- High/Low preset contract;
- capture and profiling automation;
- exact first three visual experiments;
- expensive choices intentionally deferred.

Then implement the smallest experiment that can produce the required fixed-camera captures. Prefer inspectable shader graphs/code and reproducible editor tooling over hand-tuned scene magic.

## Collaboration split

Astra owns the judgment-heavy reference implementation: look development, shader/outline choice, lighting, camera language, motion presentation and visual-performance tradeoffs.

Sol remains the scaling implementation path after the reference is approved: repetitive tooling, validators, import automation, schema work, tests and propagation to additional assets.

## Stop conditions

Stop and report rather than invent when:

- a required modelling fact is absent from the manifest;
- the canonical sheets contradict one another;
- a beautiful solution breaks ordinary combat readability;
- the desired look requires gameplay authority inside presentation;
- representative performance misses budget after one bounded optimization pass;
- the requested character quality cannot be reached with the available source assets.

Every visual artifact returned must identify its source asset, commit, camera, preset and whether it is a target, experiment or approved output.
