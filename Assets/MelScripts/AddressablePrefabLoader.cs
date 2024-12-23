using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;

public class AddressablePrefabLoader : MonoBehaviour
{
    [Header("Prefab Loading Settings")]
    [Tooltip("The label for the prefab group in Addressables.")]
    [SerializeField] private string prefabLabel = "area"; // The label for the prefab group

    [Header("Prefab Instantiation Settings")]
    [Tooltip("The position at which to instantiate the loaded prefabs.")]
    [SerializeField] private Vector3 instantiationPosition = Vector3.zero; // Position to instantiate prefab

    [Header("Debugging Settings")]
    [Tooltip("Enable to log additional debugging information.")]
    [SerializeField] private bool enableDebugLogs = true; // Whether to log additional debug information

    private AsyncOperationHandle<IList<GameObject>> handle; // Handle for the loaded prefabs

    private void Start()
    {
        if (enableDebugLogs)
            Debug.Log("Starting AddressablePrefabLoader...");

        // Load all prefabs by label and their dependencies
        LoadPrefabs(prefabLabel);
    }

    private void LoadPrefabs(string label)
    {
        // Load all prefabs asynchronously by label (returns a list)
        handle = Addressables.LoadAssetsAsync<GameObject>(label, null);
        handle.Completed += OnPrefabsLoaded;
    }

    private void OnPrefabsLoaded(AsyncOperationHandle<IList<GameObject>> loadHandle)
    {
        if (loadHandle.Status == AsyncOperationStatus.Succeeded)
        {
            // Prefabs loaded successfully
            IList<GameObject> prefabs = loadHandle.Result;

            if (enableDebugLogs)
                Debug.Log($"Loaded {prefabs.Count} prefabs with label: {prefabLabel}");

            // Instantiate each prefab in the scene
            foreach (var prefab in prefabs)
            {
                InstantiatePrefab(prefab);
            }
        }
        else
        {
            // If there was an issue loading the prefabs
            if (enableDebugLogs)
                Debug.LogError($"Failed to load prefabs with label: {prefabLabel}");
        }
    }

    private void InstantiatePrefab(GameObject prefab)
    {
        // Instantiate the prefab in the scene at the specified position
        if (prefab != null)
        {
            GameObject instance = Instantiate(prefab, instantiationPosition, Quaternion.identity);
            if (enableDebugLogs)
                Debug.Log($"Prefab instantiated: {instance.name}");
        }
        else
        {
            if (enableDebugLogs)
                Debug.LogError("Prefab was null, cannot instantiate.");
        }
    }

    private void OnDestroy()
    {
        // Release the loaded asset to free up memory
        Addressables.Release(handle);

        if (enableDebugLogs)
            Debug.Log("Released Addressable assets.");
    }
}
