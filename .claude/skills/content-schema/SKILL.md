---
name: content-schema
description: "The data schema and authoring rules for this project's abilities, talents, conditions, statuses, gear, enemies, and synergy tags — plus the validator rules that keep content producible. Load before authoring, editing, or generating any ability, talent, Specialty node, gear item, enemy, or status definition, and before designing a new content type."
---

# Content Schema

Read `canon-guard` and `combat-kernel` first. This skill governs authored content.

## 0. The two rules that control the budget

**Rule 1 — An ability costs animation. A talent must not.**
A talent may change numbers, targeting shape, cost, tags, triggers, conditions, and VFX *parameters*. A talent may **never** require a new character animation clip. If it needs a new clip, it is not a talent — it is an ability, and it must be charged against the ability budget.

**Rule 2 — Every ability maps to an existing motion archetype.**
Each character has a small approved set of motion archetypes (~8). Abilities are `archetype + timing variant + VFX set + data row`. This is how 90s anime production works and it is the only way nine Specialties are producible.

The validator enforces both. A talent or ability referencing an unapproved `motionArchetype` fails CI.

## 1. Source of truth

- **JSON under `content/` is the source of truth.** ScriptableObjects are *generated mirrors*, never hand-authored.
- Everything is `id` + `version`. Ids are stable, kebab-case, namespaced: `syn.spellslinger.rescue-pulse`.
- Definitions are immutable at runtime. Mutable campaign state is a separate, versioned store.
- Every file validates against a JSON Schema in `content/schema/`. CI runs the validator on every change.

## 2. Ability

```jsonc
{
  "id": "syn.spellslinger.rescue-pulse",
  "version": 3,
  "owner": "synthetic",              // synthetic | cyborg | human | enemy:<family>
  "specialty": "spell-slinger",
  "tags": ["support", "displacement", "conductive", "signature"],

  "cost": { "ap": 2, "charges": 1, "cooldown": 2, "resource": null },

  "requires": {
    "mode": null,                     // cyborg only: piloted | remote | docked
    "conditions": [
      { "ref": "cond.surrounded", "params": { "min": 2, "radius": 1, "adjacency": 8 },
        "on": "target" }
    ]
  },

  "targeting": {
    "kind": "single",                 // single | tile | line | cone | burst | self
    "range": { "min": 1, "max": 6 },
    "shape": null,
    "losRequired": true,
    "validTargets": ["ally"],
    "facingRule": "any"
  },

  "effects": [                        // ordered; resolution order is the array order
    { "type": "Displace", "from": "targetTile", "distance": 3,
      "massRule": "standard", "collisionRule": "depth0",
      "appliesTo": "hostilesWithin:1" },
    { "type": "GrantGuard", "target": "target", "amount": "8 + intellect / 2" },
    { "type": "ApplyStatus", "target": "target", "statusId": "st.hasted", "duration": 1 },
    { "type": "ApplyStatus", "target": "target", "statusId": "st.shock-coating", "duration": 2 }
  ],

  "bossDegradation": {                // REQUIRED on any control/displacement ability
    "vsAnchored": [
      { "type": "ApplyStatus", "statusId": "st.staggered", "duration": 1 },
      { "type": "ApplyStatus", "statusId": "st.unbalanced", "duration": 1 }
    ]
  },

  "emits": {
    "eventTags": ["Rescued", "Displace"],
    "sync": { "amount": 1, "requiresCrossCharacter": true,
              "deniedWhenCause": ["SelfMove"] }
  },

  "reaction": null,                   // { trigger, window, limit } if it is a reaction

  "presentation": {
    "motionArchetype": "syn.cast-release",
    "vfxSet": "vfx.pulse.radial",
    "cameraHint": "commitZoom",
    "durationMs": 1200,
    "cutInId": "cut.syn.pulse"
  },

  "ai": { "intent": "protect", "scoreHints": ["allyIntegrityLow", "allySurrounded"] },

  "upgrades": []                      // ids of talents that may patch this ability
}
```

### Effect types (the whole typed grammar)

`Damage` · `ApplyStatus` · `RemoveStatus` · `Displace` · `Move` · `ModifyInitiative` · `GrantGuard` · `SpendResource` · `EmitTag` · `ConsumeTag` · `Link` (Conduction) · `Conditional { if, then, else }`

If a designer needs an effect type not on this list, that is a **kernel change** — escalate; do not write a bespoke script.

Numeric fields accept the sandboxed expression DSL (`"8 + intellect / 2"`). Integer/fixed-point only.

## 3. Talent — a declarative patch, never a script

```jsonc
{
  "id": "syn.spellslinger.t.long-conduction",
  "requires": { "focus": "spell-slinger", "tier": 2, "points": 1 },
  "patches": [
    { "target": "syn.spellslinger.rescue-pulse", "op": "set",
      "path": "targeting.range.max", "value": 9 },
    { "target": "syn.spellslinger.rescue-pulse", "op": "set",
      "path": "targeting.losRequired", "value": false,
      "onlyIf": { "ref": "cond.linked", "on": "target" } }
  ],
  "addsEffects": [],
  "addsTriggers": []
}
```

`op` ∈ `set` | `add` | `mul` | `append` | `replace` | `remove`.
Patches are applied deterministically in talent-id order at loadout-snapshot time, never during battle.

## 4. Condition and Status

```jsonc
// condition — a pure predicate, evaluated by the forecast and the AI identically
{ "id": "cond.surrounded", "params": ["min", "radius", "adjacency"],
  "evaluates": "hostileCountWithin(radius, adjacency) >= min",
  "tracksCause": true,
  "displayBadge": "SURROUNDED {n}/{min}" }

// status — lives on a unit with a duration
{ "id": "st.shock-coating", "durationUnit": "activations", "stacking": "refresh",
  "modifiers": [ { "onEvent": "unitAttackResolved",
                   "effect": { "type": "ApplyStatus", "statusId": "st.shocked",
                               "target": "attackTarget", "duration": 2 } } ],
  "icon": "st_shock", "colorId": "vfx.cyan-hot" }
```

**Every condition must be displayable as a badge.** A condition the player cannot see is not a condition; it is a trap. See the forecast requirements in `combat-kernel` §2 and §9.

## 5. Gear

Few, named, consequential. **No affixes. No random drops. No +1 upgrades.**

```jsonc
{ "id": "gear.cyb.siege-brace", "slot": "signature-weapon", "owner": "cyborg",
  "grants": [ { "abilityId": "cyb.remote.siege-shot" } ],
  "patches": [ { "target": "cyb.*", "op": "add", "path": "cost.ap", "value": 0 } ],
  "drawback": { "type": "ApplyStatus", "statusId": "st.braced-immobile",
                "while": "equipped-and-firing" },
  "silhouetteImpact": "approved:hardpoint-shoulder-L" }
```

Slots: `signature-weapon` · `frame-component` · `utility-module` · `character-signature` · `mech-hardpoint` (cyborg only).

**Identity-preservation rule (validator-enforced):** gear may modify a character's own tags and archetypes. Gear may **never** grant another character's tag-creation verb. Only the human creates `Isolated`; only the cyborg creates `Guarded`-for-others; only the synthetic creates `Hacked` / `Conductive`.

**Silhouette rule:** any gear with visible geometry must reference an approved silhouette variant. Unapproved visual gear fails CI. The mech is *repaired and incrementally upgraded*, never visually replaced.

## 6. Enemies — the real content axis

With only three protagonists, **tactical variance comes from the opposition and the map, not from the roster.** Enemy design is a first-class content system, not set dressing.

Every enemy family declares: `mass`, `firewall`, `subsystems[]`, `reactionProfile`, `intentDisplay`, and at least one of `Piercing` / `GuardBreak` / `Area` so that turtling is punished.

Every enemy must publish its **intent** before its activation (icon + threatened tiles). Hidden intent is the only acceptable source of uncertainty; dice are not.

## 7. Validator rules (CI must fail on any of these)

1. An ability references a `motionArchetype` not in its owner's approved archetype list.
2. A talent introduces a new `motionArchetype`, or adds an effect requiring one.
3. A control or displacement ability lacks `bossDegradation`.
4. A condition lacks a `displayBadge`.
5. Gear grants an ability owned by a different character, or a tag-creation verb it does not own.
6. Any numeric field uses a float literal.
7. An `id` collides, or an id changes without a `version` bump plus a migration entry.
8. An ability's `emits.sync` grants Sync without `requiresCrossCharacter`.
9. Total abilities for a character exceed the **ability budget** (see §8).
10. Any content file fails its JSON Schema.
11. A `Displace` effect omits `collisionRule`, or uses a depth > 0.
12. Visual gear references an unapproved silhouette variant.

## 8. Budgets — treat these as hard caps until Juan/Mina raise them

*Proposed, not accepted canon. Adjust once the Action Vocabulary Census (see the consult doc) produces real numbers.*

| Per character | Cap |
| --- | --- |
| Motion archetypes | 8 |
| Abilities total (3 Specialties + Personal) | 22 |
| — per Specialty | 6 (1 signature + 3 core + 2 capstone-tier) |
| — Personal | 4 |
| Talent nodes total | ~48 (12 per tree) |
| Named gear across the whole game | 5 |

**Structure:** a character picks one Specialty as **Focus** (unlocks its signature + capstone, grants an extra talent tier). Non-capstone talents from the other two trees cost double points. Hybrids exist; identity does not dissolve.

Talent respec is **free** between missions — experimentation is good and costs nothing. Changing **Focus** costs a scarce resource or a narrative beat, because Focus changes the ability set the player has learned.

## 9. Authoring checklist

Before committing any new content:

- [ ] Validates against schema; `npm`/`dotnet` validator passes.
- [ ] Uses an existing motion archetype.
- [ ] Every condition it reads has a badge; every condition it creates is forecastable.
- [ ] `bossDegradation` present if it controls or displaces.
- [ ] Cannot combine with existing content to violate a `combat-kernel` §13 invariant — write the fuzz-test case if unsure.
- [ ] Sync only from cross-character tag consumption.
- [ ] Has an AI `intent` so the enemy AI and the player see the same rules.
- [ ] Reads as *this character*, not as a generic tactics ability.
