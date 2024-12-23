using UnityEditor;
using UnityEngine;

public class MissingScriptDetector : EditorWindow
{
    [MenuItem("Tools/Find Missing Scripts")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(MissingScriptDetector));
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Find Missing Scripts"))
        {
            FindMissingScriptsInScene();
        }
    }

    private static void FindMissingScriptsInScene()
    {
        // Iterate over all GameObjects in the scene
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        foreach (var obj in allObjects)
        {
            CheckForMissingScripts(obj);
        }
    }

    private static void CheckForMissingScripts(GameObject obj)
    {
        // Check if the GameObject has any components
        var components = obj.GetComponents<Component>();

        foreach (var component in components)
        {
            if (component == null)
            {
                // This component is missing
                Debug.LogWarning($"Missing script on GameObject: {obj.name}", obj);
            }
        }
    }
}