using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;

public class AddressablePrefabLoader : MonoBehaviour
{
    [Header("Prefab Loading Settings")]
    [Tooltip("The label for the prefab group in Addressables.")]
    [SerializeField] private string prefabLabel = "prefabLabel"; // The label for the prefab group

    [Header("Prefab Instantiation Settings")]
    [Tooltip("The position at which to instantiate the loaded prefab.")]
    [SerializeField] private Vector3 instantiationPosition = Vector3.zero; // Position to instantiate prefab

    [Header("Debugging Settings")]
    [Tooltip("Enable to log additional debugging information.")]
    [SerializeField] private bool enableDebugLogs = true; // Whether to log additional debug information

    private AsyncOperationHandle<GameObject> handle; // Handle for the loaded prefab

    private void Start()
    {
        if (enableDebugLogs)
            Debug.Log("Starting AddressablePrefabLoader...");

        // Load the prefab and its dependencies
        LoadPrefab(prefabLabel);
    }

    private void LoadPrefab(string label)
    {
        // Load the prefab and its dependencies asynchronously
        handle = Addressables.LoadAssetAsync<GameObject>(label);
        handle.Completed += OnPrefabLoaded;
    }

    private void OnPrefabLoaded(AsyncOperationHandle<GameObject> loadHandle)
    {
        if (loadHandle.Status == AsyncOperationStatus.Succeeded)
        {
            // Prefab loaded successfully
            GameObject prefab = loadHandle.Result;

            if (enableDebugLogs)
                Debug.Log($"Prefab loaded: {prefab.name}");

            // Instantiate the prefab in the scene
            InstantiatePrefab(prefab);
        }
        else
        {
            // If there was an issue loading the prefab
            if (enableDebugLogs)
                Debug.LogError($"Failed to load prefab with label: {prefabLabel}");
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
            Debug.Log("Released Addressable asset.");
    }
}
