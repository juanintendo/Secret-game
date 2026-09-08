# CLAUDE TECHNICAL CONSULTATION 001

Consultant: Claude (Opus 5) — game systems architecture, technical art direction, animation pipeline, adversarial production review.
Date: 2026-09-08 · **Revision 2, incorporating `MINA-CONSULTATION-ADDENDUM-001.md`**
Requested by: Mina (technical direction) and Juan (creative direction)
Working source: `C:\Users\colom\Documents\DD90s`
Implementation lead: Mina, primarily via OpenAI Astra.

> ### Revision 2 change log
>
> 1. **Operator's arms — my finding in revision 1 was wrong and is retracted.** The turnaround depicts the canonical asymmetry correctly. See §5 C1. **No longer a blocker.**
> 2. **Mech scale and footprint** — a provisional hypothesis now exists (≈3.6 m, 2×2 tiles, ≈2.05× pilot ratio, rear-upper-torso hatch). **Provisional, not canon.** Remains blocking until the spike validates it. §4.2, §7, §16, spike.
> 3. **Equipment structure — settled product direction.** Four gear pieces per protagonist, combinable 2+2 set bonuses, full 4-piece sets, character-specific weapon classes, canonical loadouts in cinematics. §3, §8.5, §10.5.
> 4. **Synthetic red-jacket visibility toggle — settled presentation direction.** §3, §11.8, §12.
> 5. **Graphics and performance policy — settled quality target**, including a minimum preset that is a separately tuned art target rather than a disabled visual system. §3, §13.2.
> 6. **Architecture blockers recounted: 6 → 5** *(superseded by revision 3: the true count is **3**).*
>
> ### Revision 3 change log
>
> 1. **Equipment ruling refined by Juan: `1 weapon + 4 gear`.** The weapon slot is separate and **never counts toward a set threshold**; only the four gear pieces do. `mech-hardpoint`, if retained, occupies one of the cyborg's four gear slots — not a fifth. The `CODEX-BRIEF` §12 five-category conflict is resolved as `1 weapon + 4 gear`, **never `5 gear + weapon`**. §3.1, §8.5, §10.5.
> 2. **Architecture blockers reclassified: 5 → 3.** Con Girl / Agility is settled and its document update is an editing task; sheet hygiene is source governance. Neither is a combat-kernel ruling. §21.
> 3. **One conditional fourth blocker identified** — the pilot/mech entity model (Q31). It blocks architecture *only* if a single-entity model is chosen. §9.6, §21.
> 4. The two images named by the addendum **remain absent from disk and remain uninspected.** §2.
>
> ### Revision 4 change log
>
> 1. **Pilot/mech entity model settled by Mina.** Separate, stable logical entity IDs regardless of deployment mode; embarked pilot is non-spatial, non-selectable, non-targetable and holds no initiative slot; remote operation may make both spatial. §9.6.1.
> 2. **The conditional fourth blocker is closed.** Q31 becomes a combat-prototype and product question, cross-referenced to Q8. **The architecture-blocker count is exactly three: Q2, Q3, Q4.** §21.3.
> 3. **§9.6 split** into the settled entity model (9.6.1) and the still-open command model (9.6.2). They were previously one section and one muddle.
> 4. **Third verification of the two named images and the addendum on disk: all three still absent.** Nothing in this document claims otherwise, and no reconciliation against the on-disk addendum has been performed. §2.
>
> ### Revision 5 change log
>
> 1. **All three files received by ZIP, extracted, hashed and inspected directly.** §2 records them with SHA-256 and how each was examined. The on-disk addendum was read in full and **matches what was integrated at revisions 2–4 — no divergence.**
> 2. **Operator's asymmetry confirmed against the dedicated reference sheet**, and the sleeve-termination specification gap is **closed**: the sleeve begins below the shoulder cap over a bare section of upper arm, finished with a purple band. §5 C1.
> 3. **Mech study inspected and measured against its own ruler and grid.** It supports the *rationale* for a broad 2×2 silhouette and shows a working cockpit. **It validates nothing tactical.** §4.2.
> 4. **New finding — the study's grid legend is internally inconsistent.** The `2×2` bracket spans 4 drawn squares and the `1×1` pilot box spans 1.4, against a legend declaring 1 m squares. Coherent only at 0.5 m per drawn square. §4.2.
> 5. **New question Q32 — tactical tile size.** The study embeds a 1 m tile as a legend; no canon document rules one. It sets movement, ranges, adjacency and map scale.
> 6. **Architecture-blocker count unchanged: exactly three — Q2, Q3, Q4.** The Q31 entity ruling stands.
> 7. **Evidence classes added** in §2, separating visual construction evidence, declared direction, provisional hypotheses and experimentally validated facts. **The last class is still empty.**
>
> ### Revision 6 change log — consultation closeout
>
> 1. **My revision-5 recommendation to adopt a 0.5 m tile is retracted.** Mina's ruling: a generated study is construction evidence, not metrology. §4.2.
> 2. **Four concepts separated and ruled** — logical kernel cell · entity footprint in cells · Unity `cellSizeMeters` · decorative study grid lines. **The kernel is dimensionless: integer cells, no metres.** §4.2, §9.1.
> 3. **Q32 reframed** from "what is the tile size" to a Unity visual-scale comparison at 1.0 / 1.25 / 1.5 m against identical combat rules. Still a sub-decision of Q4.
> 4. **Animation clearance added** to the mech validation set. §7.6.
> 5. **The `FOR SCIENTIFIC EDUCATIONAL USE ONLY` text is `MARKED FOR REMOVAL`**, together with the study's misleading grid legend and brackets. §4.2.
> 6. **Architecture blockers unchanged: exactly three — Q2, Q3, Q4.** No new consultation scope added.
> 7. **Consulting phase closed.** Handoff in `CLAUDE-CONSULTATION-HANDOFF-001.md`.

**Labels used throughout:** `CANON OBSERVED` · `RECOMMENDATION` · `OPEN QUESTION` · `RISK` · `REQUIRES PROTOTYPE`
Nothing in this document is canon. Where I state a fact about the project, it is labelled `CANON OBSERVED` and cited to a file. Everything else is mine and is labelled as such.

---

## 1. Executive Verdict

**The thesis is coherent. The visual canon has advanced considerably. The mechanical canon has not moved, and the gap between them is now the project's central production risk.**

Since the previous consultation the project has gained six orthographic character sheets covering every principal asset — the three protagonists, the original leader, Operator, and the mech. That is a real and unusual achievement for a preproduction indie project, and it closes questions that were previously blocking. It also creates a specific danger: **the art is now developed enough to make starting character production feel reasonable, while the combat thesis remains entirely unproven.** `90S-ANIME-XCOM-CODEX-CLAUDE-BRIEF-001.md` §20 warns against exactly this, and the new sheets make the temptation stronger rather than weaker.

Four things are now true that were not true at the first consultation:

1. **The cyborg's arm topology is resolved** by `Characters/ChatGPT Image Sep 8, 2026, 09_54_49 AM (1).png` — bilateral, symmetric, mechanical from the elbow.
2. **The white-haired human's costume is confirmed as the most expensive configuration in the cast**, on the character who moves most. It is not a canon violation — it contains none of the prohibited trim — but it is a budget commitment that has now been made implicitly rather than deliberately.
3. **Operator's arms are settled and were never in conflict.** Revision 1 of this document reported a contradiction. That report was wrong; the error was mine. See §5 C1.
4. **Four product directions are now settled by the addendum** — equipment structure, the Synthetic's jacket toggle, the graphics quality policy, and Operator's asymmetry — and **one hypothesis is explicitly provisional**: the mech's 3.6 m scale and 2×2 footprint.

**The addendum settles product direction. It does not advance the combat kernel.** Five rulings still block architecture, and three of them — `Integrity`, to-hit, mech occupancy — are the same three that blocked it before. They are cheap. No amount of further art or product direction will advance them.

**A note on what the addendum adds to the cost side.** Four gear pieces per protagonist with 2+2 and 4-piece set bonuses, character-specific weapon classes, and a minimum graphics preset that must be *separately art-directed* rather than merely reduced, are all correct decisions — and all three add production surface. §11.6's animation estimate and §13.2's preset work should be read as new budget lines, not as free consequences of a design choice already made.

**Verdict: coherent-with-conditions.** The conditions are that the kernel rulings are made, that the forecast is proven in text before anything is built in Unity, and that character production does not begin until the first representative encounter has been played and judged.

**What I am not doing in this document.** A previous consultation exists at
`C:\Users\colom\Desktop\Secret-game\docs\consult\CONSULT-CLAUDE-001.md`. It is **not** present in `DD90s` and Mina may not have seen it. Four of its visual findings are now obsolete — see §5. Its combat, architecture and pipeline analysis remains largely valid and I have not re-derived it here. `RECOMMENDATION`: read it after this document, treating its §3.1 visual findings as superseded.

---

## 2. Sources Inspected

Every file below was opened and inspected directly. Images were viewed and, where detail mattered, cropped and magnified. No visual fact in this document is inferred from a filename.

### Documents — read in full

| File | Bytes |
|---|---|
| `STORY-CANON-001.md` | 23,005 |
| `90S-ANIME-XCOM-PREPRODUCTION-001.md` | 33,818 |
| `90S-ANIME-XCOM-CODEX-CLAUDE-BRIEF-001.md` | 34,856 |
| `MINA-CONSULTATION-ADDENDUM-001.md` — read in full for revision 2; supplied via conversation, **not present in `DD90s`** | — |

### Team images — `Art/`

| File | SHA-256 (first 16) | Dimensions | Subject |
|---|---|---|---|
| `Art/Team 1-canon.png` | `fdebc354d8a4737d` | 1672×941 | Original team, prologue era |
| `Art/Team 2- canon.png` | `339decc678182ae8` | 1672×941 | Definitive team, main timeline |
| `Art/Team Concept 1st draft.png` | `98d23be12bb94ddb` | 1672×941 | Earlier definitive-team concept |

### Character sheets — `Characters/`

| File | SHA-256 (first 16) | Dimensions | Subject |
|---|---|---|---|
| `ChatGPT Image Sep 8, 2026, 05_51_52 AM.png` | `90f116624892d9d0` | 1438×1094 | Synthetic — base chassis, no costume |
| `ChatGPT Image Sep 8, 2026, 06_38_03 AM.png` | `90f116624892d9d0` | 1438×1094 | **Byte-identical duplicate of the above** |
| `ChatGPT Image Sep 8, 2026, 09_54_49 AM (1).png` | `53b2b0f895e60b36` | 1536×1024 | Cyborg mech pilot |
| `ChatGPT Image Sep 8, 2026, 09_54_50 AM (2).png` | `e4139f0a7b17b581` | 1374×1145 | White-haired human |
| `ChatGPT Image Sep 8, 2026, 09_54_51 AM (3).png` | `fa3002fc3406a643` | 1536×1024 | Original leader — armored state |
| `ChatGPT Image Sep 8, 2026, 09_54_51 AM (4).png` | `dd10578b70877701` | 1438×1094 | Synthetic — with red bomber jacket |
| `ChatGPT Image Sep 8, 2026, 09_54_53 AM (5).png` | `f02e248f26782821` | 1536×1024 | Operator |
| `ChatGPT Image Sep 8, 2026, 09_54_53 AM (6).png` | `9e200e16ed4e9668` | 1536×1024 | The mech |

### External reference — `Inspo/`

| File | SHA-256 (first 16) | Dimensions |
|---|---|---|
| `Inspo/danger-girl-num-0.jpg` | `0f885cae432c9509` | 800×800 |
| `Inspo/rco029_1462355049.jpg` | `13850a41d197d48c` | 1027×1600 |
| `Inspo/942396-danger_girl_alt_cover.jpg` | `c6b40cffc5f05432` | 432×649 |

Two of these three were opened directly; both are published *Danger Girl* material (Cliffhanger / Wildstorm, art by J. Scott Campbell). The third was classified by same-source grouping rather than direct inspection, and I state that rather than claiming otherwise. See §5, A4 for the risk this folder carries.

### Addendum and reference images — **RECEIVED AND INSPECTED, revision 5**

Delivered as a ZIP attachment through the conversation on 2026-09-08 after three failed attempts to locate them on disk. **All three were extracted, hashed and opened directly.**

| File | SHA-256 (first 16) | Size / dimensions | How inspected |
|---|---|---|---|
| `MINA-CONSULTATION-ADDENDUM-001.md` | `8f0fe9961285cc0c` | 7,984 B | Read in full from the extracted file |
| `operator-arm-asymmetry-reference.png` | `a527fef0665bd3dc` | 1,766,571 B · 1536×1024 | Full sheet, then left-side view and both callout insets at magnification |
| `mech-scale-footprint-study-v1.png` | `59a96719bfebab4d` | 2,301,137 B · 1536×1024 | Full sheet, then cockpit panel and top-down panel at magnification, plus **pixel measurement against the sheet's own ruler and grid** |

**Note on location.** They were supplied through the conversation, **not** through `C:\Users\colom\Documents\DD90s\art\character-sheets\`, which still does not exist. `RECOMMENDATION`: place all three at their canonical paths so future sessions find them without a hand-off, and record them in an art manifest with these hashes.

**On-disk addendum versus the version integrated at revisions 2–4.** Read in full and compared. **No divergence.** §3 of the file confirms `four equipped gear pieces plus one separate character-specific weapon slot`, that the weapon `does not contribute to two-piece or four-piece set thresholds`, and that a `mech-hardpoint`, if retained, `must occupy one of her four personalized gear slots. It is not an additional sixth slot.` That is what §3.1, §8.5 and §10.5 already implement. §7 item 1 also confirms the weapon slot is outside the unresolved slot taxonomy — which is why Q26 is a naming task.

### Evidence classes used in this document

Added in revision 5, because the distinction now carries weight.

| Class | Meaning | Example |
|---|---|---|
| **Visual construction evidence** | Read directly off an approved sheet at magnification. Binding for what it shows. | Operator's bare right arm; the mech's cockpit hatch geometry |
| **Declared project direction** | Ruled by Juan or Mina. Binding, and independent of any drawing. | `1 weapon + 4 gear`; the pilot/mech entity model; the graphics floor |
| **Provisional hypothesis** | Proposed and explicitly not canon. Requires a test. | The mech's 2×2 tactical footprint |
| **Experimentally validated fact** | Measured by a test that could have failed. **This project currently has none.** | — after the spike: forecast parity, turn-resolution time, the seven §7.6 validations |

**The fourth column is empty, and that is the single most important fact about the project's current state.** Everything believed today is drawn, declared or hypothesised. Nothing has yet been falsifiable and survived.

### Not present in `DD90s`### Not present in `DD90s`

- No consultation document. No risk register. No decision log. No art manifest.
- No enemy designs, no environment art, no UI mockups, no palette or material swatch file.
- No proportion chart shared across the cast.
- No face-construction, expression, or hair-mass sheets for any character.

---

## 3. Preserved Canon

Restated compactly so that nothing below can be mistaken for a redefinition. Every line is `CANON OBSERVED` with its source.

**Product.** Premium story-driven tactical RPG; three fixed protagonists; no appointed leader; each already individually capable — "the recruit is not level one; the unit is level one" (`STORY-CANON-001.md` §3.2). Progression runs on two axes, individual power and earned synchronization (§3.3). Synchronization fills from cooperation, not damage (`CODEX-BRIEF` §11). Combat is legible before commitment; animation celebrates a resolved decision and never conceals the rules (`PREPRODUCTION` §2.3, `CODEX-BRIEF` §2.4).

**Attribute axes.** White-haired human — Agility, verb *Exploit / Redirect*. Cyborg mech pilot — Strength, verb *Break / Anchor*. Luminous synthetic — Intellect, verb *Rewrite / Conduct* (`CODEX-BRIEF` §5, `PREPRODUCTION` §5).

**Story spine.** Original trio at near-peak power; a prepared intrusion exploits the *second* synchronized Limit Break; Operator apparently dies at HQ, a separate location struck in the same coordinated operation; the leader rejects her AI copilot's machine-optimal recommendation, creates the escape path by human judgment, and is presumed dead with no body (`STORY-CANON-001.md` §5). The survivors go underground separately for years (§6). Operator returns and recontacts each individually (§7). Authorially true and not player-known: the leader survives in some form and becomes the unknown protective intruder (§8.1).

**The synthetic is not a healer.** Support is conditional and earned (`CODEX-BRIEF` §9, §10).

**Visual identity is sacred.** Approved art is canonical reference, not moodboarding; no generated frame becomes a canonical character asset merely because it looks impressive once (`CODEX-BRIEF` §2.1, `PREPRODUCTION` §12).

**Authority.** Juan and Mina share creative direction; Mina owns technical direction (`PREPRODUCTION` §16).

### 3.1 Settled by the addendum — revision 2

All `CANON OBSERVED`, sourced to `MINA-CONSULTATION-ADDENDUM-001.md`, Juan-approved. Restated compactly; the operative detail lives in the sections named.

**Operator's arm asymmetry (§1).** The anatomical **right** arm is exposed below the shoulder armor with no sleeve, glove, bracer or wrist covering. The **left** arm carries the white sleeve and white glove with purple detailing. A left-facing profile shows only the near, camera-facing left sleeved arm; the far right arm must never appear through the torso. **Settled visual canon — not a conflict, and not a blocker.**

**Equipment structure (§3, refined by Juan's revision-3 ruling).** The shape is **`1 weapon + 4 gear`**. Exactly **four gear slots** per protagonist, plus **one separate character-specific weapon slot**. **Only the four gear pieces participate in set bonuses; the weapon never counts toward a set threshold.** Sets support a **two-piece bonus** and a **full four-piece bonus**; a character may run **two different two-piece bonuses** or one complete four-piece set. `mech-hardpoint`, if retained, **occupies one of the cyborg's four gear slots** — it is not an additional fifth gear slot or sixth equip position. Weapons are **not interchangeable between protagonists**. Set bonuses expand builds **without erasing character identity** and must not become random-affix loot soup. **Final cinematics always use the authored canonical visual loadout**, regardless of what is equipped. **The `CODEX-BRIEF` §12 five-category list reconciles only as `1 weapon + 4 gear` — never as `5 gear + weapon`.** The four gear slot names are **not yet decided** and are not invented here — see `CLAUDE-QUESTIONS-001.md` Q26, now a naming task only.

**Synthetic red-jacket visibility (§4).** **Only** the luminous Synthetic receives a character-equipment UI option to show or hide the inherited red jacket. It is a **cosmetic visibility toggle** — not a stat, a skill, an equipment slot, an item, or an alternate body. Both states use the **same canonical model and rig**. The choice may be reflected in ordinary gameplay presentation. **Authored cinematics override the toggle** and use the scene's canonical costume state; for Team 2 canon with no scene-specific exception, the jacket is shown. Portraits, UI renders, shadows, VFX anchors, hitboxes, targeting, animation timing and simulation **must not diverge** between states. Implemented as **one controlled optional garment layer**, never by duplicating the character.

**Graphics and performance policy (§5).** Maximum visual quality at the highest preset, plus industry-standard graphics and accessibility settings so slower supported systems run well. **The lowest supported preset is a separately tuned art target** — intentionally art-directed, stable, legible and polished. It may be simpler; it must never look broken, muddy, generic or graphically abandoned. **A low preset is not created by globally disabling the visual system.** What may scale and what must be preserved is enumerated in §13.2.

### 3.2 Explicitly provisional — revision 2

**Mech scale and footprint (addendum §2).** `REQUIRES PROTOTYPE`, **not canon**: mech ≈ **3.6 m**; pilot ≈ **1.75 m**; ratio ≈ **2.05×**; human footprint **1×1**; mech footprint **2×2**; a **rear-upper-torso / top-entry cockpit hatch** fitting the existing silhouette; a single mech identity and silhouette. It becomes binding **only** if the spike validates doors and traversal, narrow-map pathfinding, cover interaction, occupancy and forced movement, camera framing, encounter-space cost, and remote-control readability. **If 2×2 fails, the same broad mech is not silently compressed into 1×1** — the conflict between visual scale and tactical footprint is escalated as a decision.

---

## 4. Current Verified State

### 4.1 What the new sheets establish

**Cyborg** — `Characters/…09_54_49 AM (1).png`
`CANON OBSERVED`: Long layered blonde hair, heavy volume, falling to mid-back; blue-gray sleeveless tank; blue-gray cargo trousers with a garment tied at the waist; heavy belt with bilateral thigh rigs; **flat lug-soled black combat boots with buckle straps**; **both arms mechanical from the elbow — a dark articulated elbow assembly, a white plated forearm, a dark wrist cuff carrying a cyan indicator, and a five-digit mechanical hand.** Construction is symmetric left and right.

This **closes the previously open arm-topology question.** The written canon's phrasing — "mechanical forearms/hands" (`STORY-CANON-001.md` §2.2) — is correct. `CODEX-BRIEF` §3.1's looser "mechanical arms" is not contradictory but is less precise; `RECOMMENDATION`: tighten it to match.

`OPEN QUESTION`: The two thigh rigs are **not** identical — the wearer's right carries a different pouch assembly from the left. In a soldier's loadout that is natural and good. But a turnaround is the document a modeller builds from, so **this asymmetry must be recorded as deliberate or it will be silently "corrected", and then it will drift back.**

**White-haired human** — `Characters/…09_54_50 AM (2).png`
`CANON OBSERVED`: Short tousled pale white-lavender curly hair, chin-length, high volume; hoop earring. Black high-gloss halter playsuit with side cut-outs and horizontal strap bands at the ribs; wide buckled waist belt; **bare thighs** between the playsuit hem and the boot tops; **bilateral garter rigs** — an upper band, a vertical strap to the bodysuit, a lower band, buckles throughout, with a small pouch on the wearer's right; black fingerless gloves with buckled wrist cuffs, dark nail polish; **glossy thigh-high boots on a substantial block heel**; a long black coat **worn pushed off both shoulders with the arms through the sleeves**, so the garment hangs from the arms rather than the shoulders.

This resolves the costume question and it resolves it expensively. See §6 and §11.5.

**Original leader** — `Characters/…09_54_51 AM (3).png`
`CANON OBSERVED`: Bright green hair, heavy bangs, high ponytail to mid-back; teal-blue eyes; teal ear device. **Red bomber jacket worn open** — ribbed cuffs and hem, a black rectangular patch on each sleeve, a zippered sleeve pocket, cyan zipper tape. **Full external white/gray segmented armor** over torso, arms, legs and feet, with cyan light strips and orange accents over a black underlayer; armored gauntlets and articulated black hands; armored boots with black tread.

`RISK`: her arms and hands are fully enclosed by armor in the only sheet that exists. She is fully human (`STORY-CANON-001.md` §2.1) and **this sheet cannot show that.** A modeller or a generative pass working from it alone will produce a character who reads as a cyborg — which would destroy the prologue's central point, that a *human* overrides the machine.

**Synthetic** — `Characters/…05_51_52 AM.png` (base) and `…09_54_51 AM (4).png` (jacketed)
`CANON OBSERVED`: Black bob with blunt bangs, chin-length; opaque head — the skull is not part of the luminous structure; human face and skin on face and neck only; **iridescent chassis carrying a full readable luminous skeleton**; **black lacquer panels** on the outer upper arms through the hands and down the lateral surfaces of the legs, with the anterior and medial surfaces showing the chassis; barefoot with articulated luminous foot bones; slim but healthy proportions. The jacketed sheet shows the **same red bomber jacket** — patches on both sleeves, ribbed cuffs and hem, zippered sleeve pockets — worn open, cuffs ending at mid-forearm over the black panels.

Having both a base and a costumed sheet for the same character, at the same dimensions, is the strongest asset in the folder. `RECOMMENDATION`: make this pairing the template for every other character.

**Operator** — `Characters/…09_54_53 AM (5).png` — *corrected in revision 2*
`CANON OBSERVED`: Enormous pale blue-white curly hair to hip length; white high-collar bodysuit with purple seam piping and a purple ring clasp at the sternum; dark purple glossy shoulder caps on both shoulders; white boots with purple trim on a purple block heel.

`CANON OBSERVED` — **the arm asymmetry is present and correctly drawn**, verified by direct magnified inspection of each view:

- **Front view.** The anatomical **right** arm: purple shoulder cap, then bare skin the full length to a bare hand with visible skin tone and nail colour — no sleeve, no glove, no bracer, no wrist covering. The anatomical **left** arm: purple shoulder cap, white sleeve with purple bands at bicep and forearm, a purple wrist cuff, and a white glove.
- **Back view.** Consistent — the sleeved and gloved arm and the bare arm are on the correct sides.
- **Left-facing profile.** One arm drawn, the near camera-facing left sleeved arm. **The far right arm does not show through the torso.** The defect the addendum warns against is not present in this sheet.

**Revision 1 of this document reported the opposite and was wrong.** See §5 C1 for the correction and the methodological cause.

`OPEN QUESTION` — minor construction detail, not a blocker: the profile view shows a short section of **bare upper arm between the purple shoulder cap and the top of the white sleeve**, while the front view reads as the sleeve beginning at the cap. The sleeve's proximal termination should be fixed explicitly on her costume topology map so the modeller does not have to choose.

**Mech** — `Characters/…09_54_53 AM (6).png`
`CANON OBSERVED`: Bipedal humanoid frame; heavy white/off-white armor plating with oxidation streaking; a single red sensor eye on a compact head set low between large squared pauldrons; exposed black articulation with hydraulic pistons, ribbed hoses and red cable runs at the joints, waist and limbs; four-digit manipulator hands; heavy thigh and shin plating; broad splayed feet; a vented backpack unit.

**`RISK` — still the highest-value gap in the folder, now with a hypothesis attached.** The sheet itself carries **no pilot-scale reference, no cockpit, and no visible entry hatch.** See §5, G1 and §4.2.

### 4.2 Mech scale hypothesis — provisional; study now inspected and measured (revision 5)

Source: `MINA-CONSULTATION-ADDENDUM-001.md` §2 (read from file) and `mech-scale-footprint-study-v1.png` (inspected and measured). **Still not settled canon.**

| Parameter | Provisional value |
|---|---|
| Mech height | ≈ 3.6 m |
| Pilot height | ≈ 1.75 m |
| Height ratio | ≈ 2.05× |
| Human footprint | 1×1 tile |
| Mech footprint | **2×2 tiles** |
| Cockpit | Rear-upper-torso / top-entry hatch |
| Identity | A single mech, one silhouette; repaired and upgraded, never replaced |

#### What the study demonstrates — `visual construction evidence`

1. **The ratio is drawn, not merely asserted.** Mech and pilot share one vertical ruler marked 0 / 1 / 2 / 3 / 3.6 m. I measured the ruler at **205.4 px per metre** across the 3 m → 2 m → 1 m intervals, and the mech's silhouette fills it to the 3.6 m line. The claim is internally supported.
2. **The pilot shown is the cyborg**, and she matches her own sheet — long blonde hair, mechanical forearms from the elbow, blue-gray tank and cargo trousers, flat lug boots. **This is the first cross-character scale reference the project has**, and it partially answers the missing proportion chart (§4.4) for one pair.
3. **The cockpit is real, not gestured at.** A side orthographic section shows the **rear-upper-torso hatch hinged upward and rearward**, with a ghosted **seated pilot volume** upright in the upper torso, facing the mech's front. A pilot demonstrably fits. Two consequences worth recording now: **embarking is a climb-from-behind-and-above motion**, which is an authored animation with a real cost; and the cockpit consumes much of the upper-torso volume, which constrains where internal hardpoints could ever go.
4. **A top-down orthographic silhouette exists** — the first plan view of the mech. Broad squared pauldrons that overhang, a narrower leg mass, deepest at the shoulders.
5. **The sheet labels itself provisional throughout** — the title, the `MECH 2×2 — PROVISIONAL` bracket, a note reading `PROVISIONAL DIMENSIONS, SUBJECT TO REVISION`, and a `VALIDATE: doors · cover · pathfinding · camera` strip. It does not overclaim, and that is to its credit.

#### What measurement reveals — `RISK`, and it must be resolved before the grid is written

I measured the sheet against its own grid rather than trusting its labels. The drawn grid cell is **83.3 px**, from four consecutive clean gridlines (939.5, 1023.0, 1106.5, 1190.5, 1272.5). Against that:

| Element | Measured | In drawn squares |
|---|---|---|
| `MECH 2×2` bracket | 334 px | **4.01** |
| Top-down mech body, at the pauldrons | 319 px | **3.83** |
| `PILOT 1×1` box | 116 px | **1.39** |
| Mech width, front elevation | ≈ 492 px @ 205.4 px/m | **≈ 2.4 m** |

**The legend says `GRID = 1 m × 1 m (TACTICAL)`. Under that reading the sheet contradicts itself:** the "2×2" bracket would span 4 m, the mech body 3.8 m — against a front elevation that measures ≈2.4 m wide. A 1.4 m disagreement between two views of the same object.

At a 0.5 m drawn square the sheet reconciles arithmetically. **Revision 6: do not adopt that as the tile size, and my revision-5 recommendation to do so is retracted.**

`CANON OBSERVED` — **ruling by Mina:** *do not adopt 0.5 m as the tactical tile size merely because it reconciles the generated study's drawn grid. The image is construction evidence, not authoritative metrology.*

**She is right, and the correction is worth stating plainly.** I measured a generated drawing carefully and then let the arithmetic drive a design number. Careful measurement of an unreliable instrument is still an unreliable measurement — a generated grid is decoration that happens to be regular, not a survey. The measurement remains useful for exactly one thing: it proves the sheet's own labels and legend disagree, so **neither should be read as a dimension.**

#### The four concepts, kept separate — `CANON OBSERVED`

The confusion existed because four different things were being called "the grid".

| # | Concept | Lives in | Unit |
|---|---|---|---|
| 1 | **Logical kernel cell** | `Game.Tactics` | Integer coordinates. **No metres, ever.** |
| 2 | **Entity footprint** | Content data | Logical cells — human 1×1, mech 2×2 (candidate) |
| 3 | **`cellSizeMeters`** | Unity presentation | Metres. **Configurable.** A visual scale, not a rule |
| 4 | **Decorative / subdivided grid lines in visual studies** | Reference art | Nothing. Not a measurement instrument |

**The kernel must use integer cell coordinates and remain independent of metres.** This is the ruling, and it is also the cleanest possible expression of the engine-free boundary in §9.1: a simulation that cannot represent a metre cannot be corrupted by a visual-scale decision. Footprint is a unit property in logical cells; `cellSizeMeters` is a Unity field that changes how large everything looks and **changes no combat rule**.

**What this does to the tile-size question.** It stops being a kernel question. There is nothing to rule before the grid is written, because the grid has no metres in it. What remains is a *visual-scale* comparison — 1.0 m, 1.25 m and 1.5 m rendered against identical combat rules — and that is a spike experiment, not a decision to be made in advance. **Q32 is reframed accordingly**, and it stays a sub-decision of Q4.

#### What the study does *not* do

**It validates nothing tactical, and it does not claim to.** A scale study can support the *rationale* for testing a broad 2×2 silhouette — and this one does, because the mech is genuinely broad rather than merely tall, which was the open question at revision 3. It cannot show that 2×2 survives doors, narrow-map pathfinding, cover contribution, forced movement, camera framing, encounter-space cost or remote-control readability. Those are the seven validations in §7.6 and every one of them needs the kernel running. **The hypothesis remains untested.**

#### Production note

**`MARKED FOR REMOVAL`** — the sheet carries a stray `FOR SCIENTIFIC EDUCATIONAL USE ONLY` label in its top-right corner. **Confirmed by direct magnified inspection; it is a generation artifact and is not project text.** It must be removed before the sheet is used as a production reference. Record the removal as a **new revision** (`-v2`) rather than editing in place, and log the change with both hashes in the art manifest.

**Two further items on this sheet should be corrected in the same revision**, so the reference stops carrying misleading numbers: the `GRID = 1 m × 1 m (TACTICAL)` legend, and the `MECH 2×2` / `PILOT 1×1` brackets, which do not match the drawn squares. Per the four-concept separation above, the corrected sheet should either **state footprints in logical cells with no metric claim at all**, or drop the background grid entirely and keep the height ruler — which is the part that was sound.

### 4.3 Character source package completeness### 4.3 Character source package completeness

`CODEX-BRIEF` §13.1 specifies thirteen items per principal character. Current state:

| Item | Human | Cyborg | Synthetic | Leader | Operator | Mech |
|---|---|---|---|---|---|---|
| Orthographic front/profile/back | ✅ | ✅ | ✅ ×2 | ✅ (armored only) | ✅ | ✅ |
| Approved hero image | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ |
| Shared proportion chart | ❌ | ❌ | ❌ | ❌ | ❌ | ❌ |
| Face construction sheet | ❌ | ❌ | ❌ | ❌ | ❌ | n/a |
| Hair mass sheet | ❌ | ❌ | ❌ | ❌ | ❌ | n/a |
| Costume topology map + rig strategy | ❌ | ❌ | ❌ | ❌ | ❌ | ❌ |
| Palette / material IDs under neutral light | ❌ | ❌ | ❌ | ❌ | ❌ | ❌ |
| Material board | ❌ | ❌ | ❌ | ❌ | ❌ | ❌ |
| Hand/foot + mechanical joint reference | ❌ | partial | ❌ | ❌ | ❌ | partial |
| Gameplay-camera turnaround (≈45°) | ❌ | ❌ | ❌ | ❌ | ❌ | ❌ |
| Silhouette plate at 128 px / 64 px | ❌ | ❌ | ❌ | ❌ | ❌ | ❌ |
| Motion archetype sheet | ❌ | ❌ | ❌ | ❌ | ❌ | ❌ |
| Provenance record | ❌ | ❌ | ❌ | ❌ | ❌ | ❌ |

**Roughly 2 of 13 items per character.** That is not a criticism — orthographics are the correct first item and they are done. It is a warning against reading "the sheets are in" as "art is ready".

The two cheapest missing items are also the two that most reduce risk: the **shared proportion chart** and the **silhouette plates**. Neither requires new illustration.

### 4.4 The proportion problem

`OPEN QUESTION` / `RISK`: the six sheets were produced at three different canvas sizes — 1536×1024, 1438×1094, 1374×1145 — each with its own horizontal guide spacing. **There is therefore no established height relationship between any two characters.** Nothing in the images allows me to say whether the cyborg is taller than the synthetic, or how the mech scales against any of them.

`CODEX-BRIEF` §13.1 requires "height and proportion chart shared across the whole cast". It does not exist. In a game where all three protagonists stand on the same grid, in the same frame, in the same cinematics, this is upstream of the rig, the camera, the tile size, and every cover-height rule.

---

## 5. Contradictions and Ambiguities

### C1 — Operator's arms — **RETRACTED. There is no contradiction. My revision-1 finding was wrong.** `CANON OBSERVED`

Revision 1 of this document reported that `Characters/…09_54_53 AM (5).png` showed both arms sleeved and gloved, and raised it as a blocking conflict against two canon documents and both team images. **That was an error on my part.**

I re-inspected the sheet at magnification, view by view, rather than reading the full sheet at reduced scale. **All five sources agree:**

| Source | Right arm | Left arm |
|---|---|---|
| `STORY-CANON-001.md` §2.4 | bare below shoulder armor, ungloved | sleeved / gloved |
| `CODEX-BRIEF` §3.1 | bare below shoulder armor | sleeved / gloved |
| `Art/Team 1-canon.png` | bare, ungloved | sleeved, gloved |
| `Art/Team 2- canon.png` | bare, ungloved (holographic) | sleeved, gloved |
| `Characters/…09_54_53 AM (5).png` | **bare to the hand, visible skin tone and nail colour, no sleeve, glove, bracer or wrist covering** | **white sleeve, purple bicep and forearm bands, purple wrist cuff, white glove** |

The left-facing profile is also clean: one arm drawn, the near camera-facing left sleeved arm, and **the far right arm does not show through the torso.**

**Settled visual canon** per `MINA-CONSULTATION-ADDENDUM-001.md` §1. **Removed from the blocker count.** No decision is required from Juan.

**Why I got it wrong, because the cause matters more than the error.** I read a low-contrast detail — a pale bare arm against a pale grey background — from the full sheet at reduced scale, without magnifying it. Revision 1 of this same document scolded the previous consultation for a structurally identical mistake and closed §5 C6 with the rule *"when an image does not show something, record that it does not show it."* I then broke my own rule in the opposite direction: I recorded a positive visual claim I had not actually resolved at sufficient magnification.

`RECOMMENDATION`, and this one is procedural rather than about the game: **no visual fact enters a document unless it was read at magnification on the region in question.** Costume boundaries, glove and sleeve terminations, limb-material transitions and digit counts are exactly the class of detail that reduced-scale viewing gets wrong, and they are exactly the class of detail that model sheets exist to specify. Add it to the asset-validation checklist in §11 — a reviewer who approves a sheet from a thumbnail has not reviewed it.

#### Confirmed against the dedicated reference sheet — revision 5, `visual construction evidence`

`operator-arm-asymmetry-reference.png` has now been inspected directly (§2). It is purpose-built for exactly this question and it settles it:

- **Three labelled views** — `FRONT`, `LEFT SIDE`, `BACK` — plus **two callout insets labelled `RIGHT — BARE` and `LEFT — SLEEVED`.**
- **`RIGHT — BARE` inset:** purple shoulder cap, then **bare skin the entire length** — deltoid, bicep, forearm, and a bare hand with articulated fingers. No sleeve, no glove, no bracer, no wrist covering. Exactly as ruled.
- **`LEFT — SLEEVED` inset:** purple shoulder cap → **a short section of bare upper arm** → a purple band finishing the sleeve's top edge → white sleeve → a purple forearm band → a purple wrist cuff → a white glove.
- **`LEFT SIDE` view, magnified:** one arm drawn, the near camera-facing left sleeved arm. **The far right arm does not appear through the torso.** The defect the addendum warns against is absent.
- **The shoulder caps are identical on both sides.** Only what happens below them differs — worth recording, because a modeller could otherwise assume the cap differs too.

**This closes the specification gap I recorded at revision 2.** I had flagged that the sleeve's proximal termination was inconsistent between views. It is not a defect: **the sleeve deliberately begins below the shoulder cap, leaving a bare section of upper arm, and its top edge is finished with a purple band.** The reference makes that construction explicit, so the modeller does not have to choose. No open item remains on Operator's arms.

**Both the correction and this confirmation rest on direct magnified inspection** — the primary turnaround at revision 2, and this dedicated reference now.

### C2 — `Integrity` is both an attribute and the damage pool `RISK`

`CODEX-BRIEF` §5 lists Integrity in the primary stat family as "health/structural durability". §10 uses "Integrity/Health" as the damage pool. One name, two objects, in one document.

Unchanged since the previous consultation and still unresolved. It blocks the schema, the UI, the balance model and ordinary conversation. **Decision owner: Mina.** `RECOMMENDATION`: attribute becomes `Frame`; the pool keeps `Integrity`, because the pool is the word players will see.

### C3 — Con Girl's scaling attribute: ruled in conversation, not written into the documents `RISK`

`CODEX-BRIEF` §7.2 is titled "Con Girl — Charisma control and manipulation"; `STORY-CANON-001.md` §3.1 says "Charisma-driven". `CODEX-BRIEF` §5's stat family is Strength / Agility / Intellect / Resolve / Integrity. There is no Charisma.

Juan has ruled on this: **Charisma is a character quality and a combat fantasy, not an attribute; Con Girl scales through Agility, like Dispatch and Gunslinger; Charisma is not to be added to the stat sheet.** A character-specific `Leverage` resource may eventually exist but is explicitly *not* canon.

**That ruling is not written into any file in `DD90s`.** It exists only in conversation. `RECOMMENDATION`: write it into `CODEX-BRIEF` §5 and §7.2 and `STORY-CANON-001.md` §3.1 before any content authoring begins. I have not done so myself — editing canon documents was not in scope for this consultation and I will not redefine canon silently.

**Reclassified in revision 3: this is an editing task, not an architecture blocker.** The ruling is settled — all three of the white-haired human's Specialties scale from Agility, and "Charisma" names her fantasy and behaviour rather than an attribute. The kernel can be written today against that answer; validator rule §10.3 #7 already enforces it mechanically. What remains is transcription, and the risk it carries is drift — anyone reading `DD90s` today still finds "Charisma-driven" and would implement a stat that does not exist. **Real work with a real deadline, and not a decision anyone is waiting on.** See §21.2.

### C4 — Deterministic damage versus surfaced hit uncertainty `RISK`

`PREPRODUCTION` §4 and `CODEX-BRIEF` §4 both say "deterministic base damage with clearly surfaced hit or mitigation uncertainty", while also requiring "seeded, replayable simulation". Is there a to-hit roll?

This determines the forecast UI, the AI's evaluation function, the balance model, and whether the PRNG is load-bearing or decorative. It must be settled **before the kernel is written**, not after. **Decision owner: Mina.** `RECOMMENDATION` in §7.4.

### C5 — Prologue-first versus scenario-first: not a contradiction, but written as one `RECOMMENDATION`

`PREPRODUCTION` §13 makes the prologue "the first major playable production target". `CODEX-BRIEF` §20 makes one small combat scenario the immediate objective and warns against starting big.

These are two different firsts and both documents are right: the scenario is the **first internal engineering proof**, the prologue is the **first major narrative production target**. `RECOMMENDATION`: state that distinction in both documents so it stops reading as a conflict, and sequence the scenario first — the prologue additionally requires the leader and Operator as full characters, a costume state change that does not yet exist (§5, G2), and a working Limit Break system used twice, one of which must fail dramatically.

### C6 — Four visual findings in the previous consultation are obsolete `CANON OBSERVED`

The prior consultation at `Desktop\Secret-game\docs\consult\CONSULT-CLAUDE-001.md` §3.1 recorded findings C1, C2, C3 and C6 from a single hero image — the one now named `Art/Team Concept 1st draft.png`. Current status:

| Prior finding | Status now |
|---|---|
| The synthetic's red jacket is canon in text but absent from art | **Wrong.** Present in `Team 2- canon.png` and in her own sheet. |
| The cyborg's arms are asymmetric | **Wrong.** Bilateral and symmetric, per her sheet. In the draft image only one arm was visible; absence of a limb was read as evidence about it. |
| Her costume is "production-hostile trim" | **Wrong on the letter.** The prohibition names fur, feathers, fuzzy and particulate trim. The costume has none. The cost analysis was right; the verdict was not. |
| The prologue contradicts smallest-first | **Reclassified** — see C5. |

`RECOMMENDATION`: the useful lesson is procedural. **When an image does not show something, record that it does not show it. Do not record an absence as a fact.** Two of the four errors came from that single mistake.

### Gaps

**G1 — Mech scale and tile occupancy: a provisional hypothesis now exists; validation does not. `RISK`** *(updated in revision 2)*
The mech sheet has no pilot reference, no cockpit and no hatch. `MINA-CONSULTATION-ADDENDUM-001.md` §2 now proposes ≈3.6 m, ≈2.05× the pilot, a 2×2 footprint and a rear-upper-torso hatch — **explicitly provisional, explicitly not canon.** See §4.2 for why the ratio and the footprint are two independent claims.

What it still determines: whether Remote Arsenal puts a 1×1 and a 2×2 body on a compact grid simultaneously; whether Bulwark body-blocking covers one lane or three; minimum corridor and doorway width on every map; what "adjacent" means for the synthetic's rescue condition; whether knockback collision uses one mass class or two; and how the camera frames a unit twice a protagonist's height. **It blocks the grid, and the grid blocks everything.**

**Revision 5:** the study is now inspected and measured (§4.2). It supports the rationale and exposes a grid-legend inconsistency; it validates nothing tactical.

**It remains a blocker.** A hypothesis is not a validation, and the addendum agrees — it becomes binding only if the spike passes seven named tests (§7.6). **If 2×2 fails, the same broad mech is not compressed into 1×1 to make the grid convenient**; the conflict between visual scale and tactical footprint is escalated as a decision for Juan and Mina.

**G2 — The leader's unarmored state does not exist. `RISK`**
`STORY-CANON-001.md` §2.1 makes the armor "removable external"; `PREPRODUCTION` §13 requires the prologue to show a costume state change. Only the armored state has a sheet. This is a second silhouette, a second material set, and a transition — and it is prologue-blocking.

**G3 — The two-survivor chapter still has no combat design. `RISK`**
`PREPRODUCTION` §6.2 and §13 and `CODEX-BRIEF` §16 Gate 5 all require a main-timeline chapter played by the cyborg and synthetic **alone**. Every combat proposal in all three documents assumes three units. With two, nobody creates `Isolated`, the synthetic's conditional windows have exactly one possible target, and the three-character handoff — the thesis — is structurally impossible. This needs a deliberate answer, not difficulty tuning.

**G4 — Enemy design does not exist.** With three fixed protagonists, tactical variance must come from the opposition and the map. Enemies appear once across ~92 KB of documentation, as an open item (`CODEX-BRIEF` §19).

**G5 — "No leader" versus the player. `RECOMMENDATION`**
The fiction insists nobody commands; the interface makes the player an omniscient commander. The documents never address this. It is an opportunity rather than a bug: make initiative exchange, ceded turns and handoffs the literal mechanical expression of distributed authority. State it as intent so the UI and the ability set can serve it.

**G6 — No named final approver per asset class. `RISK`**
Shared creative direction is fine for direction and a deadlock risk at approval gates, which is where a strict anti-slop pipeline generates the most friction. `RECOMMENDATION`: one named approver per asset class, with Mina holding technical veto.

### A4 — The `Inspo/` folder is third-party copyrighted material `RISK`

All three files are published *Danger Girl* comic art. Using them privately as tonal reference is ordinary practice. Two specific risks:

1. **Derivation.** The white-haired human's design language — glamour silhouette, strap rigs, heeled boots, comic-book confidence — sits close to that property's register. Close enough that a generative pass conditioned on this folder could produce something legally and reputationally uncomfortable. `RECOMMENDATION`: never place `Inspo/` images in a generation context, a fine-tune, or an image-to-image chain that touches a canonical character.
2. **Leakage.** If this folder is ever committed to the GitHub repository it becomes a public redistribution of copyrighted art. `RECOMMENDATION`: add `Inspo/` to `.gitignore` before the first push, whenever that happens.

### Duplicate and naming findings

- `Characters/…05_51_52 AM.png` and `Characters/…06_38_03 AM.png` are **byte-identical** (SHA-256 `90f11662…`). One should be deleted. **Decision owner: Juan.**
- All eight character sheets carry generator-default filenames. They are unsortable, unversionable, and say nothing about their subject or status. `RECOMMENDATION`: rename to `SHEET-<subject>-<state>-<revision>.png` — for example `SHEET-cyborg-field-001.png`, `SHEET-synthetic-base-001.png`, `SHEET-synthetic-jacket-001.png` — and record each file's SHA-256 in an art manifest. Without a manifest, the "1st draft" mistake in §5 C6 will recur.
- `Art/Team 2- canon.png` contains a stray space before the hyphen. Trivial, but it will break a naive script eventually.

---

## 6. Highest-Risk Assumptions

Ranked by cost of learning late, not by probability.

**H1 — That nine Specialties plus three Personal trees are producible at this quality bar.**
The binding constraint on this project is authored character action, not code. Every sheet added since the last consultation *increased* this cost: the human's coat, garter rig and heeled boots; the cyborg's hair mass and mechanical hands; the synthetic's shader-driven chassis under a real cloth jacket; a full mech with its own stance set. Estimate in §11.6. **Cheapest validation: the Action Vocabulary Census, two days, no art.**

**H2 — That the forecast can be made legible.**
`CODEX-BRIEF` §9.3 makes conditional support the game's signature. If the player cannot see, *before committing*, that a move will create `SURROUNDED 3` and enable the Pulse, the entire conditional-support thesis becomes a lottery and the player learns to ignore it. **Cheapest validation: a text-mode forecast harness, one week, zero art.** This is the single highest-value experiment available and it does not require Unity.

**H3 — That three units generate enough tactical variety.**
In XCOM and FFT variety comes from squad composition. Here it must come entirely from the opposition and the map — and enemy design does not exist (G4). **Cheapest validation: build the representative encounter in §16 with three enemy families and see whether five replays feel different.**

**H4 — That stepped anime cadence, 60 fps responsiveness and deterministic simulation coexist.**
This is the engine-binding assumption. It is the one thing that genuinely requires Unity to answer. **Cheapest validation: spike day 6–8, §15.**

**H5 — That a generative-first art pipeline can hold identity across a production.**
Every character sheet in the folder is generative output. That is a legitimate use — a static, human-approved reference sheet is exactly the permitted case. But the sheets currently have no provenance record, no approval marker, and no revision number, and `CODEX-BRIEF` §2.1 explicitly warns that generated images do not become canon by looking impressive once. The risk is not this generation; it is the *next* one, and the temptation to generate combat frames instead of animating them. **Cheapest validation: run the §9 golden-render gates against the existing sheets before any modelling begins — they will fail, and the failures are the specification.**

---

## 7. Minimal Combat Thesis

The smallest ruleset that can falsify the central assumption — *that three permanent protagonists produce enough tactical depth*.

### 7.1 What must be in v0

`RECOMMENDATION`, all of it.

- **Grid:** square, 8-connected movement, discrete integer elevation. Compact — 12×12 to 16×16.
- **Tile scale:** one tile = one human-sized combatant. **Mech occupancy pending G1.**
- **LOS:** tile-centre to tile-centre with corner rules; elevation grants LOS over one intervening level.
- **Cover:** binary half/full, derived from the tile edge crossed, not from a separate object list.
- **Initiative:** a tick clock. Each unit has `nextAt`; the lowest acts. Acting adds `speedCost`. This makes haste and delay first-class and expressible as integers, and it makes the turn-order rail forecastable — which the UI requires.
- **AP:** 2 per activation. Move / primary / utility / guard / ability.
- **Reactions:** exactly three in v0 — Overwatch, Intercept (cyborg), Riposte. More than three and you cannot reason about the loop rules.
- **Forced movement:** `Displace` with an explicit mass rule and a collision rule. Collision depth 0 only — a displaced unit that hits an obstacle stops and both take a fixed impact; it does not chain.
- **Conditions:** two separate categories. *Derived* conditions (`Surrounded`, `Flanked`, `Isolated`, `Elevated`) are pure predicates recomputed from board state, never stored. *Applied* statuses (`Hasted`, `Shocked`, `Staggered`) have durations and live on units. Conflating these is the most common source of desync between forecast and result.
- **Damage:** deterministic. See 7.4.
- **Objectives:** at least one non-elimination objective in the test encounter.
- **Synchronization:** granted only when a tag created by one character is consumed by another. Never from self-created tags. Never from ordinary damage.

### 7.2 What must be excluded from v0

Talent trees. Gear. Equipment slots. Levelling. Multiple maps. Enemy variety beyond three families. The mech's stance system beyond two stances. Any animation. Any UI beyond text. Ink integration. Save/load beyond the replay log.

### 7.3 Anti-loop and anti-stall rules

`RECOMMENDATION`. Every one of these exists because its absence produces a specific exploit.

1. A unit may benefit from **at most one initiative-altering effect per activation.** Prevents haste ping-pong.
2. **Each reaction fires at most once per unit per round**, tracked on the reacting unit.
3. A reaction may **never** trigger another reaction. Depth is 1, always.
4. `Displace` **never chains.** Depth 0.
5. Synchronization requires `requiresCrossCharacter: true`. A character cannot generate Sync from her own setup.
6. An ability that refunds AP may not refund more than it cost, and refunds are capped at one per activation.
7. **Stall clock:** if a full round passes with no damage, no objective progress and no tag consumed, escalate — enemy reinforcement or objective decay. Prevents the overwatch-standoff degenerate state that afflicts grid tactics games.
8. Every effect resolution is **bounded**: a fixed maximum iteration count, asserted in tests.

### 7.4 Deterministic resolution — the recommendation for C4

`RECOMMENDATION`: **no to-hit roll.** Damage, displacement and status application resolve deterministically from board state. Uncertainty moves entirely into **hidden enemy intent** — every enemy publishes an intent icon and its threatened tiles before its activation, but not its target priority resolution, and reinforcement timing is unknown.

Why this and not dice:
- It makes the forecast *exactly* correct, which is the pillar in `CODEX-BRIEF` §2.4. A forecast that says "83%" is not legibility, it is a slot machine with a tooltip.
- It makes the AI's evaluation function and the player's forecast the same computation, which is the only way to keep them honest.
- It makes replays trivially reproducible.
- It preserves tension, because the player still cannot see everything.

The seeded PRNG stays in the architecture — for enemy AI tie-breaking and reinforcement selection — but it is **not** load-bearing for damage. `REQUIRES PROTOTYPE`: whether removing to-hit variance makes combat feel solvable rather than tense. This is the main thing the §16 encounter must answer.

### 7.5 The falsification criterion

The thesis is falsified if, across five plays of the §16 encounter by someone who did not design it:

- the player cannot state, before committing a move, what conditions it will create; **or**
- the same opening line is optimal in all five plays; **or**
- the three-character handoff never occurs without being explicitly instructed; **or**
- the player reports that only one character's turn is interesting.
### 7.6 Mech occupancy — the seven validations `REQUIRES PROTOTYPE`

Added in revision 2. Each is a pass/fail test the spike must run before the 2×2 hypothesis becomes canon. All assume the provisional ≈3.6 m / 2×2 values from §4.2.

| # | Validation | Pass criterion | Fail signal |
|---|---|---|---|
| 1 | **Doors and traversal** | The mech routes through every doorway on a representative map, or the map declares mech-impassable thresholds *deliberately* | Mech is stranded by ordinary architecture, or every door must be widened, which flattens level design |
| 2 | **Narrow-map pathfinding** | 2×2 pathing resolves on a 14×14 map with pillars and ramps, with no oscillation and no illegal diagonal squeeze | Mech cannot reach the objective on ≥ 1 of 4 map variants |
| 3 | **Cover interaction** | A 2×2 body's cover contribution is well-defined per edge, and it can itself take cover | Cover becomes ambiguous, or the mech is permanently exposed by size |
| 4 | **Occupancy and forced movement** | `Displace` on a 2×2 unit has defined collision and legality; mass class distinguishes it from 1×1 | Knockback on the mech is undefined, or trivially illegal everywhere |
| 5 | **Camera framing** | Mech and pilot both readable in one frame at the tactical camera; commit zoom frames the mech without clipping | Camera must pull back far enough that protagonists drop below 96 px (§12) |
| 6 | **Encounter-space cost** | On a 14×14 map, the mech consumes ≤ ~4 % of tiles and does not dominate lane count | The map must grow, which raises art cost on every encounter |
| 7 | **Remote-control readability** | With pilot and mech separated, the player can tell at a glance which unit acts next, which is under directive, and what each threatens | Players lose track of which body they are commanding — the failure that kills Remote Arsenal |

`RECOMMENDATION`: run tests 1, 2 and 4 in the **headless kernel in week 1**, where a footprint change is a constant. Tests 5 and 7 need the engine. Test 3 and 6 are design judgements informed by 1–4. Do not defer 1, 2 and 4 to Unity — a footprint discovered to be wrong in week 2 costs the grid.

**Revision 5 — what the inspected study changes here, and what it does not.** The study strengthens the *rationale* for testing 2×2: the mech is genuinely broad in plan view, not merely tall, which was the open doubt at revision 3. **It changes none of the seven pass criteria and validates none of them.** A drawing cannot show that a 2×2 body routes through a doorway, contributes cover per edge, accepts forced movement, frames with its pilot, or stays legible under remote control.

**Revision 6 — tile size does not gate these tests.** Per Mina's ruling (§4.2), the kernel works in **integer logical cells with no metres**, so tests 1, 2, 3, 4 and 6 run on footprints alone and are unaffected by visual scale. Only tests **5 (camera framing)** and the new **animation-clearance** check are metre-sensitive, and those belong to Unity. Run them at **1.0 m, 1.25 m and 1.5 m `cellSizeMeters`** against identical combat rules — the comparison is the experiment, and combat behaviour must be provably unchanged across all three. If it is not, the kernel has leaked metres and §9.1 has been violated.

**Validate independently**, per the ruling: traversal · doors · cover · forced movement · camera · **animation clearance** · encounter-space cost. Animation clearance is new in revision 6 and is the one that is easy to forget: a 2×2 mech's attack and stance-change arcs must not intersect adjacent occupied cells at the chosen `cellSizeMeters`, and that is a metre-space check even though occupancy is not.


---

## 8. Character and Team-System Evaluation

### 8.1 The three combat fantasies, as the sheets and documents describe them

`CANON OBSERVED` for the verbs and axes; `RECOMMENDATION` for the mechanical readings.

**White-haired human — Agility — *Exploit / Redirect*.** Her sheet is the loudest in the cast and it should be read as design intent: she is the character the enemy looks at. Her three Specialties are three answers to "what do I do with attention" — Dispatch converts it into an opening she walks through, Con Girl converts it into an enemy mistake, Gunslinger converts it into a line of fire. **She is the only character who creates `Isolated`.** That is her identity in the tag grammar and it must not be grantable by gear.

`RECOMMENDATION` on Con Girl and the C3 ruling: with Agility as the scaling axis, Con Girl's numbers should scale on Agility but its *conditions* should scale on observation — abilities that are stronger against a target she has already interacted with. That gives Juan's "leverage" fantasy a mechanical home without introducing a resource or a stat, and it stays inside the ruling.

**Cyborg — Strength — *Break / Anchor*.** She is the only protagonist with two bodies. `RECOMMENDATION`: that, not damage, is her design centre. **She is the only character who creates `Guarded`-for-others.** Her sheet's flat lug-soled boots and heavy belt rig are consistent with a character who plants and holds — good, and worth preserving against any later restyling.

**Synthetic — Intellect — *Rewrite / Conduct*.** `CODEX-BRIEF` §9 is emphatic that she is not a healer, and the recovery model in §10 depends on it. **She is the only character who creates `Hacked` and `Conductive`.** Her sheet is the cheapest to produce in the cast — the body is a shader and the jacket is her only cloth — which makes her the correct first character for the technical spike. See §15.

### 8.2 Keeping three Specialties from becoming three characters

`RECOMMENDATION`: each character has **one tag she creates** and **one resource-shaped mechanic shared across all three of her Specialties**. Dispatch, Con Girl and Gunslinger are three ways to set up and cash the same observation state; Bulwark, Remote Arsenal and Redline are three ways to spend the same commitment/position tension; Heartless, Ghost Surgery and Spell Slinger are three ways to spend the same access/conduction state.

If a Specialty does not touch its character's shared mechanic, it is a fourth character wearing her face.

### 8.3 The "no leader" problem, solved mechanically

`RECOMMENDATION`, addressing G5. Make distributed authority the literal mechanic:

- A character may **cede** her activation to another, moving her `nextAt` behind theirs. This generates Sync.
- A handoff is a **tag consumption across characters**, not a scripted pair.
- No character has a command ability, an aura, or a leadership buff. There is no rally.
- The Limit Break requires all three to have contributed tags within a window — it cannot be initiated by one.

This makes the fiction and the interface agree instead of contradicting each other, at no extra content cost.

### 8.4 Likely balance failures, predicted

`RISK`, each with the countermeasure that belongs in v0:

1. **Con Girl trivialises setup.** If she can reliably create `Surrounded` on demand, the synthetic's conditional support stops being conditional. *Countermeasure:* the enabling condition must require enemy *choices*, not just enemy positions.
2. **The mech becomes a second full character for free.** *Countermeasure:* command bandwidth — see §9.6.
3. **Overwatch turtling.** *Countermeasure:* the stall clock, plus every enemy family carrying `Piercing`, `GuardBreak` or `Area`.
4. **Rescue loops.** If rescue restores enough, the optimal line is to let a character fall deliberately. *Countermeasure:* rescue restores minimally, the Fault persists, and the second rescue in a mission costs more.
5. **One dominant opening.** The falsification criterion in §7.5 is designed to catch this.
### 8.5 Equipment structure — settled direction, and what it costs `CANON OBSERVED` + `RECOMMENDATION`

Added in revision 2. Source: `MINA-CONSULTATION-ADDENDUM-001.md` §3.

**Settled — revision 3 supersedes the revision-2 wording of this paragraph.** The structure is **1 weapon + 4 gear**, and the weapon sits outside the set system:

| Element | Ruling |
|---|---|
| Gear slots | **Exactly four** per protagonist |
| Weapon slot | **One, separate**, character-specific weapon class; not interchangeable between protagonists |
| Set participation | **Only the four gear pieces** count toward set thresholds |
| The weapon | **Never counts toward a set threshold.** Not 2-of-4, not 4-of-4, not ever |
| 2-piece bonus | Two different 2-piece bonuses may be combined |
| 4-piece bonus | A complete four-piece set provides its full-set bonus |
| `mech-hardpoint` | If retained, it **occupies one of the cyborg's four gear slots.** It is not a fifth gear slot and not a sixth equip position |
| Total equip positions | **5** — one weapon plus four gear. **Never 5 gear + a weapon.** |

Set bonuses expand builds without erasing identity and must not become random-affix loot soup. Gear may alter mechanics, stats, tags, targeting, presentation effects and build synergies within the content budget. **Cinematics always use the authored canonical loadout.**

**This resolves the `CODEX-BRIEF` §12 conflict, and it resolves it cleanly.** §12 proposes five categories — `signature-weapon`, `frame-component`, `utility-module`, `character-signature`, `mech-hardpoint`. Under the ruling they reconcile as **`signature-weapon` = the separate weapon slot**, with the remaining four as gear. Revision 2 of this document proposed reading `mech-hardpoint` as the cyborg's instance of `character-signature`; **that reading is now wrong**, because it would leave only three gear categories. `mech-hardpoint` is a **distinct fourth gear slot that the cyborg fills with a hardpoint** and the other two protagonists fill with something else.

`OPEN QUESTION`, reduced and no longer structural — **only the naming remains**, in Q26: what do the other two protagonists put in the slot the cyborg fills with `mech-hardpoint`? The count, the set arithmetic and the weapon's exclusion are all settled.

**Why excluding the weapon from set thresholds is the right call, mechanically.** It separates the two axes cleanly: the weapon carries *character identity* — it is the thing that cannot be traded away — and the four gear pieces carry *build expression*. If the weapon counted toward a set, every 4-piece set would silently constrain which weapon you could carry, and character-specific weapon classes would collapse back into a build tax. Keeping it out means a player can commit fully to a 4-piece set and still choose freely within her own weapon class. It also keeps the combinatorics honest: **4 gear positions, not 5**, is what makes the 2+2 pairing space enumerable (see below).

**This is a good structure and I would not change it.** Four slots with 2+2 or 4 is a real decision space — roughly the shape Destiny and Monster Hunter use — and it is small enough to author by hand and balance, which is exactly what "no loot soup" requires. Character-specific weapon classes are what stop gear from dissolving identity, and they do it structurally rather than by rule.

**Three consequences the addendum does not spell out, which belong in the plan:**

**1. The 2+2 combination is where balance breaks, not the 4-piece.** A 4-piece set is authored as a whole and can be tuned as a whole. **2+2 is a combinatorial surface**: across **four gear positions** (the weapon is excluded, which is what keeps this bounded), with *n* sets there are `n × (n−1) / 2` two-piece pairings, all of which must be non-degenerate. At six sets that is 15 combinations; at ten sets, 45. `RECOMMENDATION`: **cap the game at 6–8 named sets** and treat every 2-piece bonus as a *conditional* rather than a flat throughput increase — conditionals compose far more predictably than percentages, and a conditional that never fires is weak rather than broken.

**2. Identity preservation needs a validator rule, not just a principle.** §10.3 rule 5 already forbids gear granting another character's tag-creation verb. `RECOMMENDATION`: extend it — **a set bonus may not grant a tag-creation verb at all**, from any character. Sets amplify what a character already does; they never hand her a new verb. That single rule is what keeps a full 4-piece from erasing identity, and it is checkable in CI.

**3. Gear is an animation question before it is a stat question.** `RISK`: if equipped gear is visible, four slots × three protagonists × *n* sets is a mesh and material matrix, and every visible variant must pass the silhouette gate at 128 px (§11.2). This interacts directly with §11.6's animation estimate. **Whether gameplay shows equipped gear is an open question** — Q27 — and it is the single largest cost fork in the equipment system. My provisional recommendation is in that question; the short version is that weapons should be visible and armour should mostly not be.

`OPEN QUESTION`: the four gear slot names are deliberately **not invented here**. See Q26 — now a naming task only.


---

## 9. Recommended Combat Architecture

`RECOMMENDATION` throughout.

### 9.1 Assemblies and the prohibited dependency directions

```
Game.Core           primitives, tags, fixed-point math, seeded RNG, event log
Game.Tactics        grid, LOS, cover, elevation, initiative, AP, movement
Game.Abilities      definitions, targeting, conditions, costs, typed effects
Game.Forecast       ← the one that must not be skipped; see 9.3
Game.Progression    attributes, trees, gear
Game.Synchronization handoffs, assists, rescues, chains, the Limit resource
Game.AI             utility evaluation
Game.Narrative      Ink bridge, story facts, consequences
Game.Content        definitions and schemas
Game.Presentation   animation requests, cameras, VFX, audio, cinematics
Game.UI             view models
Game.Editor         validators, importers, preview scenes
Game.Tests          fixtures, property tests, golden images
```

**Hard rule:** `Game.Core`, `Game.Tactics`, `Game.Abilities`, `Game.Forecast`, `Game.Synchronization` and `Game.AI` compile with **no reference to `UnityEngine`.**

**Corollary, ruled by Mina in revision 6 and worth its own CI assertion: the kernel is dimensionless.** Positions are integer cell coordinates; footprints are counts of cells. **No metre, no `float` distance, no world-space unit appears anywhere in the simulation assemblies.** `cellSizeMeters` is a Unity presentation field and nothing in the sim may read it. This is checkable — grep the sim assemblies for `Meters`, `worldPos` and `Vector3` alongside the existing float-literal assertion — and it is what guarantees that comparing 1.0 m against 1.5 m in the spike changes appearance and nothing else. This is testable in CI with one assembly-reference assertion, and it is the single most valuable architectural constraint in the project — it is what lets Astra iterate on rules without touching the editor, and what makes ten thousand simulated encounters cheap.

**Prohibited directions:** simulation never references presentation; presentation never writes simulation state; UI never computes a rule; AI never uses information the player's forecast cannot also produce.

### 9.2 Determinism

- **No floats anywhere in the simulation.** Fixed-point or integers only. A float in a damage formula is a non-reproducible replay on a different CPU. Enforce with a CI grep and a schema validator rule.
- All collections iterate in a deterministic order. No `Dictionary` iteration without an explicit sort.
- One seeded PRNG instance, owned by `Game.Core`, drawn from only at explicitly logged points.
- Reproducibility contract: **`initialState + orderedCommandLog → identical finalState and identical eventStream`.** Assert it in CI on a corpus of recorded encounters. Every playtest becomes a regression test at zero authoring cost.

### 9.3 `Game.Forecast` — the recommendation I would defend hardest

The forecast must be produced by **the same resolver** that produces the result, run against a speculative copy of the state. Not a parallel implementation. Not a formula duplicated in the UI.

```
Forecast(state, command) -> ForecastResult {
    legality, cost,
    damageBefore/After, guardBefore/After,
    displacementPaths[],
    resultingTurnOrder[],
    conditionsOpened[], conditionsClosed[],   // the signature line
    reactionsProvoked[],
    syncDelta
}
```

`conditionsOpened` and `conditionsClosed` are the game's distinguishing UI element. They are what turns `CODEX-BRIEF` §9.3 from a hope into a mechanic.

CI must assert **forecast parity**: for a corpus of states and commands, applying the command produces exactly the forecast. Any divergence fails the build. Without this the pillar dies quietly over six months.

### 9.4 Effects — typed grammar versus bespoke C#

**Typed grammar** (data, no code): Damage, ApplyStatus, RemoveStatus, Displace, Move, ModifyInitiative, GrantGuard, SpendResource, EmitTag, ConsumeTag, Link, Conditional.

**Bespoke C#** — justified only where the mechanic is a *system*, not an effect: the mech's command and autonomy model; the Limit Break sequencer; boss subsystem degradation; the AI's utility evaluation. Four systems. If a fifth appears, that is a signal the grammar is wrong.

Anything a designer wants that is not in the grammar is a **kernel change** — escalate it, do not write a per-ability script. The failure mode this prevents is the one Mina named: a data-driven architecture that becomes an uninspectable abstraction.

### 9.5 Command validation

Every player and AI action is a `Command` — serializable, validated before application, appended to the log. Validation and application are separate functions; the UI calls validation to grey out illegal actions and never reimplements the rule.

### 9.6 The pilot/mech entity model — **settled** — and the command model — **still open**

Two different questions live here and revision 4 separates them, because one is now ruled and the other is not.

#### 9.6.1 Entity model — `CANON OBSERVED`, architecture ruling by Mina

**Pilot and mech retain separate, stable logical entity IDs in combat state, regardless of deployment mode.**

| Deployment state | Pilot | Mech |
|---|---|---|
| **Embarked / docked** | Remains in authoritative state. **Non-spatial, non-selectable, non-targetable, no independent initiative slot.** | Owns battlefield occupancy. The active selectable unit. |
| **Remote operation** | May become spatial and addressable. | Spatial and addressable. |

The ruling **does not** determine which Specialties permit which deployment mode. That stays a product decision.

**This is a stronger ruling than my revision-3 recommendation, and it is stronger in the right direction.** I proposed a two-entity model with a `docked` mode because it made the architecture indifferent to an unmade decision. Mina's ruling goes further: **entity IDs are stable and always present in authoritative state**, and the docked pilot is defined by four explicit negations rather than by absence. That distinction matters more than it looks:

- **A stable ID that is merely non-spatial is trivially serialisable.** Save data, replay logs and the event stream carry the pilot's ID in every state, so embarking and disembarking are *state transitions on an existing entity* rather than despawn/spawn events. Replay determinism (§9.2) survives a mode change for free.
- **Non-targetable is a state, not a missing object.** Targeting validation answers "why not" — the pilot is embarked — instead of failing to find an entity. That is the difference between a clean rejection and a null reference.
- **"No independent initiative slot" is precise where "one unit" would have been ambiguous.** The pilot keeps her ID and her state; she simply does not appear on the turn-order rail. The rail reads spatial entities, not all entities.

**Architectural consequences, `RECOMMENDATION`:**

1. Entity ID allocation happens once, at encounter setup, for both bodies. Never on deployment change.
2. `spatial`, `selectable`, `targetable` and `hasInitiativeSlot` are **per-entity state flags**, not inferred from deployment mode by scattered conditionals. Deployment mode sets them; every other system reads them.
3. The turn-order rail, targeting validation, LOS and the forecast all filter on those flags. **No system special-cases "is this a docked pilot".**
4. Occupancy is owned by whichever entity is spatial. In embarked state the mech's footprint is the only footprint — which keeps this ruling cleanly separable from Q4.

**Therefore Q31 does not block kernel architecture.** The unit model is determined; what remains is which Specialties permit which mode, and that is a product decision that changes no data structure. See §21.3.

#### 9.6.2 Command model — `REQUIRES PROTOTYPE`, unchanged

Distinct from the above and still open: shared AP, queued directives, or separate initiative with command bandwidth. `CODEX-BRIEF` §8.2 asks for all three to be tested.

`RECOMMENDATION`: **queued directives with command bandwidth.** In remote-operation state the mech acts autonomously on its queued directive and the pilot spends 1 AP to issue or change one. She does not get two full characters — she gets a second body whose behaviour she must *anticipate*, which is the tactically interesting version and the one that fits `Break / Anchor`.

Test all three in the headless kernel, where each is roughly a day's work, not in Unity where each is a week's. **The entity ruling above makes this cheaper to test**, because all three command models operate on the same stable two-entity representation — switching between them changes initiative and AP rules, not the object graph.

**Pass criterion:** across ten AI-vs-AI simulations, the queued-directive cyborg contributes between 30 % and 45 % of team actions. Below 30 % the mech is decoration; above 45 % she is two characters.

**Cross-reference:** `CLAUDE-QUESTIONS-001.md` Q8 (command model) and Q31 (deployment modes by Specialty) are now the two halves of what was previously one muddle. Q8 is the prototype question; Q31 is the product question. Neither blocks architecture.

### 9.7 Save data and replay

Two separate stores. **Definitions** are immutable and versioned. **Campaign state** is mutable, versioned, with an explicit migration path. The replay log is `{contentVersion, seed, initialState, commands[]}` — small, diffable, and the substrate for every regression test.

---

## 10. Content and Data Model

`RECOMMENDATION`. Enough to prove or disprove the model, not a content library.

### 10.1 The two rules that control the budget

1. **An ability costs animation. A talent must not.** A talent may change numbers, targeting shape, cost, tags, triggers, conditions and VFX *parameters*. If it needs a new clip, it is an ability and must be charged against the ability budget.
2. **Every ability maps to an existing motion archetype.** Each character has ~8 approved archetypes; an ability is `archetype + timing variant + VFX set + data row`.

**Where rule 1 protects the project:** it is the only mechanism that keeps twelve trees from becoming a thousand clips. It is also how 1990s TV anime was actually produced — a small library of authored actions, recombined.

**Where an exception is justified:** the **capstone** of each Specialty, and each character's **signature counter or Limit contribution**. Budget one new clip per Specialty capstone — nine across the game — and treat every other exception as a rejected request. `RECOMMENDATION`: make the exception explicit in the schema (`"newClipApproved": "<approval-id>"`) so that it cannot happen by accident.

### 10.2 Schema shape

JSON under `content/` is the source of truth; ScriptableObjects are generated mirrors, never hand-authored. Everything is `id` + `version`, kebab-case and namespaced (`syn.spellslinger.rescue-pulse`). Every file validates against a JSON Schema in CI.

An ability declares: owner, specialty, tags, cost, requirements (mode and conditions), targeting (kind, range, shape, LOS, valid targets, facing), ordered effects, boss degradation, emitted tags and Sync rule, reaction window, presentation request (motion archetype, VFX set, camera hint, duration, cut-in), AI intent hints, and permitted upgrades.

A talent is a **declarative patch** — `set` / `add` / `mul` / `append` / `replace` / `remove` against a path on a target ability, applied deterministically at loadout-snapshot time, never during battle. A talent that cannot be expressed as a patch is an ability.

Localization: every player-visible string is a key, never a literal, from the first ability authored. Retrofitting this is a week of tedium; doing it from the start is free.

### 10.3 Validator rules that must fail CI

1. An ability references an unapproved `motionArchetype`.
2. A talent introduces a motion archetype, or adds an effect requiring one, without an approval id.
3. A control or displacement ability lacks `bossDegradation`.
4. A condition lacks a `displayBadge`. *A condition the player cannot see is not a condition; it is a trap.*
5. Gear grants an ability or a tag-creation verb owned by another character.
6. **Any float literal in any numeric field.**
7. **An ability scales from an attribute that is not its owner's primary axis.** This encodes Juan's C3 ruling mechanically so it cannot erode.
8. An id collides, or changes without a version bump and migration entry.
9. `emits.sync` grants Sync without `requiresCrossCharacter`.
10. A character exceeds her ability budget.
11. A `Displace` omits `collisionRule` or uses depth > 0.
12. Visual gear references an unapproved silhouette variant.

### 10.4 Representative content for the prototype

Not a library. **Nine abilities total** — one signature per Specialty — plus three reactions, six conditions, eight statuses, three enemy families, one encounter. That is sufficient to prove or disprove the schema. If the model cannot express those nine cleanly, more content will not help.

**Gear, added in revision 2: two sets and one loose piece for the cyborg only.** Two 2-piece bonuses plus one 4-piece is enough to test the 2+2 combination rule, the identity-preservation validator, and whether set bonuses read as builds rather than percentages. **Do not author gear for all three protagonists in the prototype** — the schema either works on one character or it does not.

### 10.5 Gear, sets, and the cosmetic toggle — schema shape `RECOMMENDATION`

Added in revision 2, implementing §8.5 and §3.1.

**Gear and weapon are two different record types.** That separation is not cosmetic — it is what makes the "weapon never counts toward a set threshold" ruling structurally impossible to violate, rather than a rule someone has to remember.

```jsonc
// GEAR — four slots per protagonist; these are the only set participants
{ "id": "gear.cyb.siege-brace", "version": 1,
  "kind": "gear",
  "slot": "<gear-slot-enum-pending-Q26>",   // one of exactly 4
  "owner": "cyborg",
  "setId": "set.bulwark-line",              // null if not part of a set
  "grants": [ { "abilityId": "cyb.remote.siege-shot" } ],
  "patches": [ … ],
  "drawback": { … },
  "visible": true,                          // see Q27
  "silhouetteImpact": "approved:hardpoint-shoulder-L" }

// WEAPON — one slot, character-specific class, NO setId field at all
{ "id": "weapon.cyb.siege-mount", "version": 1,
  "kind": "weapon",
  "owner": "cyborg",
  "weaponClass": "cyb.heavy-mount",
  "grants": [ … ], "patches": [ … ], "visible": true }
```

**The weapon record has no `setId` field.** Not "null", not "ignored" — absent from the schema. A weapon therefore *cannot* be authored into a set even by mistake, and the set-threshold counter has nothing to read. This is the cheapest possible enforcement of Juan's ruling, and it costs one schema decision made now instead of a validator rule and a bug later.

**Slot count is a fixed enum of four**, filled once Juan names them (Q26). `mech-hardpoint` is one of the cyborg's four gear slots, not a fifth entry in the enum.

**Sets.**

```jsonc
{ "id": "set.bulwark-line", "owner": "cyborg",
  "bonuses": {
    "2": [ { "type": "Conditional", "if": {…}, "then": [ … ] } ],
    "4": [ … ]
  } }
```

Two-piece and four-piece bonuses are declared on the **set**, not smeared across the pieces. That makes 2+2 combinations enumerable, which is what allows them to be tested exhaustively rather than sampled.

**The cosmetic toggle.** It is **presentation state, not simulation state.** It belongs in the loadout/profile store, and `Game.Core` must never see it.

```jsonc
{ "id": "cosmetic.syn.jacket", "owner": "synthetic",
  "kind": "garmentVisibility",
  "affects": "presentation.only",
  "layer": "syn.garment.red-bomber",
  "cinematicOverride": "sceneCanonical",   // authored scenes win, always
  "defaultTeam2": "visible" }
```

**Validator rules added to §10.3** — these are the rules that make the settled directions enforceable rather than aspirational:

13. A weapon declares a `weaponClass` belonging to another protagonist, or is equippable by more than one protagonist.
14. **A set bonus grants a tag-creation verb.** Sets amplify; they never hand a character a new verb. (§8.5, consequence 2.)
15. A protagonist has more or fewer than **four gear slots**, or more or fewer than **one weapon slot**.
15a. **A record of `kind: "weapon"` carries a `setId`, or is counted by the set-threshold resolver.** Belt-and-braces alongside the schema separation above — the ruling is that the weapon never counts toward a set threshold, and this asserts it at runtime as well as at authoring time.
15b. **The gear slot enum contains more than four entries**, or `mech-hardpoint` is declared as a slot outside that enum. Guards against the `5 gear + weapon` misreading of `CODEX-BRIEF` §12.
16. A 2-piece bonus is a flat unconditional throughput increase rather than a conditional. (§8.5, consequence 1 — a guideline promoted to a rule because it is the thing that keeps 2+2 tractable.)
17. **Any cosmetic entry declares an effect outside `presentation.only`.** This is the rule that keeps the jacket toggle from ever becoming a stat.
18. A cinematic scene reads a cosmetic toggle or a gameplay loadout instead of its authored canonical loadout.


---

## 11. Technical-Art and Animation Pipeline

### 11.1 The identity law

`RECOMMENDATION`: **one canonical rigged asset per principal character.** Gameplay and in-engine cinematics derive from that same model and rig. LOD0 and LOD1 share mesh topology, UVs, material IDs and skeleton root hierarchy; they differ only by subdivision, hair bone count, face rig complexity, texture resolution and shadow-shape resolution. **They may never differ by design.**

### 11.2 The order of operations before any modelling

Do these four things first. None requires new illustration; all four are cheap; all four prevent expensive rework.

1. **Shared proportion chart.** Composite all six sheets onto one height reference. Resolves §4.4 and G1 simultaneously, and gives the §4.2 mech hypothesis its missing measurement.
2. **Silhouette plates at 128 px and 64 px** for each character, alpha only. This is where the human's strap rig and the cyborg's hair mass will either read or turn to mush, and it costs an afternoon.
3. **Palette and material-ID swatch file.** One file that both the 2D artists and the toon shader sample. Generated colour drifts; stable IDs are what the whole validation pipeline rests on.
4. **Costume topology map per character**, naming each garment's rig strategy. This is the document that prevents budget surprises — see 11.5.

### 11.3 Toon shading and outlines

- Flat ramp, hard shadow terminator, one controllable shadow-shape parameter per material.
- Rim light as a separate authored pass.
- **Character outlines: inverted hull.** Stable width, art-directable, no shimmer.
- **Environment outlines: screen-space depth/normal edge pass.** Never apply the screen-space pass to characters — it shimmers at gameplay distance and violates the no-uncontrolled-shimmer pillar.
- Colour lock: golden renders assert output within ΔE00 ≤ 3 of the swatch file.

### 11.4 Analog post stack

Independently toggleable Renderer Features, in this order: line-weight stabilisation (before any blur) → halation masked to emissive material IDs → luma/chroma separation → scanline/phosphor → palette-aware grain → gate weave (cinematics only) → optional tape dropout as punctuation.

**UI composites last, on a separate overlay camera, entirely outside the stack.** Text stays pixel-crisp. Every pass has a debug toggle and a golden-image test. Total post budget: **≤ 2.5 ms** (see §13).

### 11.5 Hair and clothing assignment, from the sheets

`RECOMMENDATION`. This table is the direct output of inspecting the new sheets, and it is where their cost lives.

| Element | Character | Strategy | Note |
|---|---|---|---|
| All primary silhouette masses | all | **Skinned geometry** | The silhouette is never simulated |
| Long layered blonde hair | Cyborg | 3–4 authored bone chains, ~4 bones each, spring solver with hard angular limits | Hardest hair in the cast. **Prototype first.** |
| Short tousled curls | Human | Fully skinned + 2 accent bones | Cheap |
| Enormous curled hair mass | Operator | Skinned primary volumes, 2 chains at the tips | Large but non-combat; she never fights |
| Bob | Synthetic | Skinned helmet shape, 3–5 tip bones | Cheapest |
| High ponytail | Leader | 1 chain, 5 bones | Cheap; prologue only |
| **Off-shoulder coat hanging from the arms** | Human | 2 bone chains + spring, **shoulder-independent** | **The single most expensive garment in the cast.** It is not supported at the shoulders, so it swings from the elbows. Cloth simulation will intersect the bare arms constantly. Authored chains only. |
| **Garter rig over bare thigh** | Human | Rigid attached geometry, fixed layer order | Straps crossing *bare skin* — the skin must not pinch through under deformation. Needs a corrective shape at the hip. |
| **Thigh-high boots on a block heel** | Human | Skinned, with a **dedicated foot-IK profile** | The heel changes the ankle pivot and ground-contact point. On discrete elevation this affects every traversal clip. A block heel is far more tractable than a stiletto — but budget the IK work explicitly, do not discover it. |
| Red bomber jacket | Synthetic / Leader | Cinematic-LOD cloth; bone chains at gameplay LOD | Her only cloth element. **Sleeve patches and zipper tape must survive to LOD1** — they are the inheritance evidence. |
| Luminous chassis | Synthetic | **Shader** — emissive + fresnel + scrolling iridescence keyed to material IDs | Her costume *is* a shader. Cheapest character in the cast. |
| Mechanical forearms and hands | Cyborg | Rigid skinned segments, no cloth | Five digits, symmetric. Cyan indicator is an emissive ID. |
| External armor plates | Leader | Rigid skinned segments over a body mesh | **Two states required** (G2) — this is a second costume, not a variant |
| Mech plating and hoses | Mech | Rigid segments; hoses as bone chains, 3 bones each | Do not simulate the hoses |
| Belt pouches, thigh rigs | Cyborg / Human | Skinned | Never simulated |
| Hair wisps, eye highlights, impact frames, speed lines | all | **2D overlays** | The 90s feeling lives here, and it is nearly free |

**Forbidden in gameplay LOD:** thin self-intersecting geometry that fails the 128 px silhouette test; unsupported free cloth; individual-strand hair. The human's costume passes canon but **must be validated against the 128 px plate before it is modelled**, not after.

### 11.6 Animation economics

`RECOMMENDATION` — rough ranges, assumptions stated.

Per protagonist, combat layer only:

| Group | Clips |
|---|---|
| Locomotion and traversal (idle, walk, run, turn, step-up, drop-down, cover-enter/exit) | ~14 |
| Generic combat reactions (hit light/heavy, stagger, guard-break, downed, rescue-receive, death) | ~8 |
| Motion archetypes | 8 |
| Archetype timing/VFX variants across ~22 abilities | ~16 |
| **Per character** | **~46** |

Cast total: 3 protagonists ≈ 138; mech (including two stances) ≈ 35; leader, prologue-only reduced set ≈ 25; Operator, non-combat ≈ 8. **≈ 206 authored clips for combat, before a single cinematic.**

At 0.5–1.5 person-days per polished clip including cleanup, integration and review, that is **≈ 105–310 person-days — roughly 5 to 15 person-months of animation for the combat layer alone.** Assumptions: one experienced animator; clips authored not captured; the archetype rule holds; no rework from costume changes.

**This is the number that should drive scope decisions**, and it is the strongest argument for both the archetype rule and for cutting the ninth Specialty before it is designed rather than after it is animated.

**Reusable motion families** — the leverage: (a) all three protagonists share the locomotion skeleton and can share timing, differing by pose layer; (b) reactions are shared across the cast with per-character pose offsets; (c) the mech's stance transitions are three clips, not a matrix.

### 11.7 Generative tools — the boundary

**Permitted, net labour saved:** static backgrounds and matte backdrops for B-tier scenes; storyboards, thumbnails, disposable animatics; non-hero prop and material variation with human cleanup; enemy silhouette ideation; UI exploration, never final text; supervised cleanup of the team's own authored art; **and code and tooling generation — validators, importers, the screenshot-regression rig — which is by far the highest-value use for this team.**

**Not permitted, cleanup exceeds benefit:** any canonical character frame or animation; faces and hands at any scale; in-betweens; anything that becomes a material or palette ID.

**On the existing sheets:** static, human-selected, human-approved orthographic references are the *permitted* case, and using them this way is defensible. What is missing is the paperwork that makes it safe — a provenance record and an explicit approval marker per sheet. `RECOMMENDATION`: add both before modelling. The risk is not this generation; it is the next one.
### 11.8 The Synthetic's jacket toggle — one rig, one garment layer `CANON OBSERVED` + `RECOMMENDATION`

Added in revision 2. Source: `MINA-CONSULTATION-ADDENDUM-001.md` §4.

**Settled.** Only the Synthetic gets this. It is a **cosmetic visibility toggle** — not a stat, skill, slot, item or alternate body. **Same canonical model and rig in both states.** It may show in ordinary gameplay presentation. **Authored cinematics override it** and use the scene's canonical costume state; Team 2 canon with no scene-specific exception shows the jacket. Portraits, UI renders, shadows, VFX anchors, hitboxes, targeting, animation timing and simulation **must not diverge.** Implemented as **one controlled optional garment layer, never a duplicated character.**

**This is the right call and it is nearly free.** She already has both sheets — base chassis and jacketed — at identical dimensions (§4.1), which is exactly the pairing a garment layer needs. And the fiction earns it: the jacket is inherited from a dead woman, so a player choosing to show or hide it is making a small characterisation choice, not picking a skin.

**How to build it so the non-divergence guarantee actually holds:**

| Concern | Implementation |
|---|---|
| Mesh | One body mesh. The jacket is a **separate submesh bound to the same skeleton**, toggled by renderer enable — never a second character prefab, never a blendshape body |
| Body under the jacket | **Do not delete body geometry under the garment.** Toggling visibility must not require a different body mesh. Solve interpenetration with a shrink-wrap corrective or a masked region, not by authoring two bodies |
| Hitboxes / targeting | Bound to the **body**, never the garment. The garment has no collider |
| Animation | One clip set. The garment is skinned and follows; it never gates timing |
| VFX anchors | On the body skeleton. A chassis-flare anchor must fire identically in both states |
| Shadows | The garment casts; the silhouette differs, and that is acceptable and expected. What must not differ is the **grounding shadow** used for tactical readability |
| Portraits and UI renders | Rendered from the same asset with the toggle applied. **Never a separate portrait painting per state** — that is how the two states drift |
| Cinematics | The scene declares its canonical costume state and **overrides the player toggle**. §12 |
| Luminous chassis | The jacket occludes part of the emissive chassis. Confirm the material-ID sampler still finds its sample points in both states, or the golden-render gate produces false failures |

**Automated gate, added to §11.9:** render both states from the six fixed cameras and assert that **hitbox bounds, skeleton pose, VFX anchor world positions and animation event timings are bit-identical**, and that only the garment's pixels differ. This is a cheap test and it is the one that catches divergence before it ships.

`RISK`, low but real: the toggle doubles the *validation* surface for one character even though it does not double the asset. Budget the gate, not the art.


---

### 11.9 Character asset validation gates `RECOMMENDATION`

Added in revision 2, consolidating what was scattered and adding what the addendum requires. Run on every content or shader change.

1. **Golden renders** — 6 fixed cameras per character (front / 3q / profile / back / gameplay-45° / cinematic-close) × 2 lighting rigs. Perceptual diff opens a review, never an auto-fail.
2. **Silhouette overlay** at 128 px and 64 px against the approved plate; assert IoU ≥ threshold. *This is the gate the human's strap rig and the cyborg's hair must survive.*
3. **Palette / material-ID sampler** — fixed sample points per material, ΔE00 against the swatch file.
4. **Proportion test** — head-height ratio, shoulder width and total height against the shared chart (§11.2 item 1).
5. **Animation continuity scan** — every frame of every clip; bone-length invariance, no cloth self-intersection, no attach point leaving its envelope.
6. **Screen-space readability** — gameplay-45° render downscaled to actual on-screen size; silhouette IoU must still pass. *Catches "beautiful in the viewport, mush in the game."*
7. **Jacket-state parity** *(new)* — render the Synthetic in both toggle states and assert hitbox bounds, skeleton pose, VFX anchor world positions and animation event timings are **bit-identical**; only garment pixels may differ. §11.8.
8. **Gear-variant silhouette** *(new)* — every visible gear variant passes gate 2 independently. If Q27 rules gear largely invisible, this gate shrinks to weapons only.
9. **Minimum-preset floor** *(new)* — run gates 1, 2, 3 and 6 **at the lowest supported preset**, not only at Ultra. §13.2. A preset that fails the silhouette or material-ID gate is not a supported preset.
10. **Manual review at magnification** *(new, procedural)* — no visual fact is recorded from a thumbnail. Costume boundaries, sleeve and glove terminations, limb-material transitions and digit counts are read at magnification on the region in question. This gate exists because both consultations have now produced a wrong visual finding from reduced-scale reading (§5 C1, §5 C6).

**Human gates, never automated:** final art direction, "is this the same person", and cadence and feel.

## 12. Gameplay/Cinematic Consistency Strategy

The cheapest lever available, and it fits the broadcast-switcher UI direction in `PREPRODUCTION` §8:

- Keep protagonists at **≥ 140 px tall at default zoom** on a 1080p screen. Below ~96 px no amount of asset quality is visible and the entire art investment is invisible during play — which is precisely the "visually ambitious but mechanically hollow" failure inverted.
- Add a **commit zoom**: a hard switcher-style cut to a closer camera for the action-resolution beat, using the *same asset* at cinematic LOD. Hair and face LOD switch at the same threshold.
- Every ability is therefore seen at cinematic quality without one extra asset.

### 12.1 Cinematic override rules `CANON OBSERVED` + `RECOMMENDATION`

Added in revision 2. Two settled directions converge on one rule, so state it once and enforce it in one place.

**The rule:** an authored cinematic renders the **scene's canonical costume and loadout state**, never the player's current gameplay state. This covers both:

- **Gear** — cinematics use the authored canonical visual loadout regardless of what is equipped (addendum §3).
- **The Synthetic's jacket** — cinematics override the player toggle; Team 2 canon with no scene-specific exception shows the jacket (addendum §4).

**Why it needs to be one mechanism, not two.** If gear and the jacket are overridden by separate ad-hoc code paths, they will drift, and a scene will eventually ship with the wrong one applied. `RECOMMENDATION`: every cinematic scene declares a **costume state token** in its scene metadata — `{characterId, costumeState, loadout}` — and the presentation layer resolves character appearance from that token whenever a cinematic is active, from a single resolver. Player state is not consulted. Validator rule §10.3 #18 enforces it.

**Why the rule is right, and not just tidy.** These are authored shots with authored silhouettes, lighting and composition. A player who equipped a shoulder-mounted cannon should not see it clip through a close-up framed months earlier. `RISK`, worth naming: players sometimes read this as the game ignoring their choices. The mitigation is that gear is a **build** decision rather than a **dress-up** decision — which is exactly what §8.5's "no loot soup" direction already implies — plus one deliberate exception: **the jacket state may be honoured in Tier-C battlefield cut-ins**, which are close, frequent and cheap, so the player's choice still appears where they will actually notice it.

Cinematic tiers, realistic indie slice budget (`RECOMMENDATION`, estimates):

| Tier | Slice budget | Note |
|---|---|---|
| S | 1 shot, 5–10 s | True hand-animated OVA quality is ~300–600 skilled hours for ~10 s. **Do not attempt a true S for the slice.** Deliver an exceptional hybrid that reads as S. Save true 2D for the shipping opening. |
| A | 2 shots, 20–30 s | In-engine, Cinemachine + Timeline, LOD0 rigs. The pipeline is the cost, not the shot. |
| B | 3–4 minutes | Layered art, constrained motion, camera, light, particles, typography. **This is where the 90s feeling gets delivered per dollar. Spend here.** |
| C | 12–20 cut-in lines | Portraits plus 3–5 expressions each. Cheap, enormously effective, do more than feels necessary. |

---

## 13. Performance Strategy

`RECOMMENDATION` — provisional targets to be falsified during the spike, not budgets derived from profiling. Representative PC target must be named before day 1; I assume a GTX 1660 / RX 5600-class machine at 1080p.

| Budget | Provisional target | How it gets validated |
|---|---|---|
| Frame time total | 16.6 ms (60 fps) | Spike day 9 capture |
| GPU | ≤ 11.0 ms | Frame Debugger + RenderDoc |
| CPU main thread | ≤ 5.0 ms | Unity Profiler |
| **Turn resolution** | **≤ 8 ms** | Headless benchmark, day 3 |
| **Forecast recompute on hover** | **≤ 4 ms** | Headless benchmark — this must feel instant or the pillar dies |
| Draw calls | ≤ 1200 | Frame Debugger |
| Visible skinned meshes | ≤ 14 (3 protagonists, 1 mech, 10 enemies) | Scene assembly |
| Bones — protagonist LOD1 | ≤ 120 | Rig review |
| Bones — protagonist LOD0 + face | ≤ 220 | Rig review |
| Unique materials in frame | ≤ 40 | Frame Debugger |
| Transparent overdraw at peak VFX | ≤ 2.0× screen | Overdraw view |
| Post stack total | ≤ 2.5 ms | Per-pass profiling |
| Dynamic shadow casters | ≤ 8 | Scene assembly |
| Texture residency | ≤ 2.5 GB VRAM | Memory Profiler |
| Level load | ≤ 6 s cold | Stopwatch |

**The two that matter most are the two the industry usually omits:** turn resolution and forecast recompute. If hovering a tile takes 40 ms, the player stops exploring options and the tactical layer collapses to muscle memory. Measure them in the headless kernel in week one, where they are free to fix.

### 13.2 Scalable graphics and the minimum-quality floor `CANON OBSERVED` + `RECOMMENDATION`

Added in revision 2. Source: `MINA-CONSULTATION-ADDENDUM-001.md` §5.

**Settled policy.** Maximum visual quality at the highest preset, plus industry-standard graphics and accessibility settings so slower supported systems run well. **The lowest supported preset is a separately tuned art target** — intentionally art-directed, stable, legible, polished. It may be simpler; it must never look broken, muddy, generic or graphically abandoned. **A low preset is not made by globally disabling the visual system.**

**May scale down** — internal render resolution and upscaling quality; shadow resolution, distance, cascades, contact shadows, secondary casters; volumetrics; reflections; ambient occlusion; VFX density and secondary particles; transparent layers; hair and cloth simulation complexity; environmental animation density; cinematic LOD distance; anti-aliasing; post-processing cost; texture resolution within memory budgets; crowd and background density where narratively safe.

**Must be preserved at minimum** — canonical silhouettes and proportions; faces and readable expressions at intended gameplay distance; the core toon-shading language; palette relationships; material identity **including the Synthetic's luminous chassis**; combat telegraphs, target outlines, forecast information, hit timing and VFX readability; stable frame pacing; essential grounding shadows.

**This is the correct policy and it is the more expensive one.** Say so plainly: a separately art-directed low preset is real work, not a slider. The alternative — one visual system with everything turned down — is cheaper and produces exactly the "graphically abandoned" result the policy forbids.

**How to make it tractable rather than a second art pass:**

| Principle | Implementation |
|---|---|
| **Toon shading is not a post-process, so it survives** | The ramp, terminator and colour model live in the character shader. Scaling AA, AO or volumetrics does not touch them. This is a genuine structural advantage over a photoreal target and it is why the floor is achievable at all |
| **The analog stack degrades by pass, not by strength** | Drop whole passes in a fixed priority order rather than fading everything. Keep line-weight stabilisation and halation-on-emissive to the last — they carry the 90s read. Gate weave, tape dropout and grain go first |
| **Outlines are non-negotiable** | Inverted hull is cheap and it *is* the art style. Never scale it out. Scale its width with resolution so it stays perceptually constant |
| **The luminous chassis is a shader, not a VFX system** | Emissive plus fresnel plus scrolling iridescence survives at minimum by construction. Explicitly exempt it from VFX-density scaling |
| **Grounding shadows are a readability feature** | A character with no contact shadow floats and the player misjudges elevation. Keep a cheap blob or capsule shadow at minimum rather than removing contact shadows entirely |
| **Simulation never scales** | Hit timing, telegraphs, forecast and frame pacing come from the sim (§9.1). A preset cannot change them. This is a direct benefit of the engine-free boundary |
| **Cloth and hair degrade to skinned, never to nothing** | The human's coat and the cyborg's hair fall back to fully skinned poses. Silhouette is preserved; only secondary motion is lost |

**Provisional preset targets — three tiers, `REQUIRES PROTOTYPE`:**

| | Minimum | Standard | Ultra |
|---|---|---|---|
| Reference hardware | **Q30 — undefined** | GTX 1660 / RX 5600 class | RTX 3070 class or better |
| Resolution / upscaling | 1080p, upscaled from ~67 % | 1080p native | 1440p+ native |
| Frame target | **60 fps, stable pacing** | 60 fps | 60 fps |
| Analog post passes | 3 of 7 | 5 of 7 | 7 of 7 |
| Hair / cloth | Skinned only | Bone chains | Bone chains + cinematic cloth |
| Shadows | Grounding + 1 cascade | 2 cascades + contact | Full |
| Character LOD at gameplay | LOD1 | LOD1 | LOD1, LOD0 on commit zoom |

**Note that the frame target does not drop at minimum.** `RECOMMENDATION`: for a turn-based tactical game, **stable pacing at 60 matters more than resolution**, and a minimum preset that upscales aggressively but never hitches is a better product than one that renders native and stutters. Q30 — naming the minimum machine — is what turns this column from a guess into a target.

`RISK` **R29**: the floor is asserted but never measured, and "minimum" quietly becomes "everything off". The mitigation is gate 9 in §11.9 — **run the asset-validation gates at the minimum preset, not only at Ultra** — plus the spike acceptance criteria in `CLAUDE-TWO-WEEK-SPIKE-001.md`.

---

## 14. Unity 6 URP Evaluation

`RECOMMENDATION`: **keep Unity 6 URP.** Not because it is safe, but for three project-specific reasons.

1. **Inspectability is a first-class requirement here.** Mina implements primarily through Astra. C# plus text-authored content plus assembly-definition boundaries is diffable, reviewable and agent-editable. Unreal accumulates production truth in Blueprints, materials and editor-authored sequences — binary, hard to diff, hard for an agent to reason about. For *this team's working method* that is decisive, and it is a stronger argument than any rendering comparison.
2. **The analog post stack wants exactly what URP gives.** Seven independently toggleable Renderer Features with explicit ordering and a UI exclusion mask is a natural URP structure. Unreal's post chain is more opinionated and fighting it costs more than building it.
3. **Editor scripting is the force multiplier.** Validators, importers, golden-render harnesses and screenshot regression are the highest-value generative-tool use for this team (§11.7), and Unity's editor scripting is where that pays best.

**A fourth reason, added in revision 2: the scalable-quality policy fits URP unusually well.** URP Assets are per-quality-level by design, so the three tiers in §13.2 are three URP Assets plus a Renderer Feature enable-mask — not a bespoke settings system. Because the toon ramp, the colour model and the inverted-hull outline live in the **character shader** rather than in post, scaling AA, AO, volumetrics or shadow cascades does not touch the art style. That is precisely what makes a genuinely art-directed minimum preset affordable here, and it would be materially harder in a pipeline whose look depends on post-processing.

**Why not Unreal:** it has the higher out-of-box cinematic ceiling, and if the visual target proves unreachable that is the fallback. But adopting it for cinematic quality would trade away the inspectability that makes agent-led implementation viable — the project's actual constraint is not rendering ceiling, it is throughput with a very small team.

**Why not Godot:** cleanest architecture, but you take on responsibility for cinematic tooling, the animation pipeline and console path. Lowest architectural risk, highest production risk. Wrong trade for a project whose selling point is cinematic presentation.

**What must be demonstrated before the choice becomes binding** — five gates, all in §15:

1. Toon shading plus inverted-hull outlines with **no shimmer** at gameplay camera distance, in motion.
2. The full post stack under 2.5 ms with UI excluded and faces unmuddied.
3. Stepped acting at 12 Hz against a continuously moving camera with **no input latency above one frame**.
4. Grid, cover and elevation readable with the final-intent camera at 1080p.
5. One transition from B-tier narrative presentation into playable combat with no visible asset swap.

**Results that would justify reconsidering the engine:** shimmer that cannot be removed without disabling outlines; post stack above 5 ms; stepped cadence that cannot be decoupled from camera and hit timing without a custom Playable that Unity's animation system fights.

**Results that would require changing architecture but not engine:** forecast recompute above 4 ms (fix the resolver, not the renderer); turn resolution above 8 ms (fix the data structures).

---

## 15. Two-Week Technical Spike

Full daily plan in `CLAUDE-TWO-WEEK-SPIKE-001.md`. Summary of shape and rationale:

**Week 1 is not in Unity.** Days 1–5 build the headless kernel, the forecast, and the text-mode encounter. This is deliberate: it tests the two highest-value assumptions (H1, H2) at the lowest possible cost, and it produces the replay corpus that every later regression test consumes. If the forecast is not legible in text, no Unity work would have saved it.

**Week 2 is the engine test**, using the **synthetic** as the single spike character — she is the cheapest in the cast (body is a shader, one cloth garment) and she exercises the hardest shader problem. Days 6–10 cover toon shading, outlines, the post stack, stepped cadence at 60 fps, the tactical camera, and one narrative-to-combat transition.

**What must not be built during the spike:** talent trees, gear, more than three enemy families, the mech's full stance system, the prologue, any cinematic beyond one test shot, Ink integration, save/load, or a second character model.

---

## 16. First Representative Encounter

`RECOMMENDATION` — designed to falsify, not to showcase. Playable in text first.

**Map.** 14×14, three elevation bands (0, 1, 2). A raised gantry along the north edge at elevation 2 with two access ramps. A central yard at elevation 0 with four hard-cover pillars in an irregular scatter. A conductive water channel crossing the yard diagonally at elevation 0. Two doorways on the south edge.

**Objective.** Non-elimination: hold a terminal on the gantry for two full rounds, then extract through the south doorways. Enemies reinforce from the north on a published timer.

**Opposition — three families, each answering a different protagonist.**
- **Anchored heavy** (`mass: heavy`, resists displacement, carries `GuardBreak`) — punishes turtling, degrades rather than nullifies the synthetic's control.
- **Networked drones** ×3 (`firewall: low`, share a network link) — the synthetic's `Conductive`/`Hacked` target, and the cluster the human is meant to create.
- **Ranged suppressors** ×2 on the gantry (publish threatened tiles, deny movement) — force the cyborg to body-block and the human to flank.

**The intended line** — one of several, and the encounter fails if it is the only one:
1. Human uses Con Girl to lure two drones off the network node, creating a cluster and marking one `Isolated`.
2. The cluster satisfies `SURROUNDED 2` on the cyborg, who has advanced to hold the ramp.
3. Synthetic casts the rescue pulse on the cyborg: three-tile knockback where legal, haste, temporary shock coating.
4. Two drones are displaced into the water channel — `Conductive` — enabling a Heartless chain that would otherwise be impossible.
5. Cyborg converts the opening with a stance change and intercepts the suppressor's line, generating the third contribution.
6. Sync granted, because three distinct cross-character contributions resolved.

**Variants to run on the same map:** reinforcement timer halved; the water channel dry; the heavy starting on the gantry instead of the yard; the human deployed on the far flank.

**Pass criteria.**
- Five plays produce at least three materially different opening lines.
- A player who did not design it can state, before committing, what conditions a move will create — measured by asking them, not by asking whether the UI showed it.
- The three-character handoff occurs at least once **without being instructed**.
- Turn resolution ≤ 8 ms; forecast recompute ≤ 4 ms.
- No anti-loop invariant is violated across 10,000 fuzzed AI-vs-AI simulations.

**Fail criteria.** Any of §7.5. Additionally: if the intended line is the only line that works, the encounter is a puzzle and the tactical thesis is not yet proven.

**Mech footprint validation, added in revision 2.** Build this map to exercise the §4.2 hypothesis rather than to avoid it:

- **One doorway on the south edge is 2 tiles wide, the other is 1** — so the encounter tests validation 1 directly. If the mech can only leave by one exit, that is interesting; if it cannot leave at all, the footprint is wrong.
- **The two gantry ramps are 1 tile wide**, so a 2×2 mech cannot use them. This forces the Remote Arsenal separation to *mean* something spatially — the pilot goes up, the mech holds the yard — and it is the cheapest possible test of validation 7, remote-control readability.
- **The pillar scatter is irregular** so 2×2 pathing must handle partial blocking, not just open ground (validation 2).
- Run the encounter **twice, at 1×1 and at 2×2**, on the same map. `RECOMMENDATION`: this comparison is the single most informative hour in the spike, and it costs a constant.

`REQUIRES PROTOTYPE`: if 2×2 makes the encounter unplayable, report it as the scale-versus-footprint conflict (§5 G1). **Do not resize the map to rescue the footprint, and do not shrink the mech to rescue the map.** Both are silent answers to a question Juan and Mina have reserved.

---

## 17. Vertical-Slice Scope

`RECOMMENDATION`, in order:

1. One Tier-B animated graphic-novel scene introducing the trio and one decision.
2. That decision changes deployment, objective, information or ally state.
3. The §16 encounter, art-complete, with the final-intent camera and UI.
4. One mid-mission consequence demonstrating narrative ↔ combat feedback.
5. A three-phase climax using one combination ability and one environmental interaction.
6. A 20–40 second Tier-A payoff using the same canonical assets at higher LOD.
7. An epilogue reflecting both the decision and the combat outcome.

Two characters fully art-complete, not three. The synthetic and the cyborg — they are the two survivors, the main timeline begins with them (`STORY-CANON-001.md` §7), and they are the cheapest and the most expensive rigs respectively, which means the slice proves the range.

---

## 18. Deliberate Cuts

Cut now, restore later, and say so out loud so they are not rediscovered as failures:

- **The prologue.** It requires the leader as a full character with two costume states, Operator as a physical body, prologue-only enemies and maps, and a Limit Break system used twice. It demonstrates the *old* team. Defer.
- **Six of nine Specialties.** Build one per character for the slice.
- **All talent trees.** Prove the patch model with six talents, not two hundred.
- **Most gear.** Author **two sets and one loose piece, for the cyborg only** (§10.4). That is enough to test 2+2 combination, the identity-preservation validator and whether sets read as builds. Three protagonists' worth of gear proves nothing extra.
- **Visible gear variants**, until Q27 is ruled. If gear turns out to be largely invisible, the mesh matrix never gets built at all — so do not build any of it speculatively.
- **The minimum graphics preset**, as a *tuned art target*. Define its rules now (§13.2), build the preset after the slice. What must happen during the spike is only the measurement (Q30) and the gate wiring.
- **The two-survivor chapter** (G3) — until its combat design exists.
- **Save/load** beyond the replay log.
- **The mech's third stance.**
- **True S-tier 2D.** Hybrid only, until the shipping opening.
- **Platform scope beyond PC.**

What must **not** be cut, because it is the commercial differentiator: the conditional-support forecast, character quality at gameplay camera distance, and the commit zoom.

---

## 19. Production and Commercial Feasibility

`RECOMMENDATION`, rough ranges, assumptions stated.

**Scale.** The vertical slice as scoped in §17 is roughly **6–10 person-months** — animation ~3–5, technical art ~1.5–2.5, systems ~2–3, narrative and audio ~1, running partly in parallel. Assumes a small team with one strong animator and Mina on systems with Astra. The full game at this quality bar is a **multi-year, multi-person** production; nothing in the documents suggests otherwise and nothing here should be read as suggesting a shortcut.

**What is commercially valuable and visible.** The 90s OVA look in motion at gameplay distance. The commit zoom. The forecast UI showing conditions opening and closing — this is genuinely novel presentation for the genre and it is the screenshot that differentiates. Three authored characters with real relationships.

**What is expensive and largely invisible to buyers.** True S-tier 2D animation in the slice; the ninth Specialty; deep gear systems; the two-survivor chapter's bespoke balance; save-system sophistication. Spend here later, not now.

**What would make it look like generic indie tactics.** A grey-box map with placeholder characters in the first public material; percentages on a hit chance; a UI that looks like a spreadsheet; enemies that are recoloured shapes.

**What would make it look visually ambitious but mechanically hollow.** A beautiful trailer with no legible combat; characters at 80 px during actual play; a conditional-support system the player cannot see coming. **This is the specific failure this project is most exposed to**, because its art is currently far ahead of its systems.

**The differentiation demo, when the time comes:** one 30-second capture of the §16 line — the human lures, the condition badge lights up, the pulse fires, the drones land in the water, the chain resolves, the commit zoom cuts in at cinematic quality. That single clip demonstrates the tactical thesis and the visual thesis simultaneously. Nothing else in the project demonstrates both.

---

## 20. Risk Register

Full sortable table in `CLAUDE-RISK-REGISTER-001.md` — **30 risks, of which 29 open and 1 closed** after revision 2. The top five by cost of learning late:

| ID | Risk | Prob. | Impact | Cheapest validation |
|---|---|---|---|---|
| R1 | Authored animation volume exceeds capacity | High | Critical | Action Vocabulary Census, 2 days |
| R2 | Forecast is not legible; conditional support becomes a lottery | Medium | Critical | Text-mode forecast harness, 1 week |
| R3 | Three units do not generate tactical variety; enemy design absent | Medium | Critical | §16 encounter, 5 plays |
| R4 | Mech 2×2 footprint unvalidated; blocks the grid | High | High | §7.6 seven validations, spike week 1 |
| R5 | Art production starts before the combat thesis is proven | High | High | Sequencing discipline; no mitigation once spent |

Revision 2 changes: **R11 (Operator arms) closed** — no contradiction existed. **R4 reworded** — a hypothesis now exists but is unvalidated. **Five risks added:** R26 gear set-bonus identity erosion, R27 visible-gear asset matrix cost, R28 jacket-state divergence, R29 minimum-preset floor asserted but unmeasured, R30 2×2 fails and is silently compressed to 1×1.

---

## 21. Decisions Required from Juan and Mina

Full detail, options and deadlines in `CLAUDE-QUESTIONS-001.md`.

### 21.1 Genuine architecture blockers: **three**

Reclassified in revision 3. A question belongs here only if **the combat kernel cannot be written correctly without the answer.** Settled rulings awaiting transcription, and source-governance chores, are work — they are not blockers, and counting them as such overstates how stuck the project is.

| # | Decision | Why it blocks the kernel | Owner |
|---|---|---|---|
| **Q2** | `Integrity` — attribute or pool? | One name, two objects (`CODEX-BRIEF` §5 vs §10). Determines the stat schema, the damage pool, the UI panel and the forecast's damage display. Every one of those is written once and expensively rewritten. | Mina |
| **Q3** | To-hit rolls — yes or no? | Determines whether the PRNG is load-bearing, what `Game.Forecast` can promise, and whether the AI's evaluation and the player's forecast are the same computation (§9.3). Retrofitting determinism onto a probabilistic resolver means rewriting the resolver, the AI and every test. | Mina |
| **Q4** | Mech footprint — is it 2×2? | Multi-tile occupancy is a first-class kernel feature or it is not: multi-tile pathing, multi-tile LOS, per-edge cover contribution, and a second mass class for displacement. **Provisional and untested** (§4.2, §7.6). | Juan + Mina |

### 21.2 Reclassified out of the blocker count

| # | Was | Now | Reason |
|---|---|---|---|
| ~~Q1~~ | Blocker | **CLOSED** | Operator's arms were never in conflict. My revision-1 finding was wrong (§5 C1). |
| ~~Q5~~ | Blocker | **Editing task** | Con Girl's scaling is **already settled**: all three of the white-haired human's Specialties scale from Agility; "Charisma" names her fantasy and behaviour, not an attribute. The kernel can be written today against that ruling. What remains is transcribing it into `CODEX-BRIEF` §5 and §7.2 and `STORY-CANON-001.md` §3.1 so nobody reading `DD90s` implements a Charisma stat. **Real work, real risk of drift — not an unresolved decision.** |
| ~~Q6~~ | Blocker | **Source governance** | Duplicate-sheet cleanup and a filename convention affect how reliably assets are identified. They do not affect a single line of kernel code. **Do not delete the historical sheets yet** — supersession is recorded by status, not by deletion. |

### 21.3 The conditional fourth — **closed by Mina's entity-model ruling**

Revision 3 named **Q31, the pilot/mech entity model**, as the one remaining candidate that could make the count four. It asked how many board entities the cyborg is, not how large one is — more foundational than Q4, because it determines the unit model itself.

**It is now ruled** (§9.6.1): pilot and mech retain **separate, stable logical entity IDs regardless of deployment mode**. Embarked, the pilot stays in authoritative state but is non-spatial, non-selectable, non-targetable and holds no independent initiative slot, while the mech owns occupancy and selection. In remote operation both may be spatial and addressable.

**The unit model is therefore determined, and Q31 does not block architecture.** What remains under Q31 — which Specialties permit which deployment mode — is a **product decision that changes no data structure**, because the flags it would set already exist. It is kept as a combat-prototype and product question and cross-referenced to Q8, the command model, which is the prototype half of the same subject.

**The count stays at exactly three.**

### 21.4 Verified as *not* architecture blockers

Checked explicitly against the complete documents, because "three" is only credible if the near-misses are named:

- **Q8, mech command model** (shared AP / queued directives / separate initiative + bandwidth). Coupled to Q31, but the kernel can be built to support all three and `CODEX-BRIEF` §8.2 asks for exactly that. **Prototype-decidable, not a pre-ruling.**
- **Q10, does the mech get its own `Integrity` pool.** Downstream of Q2 — it cannot be answered before Q2 and adds nothing once Q2 is answered. Not independent.
- **Q26, gear slot names.** Juan's revision-3 ruling settles the *structure* — `1 weapon + 4 gear`, weapon excluded from set thresholds, `mech-hardpoint` inside the four. Only the naming remains, and ids can be authored against a fixed four-entry enum today. **Naming task.**
- **Q27, is gear visible in gameplay.** The largest cost fork in the project, and it blocks modelling — but it is a presentation-layer decision that touches no kernel code.
- **Q30, minimum supported hardware.** Blocks *measurement*, not architecture. It is a spike day-1 prerequisite; the kernel does not depend on it.
- **Q31, pilot/mech deployment modes by Specialty.** The *entity model* is ruled (§9.6.1); what remains is which Specialties permit which mode — a product decision that sets existing flags and changes no data structure. **Combat-prototype and product question, cross-referenced to Q8.**

**Conclusion: exactly three.** The conditional fourth is closed — Mina's entity-model ruling determines the unit model, so Q31 becomes a product decision rather than an architecture one. Q2 and Q3 are rulings that can be made in an afternoon and should be made before Monday. Q4 is a spike outcome rather than an armchair ruling — that is progress, but it stays blocking until §7.6 passes, and **it must not be answered by adopting the hypothesis as canon.**

---

## 22. Recommended Immediate Next Step

**One thing, this week: build the text-mode forecast harness.**

Not Unity. Not modelling. Not the manifest, though it should follow.

A headless implementation of §7's ruleset with `Game.Forecast` producing `conditionsOpened` / `conditionsClosed`, rendered as ASCII, running the §16 encounter, replayable from `{seed, initialState, commandLog}`. One week. Zero art. It answers the two highest-cost-of-learning-late assumptions (H1 via the census it enables, H2 directly), it produces the replay corpus every later test consumes, and it is the only artefact that can falsify the project's central mechanical claim before money is spent on characters.

If a player cannot read "moving here creates `SURROUNDED 3` on the cyborg and enables the Pulse" in a terminal, they will not read it in a beautiful UI either. And if they *can*, then every subsequent decision — the engine, the rig, the animation budget — is being made against a thesis that is known to work rather than hoped to.

**Do not begin character modelling until the §16 encounter has been played and judged.**
