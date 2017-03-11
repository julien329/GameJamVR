using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.IO;

public class VRBuildSettings {
    //****************************************************************
    // Build GearVR Button
    //****************************************************************
    [MenuItem("VR Build Settings/Build GearVR")]
    static void BuildGearVRSettings() {
#if !VR_GEARVR && !VR_GOOGLEVR
        Debug.LogWarningFormat("<color=#00ffffff><size=16><b>Please Import the GoogleVR SDK.</b></size></color>");
#else
        // Define var for GoogleVR Viewer
        GvrViewer gvrViewer;
        // Define var for GoogleVR Head
        GvrHead gvrHead;
        // Try to find the main GoogleVR script
        gvrViewer = GvrViewer.Instance;
        if (gvrViewer == null) {
            Debug.LogErrorFormat("<color=#00ffffff><size=16><b>GvrViewer could not be found.</b></size><size=12> Ensure the GvrViewerMain Prefab has been added to the Scene. \\Assets\\GoogleVR\\Prefabs\\</size></color>");
            return;
        }
        gvrHead = Camera.main.transform.GetComponent<GvrHead>();
        if (gvrHead == null) {
            Debug.LogErrorFormat("<color=#00ffffff><size=16><b>GvrHead could not be found.</b></size><size=12> Ensure the GvrHead Script has been added to the Main Camera.</size></color>");
            return;
        }
        // Set the VR Mode on GoogleVR Viewer to False, this will make it go into a fullscreen mode.
        // Fullscreen mode will be broken into stereoscopic 3d render because of VR Supported Option in build settings.
        gvrViewer.VRModeEnabled = false;
        // Tell GoogleVR Head to not track position, because VR Supported mode does this now
        gvrHead.trackPosition = false;
        // Tell GoogleVR Head to not track rotation, because VR Supported mode does this now
        gvrHead.trackRotation = false;
        // Set the Virtual Reality Supported option in the Player Build Settings
        PlayerSettings.virtualRealitySupported = true;
        // This forces the inspector to update the settings on GoogleVR script
        EditorUtility.SetDirty(gvrViewer);
        // This forces the inspector to update the settings on Head script
        EditorUtility.SetDirty(gvrHead);
        // Update Custom Defines
        VRBuildSettings vrbuildsettings = new VRBuildSettings();
        vrbuildsettings.UpdateGearDefines();

#endif
    }

    //****************************************************************
    // Build GoogleVR Button
    //****************************************************************
    [MenuItem("VR Build Settings/Build GoogleVR")]
    static void BuildGoogleVRSettings() {
#if !VR_GEARVR && !VR_GOOGLEVR
        Debug.LogWarningFormat("<color=#00ffffff><size=16><b>Please Import the GoogleVR SDK.</b></size></color>");
#else
        GvrViewer gvrViewer;
        GvrHead gvrHead;
        gvrViewer = GvrViewer.Instance;
        if (gvrViewer == null) {
            Debug.LogErrorFormat("<color=#00ffffff><size=16><b>GvrViewer could not be found.</b></size><size=12> Ensure the GvrViewerMain Prefab has been added to the Scene. \\Assets\\GoogleVR\\Prefabs\\</size></color>");
            return;
        }
        gvrHead = Camera.main.transform.GetComponent<GvrHead>();
        if (gvrHead == null) {
            Debug.LogErrorFormat("<color=#00ffffff><size=16><b>GvrHead could not be found.</b></size><size=12> Ensure the GvrHead Script has been added to the Main Camera.</size></color>");
            return;
        }
        gvrViewer.VRModeEnabled = true;
        gvrHead.trackPosition = false;
        gvrHead.trackRotation = true;
        PlayerSettings.virtualRealitySupported = false;
        EditorUtility.SetDirty(gvrViewer);
        EditorUtility.SetDirty(gvrHead);
        VRBuildSettings vrbuildsettings = new VRBuildSettings();
        vrbuildsettings.UpdateGoogleDefines();
#endif
    }

    [MenuItem("VR Build Settings/VR Build Options")]
    static void SelectVRBuildOptions() {
        string[] assets = AssetDatabase.FindAssets("VRBuildOptions t:ScriptableObject");
        string path = AssetDatabase.GUIDToAssetPath(assets[0]);
        Selection.activeObject = AssetDatabase.LoadMainAssetAtPath(path);
        EditorGUIUtility.PingObject(Selection.activeObject);
    }

    //****************************************************************
    // Update Selected Prefab In All Scenes
    //****************************************************************
    [MenuItem("VR Build Settings/Update Selected Prefab In All Scenes")]
    static void UpdateSelectedPrefabInAllScenes() {
        // Get current open scene
        Scene currentScene = SceneManager.GetActiveScene();
        // Save Open Scenes is bugged in 5.5.1
        EditorSceneManager.SaveOpenScenes();
        // Save the scene manually
        EditorSceneManager.SaveScene(currentScene);

        // Save the path of the currently opened scene, so we can re-open it later
        string currentScenePath = EditorSceneManager.GetActiveScene().path;
        // Save the name of the currently selected gameobject, so we can find this prefab in other scenes
        string currentSelection = Selection.activeGameObject.name;
        // Find the prefab in the /Assets/ folder
        Object prefabRoot = PrefabUtility.GetPrefabParent(Selection.activeGameObject.transform.root.gameObject);
        // Update currently selected prefab
        PrefabUtility.ReplacePrefab(Selection.activeGameObject, prefabRoot);

        // For every scene in the build list
        foreach (UnityEditor.EditorBuildSettingsScene S in UnityEditor.EditorBuildSettings.scenes) {
            if (S.enabled) {
                // Open the scene in the editor
                EditorSceneManager.OpenScene(S.path);
                // Try to find the prefab in this scene by name
                GameObject prefab = GameObject.Find(currentSelection);
                // If the prefab was found
                if (prefab != null) {
                    // "Revert" the prefab instance to the newly updated prefab instance.
                    PrefabUtility.RevertPrefabInstance(prefab);
                }
            }
        }
        // Return to the original scene that we were working in
        EditorSceneManager.OpenScene(currentScenePath);
    }

    //****************************************************************
    // Custom Define Managment
    // https://docs.unity3d.com/Manual/PlatformDependentCompilation.html
    //****************************************************************
    public void UpdateGearDefines() {
        // Find VRBuildOptions ScriptableObject
        string[] assets = AssetDatabase.FindAssets("VRBuildOptions t:ScriptableObject");
        string path = AssetDatabase.GUIDToAssetPath(assets[0]);
        VRBuildSettingsOptions options = (VRBuildSettingsOptions)AssetDatabase.LoadAssetAtPath(path, typeof(VRBuildSettingsOptions));
        // Check if Custom Defines are enabled or disabled
        if (options.enableCustomDefines == true) {
            string androidDefines = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android);
            if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android)
                Debug.LogWarningFormat("<color=#00ffffff><size=16><b>Warning: Build Target is not Android.</b></size><size=12> Custom Define Symbols will not work. Set Android as Build Target at File > Build Settings.</size></color>");
            if (androidDefines == null || androidDefines == "") {
                // There are no defines yet, add GearVR define
                androidDefines = "VR_GEARVR";
            }
            else if (androidDefines.Contains("VR_GEARVR")) {
                // GearVR define already exists, do nothing

                // *** TESTING ONLY ****
                //androidDefines = "";
                //androidDefines = "MyCustomDefine;AnotherOne;";
            }
            else {
                // There are some other defines, add GearVR define to the end
                androidDefines += ";VR_GEARVR";
            }
            // Clear any GoogleVR defines
            if (androidDefines.Contains("VR_GOOGLEVR;")) {
                androidDefines = androidDefines.Replace("VR_GOOGLEVR;", "");
            }
            else if (androidDefines.Contains("VR_GOOGLEVR")) {
                androidDefines = androidDefines.Replace("VR_GOOGLEVR", "");
            }
            // Save Custom Defines String to Player Settings
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, androidDefines);
        }
    }

    public void UpdateGoogleDefines() {
        // Find VRBuildOptions ScriptableObject
        string[] assets = AssetDatabase.FindAssets("VRBuildOptions t:ScriptableObject");
        string path = AssetDatabase.GUIDToAssetPath(assets[0]);
        VRBuildSettingsOptions options = (VRBuildSettingsOptions)AssetDatabase.LoadAssetAtPath(path, typeof(VRBuildSettingsOptions));
        // Check if Custom Defines are enabled or disabled
        if (options.enableCustomDefines == true) {
            string androidDefines = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android);
            if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android)
                Debug.LogWarningFormat("<color=#00ffffff><size=16><b>Warning: Build Target is not Android.</b></size><size=12> Custom Define Symbols will not work. Set Android as Build Target at File > Build Settings.</size></color>");
            if (androidDefines == null || androidDefines == "") {
                // There are no defines yet, add define
                androidDefines = "VR_GOOGLEVR";
            }
            else if (androidDefines.Contains("VR_GOOGLEVR")) {
                // Define already exists, do nothing

                // *** TESTING ONLY ****
                //androidDefines = "";
                //androidDefines = "MyCustomDefine;AnotherOne;";
            }
            else {
                // There are some other defines, add GearVR define to the end
                androidDefines += ";VR_GOOGLEVR";
            }
            // Clear any GearVR defines
            if (androidDefines.Contains("VR_GEARVR;")) {
                androidDefines = androidDefines.Replace("VR_GEARVR;", "");
            }
            else if (androidDefines.Contains("VR_GEARVR")) {
                androidDefines = androidDefines.Replace("VR_GEARVR", "");
            }
            // Save Custom Defines String to Player Settings
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, androidDefines);
        }
    }
}

//****************************************************************
// Asset Post Processor
//****************************************************************
class PostAssetImport : AssetPostprocessor {
    private void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths) {
        foreach (string str in importedAssets) {
            // Root GoogleVR Folder has been imported
            if (str == "Assets/GoogleVR") {
                VRBuildSettings vrbuildsettings = new VRBuildSettings();
                vrbuildsettings.UpdateGoogleDefines();
            }
        }
    }

    [InitializeOnLoad]
    public class Startup {
        static Startup() {
#if !VR_GEARVR && !VR_GOOGLEVR
            string[] guids = AssetDatabase.FindAssets("GvrViewer");
            if (guids.Length == 0) {
                Debug.LogWarningFormat("<color=#00ffffff><size=16><b>Please Import the GoogleVR SDK.</b></size></color>");
            }
            else {
                VRBuildSettings vrbuildsettings = new VRBuildSettings();
                vrbuildsettings.UpdateGoogleDefines();
            }
#endif
        }
    }
}