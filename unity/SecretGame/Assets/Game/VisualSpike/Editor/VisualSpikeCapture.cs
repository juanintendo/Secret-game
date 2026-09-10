#nullable disable // Unity serializes these references; the builder initializes them before playback.
using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SecretGame.VisualSpike.Editor;

public static class VisualSpikeCapture
{
    [Serializable] private sealed class Metadata
    {
        public string status = "EXPERIMENT — construction study; not approved output";
        public string[] sources = { "art/SHEET-cyborg-field-001.png", "art/SHEET-mech-001.png",
            "art/HERO-definitive-trio-002.png", "art/STUDY-mech-scale-footprint-001.png (reference only)" };
        public string commit, sourceState, unity, scene, cameraId, preset, hardware, gpu, replayHash;
        public int width = 1920, height = 1080;
        public float time, cellSizeMeters;
    }
    [MenuItem("Secret Game/Visual Spike/Capture Fixed Cameras")]
    public static void Capture()
    {
        EditorSceneManager.OpenScene(VisualSpikeBuilder.Scene);
        var player = UnityEngine.Object.FindFirstObjectByType<VisualSpikePlayer>();
        player.Initialize();
        var output=Path.GetFullPath("../../captures/visual-spike-001");
        Directory.CreateDirectory(output);
        var commit=VisualSpikeBuilder.Git("rev-parse HEAD");
        var dirty=VisualSpikeBuilder.Git("status --porcelain").Length>0 ? "dirty implementation experiment" : "clean";
        var shots = new (string id,float time)[] {
            ("cyborg-front",0),("cyborg-profile",0),("cyborg-back",0),
            ("mech-front",0),("mech-profile",0),("mech-back",0),
            ("combat-wide",0),("selected-forecast",7),("movement-midpoint",2.625f),
            ("docked-start",4),("docked-midpoint",5),("docked-complete",6),
            ("hit-confirmation",8.05f) };
        foreach(bool low in new[]{false,true})
        foreach(float scale in new[]{1f,1.25f,1.5f})
        {
            player.cellSizeMeters=scale;
            player.ApplyPreset(low);
            foreach(var shot in shots)
            {
                player.Sample(shot.time); player.SetCamera(shot.id);
                var camera=player.tacticalCamera;
                var texture=new RenderTexture(1920,1080,24,RenderTextureFormat.ARGB32);
                texture.antiAliasing=low?2:4; texture.Create();
                var previous=RenderTexture.active;
                Texture2D pixels=null;
                try
                {
                    RenderPipeline.SubmitRenderRequest(camera,new UniversalRenderPipeline.SingleCameraRequest {destination=texture});
                    RenderTexture.active=texture;
                    pixels=new Texture2D(1920,1080,TextureFormat.RGB24,false);
                    pixels.ReadPixels(new Rect(0,0,1920,1080),0,0); pixels.Apply();
                    string name=shot.id+"-"+(low?"Low":"High")+"-"+scale.ToString("0.00",System.Globalization.CultureInfo.InvariantCulture);
                    File.WriteAllBytes(Path.Combine(output,name+".png"),pixels.EncodeToPNG());
                    var metadata=new Metadata {commit=commit,sourceState=dirty,unity=Application.unityVersion,
                        scene=VisualSpikeBuilder.Scene,cameraId=shot.id,preset=low?"Low":"High",time=shot.time,
                        cellSizeMeters=scale,hardware=SystemInfo.processorType,gpu=SystemInfo.graphicsDeviceName,
                        replayHash=player.Replay.FinalHash};
                    File.WriteAllText(Path.Combine(output,name+".json"),JsonUtility.ToJson(metadata,true));
                }
                finally
                {
                    RenderTexture.active=previous;
                    texture.Release(); UnityEngine.Object.DestroyImmediate(texture);
                    if(pixels) UnityEngine.Object.DestroyImmediate(pixels);
                }
            }
        }
        player.cellSizeMeters=1.25f; player.ApplyPreset(false); player.Sample(0);
        Debug.Log("VISUAL_SPIKE_CAPTURED 78 fixed frames: "+output);
    }
}
