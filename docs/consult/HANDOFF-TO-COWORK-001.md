# Handoff to Cowork — 90s Anime × Tactical RPG ("Secret-game")

Purpose of this file: a self-contained briefing so a **Cowork** session can pick up the consulting work started in Claude Code, without re-reading the original source docs from scratch. Paste/attach this file first in Cowork.

---

## 1. What this project is

A premium story-driven tactical RPG about three women rebuilding a destroyed elite combat unit. No permanent leader among the three. Each is already individually capable — **the unit is level one, not the characters.** 1990s techno-anime visual language at modern fidelity. Combat legible before commitment; synchronization/teamwork is the core progression axis, not raw damage.

**Repo:** https://github.com/juanintendo/Secret-game (was empty; now scaffolded locally at `C:\Users\colom\Desktop\Secret-game`, committed to `main`, **not yet pushed**).

## 2. Source documents (authority order — never average conflicts, cite them)

1. `docs/canon/STORY-CANON-001.md` — narrative truth, chronology, secrets, character knowledge.
2. `docs/canon/90S-ANIME-XCOM-PREPRODUCTION-001.md` — founding product/combat/visual/production direction.
3. `docs/canon/90S-ANIME-XCOM-CODEX-CLAUDE-BRIEF-001.md` — Specialties, Personal talents, recovery philosophy, cinematic-consistency pipeline, consulting questions.
4. `docs/consult/CONSULT-CLAUDE-001.md` — my full analysis. **Recommendations only, not canon**, until Juan/Mina accept them.

All four exist in the repo already. Bring them into Cowork as attachments/context if Cowork cannot read the repo directly.

## 3. Characters (fixed trio, quick reference)

- **White-haired human** — Agility — verb *Exploit/Redirect*. Only fully human member of the definitive trio. Recruited later for a concealed, specific reason (Operator requires *her*, not an equivalent). Faye Valentine / Spike Spiegel energy — irreverent, elusive, self-interested. Specialties: **Dispatch** (melee assassin), **Con Girl** (charisma CC, DPS tradeoff), **Gunslinger** (mobile ranged).
- **Cyborg mech pilot** — Strength — verb *Break/Anchor*. Survivor of the original team. Only protagonist with a mech (repaired/upgraded, never replaced). Specialties: **Bulwark** (tank/protection), **Remote Arsenal** (pilot heavy-ranged + remote mech tank), **Redline** (mech stance-dance: Assault/Pursuit/Disengage).
- **Luminous synthetic** — Intellect — verb *Rewrite/Conduct*. Survivor of the original team, fully synthetic, literal luminous skeleton (not an X-ray effect). Trauma around agency/remote access. Specialties: **Heartless** (invasive lightning damage), **Ghost Surgery** (hacking/biohacking CC), **Spell Slinger** (conditional battlefield buffs). **She is not a healer** — support is earned/conditional only.
- **Original green-haired leader** (prologue) — fully human, AI copilot *advises*, she retains final authority; defined by judgment over machine-optimal recommendations. Survives the tutorial disaster in some form and becomes the unknown protective intruder (authorial truth, not player-known).
- **Operator** — dry, composed, "GLaDOS-like" in function not voice. Physically present at HQ in the prologue; a hologram in the definitive era. Apparently dies at HQ during the tutorial collapse; restored/continues later.

## 4. Story spine (settled)

Prologue: original trio near-peak power, spectacular first Limit Break, a prepared intrusion exploits the **second** Limit Break, Operator apparently dies at HQ (separate location from the field site, same coordinated attack), the leader overrides her AI's machine-optimal call, creates an escape path, presumed dead (no body). Cyborg + synthetic survive, go underground separately for years. Operator returns, recontacts each individually, maneuvers them toward the white-haired recruit. Eight core mysteries must share causal machinery, not resolve as unrelated twists.

## 5. Combat chassis (provisional, to prototype)

Grid + discrete elevation, interleaved initiative, 2 AP/activation, facing/flank/LOS/cover, reactions/assists/rescues with explicit triggers, deterministic damage with surfaced uncertainty, objectives beyond elimination, seeded replayable sim.

## 6. What I already delivered in Claude Code (this session)

- **Scaffolded the repo**: `docs/canon/` (3 source docs + 2 approved art refs), `docs/consult/`, `docs/pipeline/`, `CLAUDE.md`, `README.md`, `.gitignore`. Committed on `main`, not pushed.
- **Six project skills** in `.claude/skills/` (load these into Cowork's context too if useful — they encode working rules, not just facts):
  - `canon-guard` — authority order, binding canon, open questions, character cards, **3 unresolved art-vs-text conflicts** (see §7).
  - `combat-kernel` — determinism rules, tick-clock anti-loop invariants, forecast-parity rule, module boundaries.
  - `content-schema` — full JSON schema for abilities/talents/gear/enemies, validator rules, budget caps.
  - `visual-gate` — anti-slop gate, hair/cloth rigging assignment table, stepped-cadence-at-60fps technique, generative-tool boundaries, automated test list.
  - `unity-pipeline` — asmdef map, sim-presentation contract, perf budgets.
  - `adversarial-review` — structured review mode with fixed output format.
- **Full 15-section consultation** at `docs/consult/CONSULT-CLAUDE-001.md` (~14k words). This is the actual deliverable — read it in full before continuing work in Cowork; do not re-derive it.

## 7. Headline findings from the consultation (do not re-litigate, build on these)

**Verdict:** thesis is coherent; production plan is not yet.

**Top risk:** authored animation volume across 9 Specialties + 3 Personal trees. Fix: *an ability costs animation, a talent must not* — abilities map to ~8 motion archetypes per character; talents only patch data fields, never require new clips.

**Second risk:** with only 3 protagonists, tactical variance must come from enemy/map design, not roster — and enemy design is currently unspecified (appears once, as an open item).

**Prototype-first priority:** the forecast UI, not the combat rules. The whole "Spell Slinger" conditional-support thesis depends on players being able to *see, pre-commitment*, that a move will create `SURROUNDED` and enable a rescue ability. Testable in text/ASCII in about a week, no art needed.

**Three unresolved art-vs-text conflicts** (need Juan/Mina rulings, not further creative work until decided):
1. White-haired human's costume — text says "clean tactical outfit, no production-hostile trim"; hero art shows garter straps/open jacket/heeled boots (expensive to rig, contradicts her being the most-animated character).
2. Synthetic's red bomber jacket — canon in text, absent in current hero art. If jacket-only, she becomes the cheapest character to produce (rest is shader).
3. Cyborg's arms — text says symmetric mechanical forearms; art shows asymmetric (full right arm + left forearm). Changes whether her signature counter is one clip or a mirrored system.

**Sequencing contradiction:** `PREPRODUCTION` §13 wants the prologue built first; `CODEX-BRIEF` §20 wants one small combat scenario first. Prologue requires 2 extra canonical characters + a costume state change + a working Limit Break system before anything is proven — recommend scenario-first.

**Four decisions block the kernel** and are cheap to resolve (an afternoon, not research):
- To-hit rolls: yes/no? (recommend: no, deterministic damage, uncertainty lives in enemy intent/hidden info instead)
- Is "Charisma" (used for Con Girl) a real attribute, or does Con Girl scale off Agility/Resolve? (not currently in the stat list)
- Is "Integrity" the attribute (structural durability) or the health pool — currently used as both?
- Prologue-first or scenario-first?

Full detail — including the Remote Arsenal command-model comparison (recommend: queued directives), the Guard/Integrity/Faults recovery model, the day-by-day two-week Unity spike, vertical-slice cuts, and the fully worked "Relay Yard" test encounter — is all in `CONSULT-CLAUDE-001.md`. That document already answers all 25 of the brief's consulting questions in the required format; use it as the working baseline in Cowork rather than starting over.

## 8. What to do in Cowork

1. Load/attach this handoff file plus `CONSULT-CLAUDE-001.md` (and the 3 canon docs if Cowork needs them separately).
2. Get Juan + Mina rulings on the 4 kernel-blocking questions and the 3 art conflicts (§7) — these are quick decisions, not design work.
3. Continue iterating on open items from the consult's §15 "Questions for Juan and Mina" and the two-week spike plan.
4. If Cowork writes new decisions/canon, append them back into `docs/canon/` or a new versioned doc (`-002`), keeping the authority order intact — never overwrite `-001` files.

## 9. Repo state note

The `Secret-game` GitHub repo is still empty remotely — everything above is committed locally on `main` but not pushed. Also: the canon docs were copied into the repo from `~/Documents/DD90s/` and were byte-identical at copy time — pick one as the single source of truth going forward so they do not silently diverge.
