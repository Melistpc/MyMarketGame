using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;

public class AssetGroupLoader : MonoBehaviour
{
    public string label = "usedassetlabel"; // Label for the group you want to load
    private AsyncOperationHandle<IList<Object>> loadedHandle;

    private void Start()
    {
        Debug.Log("Starting AssetGroupLoader...");
        LoadAssetsByLabel(label);
    }

    private void LoadAssetsByLabel(string label)
    {
        Debug.Log($"Loading assets with label: {label}");
        loadedHandle = Addressables.LoadAssetsAsync<Object>(label, null);
        loadedHandle.Completed += OnAssetsLoaded;
    }

    private void OnAssetsLoaded(AsyncOperationHandle<IList<Object>> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log($"Successfully loaded {handle.Result.Count} assets with label: {label}");
            foreach (var asset in handle.Result)
            {
                Debug.Log($"Loaded asset: {asset.name} ({asset.GetType()})");
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
        if (asset is GameObject prefab)
        {
            Debug.Log($"Instantiating prefab: {prefab.name}");
            var instance = Instantiate(prefab);

            // Log initial transform values
            Debug.Log($"Object {prefab.name} initial position: {instance.transform.position}, scale: {instance.transform.localScale}, rotation: {instance.transform.rotation.eulerAngles}");

            ValidateAndFixTransform(instance);
        }
        else
        {
            Debug.Log($"Unhandled asset type: {asset.GetType()} for asset {asset.name}");
        }
    }

    private void ValidateAndFixTransform(GameObject obj)
    {
        // Check if the position, rotation, and scale are valid
        if (!IsFinite(obj.transform.position))
        {
            Debug.LogWarning($"Object {obj.name} has invalid position {obj.transform.position}. Resetting to Vector3.zero.");
            obj.transform.position = Vector3.zero;
        }

        if (!IsFinite(obj.transform.localScale) || obj.transform.localScale == Vector3.zero)
        {
            Debug.LogWarning($"Object {obj.name} has invalid scale {obj.transform.localScale}. Resetting to Vector3.one.");
            obj.transform.localScale = Vector3.one;
        }

        if (!IsFinite(obj.transform.rotation.eulerAngles))
        {
            Debug.LogWarning($"Object {obj.name} has invalid rotation {obj.transform.rotation.eulerAngles}. Resetting to Quaternion.identity.");
            obj.transform.rotation = Quaternion.identity;
        }

        // Optionally, check if the object has a Rigidbody component and set it to kinematic if it does
        if (obj.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.isKinematic = true;
            Debug.Log($"Set Rigidbody to kinematic for {obj.name}");
        }
    }

    private bool IsFinite(Vector3 vector)
    {
        return float.IsFinite(vector.x) && float.IsFinite(vector.y) && float.IsFinite(vector.z);
    }

    private void OnDestroy()
    {
        if (loadedHandle.IsValid())
        {
            Addressables.Release(loadedHandle);
            Debug.Log($"Released Addressables handle for label: {label}");
        }
    }
}
