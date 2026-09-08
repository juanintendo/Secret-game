# 90s Anime × Tactical RPG — Codex / Claude Consulting Brief 001

Status: active implementation and external-consulting brief  
Date: 2026-09-08  
Creative direction: Juan + Mina  
Working title: TBD

## 0. Purpose and authority

This document gives a technical or design consultant enough verified context to critique and help plan the project without inventing its identity. It consolidates the current gameplay direction, visual-production constraints, character kits, and immediate development sequence.

Authority order:

1. `STORY-CANON-001.md` governs narrative truth and character knowledge.
2. `90S-ANIME-XCOM-PREPRODUCTION-001.md` governs the founding product, visual, combat, and production direction.
3. This document governs the newly defined progression/combat-class direction and the consulting questions needed to turn the concept into a prototype.

Do not import facts from Juan's unrelated beat'em-up, House, Ozymandias, Spell Out, or Arcadio projects.

## 1. Product thesis

A premium story-driven tactical RPG about three extremely capable women rebuilding a combat unit after the destruction of an earlier near-perfect team. The definitive trio has no appointed leader. Their central progression is not from incompetence to competence; each begins dangerous and experienced. The unit itself is level one.

The game must fuse:

- FFT/XCOM-like turn-based tactical readability;
- deep, character-specific RPG builds;
- relationships and narrative decisions that alter missions;
- an earned synchronization system that rewards actual teamwork;
- modern-fidelity 1990s techno-anime art;
- pristine character continuity between gameplay and cinematics;
- highly responsive, optimized gameplay.

This is not an anime skin over generic tactics. Character psychology, build identity, team synergy, animation, and tactical rules must describe the same people.

## 2. Non-negotiable pillars

### 2.1 Sacred visual identity

- Approved character art is canonical reference, not loose moodboarding.
- Faces, body proportions, hair masses, costume topology, signature materials, palette, mech silhouette, and synthetic anatomy may not drift shot to shot.
- The game uses clean high-definition 1990s OVA design language, not a permanent VHS filter.
- Analog artifacts are authored punctuation. They never obscure silhouettes, UI, or tactical information.
- Generated images or video do not become canon because one frame looks impressive.
- No malformed hands, changing limb lengths, costume mutation, fake text, topology drift, or uncontrolled frame-to-frame shimmer.

### 2.2 Gameplay and cinematics must share identity

Characters should look as close as technically practical in combat to how they look in cinematics. We should not build one attractive 2D identity and then accept generic or compromised gameplay stand-ins.

The production target is a shared character truth:

- the same approved proportions;
- the same face construction;
- the same hair silhouette;
- the same costume/material IDs;
- the same signature VFX colors and motion language;
- scalable LOD and presentation quality rather than unrelated models.

### 2.3 Smooth means responsive, not stylistically uniform

- Input, cursor, camera, UI, path preview, turn transitions, and simulation target fluid 60 fps.
- Gameplay actions acknowledge input immediately.
- Character acting may intentionally use stepped or held keys on 2s/3s where that produces 90s cel-animation cadence.
- Root motion, hit timing, camera, VFX, and UI remain temporally precise even when a pose is held.
- Low animation cadence must never become control latency or visual hitching.

### 2.4 Combat must reward understanding

- Information required for a decision is legible before commitment.
- Animation celebrates a resolved action; it does not conceal odds, range, cover, initiative, or consequences.
- Healing, protection, haste, and major buffs are primarily earned by satisfying mechanical conditions and creating teamwork, not dispensed freely by a permanent healer.
- Builds create new decision grammars, not only larger numbers.

## 3. Canonical teams

### 3.1 Original team — prologue

- Original human leader: fully human, green hair, external white/gray AI-assisted armor, short fitted red bomber jacket. Uses a tactical AI copilot but retains judgment and final field authority.
- Cyborg mech pilot: long blonde hair in current approved art, mechanical arms, sole mech operator.
- Luminous synthetic: black bob, slim healthy synthetic form, black panels, literal iridescent luminous skeletal chassis.
- Operator: physically alive and present at hidden HQ in this era; ethereal white hair, white clothing with purple details, right arm bare below shoulder armor, left arm sleeved/gloved.

The original field trio begins the tutorial near peak power and coordination. The second Limit Break becomes the opening exploited by a prepared attack. Operator appears to die at HQ. The leader saves the cyborg and synthetic and is presumed dead at the separate mission site.

### 3.2 Definitive second team — main campaign

- New white-haired human: fully human; short/tousled pale white-lavender hair in current team art; slim, agile, irreverent, elusive, provocative, individualistic; clean black tactical outfit without fur, feathers, or production-hostile trim.
- Cyborg mech pilot: survivor of the original team; long blonde hair; mechanical arms; Strength axis; her single mech is repaired and incrementally more advanced, not replaced by an unrelated machine.
- Luminous synthetic: survivor of the original team; Intellect axis; wears the original leader's short fitted red bomber jacket in the current timeline.
- Operator: restored/continued after the attack and now manifests to the team as a human-shaped hologram rather than a physically present woman.

No member of the definitive trio is the permanent leader.

## 4. Combat chassis to prototype

Current coherent hypothesis:

- compact square or diamond grid with discrete elevation;
- three protagonists deployed together;
- interleaved initiative;
- two action points per activation;
- movement, primary action, utility action, guard/overwatch, and character abilities;
- facing, flank, line of sight, elevation, and contextual cover;
- deterministic base damage with uncertainty surfaced before commitment;
- reactions, assists, rescues, and interrupts with explicit triggers;
- environmental interactions such as terminals, doors, hazards, power routing, destructible cover, and forced movement;
- objectives beyond eliminating every enemy;
- seeded, replayable simulation and action log.

Do not implement every FFT and XCOM mechanic simultaneously. The first headless prototype exists to determine which subset produces strong decisions with only three units.

## 5. Attribute and build model

The three protagonists have distinct primary axes:

| Character | Primary axis | Core battlefield promise |
| --- | --- | --- |
| White-haired human | Agility | Redirect attention, exploit openings, traverse danger, assassinate or manipulate |
| Cyborg mech pilot | Strength | Absorb, break, intercept, reposition threats, or deliver heavy force through herself and the mech |
| Luminous synthetic | Intellect | Invade systems, manipulate conditions and initiative, weaponize electricity, and create conditional support windows |

Primary attributes should drive identity, not become universal gear-score colors. A provisional stat family for testing:

- **Strength:** impact, stagger, brace, heavy-weapon handling, forced movement resistance.
- **Agility:** movement economy, flank conversion, evasion windows, reaction timing, close/ranged finesse.
- **Intellect:** system penetration, status potency, conditional duration, prediction, tactical resource manipulation.
- **Resolve:** resistance to panic, control, intrusion, and synchronization disruption.
- **Integrity:** health/structural durability; fiction and recovery presentation differ by human, cyborg, and synthetic.

Derived stats may include initiative, accuracy, crit, guard, stagger, focus, status resistance, hacking defense, movement, and reaction capacity. Keep the visible sheet small enough to forecast outcomes without spreadsheet archaeology.

## 6. Progression architecture

Each character has:

1. three mutually expressive Specialty trees comparable in strategic weight to modern World of Warcraft specializations;
2. one Personal tree unique to that character;
3. gear and loadout choices that modify a build without making characters interchangeable;
4. team/synchronization unlocks earned through relationships and demonstrated cooperation.

The Specialty trees are not nine unrelated classes. Each is a different interpretation of its character's central verb. Personal talents form connective tissue: identity-defining passives, hybrid enablers, reactions, signature upgrades, and ways to create synergy across Specialties.

Do not assume players can fill every tree. Prototype meaningful commitment with controlled cross-tree borrowing. Avoid false choice rows where one mathematically mandatory throughput talent dominates cosmetic alternatives.

### 6.1 Ability construction rule

An ability should expose most of these fields in data rather than hard-coded bespoke logic:

- AP cost;
- range, shape, targeting rules, and line-of-sight requirements;
- damage/guard/stagger profile;
- movement or forced-movement effect;
- condition requirements;
- tags and elemental/type identity;
- reaction window;
- initiative effect;
- cooldown/charge/resource cost;
- generated synchronization;
- follow-up, rescue, or handoff hooks;
- animation/VFX presentation request;
- AI evaluation hints;
- upgrade mutations.

Talents should often transform these fields or add typed effects. Avoid opaque script spaghetti per talent.

## 7. White-haired human — Agility

Core verb: **Exploit / Redirect**.

She does not become the missing leader. Her kits reward opportunism, improvisation, social manipulation, and refusing the expected route.

### 7.1 Dispatch — agile melee assassination

Fantasy: enter through an opening, kill or cripple the correct target, and escape before the formation closes.

Mechanical territory:

- high-mobility melee;
- flank and isolated-target conversion;
- marked vulnerabilities and finishers;
- short concealment or attention-drop effects rather than permanent invisibility;
- movement through threatened spaces under earned conditions;
- AP or initiative refunds tied to clean execution, not random crit loops;
- low tolerance for being pinned in open attrition.

Anti-pattern: a generic rogue who only stacks crit and backstab multipliers.

### 7.2 Con Girl — Charisma control and manipulation

Fantasy: make enemies misread intention, position, allegiance, or priority.

Mechanical territory:

- taunts, lures, false targets, hesitation, misdirection, compromised reactions;
- crowd control that trades some direct DPS;
- causing enemies to expose flanks, waste overwatch, collide with allied plans, or reveal information;
- stronger effects when she has observed a target or acquired leverage;
- partial efficacy against machines/aliens through spoofing, behavioral exploits, or tactical deception rather than universal magical seduction.

Anti-pattern: long hard-stuns that simply delete enemy turns without setup or counterplay.

### 7.3 Gunslinger — mobile ranged execution

Fantasy: precision gunplay while moving, manipulating lines, and stealing timing.

Mechanical territory:

- mobile shots and trick angles;
- reaction shots with authored triggers;
- ricochet or environmental shot opportunities where readable;
- quick-draw initiative contests;
- suppression-breaking and overwatch manipulation;
- escalating payoff from relocating between shots rather than camping.

Anti-pattern: becoming a static sniper with a different costume.

### 7.4 Personal talents — provisional themes

- improvised exits and emergency movement;
- converting enemy mistakes into team openings;
- selfish actions that can be upgraded into handoffs as trust grows;
- leverage/observation system shared across her three Specialties;
- limited gear cheating, concealed equipment, or alternate deployment options;
- relationship talents that prove she is becoming part of this unit without becoming its leader.

## 8. Cyborg mech pilot — Strength

Core verb: **Break / Anchor**.

She and the mech are one authored combat identity with multiple control arrangements. The mech magnifies her role but must not erase her individual body on the field.

### 8.1 Bulwark — classic tank with earned support

Fantasy: decide where the enemy is allowed to apply force.

Mechanical territory:

- guard, interception, body-blocking, brace, threat control, cover creation;
- redirecting or sharing damage under explicit conditions;
- counterattacks and stagger;
- support through protection, rescue, and stabilizing formation rather than free healing;
- creating safe handoff lanes for allies.

### 8.2 Remote Arsenal — mech summon/tank plus pilot heavy range

Fantasy: split her tactical presence: remotely operate the mech as an autonomous tank while she deploys heavy weaponry at range.

Mechanical territory:

- mech and pilot occupy distinct positions;
- command bandwidth/action economy prevents receiving two full characters for free;
- queued mech directives, limited autonomy, signal risk, and range/relay considerations;
- pilot uses heavy ranged weapons, siege shots, suppression, or armor break;
- enemies can exploit the separation, communication link, or exposed pilot;
- clever turns alternate between issuing orders, firing, and using allies to cover gaps.

Central prototype question: whether the mech receives its own initiative slot, consumes the pilot's AP, or operates through queued reactions. Test all three; do not choose by fantasy alone.

### 8.3 Redline — mech stance dance

Fantasy: dynamically reconfigure the mech between brutal commitment and survival/repositioning.

Provisional stances:

- **Assault:** melee heavy damage, armor break, bleed/rupture, high commitment.
- **Pursuit:** controlled advance, pinning, collision, shorter attack chains.
- **Disengage/Recovery:** lower damage, break contact, reposition, vent/repair a limited amount, buy turns, and restore operational stability.

Changing stance should carry timing and opportunity cost. Disengage is not a free reset; it trades pressure for space and partial recovery.

Anti-pattern: three colored passive buffs toggled every turn only for mathematical maintenance.

### 8.4 Personal talents — provisional themes

- bond and command language between pilot and mech;
- mechanical-arm counters and emergency actions when separated from the mech;
- repair through salvage, venting, bracing, or successful interception;
- protection converted into synchronization;
- configurable hardpoints that preserve recognizable mech silhouette;
- veteran trauma and refusal-to-fail mechanics without random disobedience.

## 9. Luminous synthetic — Intellect

Core verb: **Rewrite / Conduct**.

She is not the healer. Her Intellect expresses invasive damage, control of biological and machine conditions, initiative manipulation, and situational enhancement inspired by the original leader's judgment.

### 9.1 Heartless — invasive lightning damage

Fantasy: expose her luminous chassis as a terrifying conduction architecture and invade a target with maximum electrical/systemic violence.

Mechanical territory:

- lightning as signature element because it belongs to her body and visual language;
- charge, arc, grounding, conductivity, overload, and cascading target logic;
- high single-target damage or controlled chaining based on positioning;
- invasive effects that punish connected systems, armor, wet/conductive terrain, clustered targets, or compromised defenses;
- risk/reward through exposed chassis, heat, instability, or drawing hostile system attention;
- visual brutality without losing tactical readability.

Anti-pattern: a generic lightning mage wearing a robot skin.

### 9.2 Ghost Surgery — hacking and biohacking control

Fantasy: rewrite the battlefield by interfering with machines, implants, nerves, senses, and command protocols.

Mechanical territory:

- machine hacking, biohacking, sensor corruption, motor interruption, reaction denial;
- enemy-type-specific interfaces with a shared readable grammar;
- movement commands, targeting swaps, temporary subsystem disable, initiative delay, or behavior constraints;
- setup through scans, access, proximity, network nodes, conductive links, or ally-created openings;
- bosses lose subsystems or options rather than entire turns.

Anti-pattern: separate arbitrary spell lists for organic and mechanical enemies or universal domination.

### 9.3 Spell Slinger — conditional buffer/conductor

Fantasy: use predictive intelligence and the remembered combat philosophy of the original leader to create powerful support exactly when battlefield conditions justify it.

She does not cast unrestricted buffs on demand. The player creates or accepts a battlefield condition, unlocking a response.

Canonical example to prototype:

- An ally is surrounded by at least two enemies within a defined radius.
- This condition enables a synthetic support ability on that ally.
- Immediate cast creates a force pulse that knocks adjacent enemies back up to three tiles, subject to mass/collision rules.
- The ally gains temporary haste/initiative acceleration.
- For a limited number of turns, the ally's attacks add a shock effect.
- The pulse opens space so the rescued ally can advance, withdraw, or reposition.

Other conditional families may include:

- ally has just absorbed an attack meant for another;
- ally ends movement in a crossfire or threatened tile;
- two enemies share a conductive or hacked network;
- an ally converts another character's setup;
- a rescue or handoff has just occurred;
- the team deliberately cedes initiative.

Buffs should feel like intelligent answers to battlefield state, not upkeep spells.

### 9.4 Personal talents — provisional themes

- control over visibility/exposure of her luminous chassis;
- distinctions between self-authored thought, instruction, memory, and intrusion;
- conductivity and system-access grammar shared across all three Specialties;
- earned defensive layers when she correctly predicts or rewrites an enemy action;
- inherited techniques inspired by the first human leader, expressed as judgment conditions rather than leadership aura;
- protection against hacking that can be extended to allies through synchronized play.

## 10. Recovery without a healer

The game should distinguish at least three forms of survivability:

- **Integrity/Health:** consequential damage; expensive or limited to restore during a mission.
- **Guard/Stability:** renewable protection earned through stance, cover, interception, positioning, or abilities.
- **Wounds/Trauma/System faults:** longer-lived consequences treated between missions or through rare resources and narrative choices.

In-mission recovery should emerge from mechanical success:

- a Bulwark interception grants guard or stabilizes the protected ally;
- a Redline disengage vents/repairs the mech at the cost of pressure and damage;
- a Dispatch finisher may restore momentum or cleanse a pin, not magically heal flesh;
- a Con Girl manipulation can cause an enemy to waste an attack and create breathing room;
- a synthetic conditional rescue provides shielding, knockback, haste, or limited repair;
- completing handoffs, rescues, or chains can trigger team-wide stabilization thresholds;
- environmental resources may provide finite medical/repair opportunities.

Recovery is still possible, but the player must manufacture the window. Damage retains meaning and no character is forced into permanent heal-bot duty.

## 11. Synchronization and team synergy

The shared Limit/Synchronization resource does not fill primarily from ordinary damage. It rewards cooperation:

- converting another character's setup;
- rescue/interception;
- covering a withdrawal;
- ceding or exchanging initiative;
- using information another character produced;
- combining forced movement, control, and damage;
- accepting a cost for another character;
- completing character-specific handoffs.

Progression ladder:

1. basic joint attacks;
2. handoffs;
3. assists during another turn;
4. rescues that break or bend a normal rule;
5. three-character chains;
6. synchronized Limit Breaks.

Talents must expose synergy tags and triggers. Do not encode every cross-character interaction as a unique hard-coded pair. A typed grammar such as `Expose`, `Displace`, `Conductive`, `Guarded`, `Hacked`, `Isolated`, `Intercepted`, and `Rescued` can support authored combinations while remaining inspectable.

## 12. Gear philosophy

Gear expands expression without erasing identity.

Recommended slots to prototype:

- signature weapon/hardpoint;
- armor/frame component;
- utility module;
- character-specific signature slot;
- mech hardpoints for the cyborg only.

Gear can:

- change targeting shape, range, elemental/status interaction, reaction trigger, or cost;
- strengthen a Specialty or enable a controlled hybrid;
- create synergy with another character's tags;
- introduce a meaningful drawback or condition.

Avoid:

- frequent disposable loot with trivial +1 upgrades;
- equipment that grants another protagonist's unique verb wholesale;
- visual gear that destroys canonical silhouettes;
- random affix soup that makes builds impossible to author, balance, or animate.

Prefer fewer named components with visible mechanical consequences and controlled cosmetic integration.

## 13. Canonical character-consistency pipeline

### 13.1 Character source package

Every principal character needs a versioned identity package:

- approved hero image and neutral model sheet;
- orthographic front/profile/back;
- height and proportion chart shared across the cast;
- face construction sheet with neutral, three-quarter, profile, and extreme expressions;
- hair mass sheet showing primary volumes rather than individual strands;
- costume topology map and layer order;
- palette with stable color/material IDs under neutral light;
- material board: cloth, lacquer, skin, armor, luminous chassis, hologram;
- hand/foot anatomy and mechanical-joint reference;
- signature pose/motion principles;
- forbidden-drift examples;
- current canonical revision and provenance.

### 13.2 Shared 3D truth

Recommended production hypothesis:

- one canonical high-quality rigged 3D character per principal cast member;
- gameplay and in-engine cinematics derive from that same base model and rig;
- LODs, texture resolution, hair simulation level, and facial complexity scale by camera distance;
- silhouette-critical hair and clothing use controlled authored bones/cloth, not unconstrained simulation;
- toon shader reproduces approved 2D color separation and shadow shapes;
- signature 2D effects and impact frames layer over the 3D action.

This does not prohibit fully authored 2D S-tier sequences. It gives those sequences an authoritative pose, lens, proportion, costume, and lighting reference.

### 13.3 2D cinematic workflow

For S-tier anime sequences:

1. script and shot objective;
2. storyboard using canonical model sheets;
3. 3D previs/layout with canonical rigs, camera, props, and environment scale;
4. locked timing and eyelines;
5. key animation/cleanup based on the approved layout;
6. supervised in-betweens;
7. consistent color model and shadow library;
8. compositing with controlled analog effects;
9. identity, topology, temporal-continuity, and frame-stepping review.

Generative video may be used for exploratory motion ideas or disposable animatics. It is not the final source of truth for faces, hands, costumes, or canonical animation unless every frame can pass cleanup and continuity review.

### 13.4 A/B/C scene workflow

- **A:** in-engine cinematic using high LOD canonical rigs, bespoke cameras/animation, 2D inserts, strong compositing.
- **B:** animated graphic novel using approved layered character art, constrained motion, camera, light, particles, and voice.
- **C:** battlefield dialogue with canonical portraits/cut-ins and concise motion.

All tiers share palette, face design, costume revision, voice, terminology, and shot metadata.

### 13.5 Automated and human gates

- golden renders from fixed cameras under neutral and scene lighting;
- silhouette overlays against the approved sheet;
- face/landmark and proportion comparison as warning tools, never sole approval;
- palette/material-ID checks;
- animation frame stepping for limb, hair, costume, and prop continuity;
- screen-space readability test at gameplay camera distance;
- performance capture on target hardware;
- screenshot regression for shaders, UI, VFX, and cinematics;
- final human art-direction approval.

## 14. Rendering and performance direction

Current engine recommendation remains provisional: Unity 6 URP, pinned to a supported 6000.x version, subject to a technical-art spike.

Targets for the spike:

- stable 60 fps gameplay on the chosen representative PC target;
- three high-quality protagonists, one mech, representative enemies, tactical UI, and combat VFX in one scene;
- visually faithful toon shading under neutral and dramatic lighting;
- clean hair and clothing deformation;
- readable grid/cover/elevation with the final-intent camera;
- analog post stack that excludes UI and never muddies faces;
- seamless transition from B-tier narrative presentation to playable combat;
- one A-tier close shot using the same canonical character asset at higher LOD.

Set explicit budgets during the spike rather than after content production:

- CPU frame time;
- GPU frame time;
- draw calls and visible materials;
- skeletal bones and skinned meshes;
- texture residency;
- transparent VFX overdraw;
- dynamic lights and shadow casters;
- animation and AI evaluation cost;
- tactical simulation turn-resolution time.

## 15. Technical architecture

Keep simulation truth separate from presentation:

- `Game.Core`: deterministic primitives, tags, effects, seeded randomness, logs.
- `Game.Tactics`: grid, navigation, LOS, cover, initiative, AP, reactions, forced movement, objectives.
- `Game.Abilities`: data definitions, targeting, conditions, costs, typed effects, upgrades.
- `Game.Progression`: attributes, Specialty trees, Personal trees, gear, respec/version rules.
- `Game.Synchronization`: handoffs, assists, rescues, chains, Limit resource and team-state rules.
- `Game.AI`: utility evaluation using the same surfaced rules available to the player.
- `Game.Narrative`: Ink bridge, story facts, relationship state, consequences, save migration.
- `Game.Presentation`: animation requests, cameras, VFX, audio, cut-ins, cinematics, post.
- `Game.UI`: input-independent view models and accessible forecast displays.
- `Game.Content`: versioned definitions and schemas.
- `Game.Editor`: validators, importers, preview scenes, balance tools, screenshot harness.
- `Game.Tests`: deterministic combat fixtures, property tests, content validation, golden images.

Gameplay logic must never depend on Timeline clips or animation completion to decide truth. Simulation resolves authoritative events; presentation consumes the event stream and may accelerate/skip safely.

## 16. Prototype order

### Gate 0 — identity and documentation lock

- finish canonical sheets for every current principal character;
- establish canonical artwork manifest and revision naming;
- settle working names or stable codenames;
- review this brief with Claude/Codex as adversarial consultants;
- decide only the smallest unresolved rules needed for the first prototype.

### Gate 1 — headless combat kernel

- grid, elevation, LOS, facing/cover subset;
- interleaved initiative and two AP;
- typed condition/effect grammar;
- one representative action from each Specialty family, not entire trees;
- action log and deterministic replay;
- tests for forced movement, reactions, status duration, and initiative mutation.

### Gate 2 — synergy prototype

Implement one authored chain:

1. Human manipulates or displaces enemies into a threatening cluster.
2. The cluster satisfies the synthetic's conditional support window.
3. Synthetic casts the rescue pulse on the surrounded cyborg: three-tile knockback where legal, haste, and temporary shock attacks.
4. Cyborg changes stance or intercepts, converting the opening into a handoff.
5. The sequence grants Synchronization because three distinct contributions resolved.

This single scenario should test conditions, forced movement, initiative, buffs, tags, forecast UI, animation requests, and shared resource gain.

### Gate 3 — character and mech controller spike

- canonical cyborg and mech in a representative small map;
- idle, move, attack, hit, reaction, stance transition, and disengage;
- camera and input at 60 fps;
- stepped acting without latency;
- animation cancel/skip rules that never corrupt simulation.

### Gate 4 — cinematic consistency spike

- one 10–20 second A-tier in-engine sequence;
- one 5–10 second S-tier-quality test shot or exceptionally polished hybrid;
- same character identity, palette, props, and environment across gameplay and both shots;
- frame-by-frame review and performance capture.

### Gate 5 — vertical slice

- prologue fragment demonstrating the original team's near-perfect coordination;
- collapse at the second Limit Break;
- one two-survivor main-timeline mission;
- one later three-character mission demonstrating the synergy prototype;
- a meaningful choice whose consequence changes tactical state.

## 17. Questions for serious Claude/Codex consultation

The consultant should not redesign the game's identity. It should pressure-test feasibility, expose contradictions, and propose bounded experiments.

### Combat and progression

1. What is the smallest deterministic ruleset that can validate three-unit tactical depth?
2. How should interleaved initiative and two AP interact with haste, delay, reactions, assists, and stance changes without producing infinite loops?
3. What command model best supports Remote Arsenal: shared AP, queued mech directives, or separate initiative with command bandwidth?
4. How can nine Specialty trees plus three Personal trees remain expressive but financially producible for animation, VFX, AI, UI, and balance?
5. Which effects need a generic typed grammar, and which deserve bespoke code?
6. How should conditional buffs be forecast so the player can intentionally create them rather than discover them accidentally?
7. How should bosses resist hacking/control without invalidating Ghost Surgery?
8. Which recovery model preserves consequence while avoiding healer dependence and death spirals?
9. What respec and gear model supports experimentation without dissolving character identity?

### Animation and art consistency

10. Is shared canonical 3D truth plus selective authored 2D the best cost/quality strategy for this team size and visual target?
11. What concrete Unity asset, rig, shader, Timeline, render, and validation pipeline minimizes character drift?
12. How should stepped character animation coexist with 60 fps camera, root motion, UI, VFX, and hit timing?
13. Which hair/clothing elements should be geometry, bones, cloth simulation, cards, or 2D accents for stable silhouettes?
14. What is a realistic shot-length and quality budget for S/A/B/C cinematics in an indie vertical slice?
15. Where can generative tools safely reduce labor, and where will they create unacceptable temporal/identity cleanup cost?
16. What automated golden-render and screenshot tests can catch shader, palette, silhouette, and costume regressions?

### Architecture and production risk

17. What should be proven in a two-week Unity technical-art spike before the engine choice becomes binding?
18. What content schemas make abilities, talents, conditions, gear, and synergy inspectable and agent-editable without turning the system into a generic effect engine monster?
19. What are the highest-risk assumptions in this brief, ranked by cost of learning late?
20. What should be explicitly cut from the first vertical slice while preserving the commercial fantasy?

## 18. Required consultant response format

Ask Claude or any external consultant to return:

1. **Executive verdict:** feasible thesis, primary risk, and recommended prototype focus.
2. **Contradictions found:** direct citations to sections of this brief.
3. **Risk register:** probability, impact, earliest cheap test, and decision deadline.
4. **Combat proposal:** minimal rules, initiative model, recovery model, mech-command model.
5. **Progression proposal:** tree structure, cross-tree limits, content/animation cost control.
6. **Visual pipeline:** canonical asset strategy, cinematic tiers, generative-tool boundaries, validation gates.
7. **Two-week spike:** daily/ordered deliverables and pass/fail criteria.
8. **Vertical-slice cuts:** what not to build yet.
9. **Open questions for Juan and Mina:** only decisions that materially alter implementation.

The consultant must label assumptions, distinguish settled canon from recommendations, and avoid replacing the project with a generic tactics-game template.

## 19. Immediate decisions still open

- working title and stable character codenames/names;
- final tactical verb names and Specialty names;
- exact initiative clock and haste/delay mathematics;
- exact AP/reaction economy;
- Remote Arsenal mech command model;
- exact Integrity/Guard/Wound recovery rules;
- tree size, level cadence, respec rules, and cross-tree access;
- gear slot count and loot economy;
- synthetic vulnerability/cost for Heartless;
- enemy families and how hacking/biohacking stays broadly useful;
- Unity spike target hardware and measurable frame budgets;
- exact 2D/3D balance for S-tier cinematics;
- platform scope beyond PC-first.

## 20. Current next objective

Do not begin by implementing three full talent trees per character. The immediate objective is to build and evaluate one small combat scenario that demonstrates the entire thesis:

- clear tactical forecast;
- each protagonist acting through her distinct attribute and verb;
- a conditional synthetic rescue/buff created intentionally by positioning;
- forced movement, haste, shock, stance choice, and recovery;
- a three-character handoff that generates Synchronization;
- responsive 60 fps presentation;
- character identity preserved at gameplay camera distance and in one close cinematic shot.

If this scenario is not fun, legible, visually faithful, and technically stable, more talents will only make the failure larger.
