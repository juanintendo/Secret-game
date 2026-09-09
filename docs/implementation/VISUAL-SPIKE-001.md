# Visual Spike 001 — canonical combat beauty proof

**Status:** ready to begin after Tactical Foundation Checkpoint 001 records Unity **7/7**.

**Engine:** Unity 6.3 LTS `6000.3.23f1`, URP 17.3.

**Purpose:** prove that the canonical Cyborg and her single mech can look premium during ordinary tactical gameplay—not only in a hero render—while consuming the approved deterministic event stream.

## Product question

Can one representative combat shot preserve the identity and 1990s OVA language of the approved sheets at modern fidelity, remain readable as a tactics game and hold a stable 60 fps target on representative hardware?

The spike is a falsification test. It is not a request to make an entire art pipeline or final level.

## Canonical visual sources

Read `art/ART-MANIFEST.md` first. For this spike use:

| Asset | Authority in this spike |
|---|---|
| `art/SHEET-cyborg-field-001.png` | Cyborg face direction, blonde hair mass, clothing topology, symmetric mechanical forearms/hands, silhouette |
| `art/SHEET-mech-001.png` | single-mech silhouette, plating language, oxidation, red sensor, articulation and cable language |
| `art/HERO-definitive-trio-002.png` | target mood, upgraded-mech relationship and main-timeline palette context |
| `art/STUDY-mech-scale-footprint-001.png` | reference only for cockpit construction and scale exploration; never metrology |

The generated images are modelling references, not frames to ship.

## One scene, one shot, one action

Build exactly one representative scene:

- a compact industrial relay-yard arena;
- one canonical Cyborg presentation rig;
- one presentation model of the same historical mech;
- one neutral enemy proxy;
- tactical camera plus a short contextual push-in;
- idle, selection, two-cell movement, one hit reaction and one Docked transition;
- one forecast overlay driven by kernel output;
- one resolved attack with restrained VFX and immediate hit confirmation.

No campaign environment, dialogue cinematic, skill tree, inventory screen, final enemy or second protagonist belongs in this spike.

## Visual stack under test

### Character identity

- Gameplay camera must retain the Cyborg's blonde hair silhouette, blue-gray field clothing and bilateral elbow-down prosthetics.
- The mech remains one machine. Modular presentation may change attachments later; this spike must not create a second identity.
- Proportions are compared against the sheets using matched orthographic capture, not memory.

### Rendering

- URP forward rendering.
- Controlled toon-light bands with authored thresholds.
- Stable screen-space or geometry-assisted outlines without face crawling.
- Material families for skin, cloth, painted metal, exposed mechanics, hair and emissive indicators.
- Oxidation remains a material/readability layer, not noisy procedural dirt.
- Essential grounding shadow survives every quality preset.

### Camera and animation

- Input, camera, targeting, hit confirmation and UI remain continuous at display frame rate.
- Character posing may use stepped OVA cadence; root movement and camera may not stutter with it.
- Docked must read as “pilot boards and leaves the tactical map,” not as an unexplained mode toggle.
- Camera never hides occupied cells or the 2×2 provisional footprint during decision time.

## Required captures

Produce reproducible captures from fixed camera IDs:

1. orthographic front/profile/back comparison for Cyborg;
2. orthographic front/profile/back comparison for mech;
3. normal combat wide shot;
4. selected-unit shot with outline and forecast;
5. hit-confirmation frame;
6. Docked transition start, midpoint and completion;
7. High and Low preset comparison from the same camera.

Store capture metadata beside images: commit, Unity version, scene, camera ID, preset, resolution and hardware.

## Measurable gates

| Gate | Pass condition |
|---|---|
| Identity | Juan approves Cyborg and mech as the same designs shown in the canonical sheets; no known topology contradiction |
| Readability | At normal combat distance, silhouette, selected unit, occupied cells, target and forecast remain distinguishable without camera movement |
| Temporal clarity | Hit confirmation begins immediately after the resolved event; stepped animation does not delay rules feedback |
| Kernel boundary | Removing all presentation objects leaves identical command/event/hash results |
| Outline stability | No visible outline popping/crawling in the fixed movement and camera loop |
| High preset | Delivers the intended premium target image rather than merely enabling more effects |
| Low preset | Preserves face read, silhouette, toon bands, palette, targeting, essential shadows and stable frame pacing |
| Performance | Provisional 60 fps frame budget on the selected representative machine; CPU/GPU timings captured separately |

Juan's visual approval is a real gate, but it is not the only gate. A beautiful still that breaks in motion fails.

## Three scale presentations

Run the same logical replay at `cellSizeMeters` values `1.0`, `1.25` and `1.5`.

- Event stream and final state hash must remain bit-identical.
- Compare framing, doorway credibility, attack-animation clearance and character readability.
- Do not infer the winner from the generated study's drawn grid.
- Record a recommendation; do not promote it to canon without Juan and Mina's ruling.

## Deliberate production shortcuts

Permitted:

- one high-quality hero rig with only the motions required above;
- proxy enemy and modular arena kit;
- temporary audio;
- hand-authored material values for one lighting setup;
- supervised procedural assistance for texture masks and cleanup.

Forbidden:

- shipping generated character frames;
- frame-to-frame generative video for canonical animation;
- three separate mech models for the provisional Specialties;
- duplicated gameplay/cinematic character identities;
- visual scripts that recalculate combat;
- hiding missing gameplay readability with cinematic cuts.

## Exit decision

The spike ends with one of four verdicts:

- `PASS` — visual target, readability, motion and performance are credible;
- `PASS WITH PIPELINE CHANGES` — look works but production method must change;
- `FAIL VISUAL TARGET` — the scene does not match the approved identity/quality bar;
- `RECONSIDER ENGINE OR SCOPE` — representative evidence shows URP or the production model cannot carry the promise affordably.

Do not expand asset production before that verdict.
