using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class UnusedAssetChecker : EditorWindow
{
    [MenuItem("Tools/Check Unused Assets")]
    static void CheckUnusedAssets()
    {
        string[] allAssets = AssetDatabase.FindAssets(""); // Get all assets
        List<string> unusedAssets = new List<string>();

        foreach (var guid in allAssets)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            if (!AssetDatabase.IsValidFolder(path)) // Ignore folders
            {
                var references = AssetDatabase.GetDependencies(path); // Get all references
                bool isUsed = false;

                foreach (var reference in references)
                {
                    if (!reference.Equals(path)) // If it is used, ignore
                    {
                        isUsed = true;
                        break;
                    }
                }

                if (!isUsed) // If the asset is not used
                {
                    unusedAssets.Add(path);
                }
            }
        }

        // Print all unused assets
        if (unusedAssets.Count > 0)
        {
            Debug.Log("Unused Assets:");
            foreach (var asset in unusedAssets)
            {
                Debug.Log(asset);
            }
        }
        else
        {
            Debug.Log("No unused assets found.");
        }
    }
}