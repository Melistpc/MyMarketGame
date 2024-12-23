using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;

public class AssetGroupLoader : MonoBehaviour
{
    [Tooltip("Label assigned to the assets in Addressables.")]
    public string label = "usedassetlabel"; // Label for the group you want to load

    private void Start()
    {
        // Start by downloading dependencies for smoother loading
        PreloadDependencies(label);
    }

    private void PreloadDependencies(string label)
    {
        Debug.Log($"Downloading dependencies for assets labeled: {label}");
        Addressables.DownloadDependenciesAsync(label).Completed += handle =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log($"Dependencies for {label} downloaded successfully!");
                // Load the assets after dependencies are downloaded
                LoadAssetsByLabel(label);
            }
            else
            {
                Debug.LogError($"Failed to download dependencies for {label}");
            }
        };
    }

    private void LoadAssetsByLabel(string label)
    {
        Debug.Log($"Loading assets with label: {label}");
        Addressables.LoadAssetsAsync<Object>(label, null).Completed += OnAssetsLoaded;
    }

    private void OnAssetsLoaded(AsyncOperationHandle<IList<Object>> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log($"Successfully loaded {handle.Result.Count} assets with label: {label}");
            foreach (var asset in handle.Result)
            {
                Debug.Log($"Loaded asset: {asset.name}");
                HandleAsset(asset);
            }
        }
        else
        {
            Debug.LogError($"Failed to load assets for label: {label}");
        }
    }

    private void HandleAsset(Object asset)
    {
        // Example: Handle different asset types
        if (asset is GameObject)
        {
            // Instantiate GameObject prefabs
            Instantiate(asset as GameObject);
            Debug.Log($"Instantiated prefab: {asset.name}");
        }
        else if (asset is Texture)
        {
            // Example: Log a message for loaded textures
            Debug.Log($"Loaded texture: {asset.name}");
        }
        else if (asset is Material)
        {
            // Example: Log a message for loaded materials
            Debug.Log($"Loaded material: {asset.name}");
        }
        else
        {
            Debug.Log($"Unhandled asset type: {asset.GetType()}");
        }
    }

    private void OnDestroy()
    {
        // Optionally release assets when no longer needed
        Addressables.Release(label);
        Debug.Log($"Released assets for label: {label}");
    }
}
