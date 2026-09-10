# Visual Spike 001 — bounded technical-art plan

Status: EXPERIMENT. Baseline: 47d60960c8113fc664fb3a1414daacd0c0c25c82.
The accepted tactical foundation is frozen. This deliverable is the smallest executable reference experiment; premium character approval remains a separate gate.

## Sources inspected

Inspected at original resolution: `art/SHEET-cyborg-field-001.png`, `art/SHEET-mech-001.png`, `art/HERO-definitive-trio-002.png`, and `art/STUDY-mech-scale-footprint-001.png`. The first two govern construction, the hero governs mood, and the study supplies only provisional boarding/scale hypotheses. No generated pixels are used as game frames.

## Scene and assets

One `VisualSpike001` relay yard, one Cyborg study rig, one mech study rig, one neutral capsule enemy, a tactical orthographic camera and fixed orthographic comparison cameras. Hand-authored procedural meshes/material assemblies provide inspectable construction studies. They are not approved production character assets. One continuous replay covers selection, two-cell movement, boarding and one resolved mech attack. All movement, occupancy, damage and forecasts come from the existing resolver.

## Modelling and rigging limits

Preserve blonde layered mid-back hair, blue-gray sleeveless tank/cargos/waist garment, bare upper arms, symmetric elbow-down plated prosthetics, cyan wrists, five-digit hands, bilateral thigh rigs and flat black boots. Preserve squared mech shoulders, low compact head, one red sensor, white armor, black joints, red cables, broad feet, four-digit hands and vented backpack. Use a single mech hierarchy across all views and modes.

Missing: approved production meshes/skin weights, face construction, hair masses, detailed costume topology, material swatches, motion archetypes and definitive upgrade breakdown. Pouch intent and prosthetic removability remain unresolved. Reproduce visible pouch asymmetry without declaring its intent. Do not invent internal prosthetic construction, weapons or upgrade variants. Rear/top boarding and 1.75/3.6 presentation heights are explicit reference hypotheses. The study rig cannot establish premium facial fidelity or final docking mechanics; stop character production at this experiment and report those gates open.

## Renderer, materials and outlines

URP forward; one code-based three-band lit shader for skin, cloth, hair, painted metal, mechanics and indicators. Broad authored oxidation accents only. Separate inverted-hull outline shader with pixel-width extrusion, front-face culling and shared meshes; no temporal history. Risks: hard-edge separation, small-part clutter, silhouette inflation and near-plane artifacts. Omit face/internal detail hulls; use the fixed motion/camera loop to inspect stability. No promise of an approved outline until captures are reviewed.

## Lighting and camera

One shadowed warm directional key, cool ambient fill, cyan/red emissive industrial fixtures. Real grounding shadows on both presets. Tactical orthographic decision framing shows the full logical arena; a modest continuous push-in occurs only during attack playback. Root motion/camera sample continuous time; limb idle and hit posing sample 12 Hz. Event hit confirmation starts immediately, independent of pose cadence. Boarding follows rear approach, upward climb and descent into the provisional upper torso; presentation never writes combat state.

## High/Low contract

Same identity geometry, palette, three light bands, outline, forecast and occupancy overlays. High: 4x MSAA, 2048 main shadow map, soft shadows. Low: 2x MSAA, 1024 main shadow map, hard shadows, slightly wider outline. No removal of essential systems. Both target 60 fps; target hardware is not specified, so local-machine timings are evidence only.

## Capture and profiling

Editor tooling builds a saved scene and assets reproducibly, then captures front/profile/back per character; wide; selection/forecast; hit; docking start/mid/end; High/Low wide comparisons at 1920x1080. Each PNG gets source paths, git commit/dirty state, engine, scene, camera ID, preset, scale, time and hardware metadata. A standalone continuous replay records frame pacing and separate CPU/GPU FrameTimingManager readings; unavailable GPU measurements remain unavailable, never zero-cost. Warm up before sampling; report p95/p99 and missed 16.67 ms frames. No stable-60 claim from screenshots or headless tests.

## First three experiments (in order)

1. Identity/material/outline: fixed orthographic comparisons and tactical High/Low captures. Record construction limitations; request visual approval only after evidence exists.
2. Temporal/authority: continuous movement, rear/top Docked study and attack; compare forecast/event/hash results with and without presentation and at cell sizes 1.0, 1.25 and 1.5. Recommend 1.25 only as an initial framing hypothesis, pending comparisons.
3. Performance/readability: capture a warmed standalone motion loop, inspect outline movement and occupied-cell visibility, record CPU/GPU and frame pacing. At most one bounded optimization pass before reporting a budget failure.

## Deferred

Final sculpt/retopology, facial rig, cloth/hair simulation, production textures, final hatch topology, hardpoints, upgraded variants, extra characters, full UI, audio production, cinematics, postprocessing stack and content pipelines. No canon or kernel changes. Approval and hardware performance remain explicit exit gates.
