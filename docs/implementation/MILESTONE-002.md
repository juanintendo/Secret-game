# Milestone 002 — command, event, replay and forecast contract

## Purpose

Establish one authoritative path from a validated command to an ordered event trace. Forecast runs that same path against a cloned state; replay runs an ordered command list from an initial state. Presentation, AI and future tests can consume the result without duplicating combat formulas.

## Implemented

- Typed commands for movement, deterministic damage and pilot/mech deployment mode.
- Pre-mutation command validation with explicit rejection reasons.
- Typed movement, Integrity damage and deployment events.
- A single resolver that validates a complete command before applying any event.
- A monotonically ordered event log with a deterministic state hash after every event.
- Speculative forecast over a cloned state; the authoritative state remains untouched.
- Replay from an initial state and ordered command list.
- First-divergence reporting by resolved event index.
- Multi-cell occupancy rejection for rectangular footprints.

## Evidence

The executable harness covers 13 named properties, including exact forecast/result parity across 500 state-command pairs, deterministic replay across deployment transitions, illegal-command safety, and a provisional aggregate forecast budget of 2,000 ms for 500 evaluations.

## Boundary

Movement range, map bounds, terrain, line of sight, cover, initiative, reactions, conditions, displacement, Sync and presentation are deliberately absent. The current movement command proves occupancy and event plumbing; it is not yet the game's movement rule.

## Gate toward Unity/Astra

This milestone proves the forecast contract but does not close headless week 1. Unity work begins only after the next headless milestones add the representative grid rules, queryable initiative, condition-window forecast output and a text-mode encounter whose replay remains deterministic.
