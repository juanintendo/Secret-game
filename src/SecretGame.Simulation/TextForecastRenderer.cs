using System.Text;

namespace SecretGame.Simulation;

public sealed class TextForecastRenderer
{
    public string RenderState(CombatState state)
    {
        var occupied = new Dictionary<Cell, char>();
        foreach (var entity in state.Entities.Values.Where(entity => entity.Flags.Spatial))
        {
            var marker = Marker(entity.Id);
            foreach (var cell in entity.Footprint.OccupiedCells(entity.Anchor)) occupied[cell] = marker;
        }

        var text = new StringBuilder();
        for (var y = 0; y < state.Map.Height; y++)
        {
            for (var x = 0; x < state.Map.Width; x++)
            {
                var cell = new Cell(x, y);
                text.Append(occupied.TryGetValue(cell, out var marker)
                    ? marker
                    : state.Map.GetTerrain(cell).Blocked ? '#' : '.');
            }
            text.AppendLine();
        }
        return text.ToString();
    }

    public string RenderForecast(ForecastResult forecast)
    {
        if (!forecast.IsLegal) return $"ILLEGAL: {forecast.RejectionReason}";
        var text = new StringBuilder();
        foreach (var resolved in forecast.Events)
            text.AppendLine(RenderEvent(resolved.Payload));
        AppendSignals(text, "OPENS", forecast.ConditionsOpened);
        AppendSignals(text, "CLOSES", forecast.ConditionsClosed);
        text.Append("RESULT HASH ").Append(forecast.ResultingStateHash[..12]);
        return text.ToString();
    }

    private static string RenderEvent(CombatEvent payload) => payload switch
    {
        ActionPointsSpentEvent spent =>
            $"AP {RelayYardScenario.NameOf(spent.EntityId)}: {spent.Before} -> {spent.After}",
        ActionPointsRefreshedEvent refreshed =>
            $"ACTIVATE {RelayYardScenario.NameOf(refreshed.EntityId)}: AP {refreshed.Before} -> {refreshed.After}, next {refreshed.NewNextActAt}",
        ActivationEndedEvent ended =>
            $"END {RelayYardScenario.NameOf(ended.EntityId)}",
        EntityMovedEvent moved =>
            $"MOVE {RelayYardScenario.NameOf(moved.EntityId)}: {moved.From} -> {moved.To} ({moved.Path.StepCount} steps)",
        EntityDisplacedEvent displaced =>
            $"DISPLACE {RelayYardScenario.NameOf(displaced.TargetId)}: {displaced.From} -> {displaced.To} ({displaced.Path.StepCount} steps, {displaced.Stop})",
        IntegrityDamagedEvent damaged =>
            $"DAMAGE {RelayYardScenario.NameOf(damaged.TargetId)}: {damaged.Before} -> {damaged.After}",
        ConditionAppliedEvent condition =>
            $"STATUS {RelayYardScenario.NameOf(condition.TargetId)} gains {condition.Kind} ({condition.Duration})",
        ConditionsAdvancedEvent advanced =>
            $"STATUS TICK {RelayYardScenario.NameOf(advanced.EntityId)}: {advanced.Before.Items.Count} -> {advanced.After.Items.Count}",
        ReactionChargeRefreshedEvent refreshed =>
            $"REACTION {RelayYardScenario.NameOf(refreshed.EntityId)}: {refreshed.Before} -> {refreshed.After}",
        ReactionChargeSpentEvent spent =>
            $"REACTION SPENT {RelayYardScenario.NameOf(spent.EntityId)}: {spent.Before} -> {spent.After}",
        ReactionTriggeredEvent triggered =>
            $"TRIGGER {triggered.ReactionId} by {RelayYardScenario.NameOf(triggered.ReactorId)} after effect {triggered.TriggerEffectIndex}",
        DeploymentModeChangedEvent deployment =>
            $"MODE {RelayYardScenario.NameOf(deployment.PilotId)} + {RelayYardScenario.NameOf(deployment.MechId)}: {deployment.Mode}",
        _ => payload.GetType().Name
    };

    private static void AppendSignals(
        StringBuilder text,
        string label,
        IReadOnlyList<ConditionSignal> signals)
    {
        if (signals.Count == 0) return;
        text.Append(label).Append(' ')
            .AppendJoin(", ", signals.Select(signal =>
                $"{RelayYardScenario.NameOf(signal.EntityId)}:{signal.Kind}"))
            .AppendLine();
    }

    private static char Marker(EntityId id)
    {
        if (id == RelayYardScenario.Human) return 'H';
        if (id == RelayYardScenario.Cyborg) return 'C';
        if (id == RelayYardScenario.Mech) return 'M';
        if (id == RelayYardScenario.Synthetic) return 'S';
        return 'E';
    }
}
