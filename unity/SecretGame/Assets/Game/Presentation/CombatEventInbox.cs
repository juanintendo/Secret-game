using System;
using System.Collections.Generic;
using SecretGame.Simulation;
using UnityEngine;

namespace SecretGame.Presentation;

/// <summary>
/// Presentation-side entry point. It accepts resolved events and has no access to authoritative state.
/// Playback timing and visuals belong downstream of this inbox.
/// </summary>
public sealed class CombatEventInbox : MonoBehaviour
{
    private readonly Queue<ResolvedEvent> _pending = new();

    public event Action<ResolvedEvent>? EventDequeued;

    public int PendingCount => _pending.Count;

    public void Accept(IReadOnlyList<ResolvedEvent> events)
    {
        if (events is null) throw new ArgumentNullException(nameof(events));
        foreach (var resolvedEvent in events) _pending.Enqueue(resolvedEvent);
    }

    public bool TryDequeue()
    {
        if (_pending.Count == 0) return false;
        EventDequeued?.Invoke(_pending.Dequeue());
        return true;
    }
}
