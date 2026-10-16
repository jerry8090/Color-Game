using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using ColorGame.Core;
using ColorGame.DevTools;
using ColorGame.Machine;

/// <summary>
/// Builds the 3D arcade cabinet: wooden cabinet, teal-rimmed tray, inclined shelf, rack (gate), three 6-face cubes.
/// Menu: Tools > Color Game > Build 3D Machine
/// It draws into its own Render Texture (MachineRT3D). Your gameplay objects are not changed.
/// </summary>
public static class ColorGame3DBuilder
{
    private const string LayerName = "Machine";
    private const string MaterialFolder = "Assets/ColorGame/Materials";
    private const string PrefabFolder = "Assets/ColorGame/Prefabs";
    private const string ConfigPath = "Assets/ColorGame/ScriptableObjects/ColorGameConfig.asset";
    private const string WoodTexturePath = MaterialFolder + "/WoodGrain.png";
    private const string CubeMeshPath = PrefabFolder + "/ColorCubeMesh.asset";
    private const string CubePrefabPath = PrefabFolder + "/ColorCube3D.prefab";
    private const string RenderTexturePath = MaterialFolder + "/MachineRT3D.asset";

    private const float CubeEdge = 0.9f;
    private const float ShelfAngle = 24f;
    private static readonly Vector3 ShelfPosition = new Vector3(0f, 3.2f, -2.9f);

    [MenuItem("Tools/Color Game/Build 3D Machine")]
    public static void Build()
    {
        if (EditorApplication.isPlaying)
        {
            Debug.LogWarning("[3DBuilder] Stop Play mode first.");
            return;
        }

        if (GameObject.Find("ColorMachine3D") != null)
        {
            Debug.LogError("[3DBuilder] A 'ColorMachine3D' object already exists. Delete it first to rebuild.");
            return;
        }

        ColorGameConfig config = AssetDatabase.LoadAssetAtPath<ColorGameConfig>(ConfigPath);
        if (config == null)
        {
            Debug.LogError("[3DBuilder] ColorGameConfig not found at " + ConfigPath);
            return;
        }

        Shader lit = Shader.Find("Universal Render Pipeline/Lit");
        if (lit == null)
        {
            Debug.LogError("[3DBuilder] The URP Lit shader was not found. Is this a URP project?");
            return;
        }

        int layer = EnsureLayer();
        if (layer < 0) return;

        int rendererIndex = FindUniversalRendererIndex();
        if (rendererIndex < 0)
        {
            Debug.LogWarning("[3DBuilder] No 'Universal Renderer' (3D) found in your URP asset's Renderer List. " +
                             "The cabinet will NOT be visible until you add one (see the setup steps), " +
                             "then select MachineCamera3D and choose it under Rendering > Renderer.");
        }

        // ---------- Materials ----------
        Texture2D wood = EnsureWoodTexture();
        Material woodGold = MakeLit(lit, "Machine3D_WoodGold", Color.white, 0.35f, wood, new Vector2(2f, 2f));
        Material woodFloor = MakeLit(lit, "Machine3D_WoodFloor", new Color(1f, 0.93f, 0.80f), 0.30f, wood, new Vector2(3f, 3f));
        Material woodDark = MakeLit(lit, "Machine3D_WoodDark", new Color(0.78f, 0.58f, 0.42f), 0.30f, wood, new Vector2(2f, 2f));
        Material ground = MakeLit(lit, "Machine3D_Ground", new Color(0.95f, 0.75f, 0.55f), 0.25f, wood, new Vector2(8f, 8f));
        Material teal = MakeLit(lit, "Machine3D_Teal", new Color32(31, 181, 168, 255), 0.55f, null, Vector2.one);
        Material rug = MakeLit(lit, "Machine3D_Rug", new Color32(60, 170, 70, 255), 0.10f, null, Vector2.one);
        Material cubeFace = MakeLit(lit, "Machine3D_CubeFace", Color.white, 0.60f, null, Vector2.one);
        Material cubeFrame = MakeLit(lit, "Machine3D_CubeFrame", new Color32(244, 240, 230, 255), 0.45f, null, Vector2.one);

        PhysicsMaterial cubePhysics = EnsurePhysics("Machine3D_Cube", 0.30f, 0.35f, 0.30f, PhysicsMaterialCombine.Average);
        PhysicsMaterial trayPhysics = EnsurePhysics("Machine3D_Tray", 0.35f, 0.40f, 0.30f, PhysicsMaterialCombine.Average);
        PhysicsMaterial shelfPhysics = EnsurePhysics("Machine3D_Shelf", 0.10f, 0.15f, 0.05f, PhysicsMaterialCombine.Average);
        PhysicsMaterial gatePhysics = EnsurePhysics("Machine3D_Gate", 0.0f, 0.0f, 0.0f, PhysicsMaterialCombine.Minimum);

        RenderTexture renderTexture = EnsureRenderTexture();
        GameObject cubePrefab = CreateCubePrefab(cubeFace, cubeFrame, cubePhysics, layer);
        if (cubePrefab == null) return;

        // ---------- Scene ----------
        GameObject root = new GameObject("ColorMachine3D");
        Undo.RegisterCreatedObjectUndo(root, "Build 3D Machine");
        root.layer = layer;
        root.transform.position = new Vector3(100f, 0f, 0f); // far from everything else
        Transform t = root.transform;

        // Camera
        GameObject cameraObject = NewChild("MachineCamera3D", t, new Vector3(0f, 8.5f, 11.5f), layer);
        cameraObject.transform.LookAt(t.TransformPoint(new Vector3(0f, 2.8f, -1.2f)));
        Camera cam = cameraObject.AddComponent<Camera>();
        cam.orthographic = false;
        cam.fieldOfView = 32f;
        cam.nearClipPlane = 0.3f;
        cam.farClipPlane = 80f;
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = new Color32(40, 24, 48, 255);
        cam.cullingMask = 1 << layer;
        cam.targetTexture = renderTexture;
        cam.depth = -9f;

        UniversalAdditionalCameraData cameraData = cam.GetUniversalAdditionalCameraData();
        if (rendererIndex >= 0) cameraData.SetRenderer(rendererIndex);
        cameraData.renderShadows = true;
        cameraData.renderPostProcessing = false;

        // Lights
        GameObject sun = NewChild("Sun", t, new Vector3(0f, 10f, 0f), layer);
        sun.transform.rotation = Quaternion.Euler(52f, -28f, 0f);
        Light sunLight = sun.AddComponent<Light>();
        sunLight.type = LightType.Directional;
        sunLight.color = new Color(1f, 0.95f, 0.86f);
        sunLight.intensity = 1.2f;
        sunLight.shadows = LightShadows.Soft;
        sunLight.shadowStrength = 0.8f;

        GameObject fill = NewChild("FillLight", t, new Vector3(0f, 6f, 7f), layer);
        Light fillLight = fill.AddComponent<Light>();
        fillLight.type = LightType.Point;
        fillLight.range = 30f;
        fillLight.intensity = 8f;
        fillLight.color = new Color(1f, 0.88f, 0.75f);
        fillLight.shadows = LightShadows.None;

        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
        RenderSettings.ambientLight = new Color(0.52f, 0.48f, 0.58f);

        // Environment
        Transform environment = NewChild("Environment", t, Vector3.zero, layer).transform;
        Box("Ground", environment, new Vector3(0f, -0.7f, 0f), Vector3.zero, new Vector3(40f, 0.4f, 40f), ground, trayPhysics, layer, true);
        GameObject rugObject = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        rugObject.name = "Rug";
        rugObject.layer = layer;
        rugObject.transform.SetParent(environment, false);
        rugObject.transform.localPosition = new Vector3(0f, -0.49f, 1f);
        rugObject.transform.localScale = new Vector3(15f, 0.01f, 15f);
        rugObject.GetComponent<MeshRenderer>().sharedMaterial = rug;
        Object.DestroyImmediate(rugObject.GetComponent<Collider>());

        // Tray (recessed floor, teal rim, invisible safety walls)
        Transform tray = NewChild("Tray", t, Vector3.zero, layer).transform;
        Box("TrayFloor", tray, new Vector3(0f, -0.2f, 0.5f), Vector3.zero, new Vector3(7.0f, 0.4f, 5.6f), woodFloor, trayPhysics, layer, true);
        Box("RimLeft", tray, new Vector3(-3.675f, 0.5f, 0.5f), Vector3.zero, new Vector3(0.35f, 1.0f, 5.6f), teal, trayPhysics, layer, true);
        Box("RimRight", tray, new Vector3(3.675f, 0.5f, 0.5f), Vector3.zero, new Vector3(0.35f, 1.0f, 5.6f), teal, trayPhysics, layer, true);
        Box("RimFront", tray, new Vector3(0f, 0.35f, 3.475f), Vector3.zero, new Vector3(7.7f, 0.7f, 0.35f), teal, trayPhysics, layer, true);
        InvisibleWall("WallLeft", tray, new Vector3(-3.8f, 4f, 0.5f), new Vector3(0.6f, 8f, 6f), trayPhysics, layer);
        InvisibleWall("WallRight", tray, new Vector3(3.8f, 4f, 0.5f), new Vector3(0.6f, 8f, 6f), trayPhysics, layer);
        InvisibleWall("WallFront", tray, new Vector3(0f, 4f, 3.6f), new Vector3(8f, 8f, 0.6f), trayPhysics, layer);

        // Cabinet back
        Transform cabinet = NewChild("Cabinet", t, Vector3.zero, layer).transform;
        Box("BackBlock", cabinet, new Vector3(0f, 1.25f, -2.9f), Vector3.zero, new Vector3(7.7f, 2.5f, 1.2f), woodDark, trayPhysics, layer, true);
        Box("BackPanel", cabinet, new Vector3(0f, 3.5f, -3.7f), Vector3.zero, new Vector3(7.7f, 7.0f, 0.4f), woodGold, trayPhysics, layer, true);
        Box("Header", cabinet, new Vector3(0f, 7.25f, -3.6f), Vector3.zero, new Vector3(8.1f, 0.5f, 0.7f), teal, trayPhysics, layer, false);
        Box("PostLeft", cabinet, new Vector3(-3.95f, 3.5f, -3.0f), Vector3.zero, new Vector3(0.45f, 7.0f, 1.4f), teal, trayPhysics, layer, false);
        Box("PostRight", cabinet, new Vector3(3.95f, 3.5f, -3.0f), Vector3.zero, new Vector3(0.45f, 7.0f, 1.4f), teal, trayPhysics, layer, false);

        // Inclined shelf (slopes toward the viewer)
        GameObject shelf = NewChild("ShelfAssembly", t, ShelfPosition, layer);
        shelf.transform.localRotation = Quaternion.Euler(ShelfAngle, 0f, 0f);
        Box("ShelfSlab", shelf.transform, new Vector3(0f, -0.5f, 0f), Vector3.zero, new Vector3(5.4f, 1.0f, 2.0f), woodGold, shelfPhysics, layer, true);
        Box("RailLeft", shelf.transform, new Vector3(-2.6f, 0.5f, 0f), Vector3.zero, new Vector3(0.2f, 1.0f, 2.0f), teal, shelfPhysics, layer, true);
        Box("RailRight", shelf.transform, new Vector3(2.6f, 0.5f, 0f), Vector3.zero, new Vector3(0.2f, 1.0f, 2.0f), teal, shelfPhysics, layer, true);

        // Rack: a kinematic gate in front of the cubes. Zero friction, so it can never drag a cube upward.
        GameObject rack = NewChild("Rack", t, Vector3.zero, layer);
        rack.transform.position = shelf.transform.TransformPoint(new Vector3(0f, 0f, 0.70f));
        rack.transform.rotation = shelf.transform.rotation;
        Rigidbody rackBody = rack.AddComponent<Rigidbody>();
        rackBody.isKinematic = true;
        rackBody.useGravity = false;
        rackBody.interpolation = RigidbodyInterpolation.Interpolate;
        Box("RackPanel", rack.transform, new Vector3(0f, 0.75f, 0f), Vector3.zero, new Vector3(4.9f, 1.5f, 0.16f), woodGold, gatePhysics, layer, true);
        Box("RackTopBeam", rack.transform, new Vector3(0f, 1.58f, 0f), Vector3.zero, new Vector3(5.0f, 0.16f, 0.3f), teal, gatePhysics, layer, false);
        Box("RackBarLeft", rack.transform, new Vector3(-2.45f, 0.75f, 0f), Vector3.zero, new Vector3(0.16f, 1.5f, 0.3f), teal, gatePhysics, layer, false);
        Box("RackBarRight", rack.transform, new Vector3(2.45f, 0.75f, 0f), Vector3.zero, new Vector3(0.16f, 1.5f, 0.3f), teal, gatePhysics, layer, false);

        // Cubes, resting on the shelf against the rack
        Transform cubesParent = NewChild("Cubes", t, Vector3.zero, layer).transform;
        ColorCube[] cubes = new ColorCube[3];
        for (int i = 0; i < cubes.Length; i++)
        {
            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(cubePrefab, cubesParent);
            instance.name = "Cube" + (i + 1);
            Vector3 position = shelf.transform.TransformPoint(new Vector3((i - 1) * 1.15f, CubeEdge * 0.5f + 0.005f, 0.15f));
            instance.transform.SetPositionAndRotation(position, shelf.transform.rotation);
            cubes[i] = instance.GetComponent<ColorCube>();
        }

        // Temporary preview tool
        Cabinet3DPreview preview = root.AddComponent<Cabinet3DPreview>();
        SerializedObject previewObject = new SerializedObject(preview);
        previewObject.FindProperty("config").objectReferenceValue = config;
        previewObject.FindProperty("rackBody").objectReferenceValue = rackBody;
        SerializedProperty cubeList = previewObject.FindProperty("cubes");
        cubeList.arraySize = cubes.Length;
        for (int i = 0; i < cubes.Length; i++) cubeList.GetArrayElementAtIndex(i).objectReferenceValue = cubes[i];
        previewObject.ApplyModifiedPropertiesWithoutUndo();

        // The normal camera must not draw the machine.
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            mainCamera.cullingMask &= ~(1 << layer);
            EditorUtility.SetDirty(mainCamera);
        }

        EditorSceneManager.MarkSceneDirty(root.scene);
        Selection.activeGameObject = root;
        Debug.Log("[3DBuilder] 3D machine built at X = 100. Save the scene (Ctrl+S). " +
                  "To see it in the game view, set MachineImage > Raw Image > Texture to MachineRT3D.");
    }

    // ---------- Helpers ----------

    private static GameObject NewChild(string name, Transform parent, Vector3 localPosition, int layer)
    {
        GameObject go = new GameObject(name);
        go.layer = layer;
        go.transform.SetParent(parent, false);
        go.transform.localPosition = localPosition;
        return go;
    }

    private static GameObject Box(string name, Transform parent, Vector3 localPosition, Vector3 localEuler, Vector3 size,
                                  Material material, PhysicsMaterial physics, int layer, bool withCollider)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = name;
        go.layer = layer;
        go.transform.SetParent(parent, false);
        go.transform.localPosition = localPosition;
        go.transform.localRotation = Quaternion.Euler(localEuler);
        go.transform.localScale = size;
        go.GetComponent<MeshRenderer>().sharedMaterial = material;

        BoxCollider box = go.GetComponent<BoxCollider>();
        if (withCollider) box.sharedMaterial = physics;
        else Object.DestroyImmediate(box);

        return go;
    }

    private static void InvisibleWall(string name, Transform parent, Vector3 localPosition, Vector3 size,
                                      PhysicsMaterial physics, int layer)
    {
        GameObject go = NewChild(name, parent, localPosition, layer);
        BoxCollider box = go.AddComponent<BoxCollider>();
        box.size = size;
        box.sharedMaterial = physics;
    }

    private static int EnsureLayer()
    {
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty layers = tagManager.FindProperty("layers");
        int firstFree = -1;

        for (int i = 8; i < 32; i++)
        {
            SerializedProperty element = layers.GetArrayElementAtIndex(i);
            if (element.stringValue == LayerName) return i;
            if (firstFree < 0 && string.IsNullOrEmpty(element.stringValue)) firstFree = i;
        }

        if (firstFree < 0)
        {
            Debug.LogError("[3DBuilder] No free User Layer slot.");
            return -1;
        }

        layers.GetArrayElementAtIndex(firstFree).stringValue = LayerName;
        tagManager.ApplyModifiedProperties();
        return firstFree;
    }

    // Finds a 3D "Universal Renderer" in the URP asset's Renderer List (the 2D Renderer is not one).
    private static int FindUniversalRendererIndex()
    {
        RenderPipelineAsset pipeline = GraphicsSettings.currentRenderPipeline;
        if (pipeline == null) return -1;

        SerializedObject pipelineObject = new SerializedObject(pipeline);
        SerializedProperty list = pipelineObject.FindProperty("m_RendererDataList");
        if (list == null) return -1;

        for (int i = 0; i < list.arraySize; i++)
        {
            if (list.GetArrayElementAtIndex(i).objectReferenceValue is UniversalRendererData) return i;
        }
        return -1;
    }

    private static Material MakeLit(Shader shader, string name, Color color, float smoothness, Texture2D texture, Vector2 tiling)
    {
        string path = MaterialFolder + "/" + name + ".mat";
        Material material = AssetDatabase.LoadAssetAtPath<Material>(path);

        if (material == null)
        {
            material = new Material(shader);
            AssetDatabase.CreateAsset(material, path);
        }

        material.color = color;
        material.SetFloat("_Smoothness", smoothness);
        material.SetFloat("_Metallic", 0f);
        if (texture != null)
        {
            material.mainTexture = texture;
            material.mainTextureScale = tiling;
        }
        EditorUtility.SetDirty(material);
        return material;
    }

    private static PhysicsMaterial EnsurePhysics(string name, float dynamicFriction, float staticFriction,
                                                 float bounciness, PhysicsMaterialCombine frictionCombine)
    {
        string path = MaterialFolder + "/" + name + ".asset";
        PhysicsMaterial material = AssetDatabase.LoadAssetAtPath<PhysicsMaterial>(path);

        if (material == null)
        {
            material = new PhysicsMaterial(name);
            AssetDatabase.CreateAsset(material, path);
        }

        material.dynamicFriction = dynamicFriction;
        material.staticFriction = staticFriction;
        material.bounciness = bounciness;
        material.frictionCombine = frictionCombine;
        EditorUtility.SetDirty(material);
        return material;
    }

    private static RenderTexture EnsureRenderTexture()
    {
        RenderTexture texture = AssetDatabase.LoadAssetAtPath<RenderTexture>(RenderTexturePath);
        if (texture == null)
        {
            texture = new RenderTexture(1280, 860, 24, RenderTextureFormat.ARGB32)
            {
                name = "MachineRT3D",
                antiAliasing = 4,
                filterMode = FilterMode.Bilinear
            };
            AssetDatabase.CreateAsset(texture, RenderTexturePath);
        }
        return texture;
    }

    // Procedural golden wood: long grain lines and plank seams. Mirrored so it tiles without a seam.
    private static Texture2D EnsureWoodTexture()
    {
        if (!File.Exists(WoodTexturePath))
        {
            const int size = 512;
            Color32[] pixels = new Color32[size * size];
            Color baseColor = new Color(0.86f, 0.62f, 0.25f);

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    int mx = x < size / 2 ? x : size - 1 - x;
                    int my = y < size / 2 ? y : size - 1 - y;

                    float grain = Mathf.PerlinNoise(mx * 0.012f + 3.7f, my * 0.35f);
                    float rings = Mathf.PerlinNoise(mx * 0.020f + 11f, my * 0.030f);
                    float seam = (y % 128) < 3 ? 0.6f : 1f;
                    float shade = (0.78f + 0.32f * grain) * (0.92f + 0.12f * rings) * seam;

                    Color c = baseColor * shade;
                    pixels[y * size + x] = new Color32(
                        (byte)(Mathf.Clamp01(c.r) * 255f),
                        (byte)(Mathf.Clamp01(c.g) * 255f),
                        (byte)(Mathf.Clamp01(c.b) * 255f), 255);
                }
            }

            Texture2D generated = new Texture2D(size, size, TextureFormat.RGBA32, false);
            generated.SetPixels32(pixels);
            generated.Apply();
            byte[] png = generated.EncodeToPNG();
            Object.DestroyImmediate(generated);

            string projectRoot = Directory.GetParent(Application.dataPath).FullName;
            File.WriteAllBytes(Path.Combine(projectRoot, WoodTexturePath), png);
            AssetDatabase.ImportAsset(WoodTexturePath, ImportAssetOptions.ForceUpdate);

            TextureImporter importer = AssetImporter.GetAtPath(WoodTexturePath) as TextureImporter;
            if (importer != null)
            {
                importer.wrapMode = TextureWrapMode.Repeat;
                importer.mipmapEnabled = true;
                importer.anisoLevel = 4;
                importer.SaveAndReimport();
            }
        }

        return AssetDatabase.LoadAssetAtPath<Texture2D>(WoodTexturePath);
    }

    private static GameObject CreateCubePrefab(Material face, Material frame, PhysicsMaterial physics, int layer)
    {
        Mesh mesh = AssetDatabase.LoadAssetAtPath<Mesh>(CubeMeshPath);
        if (mesh == null)
        {
            mesh = ColorCube.BuildMesh();
            AssetDatabase.CreateAsset(mesh, CubeMeshPath);
        }

        GameObject go = new GameObject("ColorCube3D");
        go.layer = layer;
        go.transform.localScale = Vector3.one * CubeEdge;

        MeshFilter meshFilter = go.AddComponent<MeshFilter>();
        meshFilter.sharedMesh = mesh;

        MeshRenderer meshRenderer = go.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterials = new[] { face, face, face, face, face, face, frame };

        BoxCollider box = go.AddComponent<BoxCollider>();
        box.size = Vector3.one;
        box.sharedMaterial = physics;

        Rigidbody rb = go.AddComponent<Rigidbody>();
        rb.mass = 1f;
        rb.linearDamping = 0.05f;
        rb.angularDamping = 0.3f;
        rb.maxAngularVelocity = 14f;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.solverIterations = 12;
        rb.solverVelocityIterations = 4;

        ColorCube cube = go.AddComponent<ColorCube>();
        SerializedObject cubeObject = new SerializedObject(cube);
        cubeObject.FindProperty("meshFilter").objectReferenceValue = meshFilter;
        cubeObject.FindProperty("meshRenderer").objectReferenceValue = meshRenderer;
        cubeObject.FindProperty("body").objectReferenceValue = rb;
        cubeObject.FindProperty("faceMaterialTemplate").objectReferenceValue = face;
        cubeObject.FindProperty("frameMaterial").objectReferenceValue = frame;
        cubeObject.ApplyModifiedPropertiesWithoutUndo();

        GameObject prefab = PrefabUtility.SaveAsPrefabAsset(go, CubePrefabPath);
        Object.DestroyImmediate(go);

        if (prefab == null) Debug.LogError("[3DBuilder] Could not save the cube prefab at " + CubePrefabPath);
        return prefab;
    }
}