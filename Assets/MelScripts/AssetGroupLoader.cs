using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;

public class AssetGroupLoader : MonoBehaviour
{
    public string label; // Label for the group you want to load

    private void Start()
    {
        LoadAssetsByLabel(label);
    }

    private void LoadAssetsByLabel(string label)
    {
        Addressables.LoadAssetsAsync<Object>(label, null).Completed += OnAssetsLoaded;
    }

    private void OnAssetsLoaded(AsyncOperationHandle<IList<Object>> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            foreach (var asset in handle.Result)
            {
                Debug.Log($"Loaded asset: {asset.name}");
                // Handle each asset, e.g., instantiate or assign it
            }
        }
        else
        {
            Debug.LogError($"Failed to load assets for label: {label}");
        }
    }
}