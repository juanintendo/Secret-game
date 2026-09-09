using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SecretGame.Simulation;

const int encounterCount = 10000;
const int activationsPerEncounter = 8;
var timer = Stopwatch.StartNew();
var commandCount = 0;
var eventCount = 0;

for (var encounter = 0; encounter < encounterCount; encounter++)
{
    var random = new DeterministicSequence((uint)encounter + 1);
    var initial = CreateState(encounter);
    var state = initial.Clone();
    var resolver = new CombatResolver();
    var forecast = new CombatForecast(resolver);
    var commands = new List<CombatCommand>();

    for (var activation = 0; activation < activationsPerEncounter; activation++)
    {
        var actorId = new InitiativeTimeline().Query(state, 1).Single().EntityId;
        Resolve(new BeginActivationCommand(actorId));
        for (var action = 0; action < 2; action++)
        {
            var actor = state.Entities[actorId];
            var targets = state.Entities.Values
                .Where(candidate => candidate.Faction != actor.Faction && candidate.Flags.Targetable)
                .OrderBy(candidate => candidate.Id)
                .ToArray();
            var target = targets[random.Next(targets.Length)];
            CombatCommand command;
            if (random.Next(4) == 0)
            {
                command = new ApplyConditionCommand(actorId, target.Id, ConditionKind.Marked, 1 + random.Next(2));
            }
            else
            {
                var effects = new List<CombatEffect> { new DamageEffect(target.Id, 1 + random.Next(4)) };
                if (random.Next(3) == 0)
                    effects.Add(new DisplaceEffect(target.Id, (CompassDirection)random.Next(8), 1 + random.Next(2), 1));
                var reactor = state.Entities.Values
                    .Where(candidate => candidate.Faction == target.Faction
                        && candidate.Id != target.Id
                        && candidate.ReactionCharges > 0)
                    .OrderBy(candidate => candidate.Id)
                    .FirstOrDefault();
                var reactions = reactor is not null && random.Next(2) == 0
                    ? new[]
                    {
                        new ReactionInvocation(
                            reactor.Id,
                            0,
                            "fuzz.reaction-shot",
                            new CombatEffect[] { new DamageEffect(actorId, 1) })
                    }
                    : Array.Empty<ReactionInvocation>();
                command = new EffectStackCommand(actorId, 1, effects, reactions);
            }
            Resolve(command);
        }
        Resolve(new EndActivationCommand(actorId));
    }

    var replay = new CombatReplay(new CombatResolver()).Run(CreateState(encounter), commands);
    Require(replay.FinalStateHash == state.DeterministicHash(), $"Replay hash diverged in encounter {encounter}.");
    Require(ReplayVerifier.FindFirstDivergence(state.Events, replay.Events) is null,
        $"Event stream diverged in encounter {encounter}.");

    void Resolve(CombatCommand command)
    {
        var predicted = forecast.Evaluate(state, command);
        Require(predicted.IsLegal, $"Generated illegal command in encounter {encounter}: {predicted.RejectionReason}");
        var result = resolver.Resolve(state, command);
        Require(predicted.ResultingStateHash == result.ResultingStateHash, $"Forecast hash diverged in encounter {encounter}.");
        Require(ReplayVerifier.FindFirstDivergence(predicted.Events, result.Events) is null,
            $"Forecast events diverged in encounter {encounter}.");
        Require(state.ActiveEntityId is null || state.Entities[state.ActiveEntityId.Value].ActionPoints is >= 0 and <= 2,
            $"AP invariant failed in encounter {encounter}.");
        var reactionEvents = result.Events.Select(item => item.Payload).OfType<ReactionTriggeredEvent>().ToArray();
        Require(reactionEvents.Length <= 2, $"Reaction cap failed in encounter {encounter}.");
        Require(reactionEvents.Select(item => item.ReactorId).Distinct().Count() == reactionEvents.Length,
            $"A unit reacted twice in encounter {encounter}.");
        commands.Add(command);
        commandCount++;
        eventCount += result.Events.Count;
    }
}

timer.Stop();
Console.WriteLine($"PASS {encounterCount} deterministic bounded encounters");
Console.WriteLine($"Commands: {commandCount}; events: {eventCount}; elapsed-ms: {timer.ElapsedMilliseconds}");
return 0;

static CombatState CreateState(int encounter)
{
    var offset = encounter % 2;
    var entities = new[]
    {
        Entity(1, 1, 1 + offset, Faction.Player, 0, 10),
        Entity(2, 1, 7, Faction.Player, 1, 11),
        Entity(101, 8, 1, Faction.Enemy, 0, 10),
        Entity(102, 8, 7 - offset, Faction.Enemy, 1, 12)
    };
    return new CombatState(new BattleMap(10, 10), CombatRules.SpikeDefault, entities);
}

static CombatEntity Entity(int id, int x, int y, Faction faction, int nextActAt, int interval) =>
    new(new EntityId((ulong)id), new Cell(x, y), new Footprint(1, 1), EntityFlags.Active, new IntegrityPool(40, 40), nextActAt, interval)
    { Faction = faction };

static void Require(bool condition, string message)
{
    if (!condition) throw new InvalidOperationException(message);
}

sealed class DeterministicSequence
{
    private uint _state;
    public DeterministicSequence(uint seed) => _state = seed == 0 ? 1u : seed;
    public int Next(int exclusiveMaximum)
    {
        _state ^= _state << 13;
        _state ^= _state >> 17;
        _state ^= _state << 5;
        return (int)(_state % (uint)exclusiveMaximum);
    }
}
