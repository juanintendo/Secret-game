using System.Linq;
using NUnit.Framework;
using SecretGame.Simulation;
using SecretGame.VisualSpike.Editor;
using UnityEditor;
using UnityEngine;

namespace SecretGame.VisualSpike.Tests;

public sealed class VisualSpikeTests
{
    [Test] public void FixtureUsesTwoCellPathAndStableDockedIdentities()
    {
        var replay=new VisualReplay();
        var movement=replay.Steps.SelectMany(x=>x.Events).Select(x=>x.Payload).OfType<EntityMovedEvent>().Single();
        Assert.That(movement.Path.StepCount,Is.EqualTo(2));
        var snapshot=replay.SnapshotAt(6);
        Assert.That(snapshot.Count,Is.EqualTo(3));
        Assert.That(snapshot.Single(x=>x.Id==VisualReplay.Pilot).Flags,Is.EqualTo(EntityFlags.Docked));
        Assert.That(snapshot.Single(x=>x.Id==VisualReplay.Mech).Footprint,Is.EqualTo(new Footprint(2,2)));
        Assert.That(snapshot.Single(x=>x.Id==VisualReplay.Pilot).Flags.Spatial,Is.False);
    }
    [Test] public void ForecastEventsAndHashEqualResolvedAttack()
    {
        var replay=new VisualReplay();
        var attack=replay.Steps.Single(x=>x.Time==VisualReplay.AttackTime);
        Assert.That(attack.Events,Is.EqualTo(replay.AttackForecast.Events));
        Assert.That(attack.Hash,Is.EqualTo(replay.AttackForecast.ResultingStateHash));
        Assert.That(attack.Snapshot.Single(x=>x.Id==VisualReplay.Enemy).Integrity.Current,Is.EqualTo(13));
    }
    [Test] public void ScalePresetAndPresentationRemovalCannotChangeReplay()
    {
        VisualSpikeBuilder.Build();
        UnityEditor.SceneManagement.EditorSceneManager.OpenScene(VisualSpikeBuilder.Scene);
        var player=Object.FindFirstObjectByType<VisualSpikePlayer>();
        Assert.That(player, Is.Not.Null, "Saved scene must retain the executable MonoBehaviour.");
        player.Initialize();
        var baseline=new VisualReplay();
        var events=baseline.Steps.SelectMany(x=>x.Events).ToArray();
        foreach(float scale in new[]{1f,1.25f,1.5f})
        foreach(bool low in new[]{false,true})
        {
            player.cellSizeMeters=scale; player.ApplyPreset(low);
            foreach(float time in new[]{0,2.625f,4,5,6,7,8.01f,10})
                player.Sample(time);
            Assert.That(player.Replay.FinalHash,Is.EqualTo(baseline.FinalHash));
            Assert.That(player.Replay.Steps.SelectMany(x=>x.Events).ToArray(),Is.EqualTo(events));
            player.Sample(5);
            Assert.That(player.pilot.gameObject.activeSelf,Is.True);
            Assert.That(player.occupancy[0].enabled,Is.False,"Docked pilot is already non-spatial during visual boarding.");
            player.Sample(6);
            Assert.That(player.pilot.gameObject.activeSelf,Is.False);
            player.Sample(7);
            Assert.That(player.forecastLine.enabled,Is.True);
            player.Sample(8);
            Assert.That(player.HitText,Does.Contain("7"),"Hit confirmation must not wait for stepped posing.");
        }
        player.ApplyPreset(false);
        Object.DestroyImmediate(player.pilot.gameObject);
        Object.DestroyImmediate(player.mech.gameObject);
        Object.DestroyImmediate(player.enemy.gameObject);
        Object.DestroyImmediate(player.gameObject);
        Assert.That(new VisualReplay().FinalHash,Is.EqualTo(baseline.FinalHash));
    }
    [Test] public void ShadersImportWithoutErrors()
    {
        Assert.That(ShaderUtil.ShaderHasError(Shader.Find("SecretGame/VisualSpike/Toon")),Is.False);
        Assert.That(ShaderUtil.ShaderHasError(Shader.Find("SecretGame/VisualSpike/Outline")),Is.False);
    }
}
