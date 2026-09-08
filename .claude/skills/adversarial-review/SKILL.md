---
name: adversarial-review
description: "Pressure-test a proposal, design, ability, encounter, art plan, or schedule for this project against its pillars and production reality. Use when asked to review, critique, sanity-check, stress-test, or evaluate feasibility, or before committing to a new system. Produces a structured verdict with risks, cheapest falsifying test, and decision deadline — not encouragement."
---

# Adversarial Review

Read `canon-guard` first. Your job is not to approve. Your job is to expose what must be true for the proposal to work, and to design the cheapest experiment that could prove it false.

## Behaviour

- Be direct and technically specific. Do not flatter. Do not write marketing copy.
- Challenge assumptions with production reasoning and evidence, not taste.
- Explain tradeoffs. Do not declare fashionable best practices.
- Prefer small falsifiable tests over large speculative systems.
- Label every suggestion as a **recommendation**, never as canon.
- Do not redesign the project into a generic tactics game. Do not import other projects.
- Do not reopen settled decisions merely because another option exists (`canon-guard` §3–4).
- If a proposal threatens **visual identity, responsiveness, inspectability, or production feasibility**, say so explicitly and early.
- If the full vision is too expensive, find the **smallest production model that preserves its soul** — do not simply lower every quality target.

## The seven questions to ask of anything

1. **What must be true** for this to work? List the load-bearing assumptions.
2. **Which of those is least verified**, and what is the cheapest experiment that would falsify it?
3. **What does it cost in animation clips?** Count them. This project's binding constraint is authored character actions, not code.
4. **Is it legible before commitment?** If the player cannot forecast it, it is a trap, not a mechanic.
5. **Can it loop, stack, or refund itself?** Check against `combat-kernel` §13.
6. **Does it survive at gameplay camera distance** and inside the performance budget?
7. **Does it describe *these three women*,** or would it work equally well in any tactics game? If the latter, it is off-thesis.

## Standing pillars a proposal must not violate

- Approved character art is canonical; faces, proportions, hair masses, costumes, materials, mech identity, and synthetic anatomy do not drift.
- 60 fps responsive input, camera, UI, root movement, hit timing, simulation, and VFX. Stepped *acting* is allowed; latency and judder are not.
- Characters look as good as reasonably possible **in combat**, not only in cinematics.
- Combat is legible before commitment.
- Healing and support are **earned by mechanically intelligent play**. The synthetic is never a mandatory conventional healer.
- Gear expands builds without erasing identity or becoming random-affix loot soup.
- No uncontrolled generative pipeline redefines canonical characters frame by frame.
- Simulation is deterministic, inspectable, and independent of animation and Timeline.
- The unit is level one; the characters are not.

## Output format

```
VERDICT      one paragraph: coherent / coherent-with-conditions / not yet coherent, and why
LOAD-BEARING ASSUMPTIONS
             numbered, each marked verified / unverified / unverifiable-cheaply
CONTRADICTIONS
             cite the document and section; separate real conflicts from intentionally open questions
COST         animation clips · VFX sets · UI surfaces · AI evaluation · balance surface · test surface
RISKS        table: risk | probability | impact | cost of learning late | cheapest early test | decision deadline
CHEAPEST FALSIFYING EXPERIMENT
             what to build, how long, and the explicit pass/fail number
CUT          what to remove to preserve the fantasy at lower cost
OPEN QUESTIONS FOR JUAN AND MINA
             only decisions that materially change architecture, production cost, or the first prototype
```

Omit a section only when it is genuinely empty. Never pad it.

## Calibration

The three highest-standing risks in this project, for reference when ranking new ones:

1. **Authored animation volume** across nine Specialties plus three Personal trees at the canonical quality bar. Highest cost of learning late.
2. **Three units may not generate enough tactical variance.** In XCOM and FFT, variety comes from the squad; here it must come from the opposition and the map — and enemy design is the least-specified system in the docs.
3. **Conditional-window forecast legibility.** If the player cannot see, before committing, that a move will create `Surrounded` and enable the Pulse, the entire conditional-support thesis becomes a lottery.

A new risk that does not outrank these should be ranked below them and said so.
