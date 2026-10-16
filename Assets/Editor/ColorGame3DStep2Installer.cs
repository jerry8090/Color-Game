using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using ColorGame.Core;
using ColorGame.DevTools;
using ColorGame.Gameplay;
using ColorGame.Machine;

/// <summary>
/// Menu: Tools > Color Game > 3D Step 2 - Install Round Presenter
///  - adds Machine3DPresenter to ColorMachine3D and fills in its references
///  - points ColorGameManager > Roll Presenter at it
///  - removes the temporary Cabinet3DPreview (it needs normal physics, which Step 2 turns off)
///  - makes the rack gate shorter so it no longer hides the cubes
///  - raises the machine camera for a clearer view of the shelf and tray
/// </summary>
public static class ColorGame3DStep2Installer
{
    private const string ConfigPath = "Assets/ColorGame/ScriptableObjects/ColorGameConfig.asset";

    // Gate (rack) size along the shelf. It must stay above the middle of a cube (0.45) to hold it back.
    private const float GateHeight = 0.55f;

    // Machine camera, relative to ColorMachine3D.
    private static readonly Vector3 CameraPosition = new Vector3(0f, 11f, 9.5f);
    private static readonly Vector3 CameraLookAt = new Vector3(0f, 2.2f, -0.3f);
    private const float CameraFieldOfView = 34f;

    [MenuItem("Tools/Color Game/3D Step 2 - Install Round Presenter")]
    public static void Install()
    {
        if (EditorApplication.isPlaying)
        {
            Debug.LogWarning("[Step2] Stop Play mode first.");
            return;
        }

        GameObject root = GameObject.Find("ColorMachine3D");
        if (root == null)
        {
            Debug.LogError("[Step2] 'ColorMachine3D' was not found in the open scene. Open the ColorGame scene.");
            return;
        }

        ColorGameManager gameManager = Object.FindFirstObjectByType<ColorGameManager>();
        if (gameManager == null)
        {
            Debug.LogError("[Step2] ColorGameManager was not found in the open scene.");
            return;
        }

        ColorGameConfig config = AssetDatabase.LoadAssetAtPath<ColorGameConfig>(ConfigPath);
        if (config == null)
        {
            Debug.LogError("[Step2] ColorGameConfig not found at " + ConfigPath);
            return;
        }

        Transform rack = root.transform.Find("Rack");
        Rigidbody rackBody = rack != null ? rack.GetComponent<Rigidbody>() : null;
        ColorCube[] cubes = new ColorCube[3];
        for (int i = 0; i < cubes.Length; i++)
        {
            Transform cube = root.transform.Find("Cubes/Cube" + (i + 1));
            cubes[i] = cube != null ? cube.GetComponent<ColorCube>() : null;
        }

        if (rackBody == null || cubes[0] == null || cubes[1] == null || cubes[2] == null)
        {
            Debug.LogError("[Step2] Could not find Rack or Cubes/Cube1..3 under ColorMachine3D.");
            return;
        }

        // Remove the temporary preview tester.
        Cabinet3DPreview preview = root.GetComponent<Cabinet3DPreview>();
        if (preview != null) Undo.DestroyObjectImmediate(preview);

        // Add and fill the presenter.
        Machine3DPresenter presenter = root.GetComponent<Machine3DPresenter>();
        if (presenter == null) presenter = Undo.AddComponent<Machine3DPresenter>(root);

        SerializedObject presenterObject = new SerializedObject(presenter);
        presenterObject.FindProperty("config").objectReferenceValue = config;
        presenterObject.FindProperty("gameManager").objectReferenceValue = gameManager;
        presenterObject.FindProperty("rackBody").objectReferenceValue = rackBody;
        SerializedProperty cubeList = presenterObject.FindProperty("cubes");
        cubeList.arraySize = cubes.Length;
        for (int i = 0; i < cubes.Length; i++) cubeList.GetArrayElementAtIndex(i).objectReferenceValue = cubes[i];
        presenterObject.ApplyModifiedProperties();

        // Point the game manager at the 3D machine.
        SerializedObject managerObject = new SerializedObject(gameManager);
        SerializedProperty presenterSlot = managerObject.FindProperty("rollPresenter");
        if (presenterSlot == null)
        {
            Debug.LogError("[Step2] ColorGameManager has no 'rollPresenter' field.");
            return;
        }
        presenterSlot.objectReferenceValue = presenter;
        managerObject.ApplyModifiedProperties();

        // Shorter gate so it does not hide the cubes.
        ResizeGate(rack);

        // Camera.
        Transform cameraTransform = root.transform.Find("MachineCamera3D");
        if (cameraTransform != null)
        {
            Undo.RecordObject(cameraTransform, "Step 2 camera");
            cameraTransform.localPosition = CameraPosition;
            cameraTransform.LookAt(root.transform.TransformPoint(CameraLookAt));

            Camera cam = cameraTransform.GetComponent<Camera>();
            if (cam != null)
            {
                Undo.RecordObject(cam, "Step 2 camera");
                cam.fieldOfView = CameraFieldOfView;
            }
        }

        EditorUtility.SetDirty(presenter);
        EditorUtility.SetDirty(gameManager);
        EditorSceneManager.MarkSceneDirty(root.scene);
        Debug.Log("[Step2] Done. Machine3DPresenter is installed and ColorGameManager > Roll Presenter points to it. Save the scene (Ctrl+S).");
    }

    private static void ResizeGate(Transform rack)
    {
        Transform panel = rack.Find("RackPanel");
        if (panel != null)
        {
            Undo.RecordObject(panel, "Step 2 gate");
            Vector3 scale = panel.localScale;
            scale.y = GateHeight;
            panel.localScale = scale;
            panel.localPosition = new Vector3(0f, GateHeight * 0.5f, 0f);
        }

        Transform beam = rack.Find("RackTopBeam");
        if (beam != null)
        {
            Undo.RecordObject(beam, "Step 2 gate");
            Vector3 scale = beam.localScale;
            scale.y = 0.12f;
            beam.localScale = scale;
            beam.localPosition = new Vector3(0f, GateHeight + 0.06f, 0f);
        }

        foreach (string barName in new[] { "RackBarLeft", "RackBarRight" })
        {
            Transform bar = rack.Find(barName);
            if (bar == null) continue;

            Undo.RecordObject(bar, "Step 2 gate");
            Vector3 scale = bar.localScale;
            scale.y = GateHeight + 0.12f;
            bar.localScale = scale;
            Vector3 position = bar.localPosition;
            position.y = (GateHeight + 0.12f) * 0.5f;
            bar.localPosition = position;
        }
    }
}