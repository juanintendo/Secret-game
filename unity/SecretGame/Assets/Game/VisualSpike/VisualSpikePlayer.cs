#nullable disable // Unity serializes these references; the builder initializes them before playback.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SecretGame.Simulation;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SecretGame.VisualSpike
{

public sealed class VisualSpikePlayer : MonoBehaviour
{
    public Transform arena;
    public Transform pilot, mech, enemy, pilotLeftArm, pilotRightArm, mechRightArm, hatch;
    public Camera tacticalCamera;
    public Light key;
    public UniversalRenderPipelineAsset high, low;
    public Material outline;
    public TextMesh feedback;
    public LineRenderer[] occupancy;
    public LineRenderer forecastLine, impact;
    public float cellSizeMeters = 1.25f;
    public bool lowPreset, autoplay = true;
    public float playhead;
    public string sourceCommit;
    public string sourceState;
    public VisualReplay Replay { get; private set; }
    public string CameraId { get; private set; } = "combat-wide";
    public string HitText { get; private set; } = "";
    private readonly FrameTiming[] timings = new FrameTiming[1];
    private readonly List<float> frameMs = new(), cpuMs = new(), gpuMs = new();
    private float runTime;
    private bool profiling, exported, initialized;
    private ulong lastTimestamp;
    private MaterialPropertyBlock block;
    private Renderer[] enemyRenderers;
    private Renderer[] pilotOutlines, mechOutlines;
    private MaterialPropertyBlock selectedInk, regularInk;
    private static readonly int Emission = Shader.PropertyToID("_EmissionColor");
    public Vector3 World(Cell cell, Footprint footprint) =>
        new((cell.X + footprint.Width * .5f) * cellSizeMeters, 0,
            (cell.Y + footprint.Height * .5f) * cellSizeMeters);

    public void Initialize()
    {
        if (initialized) return;
        Replay = new VisualReplay();
        block = new MaterialPropertyBlock();
        enemyRenderers = enemy.GetComponentsInChildren<Renderer>();
        pilotOutlines = pilot.GetComponentsInChildren<Renderer>().Where(x => x.sharedMaterial == outline).ToArray();
        mechOutlines = mech.GetComponentsInChildren<Renderer>().Where(x => x.sharedMaterial == outline).ToArray();
        selectedInk = new MaterialPropertyBlock();
        regularInk = new MaterialPropertyBlock();
        selectedInk.SetColor("_Color", new Color(.08f,.75f,.88f));
        initialized = true;
        ApplyPreset(lowPreset);
    }
    private void Start()
    {
        Initialize();
        Application.targetFrameRate = 60;
        Application.runInBackground = true;
        QualitySettings.vSyncCount = 0;
        var args = Environment.GetCommandLineArgs();
        profiling = Array.IndexOf(args, "--visual-profile") >= 0;
        if (Array.IndexOf(args, "--visual-low") >= 0) ApplyPreset(true);
        Sample(0);
    }
    public void ApplyPreset(bool useLow)
    {
        lowPreset = useLow;
        GraphicsSettings.defaultRenderPipeline = useLow ? low : high;
        QualitySettings.renderPipeline = useLow ? low : high;
        key.shadows = useLow ? LightShadows.Hard : LightShadows.Soft;
        outline.SetFloat("_Width", useLow ? 1.65f : 1.35f);
    }
    private void Update()
    {
        if (!initialized) return;
        if (autoplay) Sample((playhead + Time.unscaledDeltaTime) % VisualReplay.Duration);
        if (!profiling || exported) return;
        runTime += Time.unscaledDeltaTime;
        FrameTimingManager.CaptureFrameTimings();
        if (runTime >= 10)
        {
            frameMs.Add(Time.unscaledDeltaTime * 1000);
            if (FrameTimingManager.GetLatestTimings(1, timings) > 0 &&
                timings[0].frameStartTimestamp != lastTimestamp)
            {
                lastTimestamp = timings[0].frameStartTimestamp;
                if (timings[0].cpuFrameTime > 0) cpuMs.Add((float)timings[0].cpuFrameTime);
                if (timings[0].gpuFrameTime > 0) gpuMs.Add((float)timings[0].gpuFrameTime);
            }
        }
        if (runTime >= 40)
        {
            ExportProfile();
            exported = true;
            Application.Quit();
        }
    }
    public void Sample(float time)
    {
        Initialize();
        playhead = Mathf.Clamp(time, 0, VisualReplay.Duration);
        arena.localScale = new Vector3(cellSizeMeters/1.25f,1,cellSizeMeters/1.25f);
        var snapshot = Replay.SnapshotAt(playhead);
        var p = snapshot.Single(e => e.Id == VisualReplay.Pilot);
        var m = snapshot.Single(e => e.Id == VisualReplay.Mech);
        var e = snapshot.Single(e => e.Id == VisualReplay.Enemy);
        pilot.position = World(p.Anchor, p.Footprint);
        mech.position = World(m.Anchor, m.Footprint);
        enemy.position = World(e.Anchor, e.Footprint);
        pilot.rotation = Quaternion.identity;
        mech.rotation = Quaternion.identity;
        enemy.rotation = Quaternion.identity;
        // Interpolate only the path supplied by the resolved movement event.
        foreach (var step in Replay.Steps)
        foreach (var resolved in step.Events)
            if (resolved.Payload is EntityMovedEvent moved && playhead >= step.Time && playhead < step.Time + 1.25f)
            {
                var cells = moved.Path.Cells;
                float f = (playhead - step.Time) / 1.25f * (cells.Count - 1);
                int i = Mathf.Min(Mathf.FloorToInt(f), cells.Count - 2);
                pilot.position = Vector3.Lerp(World(cells[i], p.Footprint), World(cells[i + 1], p.Footprint), f - i);
            }
        float dock = Mathf.Clamp01((playhead - VisualReplay.DockTime) / 2);
        // This is an explicitly provisional rear/top boarding trajectory, not hatch canon.
        if (dock > 0 && dock < 1)
        {
            var start = World(p.Anchor, p.Footprint);
            var rear = mech.position + new Vector3(.85f, 0, -.8f);
            var top = mech.position + new Vector3(.85f, 3.2f, -.6f);
            var seat = mech.position + new Vector3(0, 1.65f, -.1f);
            pilot.position = dock < .3f ? Vector3.Lerp(start, rear, Smooth(dock / .3f)) :
                dock < .72f ? Vector3.Lerp(rear, top, Smooth((dock-.3f)/.42f)) :
                Vector3.Lerp(top, seat, Smooth((dock-.72f)/.28f));
        }
        pilot.gameObject.SetActive(p.Flags.Spatial || (playhead >= VisualReplay.DockTime && dock < 1));
        hatch.localRotation = Quaternion.Euler(-75 * Mathf.Sin(dock * Mathf.PI), 0, 0);
        var poseTime = Mathf.Floor(playhead * 12) / 12;
        float idle = Mathf.Sin(poseTime * 2.2f) * 2;
        pilotLeftArm.localRotation = Quaternion.Euler(idle, 0, dock > 0 && dock < 1 ? -55 : -5);
        pilotRightArm.localRotation = Quaternion.Euler(-idle, 0, dock > 0 && dock < 1 ? 55 : 5);
        float hitAge = playhead - VisualReplay.AttackTime;
        if (playhead >= 7)
        {
            // Face the already-resolved target; this does not choose or validate a target.
            var facing = Quaternion.LookRotation(enemy.position - mech.position);
            mech.rotation = Quaternion.Slerp(Quaternion.identity, facing, Smooth(Mathf.Clamp01(playhead - 7)));
        }
        float recoil = hitAge >= 0 && hitAge < .5f ? Mathf.Sin(Mathf.Floor(hitAge * 12) / 12 * Mathf.PI / .5f) : 0;
        mechRightArm.localRotation = Quaternion.Euler(-55 * recoil, 0, -20 * recoil);
        enemy.localRotation = Quaternion.Euler(0, 0, -14 * recoil);
        HitText = "";
        bool hit = hitAge >= 0 && hitAge < .5f;
        foreach (var step in Replay.Steps)
        foreach (var resolved in step.Events)
            if (resolved.Payload is IntegrityDamagedEvent damage && hit)
                HitText = damage.Amount + "  /  INTEGRITY " + damage.After;
        block.SetColor(Emission, hit ? new Color(.8f,.25f,.04f) : Color.black);
        foreach (var renderer in enemyRenderers) renderer.SetPropertyBlock(block);
        impact.enabled = hit;
        impact.SetPosition(0, enemy.position + new Vector3(-.35f,1.1f,0));
        impact.SetPosition(1, enemy.position + new Vector3(.35f,1.8f,0));
        int index = 0;
        foreach (var entity in snapshot)
        {
            var line = occupancy[index++];
            line.enabled = entity.Flags.Spatial;
            var center = World(entity.Anchor, entity.Footprint);
            float x = entity.Footprint.Width * cellSizeMeters / 2, z = entity.Footprint.Height * cellSizeMeters / 2;
            line.SetPositions(new[] { center + new Vector3(-x,.035f,-z), center + new Vector3(x,.035f,-z),
                center + new Vector3(x,.035f,z), center + new Vector3(-x,.035f,z), center + new Vector3(-x,.035f,-z) });
        }
        forecastLine.enabled = playhead >= 6 && playhead < VisualReplay.AttackTime && Replay.AttackForecast.IsLegal;
        forecastLine.SetPosition(0, mech.position + Vector3.up * .08f);
        forecastLine.SetPosition(1, enemy.position + Vector3.up * .08f);
        SecretGame.Simulation.EntityId? selected = null;
        foreach (var step in Replay.Steps)
        {
            if (step.Time > playhead) break;
            foreach (var resolved in step.Events)
            {
                if (resolved.Payload is ActionPointsRefreshedEvent activation) selected = activation.EntityId;
                if (resolved.Payload is ActivationEndedEvent) selected = null;
            }
        }
        foreach (var renderer in pilotOutlines) renderer.SetPropertyBlock(selected == VisualReplay.Pilot ? selectedInk : regularInk);
        foreach (var renderer in mechOutlines) renderer.SetPropertyBlock(selected == VisualReplay.Mech ? selectedInk : regularInk);
        feedback.text = hit ? HitText : forecastLine.enabled
            ? "FORECAST " + Replay.AttackForecast.Events.Select(x => x.Payload).OfType<IntegrityDamagedEvent>().First().Amount + " INTEGRITY"
            : "";
        feedback.transform.position = enemy.position + Vector3.up * 2.15f;
        SetCamera("combat-wide");
    }
    private static float Smooth(float value) => value * value * (3 - 2 * value);
    public void SetCamera(string id)
    {
        CameraId = id;
        tacticalCamera.orthographic = true;
        if (id.StartsWith("cyborg-") || id.StartsWith("mech-"))
        {
            bool cyborg = id.StartsWith("cyborg-");
            var root = cyborg ? pilot : mech;
            var center = root.position + Vector3.up * (cyborg ? .875f : 1.8f);
            var direction = id.EndsWith("front") ? Vector3.forward :
                id.EndsWith("back") ? Vector3.back : Vector3.right;
            tacticalCamera.transform.position = center + direction * 10;
            tacticalCamera.transform.LookAt(center);
            tacticalCamera.orthographicSize = cyborg ? 1.04f : 2.1f;
            tacticalCamera.cullingMask = 1 << (cyborg ? 8 : 9);
        }
        else
        {
            tacticalCamera.cullingMask = ~0;
            var center = new Vector3(4 * cellSizeMeters, .4f, 3 * cellSizeMeters);
            tacticalCamera.transform.position = center + new Vector3(11, 15, 10);
            tacticalCamera.transform.LookAt(center);
            float push = playhead >= 8 && playhead < 9 ? Mathf.Sin((playhead-8)*Mathf.PI) * .28f : 0;
            tacticalCamera.orthographicSize = 5.3f * cellSizeMeters / 1.25f - push;
        }
        feedback.transform.rotation = tacticalCamera.transform.rotation;
    }
    private void OnGUI()
    {
        GUI.color = Color.white;
        GUILayout.BeginArea(new Rect(20,20,420,200), GUI.skin.box);
        GUILayout.Label("RELAY YARD  /  VISUAL SPIKE 001");
        GUILayout.Label("CONSTRUCTION EXPERIMENT  /  " + (lowPreset ? "LOW" : "HIGH"));
        GUILayout.Label("Cyborg 200  /  Mech 210  /  Enemy 300");
        if (forecastLine.enabled)
        {
            var damage = Replay.AttackForecast.Events.Select(x => x.Payload).OfType<IntegrityDamagedEvent>().First();
            GUILayout.Label("FORECAST: " + damage.Amount + " integrity   /   target " + damage.TargetId.Value);
        }
        GUILayout.Label(HitText);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(autoplay ? "Pause" : "Play")) autoplay = !autoplay;
        if (GUILayout.Button("Restart")) Sample(0);
        if (GUILayout.Button(lowPreset ? "High" : "Low")) ApplyPreset(!lowPreset);
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }
    [Serializable] private sealed class Profile
    {
        public string commit, sourceState, unity, scene, camera, preset, hardware, gpu, replayHash;
        public int width, height, samples, cpuSamples, gpuSamples, missedBudget;
        public float frameP95, frameP99, cpuP95, gpuP95;
        public string status;
    }
    private static float Percentile(List<float> values, float fraction)
    {
        if (values.Count == 0) return -1;
        var sorted = values.OrderBy(x => x).ToArray();
        return sorted[Mathf.Clamp(Mathf.CeilToInt(sorted.Length * fraction)-1, 0, sorted.Length-1)];
    }
    private void ExportProfile()
    {
        var report = new Profile { commit = sourceCommit, sourceState = sourceState, unity = Application.unityVersion,
            scene = "VisualSpike001", camera = CameraId, preset = lowPreset ? "Low" : "High",
            hardware = SystemInfo.processorType, gpu = SystemInfo.graphicsDeviceName,
            width = Screen.width, height = Screen.height, replayHash = Replay.FinalHash,
            samples = frameMs.Count, cpuSamples = cpuMs.Count, gpuSamples = gpuMs.Count,
            frameP95 = Percentile(frameMs,.95f), frameP99 = Percentile(frameMs,.99f),
            cpuP95 = Percentile(cpuMs,.95f), gpuP95 = Percentile(gpuMs,.95f),
            // Windows' 60 Hz limiter normally lands fractionally around 16.667 ms.
            // Count material misses, not timer quantization at the exact boundary.
            missedBudget = frameMs.Count(x => x > 17f),
            status = "LOCAL EXPERIMENT; representative hardware and visual approval pending; -1 = unavailable" };
        var args = Environment.GetCommandLineArgs();
        int output = Array.IndexOf(args, "--profile-output");
        string path = output >= 0 && output+1 < args.Length ? args[output+1] :
            Path.Combine(Application.persistentDataPath, "visual-spike-" + report.preset + ".json");
        Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(path)));
        File.WriteAllText(path, JsonUtility.ToJson(report,true));
        Debug.Log("Visual spike profile: " + path);
    }
}
}
