#nullable disable // Unity serializes these references; the builder initializes them before playback.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SecretGame.VisualSpike.Editor;

public static class VisualSpikeBuilder
{
    public const string Root = "Assets/Game/VisualSpike";
    public const string Output = Root + "/Reference";
    public const string Scene = Output + "/VisualSpike001.unity";
    private static readonly Dictionary<string, Material> materials = new();
    private static Material ink;
    private static int meshIndex;

    [MenuItem("Secret Game/Visual Spike/Build Reference Scene")]
    public static void Build()
    {
        Directory.CreateDirectory(Output);
        Directory.CreateDirectory(Output + "/Materials");
        Directory.CreateDirectory(Output + "/Meshes");
        AssetDatabase.Refresh();
        materials.Clear();
        meshIndex = 0;
        var toon = Shader.Find("SecretGame/VisualSpike/Toon");
        if (!toon || !Shader.Find("SecretGame/VisualSpike/Outline")) throw new Exception("Spike shaders not imported.");
        Mat("Cloth", new Color(.24f,.32f,.46f));
        Mat("Skin", new Color(.83f,.56f,.4f));
        Mat("Hair", new Color(.83f,.55f,.19f));
        Mat("HairLight", new Color(.98f,.72f,.33f));
        Mat("Armor", new Color(.76f,.78f,.75f));
        Mat("Mechanics", new Color(.085f,.1f,.13f));
        Mat("Rust", new Color(.28f,.12f,.065f));
        Mat("Red", new Color(.5f,.045f,.03f));
        Mat("Cyan", new Color(.06f,.55f,.65f), new Color(.05f,.5f,.65f));
        Mat("Sensor", new Color(.75f,.035f,.015f), new Color(1.5f,.07f,.025f));
        Mat("Floor", new Color(.16f,.21f,.27f));
        Mat("Concrete", new Color(.24f,.29f,.34f));
        Mat("Enemy", new Color(.48f,.33f,.25f));
        Mat("Grid", new Color(.2f,.3f,.36f), new Color(.025f,.05f,.065f));
        ink = Save(new Material(Shader.Find("SecretGame/VisualSpike/Outline")), Output + "/Materials/Outline.mat");
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        var host = new GameObject("Visual Spike 001 — EXPERIMENT");
        var player = host.AddComponent<VisualSpikePlayer>();
        if (string.IsNullOrEmpty(AssetDatabase.GetAssetPath(MonoScript.FromMonoBehaviour(player))))
            throw new Exception("Player script must be a persistent MonoScript before saving the scene.");
        player.sourceCommit = Git("rev-parse HEAD");
        player.sourceState = Git("status --porcelain").Length > 0 ? "dirty implementation experiment" : "clean";
        player.high = Pipeline("High",4,2048,true);
        player.low = Pipeline("Low",2,1024,false);
        player.outline = ink;
        player.pilot = Cyborg(out var left, out var right);
        player.pilotLeftArm = left; player.pilotRightArm = right;
        player.mech = Mech(out var arm, out var hatch);
        player.mechRightArm = arm; player.hatch = hatch;
        player.enemy = new GameObject("Enemy 300 — neutral proxy").transform;
        Part(player.enemy,"Proxy",PrimitiveType.Capsule,new Vector3(0,.8f,0),new Vector3(.55f,.8f,.55f),"Enemy");
        player.arena = Arena(player.cellSizeMeters);
        var sun = new GameObject("Warm key").AddComponent<Light>();
        sun.type = LightType.Directional; sun.color = new Color(1,.9f,.77f); sun.intensity = 1.1f;
        sun.transform.rotation = Quaternion.Euler(42,145,0); sun.shadows = LightShadows.Soft;
        sun.shadowBias = .025f; sun.shadowNormalBias = .2f; player.key = sun;
        RenderSettings.ambientMode = AmbientMode.Flat;
        RenderSettings.ambientLight = new Color(.21f,.3f,.43f);
        var camera = new GameObject("combat-wide").AddComponent<Camera>();
        camera.tag = "MainCamera"; camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = new Color(.035f,.055f,.085f);
        camera.nearClipPlane = .1f; camera.farClipPlane = 100;
        camera.gameObject.AddComponent<UniversalAdditionalCameraData>();
        player.tacticalCamera = camera;
        player.occupancy = new[] { Line("Pilot occupied cells",5,"Cyan",.025f),
            Line("Mech occupied cells — provisional 2x2",5,"Cyan",.035f),Line("Target occupied cell",5,"Sensor",.035f) };
        player.forecastLine = Line("Resolved forecast target",2,"Cyan",.04f);
        player.impact = Line("Immediate event hit",2,"Sensor",.1f);
        player.feedback = new GameObject("Kernel forecast and immediate hit text").AddComponent<TextMesh>();
        player.feedback.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        player.feedback.GetComponent<Renderer>().sharedMaterial = player.feedback.font.material;
        player.feedback.fontSize = 64; player.feedback.characterSize = .035f;
        player.feedback.anchor = TextAnchor.MiddleCenter;
        player.feedback.color = new Color(.2f,.9f,1);
        // Batch rigid surfaces by material per articulation group, then share the mesh with its hull.
        foreach (var group in new[] {player.pilot,left,right,player.mech,arm,hatch,player.enemy})
            Batch(group);
        foreach (var group in new[] {player.pilot,player.mech,player.enemy}) AddOutlines(group);
        SetLayer(player.pilot,8); SetLayer(player.mech,9);
        player.Sample(0);
        EditorSceneManager.SaveScene(scene, Scene);
        EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(Scene,true) };
        PlayerSettings.enableFrameTimingStats = true;
        AssetDatabase.SaveAssets();
        Debug.Log("VISUAL_SPIKE_BUILT " + Scene + " replay=" + player.Replay.FinalHash);
    }
    private static Material Mat(string name, Color color, Color emission = default)
    {
        var material = new Material(Shader.Find("SecretGame/VisualSpike/Toon"));
        material.SetColor("_BaseColor",color); material.SetColor("_EmissionColor",emission);
        material = Save(material,Output+"/Materials/"+name+".mat");
        materials.Add(name,material);
        return material;
    }
    private static T Save<T>(T asset, string path) where T : UnityEngine.Object
    {
        asset.name = Path.GetFileNameWithoutExtension(path);
        var existing = AssetDatabase.LoadAssetAtPath<T>(path);
        if (existing)
        {
            EditorUtility.CopySerialized(asset,existing);
            UnityEngine.Object.DestroyImmediate(asset);
            EditorUtility.SetDirty(existing);
            return existing;
        }
        AssetDatabase.CreateAsset(asset,path);
        return asset;
    }
    private static UniversalRenderPipelineAsset Pipeline(string name,int msaa,int shadow,bool soft)
    {
        var data = Save(ScriptableObject.CreateInstance<UniversalRendererData>(),Output+"/"+name+"Renderer.asset");
        var pipeline = ScriptableObject.CreateInstance<UniversalRenderPipelineAsset>();
        var serialized = new SerializedObject(pipeline);
        var renderers = serialized.FindProperty("m_RendererDataList");
        renderers.arraySize = 1; renderers.GetArrayElementAtIndex(0).objectReferenceValue = data;
        serialized.FindProperty("m_MainLightShadowsSupported").boolValue = true;
        serialized.FindProperty("m_SoftShadowsSupported").boolValue = soft;
        serialized.ApplyModifiedPropertiesWithoutUndo();
        pipeline.msaaSampleCount = msaa;
        pipeline.mainLightShadowmapResolution = shadow; pipeline.shadowDistance = 40;
        pipeline.shadowCascadeCount = 1;
        pipeline.supportsHDR = false;
        return Save(pipeline,Output+"/"+name+".asset");
    }
    private static Transform Bone(Transform parent,string name,Vector3 position)
    {
        var bone = new GameObject(name).transform;
        bone.SetParent(parent,false); bone.localPosition = position;
        return bone;
    }
    private static Transform Part(Transform parent,string name,PrimitiveType type,Vector3 p,Vector3 size,string mat)
    {
        var part = GameObject.CreatePrimitive(type);
        part.name = name; part.transform.SetParent(parent,false);
        part.transform.localPosition = p; part.transform.localScale = size;
        UnityEngine.Object.DestroyImmediate(part.GetComponent<Collider>());
        part.GetComponent<Renderer>().sharedMaterial = materials[mat];
        return part.transform;
    }
    private static Transform Box(Transform parent,string name,float x,float y,float z,float w,float h,float d,string mat) =>
        Part(parent,name,PrimitiveType.Cube,new Vector3(x,y,z),new Vector3(w,h,d),mat);
    private static Transform Oval(Transform parent,string name,float x,float y,float z,float w,float h,float d,string mat) =>
        Part(parent,name,PrimitiveType.Sphere,new Vector3(x,y,z),new Vector3(w,h,d),mat);

    private static Transform Cyborg(out Transform left,out Transform right)
    {
        var root = new GameObject("Cyborg 200 — sheet construction study").transform;
        Oval(root,"Cargo hips",0,.88f,0,.37f,.3f,.23f,"Cloth");
        Oval(root,"Sleeveless tank",0,1.2f,0,.38f,.46f,.23f,"Cloth");
        Oval(root,"Bare neckline",0,1.42f,.018f,.27f,.16f,.18f,"Skin");
        Box(root,"Tank front",0,1.26f,.104f,.27f,.27f,.03f,"Cloth");
        Oval(root,"Neck",0,1.47f,0,.11f,.14f,.11f,"Skin");
        Oval(root,"Face study",0,1.6f,.01f,.205f,.265f,.19f,"Skin");
        Oval(root,"Hair crown",0,1.69f,-.02f,.27f,.15f,.23f,"Hair");
        Oval(root,"Layered mid-back hair",0,1.36f,-.135f,.36f,.65f,.17f,"Hair");
        for (int i=0;i<11;i++)
        {
            float x=(i-5)*.03f;
            var lockPart = Oval(root,"Back hair lock "+i,x,1.27f,-.2f,.065f,.43f,.07f,i%3==0?"HairLight":"Hair");
            lockPart.localRotation=Quaternion.Euler(0,0,(i-5)*-3);
        }
        for (int s=-1;s<=1;s+=2)
        {
            Oval(root,"Framing hair",s*.118f,1.5f,.01f,.085f,.37f,.15f,"Hair");
            var fringe = Oval(root,"Swept fringe",s*.053f,1.688f,.092f,.125f,.09f,.08f,"HairLight");
            fringe.localRotation=Quaternion.Euler(0,0,s*25);
            Box(root,"Eye",s*.044f,1.623f,.102f,.038f,.009f,.009f,"Mechanics");
            Box(root,"Brow",s*.044f,1.644f,.104f,.043f,.009f,.009f,"Hair");
            Oval(root,"Cargo leg",s*.107f,.5f,0,.19f,.75f,.21f,"Cloth");
            Box(root,"Cargo pocket",s*.19f,.56f,.015f,.055f,.13f,.13f,"Cloth");
            Box(root,"Thigh strap",s*.11f,.73f,0,.205f,.035f,.235f,"Mechanics");
            Box(root,"Thigh vertical webbing",s*.184f,.8f,.02f,.027f,.23f,.12f,"Mechanics");
            Box(root,"Visible pouch study",s*.205f,.82f,.025f,.075f,s<0?.17f:.12f,.13f,"Mechanics");
            Oval(root,"Flat boot",s*.11f,.105f,.04f,.18f,.21f,.29f,"Mechanics");
            Box(root,"Lug sole",s*.11f,.025f,.04f,.19f,.045f,.3f,"Mechanics");
            for (int buckle=0;buckle<2;buckle++)
                Box(root,"Boot buckle",s*.11f,.1f+buckle*.052f,.17f,.08f,.019f,.01f,"Armor");
        }
        Box(root,"Belt",0,.975f,0,.38f,.055f,.25f,"Mechanics");
        Box(root,"Buckle",0,.975f,.139f,.067f,.045f,.024f,"Armor");
        Box(root,"Waist tied garment",0,.91f,-.045f,.42f,.09f,.26f,"Cloth");
        var tie = Box(root,"Tied garment tails",.035f,.78f,.14f,.15f,.29f,.035f,"Cloth");
        tie.localRotation = Quaternion.Euler(0,0,14);
        left = Arm(root,-1); right = Arm(root,1);
        return root;
    }
    private static Transform Arm(Transform root,int s)
    {
        var arm = Bone(root,s<0?"Left symmetric arm":"Right symmetric arm",new Vector3(s*.23f,1.37f,0));
        Oval(arm,"Bare upper arm",0,-.13f,0,.13f,.3f,.14f,"Skin");
        Oval(arm,"Mechanical elbow",0,-.29f,0,.13f,.115f,.14f,"Mechanics");
        Oval(arm,"White forearm plating",0,-.41f,.008f,.13f,.23f,.13f,"Armor");
        Box(arm,"Wrist cuff",0,-.55f,0,.105f,.06f,.11f,"Mechanics");
        Oval(arm,"Cyan wrist",0,-.547f,.061f,.04f,.025f,.015f,"Cyan");
        Box(arm,"Mechanical palm",0,-.615f,0,.09f,.075f,.055f,"Armor");
        for(int f=0;f<4;f++)
            Box(arm,"Finger "+f,(f-1.5f)*.022f,-.687f,.005f,.016f,.073f,.022f,"Mechanics");
        var thumb=Box(arm,"Thumb",s*.065f,-.637f,.018f,.024f,.057f,.026f,"Mechanics");
        thumb.localRotation=Quaternion.Euler(0,0,s*28);
        return arm;
    }
    private static Transform Mech(out Transform right,out Transform hatch)
    {
        var root = new GameObject("Mech 210 — single sheet construction study").transform;
        Box(root,"Exposed torso",0,2.62f,0,1.12f,1.0f,.65f,"Mechanics");
        Box(root,"Vented backpack",0,2.8f,-.49f,.88f,.77f,.31f,"Armor");
        for(int i=0;i<6;i++) Box(root,"Back vent",0,2.59f+i*.07f,-.653f,.62f,.025f,.012f,"Mechanics");
        Oval(root,"Waist articulation",0,1.93f,0,.68f,.46f,.57f,"Mechanics");
        Box(root,"Pelvis plate",0,1.7f,.18f,.65f,.45f,.35f,"Armor");
        Box(root,"Low compact head",0,3.28f,.1f,.55f,.42f,.49f,"Mechanics");
        Box(root,"Head brow plate",0,3.48f,.1f,.67f,.18f,.58f,"Armor");
        Oval(root,"Single red sensor",0,3.32f,.361f,.14f,.14f,.035f,"Sensor");
        Box(root,"Face jaw",0,3.16f,.31f,.2f,.21f,.11f,"Armor");
        for(int s=-1;s<=1;s+=2)
        {
            Box(root,"Squared pauldron",s*.98f,3.04f,0,.79f,.73f,.87f,"Armor");
            Box(root,"Chest plate",s*.345f,2.72f,.39f,.54f,.56f,.18f,"Armor");
            Box(root,"Broad thigh plate",s*.43f,1.48f,.07f,.56f,.6f,.52f,"Armor");
            Oval(root,"Knee articulation",s*.44f,1.05f,0,.4f,.38f,.42f,"Mechanics");
            Box(root,"Shin plating",s*.47f,.66f,.07f,.49f,.57f,.48f,"Armor");
            var foot=Box(root,"Broad splayed foot",s*.49f,.17f,.19f,.57f,.3f,.85f,"Armor");
            foot.localRotation=Quaternion.Euler(0,s*9,0);
            Box(root,"Foot tread",s*.49f,.035f,.2f,.59f,.07f,.86f,"Mechanics");
            Box(root,"Shin piston",s*.73f,.7f,-.13f,.045f,.65f,.045f,"Mechanics");
            Box(root,"Red waist cable",s*.48f,2.04f,.3f,.037f,.35f,.037f,"Red");
            Box(root,"Red shin cable",s*.69f,.75f,.21f,.031f,.47f,.03f,"Red");
            for(int i=0;i<4;i++)
                Box(root,"Authored shoulder oxidation",s*(.75f+i*.13f),3.18f-(i%2)*.08f,.443f,.028f,.12f,.01f,"Rust");
        }
        for(int i=0;i<5;i++) Box(root,"Sternum vents",0,2.59f+i*.065f,.356f,.25f,.024f,.06f,"Armor");
        // Left manipulator remains part of this same machine; no alternate identity.
        var left = MechArm(root,-1);
        right = MechArm(root,1);
        // Batch the non-animated arm separately before the parent is batched.
        Batch(left);
        hatch=Bone(root,"Provisional rear top hatch",new Vector3(0,3.2f,-.54f));
        Box(hatch,"Hatch construction study",0,.03f,.18f,.71f,.09f,.48f,"Armor");
        return root;
    }
    private static Transform MechArm(Transform root,int s)
    {
        var arm=Bone(root,s<0?"Left manipulator":"Right attack manipulator",new Vector3(s*1.0f,2.66f,0));
        Oval(arm,"Shoulder joint",0,-.08f,0,.35f,.4f,.36f,"Mechanics");
        Box(arm,"Upper arm plate",0,-.29f,.03f,.4f,.4f,.41f,"Armor");
        Oval(arm,"Elbow",0,-.58f,0,.32f,.26f,.33f,"Mechanics");
        Box(arm,"Forearm plate",0,-.88f,.04f,.45f,.46f,.46f,"Armor");
        Box(arm,"Forearm red cable",s*.25f,-.8f,0,.036f,.4f,.04f,"Red");
        Box(arm,"Manipulator palm",0,-1.21f,.015f,.32f,.25f,.22f,"Mechanics");
        Box(arm,"Hand plating",0,-1.2f,.14f,.3f,.2f,.06f,"Armor");
        for(int i=0;i<3;i++) Box(arm,"Manipulator finger "+i,(i-1)*.105f,-1.43f,.05f,.074f,.22f,.08f,"Mechanics");
        var thumb=Box(arm,"Opposing fourth digit",s*.23f,-1.31f,.04f,.09f,.2f,.09f,"Mechanics");
        thumb.localRotation=Quaternion.Euler(0,0,s*30);
        return arm;
    }
    private static Transform Arena(float cell)
    {
        var arena=new GameObject("Relay yard — 8 x 6 logical cells").transform;
        Box(arena,"Platform",4*cell,-.15f,3*cell,8*cell,.3f,6*cell,"Floor");
        for(int x=0;x<=8;x++) Box(arena,"Grid",x*cell,.008f,3*cell,.012f,.01f,6*cell,"Grid");
        for(int z=0;z<=6;z++) Box(arena,"Grid",4*cell,.008f,z*cell,8*cell,.01f,.012f,"Grid");
        // All scenic solids are outside the logical arena; no false cover or occupancy.
        for(int i=0;i<5;i++)
        {
            float x=(i+.5f)*2;
            Box(arena,"Relay housing",x,.7f,8.3f,1.6f,1.4f,.7f,"Concrete");
            Box(arena,"Relay luminous strip",x,1.1f,7.93f,1.3f,.055f,.035f,"Cyan");
            for(int v=0;v<4;v++) Box(arena,"Relay vent",x,.42f+v*.1f,7.93f,1.15f,.035f,.025f,"Mechanics");
        }
        Batch(arena);
        return arena;
    }
    private static LineRenderer Line(string name,int count,string material,float width)
    {
        var line=new GameObject(name).AddComponent<LineRenderer>();
        line.sharedMaterial=materials[material]; line.positionCount=count;
        line.startWidth=line.endWidth=width; line.useWorldSpace=true;
        line.shadowCastingMode=ShadowCastingMode.Off; line.receiveShadows=false;
        return line;
    }
    private static void Batch(Transform group)
    {
        var filters=group.Cast<Transform>().Select(x=>x.GetComponent<MeshFilter>()).Where(x=>x).ToArray();
        foreach(var materialGroup in filters.GroupBy(x=>x.GetComponent<Renderer>().sharedMaterial))
        {
            var combine=materialGroup.Select(x=>new CombineInstance {
                mesh=x.sharedMesh, transform=group.worldToLocalMatrix*x.transform.localToWorldMatrix }).ToArray();
            var mesh=new Mesh {name=group.name+" "+materialGroup.Key.name,indexFormat=IndexFormat.UInt32};
            mesh.CombineMeshes(combine);
            mesh=Save(mesh,Output+"/Meshes/Study-"+(meshIndex++).ToString("D3")+".asset");
            var part=new GameObject(materialGroup.Key.name);
            part.transform.SetParent(group,false);
            part.AddComponent<MeshFilter>().sharedMesh=mesh;
            part.AddComponent<MeshRenderer>().sharedMaterial=materialGroup.Key;
        }
        foreach(var filter in filters) UnityEngine.Object.DestroyImmediate(filter.gameObject);
    }
    private static void AddOutlines(Transform root)
    {
        foreach(var filter in root.GetComponentsInChildren<MeshFilter>())
        {
            // Small face/hair features use their authored dark accents; hulls surround major material masses.
            var material=filter.GetComponent<Renderer>().sharedMaterial;
            if(material==materials["Cyan"] || material==materials["Sensor"] || material==materials["HairLight"] ||
                material==materials["Red"] || material==materials["Rust"]) continue;
            var hull=new GameObject("Outline");
            hull.transform.SetParent(filter.transform,false);
            hull.AddComponent<MeshFilter>().sharedMesh=filter.sharedMesh;
            var renderer=hull.AddComponent<MeshRenderer>(); renderer.sharedMaterial=ink;
            renderer.shadowCastingMode=ShadowCastingMode.Off; renderer.receiveShadows=false;
        }
    }
    private static void SetLayer(Transform root,int layer)
    {
        foreach(var transform in root.GetComponentsInChildren<Transform>(true)) transform.gameObject.layer=layer;
    }
    public static string Git(string arguments)
    {
        using var process = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("git",arguments) {
            WorkingDirectory=Path.GetFullPath("../.."),UseShellExecute=false,RedirectStandardOutput=true,
            RedirectStandardError=true,CreateNoWindow=true });
        var result=process.StandardOutput.ReadToEnd().Trim();
        process.WaitForExit();
        if(process.ExitCode!=0) throw new Exception("git metadata failed: "+process.StandardError.ReadToEnd());
        return result;
    }
    [MenuItem("Secret Game/Visual Spike/Build Windows Player")]
    public static void BuildPlayer()
    {
        if(!File.Exists(Scene)) Build();
        var report=BuildPipeline.BuildPlayer(new BuildPlayerOptions {
            scenes=new[]{Scene},locationPathName="../../Build/VisualSpike001/VisualSpike001.exe",
            target=BuildTarget.StandaloneWindows64,options=BuildOptions.None });
        if(report.summary.result!=BuildResult.Succeeded) throw new Exception("Player build failed: "+report.summary.result);
    }
}
