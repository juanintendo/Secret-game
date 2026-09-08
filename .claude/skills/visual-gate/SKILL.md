---
name: visual-gate
description: "The anti-slop production gate for this project's 1990s-anime visual identity: canonical character source packages, shared-3D LOD rules, toon shading, hair/cloth assignment, stepped animation cadence at 60 fps, cinematic tiers, generative-tool boundaries, and the automated golden-render tests. Load before any art, shader, rig, animation, VFX, cinematic, UI-visual, or asset-import task, and before approving any generated image or video."
---

# Visual Gate

Read `canon-guard` first. Approved character art is canonical reference, not moodboarding.

## 0. The identity law

**One canonical rigged 3D asset per principal character. Gameplay and in-engine cinematics derive from that same model and rig.**

LOD0 (cinematic) and LOD1 (gameplay) **must share**: mesh topology, UVs, material IDs, and skeleton root hierarchy. They may differ only by: subdivision, hair bone count, face rig complexity, texture resolution, and shadow-shape resolution.

**They may never differ by design.** If a gameplay model has a different costume, silhouette, or face construction than the cinematic model, the pipeline has failed and you stop.

This does not forbid fully authored 2D S-tier sequences. It gives them an authoritative pose, lens, proportion, costume, and lighting reference.

## 1. Character source package — required before a character enters production

- Approved hero image + neutral model sheet.
- **Orthographic front / profile / back.**
- Height and proportion chart shared across the whole cast.
- Face construction sheet: neutral, three-quarter, profile, extreme expressions.
- Hair mass sheet — primary volumes, not individual strands.
- **Costume topology map that names each garment's rig strategy** (skinned / bone-chain / cloth / card / shader / 2D overlay). This is the document that prevents budget surprises. §4 is the assignment table.
- Palette with stable colour/material IDs under neutral light, exported as a **swatch strip** that both the 2D artists and the toon shader read from.
- Material board: cloth, lacquer, skin, armor, luminous chassis, hologram.
- Hand/foot anatomy and mechanical-joint reference.
- **Turnaround at the gameplay camera angle** (≈45° down) — orthographic sheets lie about how a design reads from the tactical camera.
- **Silhouette-only plate at 128 px and 64 px height.**
- **Motion archetype sheet** — the ~8 signature actions (this is also the content budget; see `content-schema` §0).
- Signature VFX colour IDs (2–3 locked emissive colours per character).
- Forbidden-drift examples.
- Current canonical revision + provenance record.

Existing assets: `docs/canon/art/SHEET-synthetic-orthographic-001.png`, `docs/canon/art/HERO-definitive-trio-001.png`. **Three art-vs-text conflicts are unresolved — see `canon-guard` §5 before locking any costume.**

## 2. Toon shading

- Flat ramp with a hard shadow terminator; one controllable shadow-shape parameter per material.
- Rim / edge light as a separate authored pass.
- **Character outlines: inverted hull.** Stable, art-directable width, no shimmer.
- **Environment outlines: screen-space depth/normal edge pass.** Never apply the screen-space pass to characters — it shimmers at gameplay distance and violates the "no uncontrolled frame-to-frame shimmer" pillar.
- Colour model lock: a single swatch file defines the exact flat / shadow / highlight colour per material ID. The shader samples it; golden-render tests assert the output matches within ΔE00 ≤ 3.

## 3. Analog post stack — order matters, UI is never inside it

Independently toggleable Renderer Features, in this order:

1. Line-weight stabilization (**before** any blur)
2. Halation / bloom, masked to emissive material IDs
3. Luma / chroma separation
4. Scanline / phosphor texture
5. Palette-aware grain
6. Gate weave — **cinematics only**
7. Optional tape dropout, as punctuation only

**UI is composited last, on a separate overlay camera, excluded from the entire stack.** Text must stay pixel-crisp.

No global VHS preset. No permanent chromatic blur. No grain strong enough to erase illustration or tactical information. Every pass has a debug toggle and a golden-image test.

## 4. Hair and clothing assignment

| Element | Strategy | Budget note |
| --- | --- | --- |
| All primary silhouette masses | **Skinned geometry** | The silhouette is never simulated |
| Synthetic's bob | Skinned helmet shape, 3–5 bones at the tips | Cheap |
| Cyborg's long layered blonde hair | **3–4 authored bone chains, 4 bones each (~16 bones), spring solver with hard angular limits** | Not cloth sim. Hardest hair in the cast — prototype it first |
| Human's tousled short hair | Fully skinned, 2 accent bones | Cheap |
| Jacket tails, open jacket, long straps | Bone chains + spring | 2 chains max per garment |
| Belt pouches | Skinned | Never simulated |
| Synthetic's red bomber jacket | Controlled cloth — **cinematic LOD only** | Never cloth in gameplay LOD |
| Hair wisps / flyaways | **Alpha cards** attached to the mass | Reads as ink lines, near-free |
| Synthetic's entire luminous chassis | **Shader** — emissive + fresnel + scrolling iridescence keyed to material IDs | Her costume *is* a shader. Cheapest character in the cast |
| Lacquer panel specular, shock coating, mech heat/vent | Shader parameters | Free per-ability variation |
| Eye highlights, eye cuts, impact frames, speed lines, chassis flare accents | **2D overlays** | The 90s feeling lives here |
| Anime hair highlight band | Authored band texture in hair UV space | More art-directable than a specular model |

**Forbidden in gameplay LOD:** garter straps and other thin self-intersecting geometry (reads as noise at 128 px), unsupported free cloth, individual-strand hair, and heeled boots with separate ankle geometry that breaks foot IK on stairs and elevation changes. *(This directly implicates the unresolved costume conflict `canon-guard` §5 A1.)*

## 5. Stepped cadence at 60 fps — the correct implementation

The usual mistake is sampling the whole character at 30 Hz, which judders against a moving camera. Do this instead:

**Split the rig.**
- **Sampled every frame, continuous:** root/pelvis transform, anything that tracks the world (weapon muzzle, IK targets, attach points, camera targets), grid movement interpolation.
- **Sampled stepped:** pose layers (spine-up, limbs, face), by quantizing the clip's time cursor — `t_step = floor(t * rate) / rate`, with `rate = 12` for 2s or `8` for 3s.

**Implement as a custom Playable / AnimationJob that quantizes clip time per layer.** Not a global timescale hack. Not baked 12 fps clips — baking destroys blending and locks the cadence.

**Never quantize:** camera, UI, VFX simulation, root motion, hit timing, projectiles, or grid movement. A body pose stepped on 2s sliding continuously across a pan is exactly what good cel animation looks like.

**Input acknowledgment is always on frame 1.** Fluid cursor/UI response, an audio click, and a camera micro-move fire immediately; the stepped pose may begin on the next step boundary (≤ 83 ms at 12 Hz) without reading as late.

**Hit timing lives in the simulation, not the animation.** The sim resolved already; presentation places the impact at an authored millisecond offset. A held pose can never delay damage.

Impact frames and smears deliberately break cadence — per-clip cadence override must be supported.

## 6. Making combat look like the cinematics

The cheapest available lever, and it fits the broadcast-switcher UI direction:

- Keep protagonists at **≥ 140 px tall at default zoom** on a 1080p screen. Below ~96 px, no amount of asset quality is visible.
- Add a **commit zoom**: a hard switcher-style cut to a closer camera for the action-resolution beat, using the same asset at cinematic LOD. Hair and face LOD switch at the same threshold.
- Every ability therefore gets seen at cinematic quality without a single extra asset.

## 7. Cinematic tiers — realistic slice budget

*(Estimates, not canon.)*

| Tier | Slice budget | Note |
| --- | --- | --- |
| **S** | 1 shot, 5–10 s | True hand-animated OVA quality is roughly 300–600 skilled hours or a 4–8 week funded outsource for ~10 s. **Do not attempt a true S for the vertical slice** — deliver an exceptional hybrid (3D layout + toon render + 2D impact frames + heavy comp) that reads as S. Save true 2D for the shipping opening. |
| **A** | 2 shots, 20–30 s total | In-engine, Cinemachine + Timeline, LOD0 rigs. The *pipeline* is the cost, not the shot. |
| **B** | 3–4 minutes | Layered art, constrained motion, camera, light, particles, typography. **This is where the 90s feeling gets delivered per dollar. Spend here.** |
| **C** | 12–20 cut-in lines | Portraits + 3–5 expressions each. Cheap, enormously effective. Do more than feels necessary. |

All tiers share palette, face design, costume revision, voice, terminology, and shot metadata.

## 8. Generative tools — the boundary

**Permitted (net labour saved):**
- Static background / matte-style backdrops for B-tier scenes (single frames, no temporal problem, overpaintable).
- Storyboard and thumbnail exploration; disposable animatics for timing.
- Non-hero prop and material variation, with human cleanup.
- UI/typography *exploration* — never final text.
- Enemy silhouette ideation (not final designs).
- Supervised upscaling/cleanup of your own authored art.
- **Code and tooling generation** — validators, importers, test harnesses, the screenshot-regression rig. Highest-value use for this team by far.

**Not permitted (cleanup exceeds labour saved):**
- Any canonical character frame or animation. Identity drift is per-frame; cleanup is per-frame; you pay full animation cost *plus* review cost. Budget ~1.5–3× authored cost for temporally-consistent character video cleanup at this quality bar.
- Faces and hands, at any scale.
- In-betweens for S-tier — interpolation smears the line and mutates costume detail.
- Anything that becomes a material or palette ID. Generated colour drifts; the whole pipeline depends on stable IDs.

**Entry rule:** generated pixels may enter the project only as (a) a static background, (b) a reference/underlay that is overpainted, or (c) a non-hero texture — each with a provenance record. **Nothing generated ever becomes a canonical character asset, and nothing generated is ever the source of a material ID.**

## 9. Automated gates — run on every content or shader change

1. **Golden renders** — 6 fixed cameras per character (front / 3q / profile / back / gameplay-45° / cinematic-close) × 2 lighting rigs (neutral + scene key). Perceptual diff; any difference opens a review, not an auto-fail.
2. **Silhouette overlay** — alpha-only render at 128 px and 64 px vs. the approved plate; assert IoU ≥ threshold.
3. **Palette / material-ID sampler** — fixed sample points per material; assert ΔE00 against the swatch file.
4. **Proportion test** — head-height ratio, shoulder width, total height from the golden render vs. the proportion chart.
5. **Animation continuity scan** — step every frame of every clip; assert bone-length invariance, no hair/cloth proxy self-intersection, and no frame where a tracked attach point leaves its envelope.
6. **Screen-space readability** — render the gameplay-45° camera at 1080p, downscale the character to actual on-screen size, assert silhouette IoU still passes. *This is the test that catches "beautiful in the viewport, mush in the game."*
7. **UI regression** — screenshot each screen at min/max resolution; assert text is rendered by the text system (no rasterized text) and run a contrast check.
8. **Post-stack per-pass goldens** — a shader change must not silently muddy faces.
9. **Colourblind simulation** on tactical overlays (protanopia / deuteranopia) with a legibility assertion. The UI direction is cyan/green/magenta/red — that is a red-green problem. Shape and pattern coding from day one; this is **not** a cuttable feature.

**Human gates (never automated):** final art-direction approval, "is this the same person," and cadence/feel review.

## 10. Rejection criteria — refuse the asset, do not negotiate

Extra fingers · drifting faces · inconsistent costumes · changing limb lengths · unreadable silhouettes · fake or malformed text · topology drift · uncontrolled frame-to-frame shimmer · a generated frame that looks impressive once but cannot pass §9 · any asset without a provenance record.
