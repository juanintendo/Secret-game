using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SecretGame.Simulation;

const int warmup = 1000;
const int samples = 10000;
for (var index = 0; index < warmup; index++)
{
    MeasureTurn();
    MeasureForecast();
}

var turns = Enumerable.Range(0, samples).Select(_ => MeasureTurn()).OrderBy(value => value).ToArray();
var forecasts = Enumerable.Range(0, samples).Select(_ => MeasureForecast()).OrderBy(value => value).ToArray();
Print("turn", turns, 8, 16);
Print("forecast", forecasts, 4, 16);
return Percentile(turns, 99) <= Milliseconds(8) && Percentile(forecasts, 99) <= Milliseconds(4) ? 0 : 1;

static long MeasureTurn()
{
    var state = CreateState(false);
    var resolver = new CombatResolver();
    var start = Stopwatch.GetTimestamp();
    resolver.Resolve(state, new BeginActivationCommand(new EntityId(1)));
    resolver.Resolve(state, Stack());
    resolver.Resolve(state, new EndActivationCommand(new EntityId(1)));
    return Stopwatch.GetTimestamp() - start;
}

static long MeasureForecast()
{
    var state = CreateState(true);
    var start = Stopwatch.GetTimestamp();
    new CombatForecast(new CombatResolver()).Evaluate(state, Stack());
    return Stopwatch.GetTimestamp() - start;
}

static CombatState CreateState(bool active)
{
    var source = new CombatEntity(new EntityId(1), new Cell(1, 1), new Footprint(1, 1), EntityFlags.Active, new IntegrityPool(10, 10));
    var target = new CombatEntity(new EntityId(2), new Cell(4, 1), new Footprint(1, 1), EntityFlags.Active, new IntegrityPool(20, 20))
        { Faction = Faction.Enemy };
    return new CombatState(new BattleMap(8, 5), CombatRules.SpikeDefault, new[] { source, target }, active ? source.Id : null);
}

static EffectStackCommand Stack() => new(new EntityId(1), 1, new CombatEffect[]
{
    new DamageEffect(new EntityId(2), 3),
    new ApplyConditionEffect(new EntityId(2), ConditionKind.Marked, 1)
});

static void Print(string name, long[] values, int passMs, int failMs)
{
    var p50 = ToMicroseconds(Percentile(values, 50));
    var p95 = ToMicroseconds(Percentile(values, 95));
    var p99 = ToMicroseconds(Percentile(values, 99));
    var max = ToMicroseconds(values[^1]);
    var status = p99 <= passMs * 1000 ? "PASS" : p99 > failMs * 1000 ? "FAIL" : "REVIEW";
    Console.WriteLine($"{name}: p50={p50}us p95={p95}us p99={p99}us max={max}us [{status}]");
}

static long Percentile(long[] values, int percentile) => values[(values.Length - 1) * percentile / 100];
static long Milliseconds(int value) => value * Stopwatch.Frequency / 1000;
static long ToMicroseconds(long ticks) => ticks * 1000000 / Stopwatch.Frequency;
