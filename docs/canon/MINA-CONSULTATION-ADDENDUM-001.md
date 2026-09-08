# Mina Consultation Addendum 001

**Status:** Juan-approved project direction, plus one explicitly provisional mech-scale recommendation.

**Purpose:** Merge these rulings into the appropriate Claude consultation, questions, risk-register, spike, canon-index, art-manifest, content-schema, equipment, UI, rendering, and validation documents. This addendum does not authorize gameplay implementation or a GitHub push.

## 1. Operator arm asymmetry — settled visual canon

- Operator's anatomical **right arm** is exposed below the shoulder armor and has no sleeve or glove.
- Operator's **left arm** is covered by the white sleeve and white glove with purple detailing.
- The existing primary turnaround already depicts this correctly in its front and back views.
- A left-facing profile correctly shows only the near/camera-facing left sleeved arm. The far right arm must not appear through the torso.
- Use `art/character-sheets/operator-arm-asymmetry-reference.png` as the explicit construction reference.
- Correct the consultation claim that the available Operator sheet shows two sleeved/gloved arms.

## 2. Mech scale and footprint — provisional recommendation

- Prototype the single mech at approximately **3.6 m** tall beside an approximately **1.75 m** pilot.
- Prototype the mech as a **2x2-tile** unit; a human occupies **1x1**.
- Prototype a rear-upper-torso/top-entry cockpit hatch that fits the existing silhouette.
- This is **not yet settled canon**. It becomes binding only if the technical spike validates:
  - doors and traversal;
  - narrow-map pathfinding;
  - cover interaction;
  - occupancy and forced movement;
  - camera framing;
  - encounter-space cost;
  - remote-control readability.
- If 2x2 fails, do not silently compress the same broad mech into 1x1. Escalate the conflict between visual scale and tactical footprint.
- Use `art/character-sheets/mech-scale-footprint-study-v1.png` as the provisional visual study.

## 3. Equipment structure — settled product direction

- Every playable protagonist has exactly **four equipped gear pieces plus one separate character-specific weapon slot**.
- The weapon does **not** count among the four gear pieces and does not contribute to two-piece or four-piece set thresholds.
- The exact four slot categories remain to be named and must respect each character's identity.
- Gear sets support:
  - a combinable **two-piece set bonus**;
  - a **full four-piece set bonus**.
- A character may combine two different two-piece bonuses or complete one four-piece set.
- Set bonuses must expand builds without erasing character identity or becoming random-affix loot soup.
- Each protagonist uses **character-specific weapon classes** in the separate weapon slot. Weapons are not freely interchangeable between protagonists.
- A cyborg `mech-hardpoint`, if retained as an equipment category, must occupy one of her four personalized gear slots. It is not an additional sixth slot.
- Weapons and gear may alter mechanics, stats, tags, targeting, presentation effects, and build synergies within the content-budget rules.
- Final cinematics always use the authored **canonical visual loadout**, regardless of currently equipped gameplay items.
- The project still needs a ruling on whether ordinary gameplay visually displays all equipped gear, only selected weapon changes, or canonical clothing with limited equipment overlays.

## 4. Synthetic red-jacket visibility — settled presentation feature

- Only the luminous Synthetic receives a character-equipment UI option to show or hide the inherited red jacket.
- This is a **cosmetic visibility toggle**, not an equipment slot, stat modifier, separate item, or alternate body.
- Both states use the same canonical underlying Synthetic model and rig.
- The choice may be reflected in normal gameplay presentation.
- Authored cinematics override the player toggle and use the scene's canonical costume state. For Team 2 canon where no scene-specific exception exists, the inherited red jacket is shown.
- Portraits, UI renders, shadows, VFX anchors, hitboxes, targeting, animation timing, and simulation must not diverge between jacket-visible and jacket-hidden states.
- The jacket must be implemented as a controlled optional garment layer, not by duplicating the full character.

## 5. Graphics and performance policy — settled quality target

- The project aims for maximum visual quality at the highest preset.
- It must also ship with normal industry-standard graphics and accessibility/performance settings so slower supported systems can run it well.
- The lowest supported preset must remain intentionally art-directed, stable, legible, and visually polished. It may be simpler, but it must never look broken, muddy, generic, or graphically abandoned.
- Performance settings may scale:
  - internal render resolution and upscaling quality;
  - shadow resolution, distance, cascades, contact shadows, and secondary shadow casters;
  - volumetrics;
  - reflection quality;
  - ambient occlusion;
  - VFX density and secondary particles;
  - transparent-effect layers;
  - hair and cloth simulation complexity;
  - environmental animation density;
  - cinematic LOD distance;
  - anti-aliasing;
  - post-processing cost;
  - texture resolution within memory budgets;
  - crowd and background-detail density where narratively safe.
- The minimum preset must preserve:
  - canonical character silhouettes and proportions;
  - faces and readable expressions at intended gameplay distances;
  - the core toon-shading language;
  - palette relationships;
  - material identity, including the Synthetic's luminous chassis;
  - combat telegraphs, target outlines, forecast information, hit timing, and VFX readability;
  - stable frame pacing;
  - essential lighting and shadow cues needed to ground characters in the scene.
- Do not create a low preset by globally disabling the visual system. Build it as a separately tuned art target.
- Establish representative low-spec hardware and measurable frame-time, memory, loading, and image-quality gates during the technical-art spike.
- Prefer graceful reductions in secondary richness over degradation of protagonist identity.

## 6. Required report updates

Claude should:

1. Correct the Operator contradiction in `CLAUDE-TECHNICAL-CONSULTATION-001.md`.
2. Remove the Operator arm question from the blocker count and questions file.
3. Add the mech 2x2/3.6 m hypothesis to the spike as a falsifiable test, not settled canon.
4. Add four gear slots, 2/4 set logic, and character-specific weapons to progression, content-schema, animation-cost, UI, and risk analysis.
5. Add the Synthetic jacket toggle to character UI, presentation architecture, asset validation, and cinematic override rules.
6. Add scalable graphics settings and the minimum-quality floor to technical art, performance, risk, and spike acceptance criteria.
7. Recalculate affected blocking-question counts and report any new architecture questions.
8. Do not push or implement until Juan and Mina explicitly authorize the next phase.

## 7. New questions that remain material

1. What are the names and mechanical responsibilities of the four gear slots? The weapon slot is already separate and is not part of this unresolved taxonomy.
2. Do equipped gear pieces appear visually during gameplay, or do only weapons and approved overlays change?
3. Are set bonuses tied to named authored sets only, or may compatible pieces share a family tag?
4. Can a two-piece bonus from one set combine with a two-piece bonus from another? **Current ruling: yes.**
5. Does the Synthetic's jacket preference persist per save, per loadout, or globally? Recommendation: per loadout with a global convenience override only if user testing demands it.
6. What exact minimum hardware defines the lowest supported preset?
7. Does the 2x2 mech remain on-map while the pilot is physically inside it, remotely operating it, or both depending on Specialty state?
