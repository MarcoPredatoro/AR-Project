using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class TrackedImageSpawner : MonoBehaviour
{
    public ARTrackedImageManager trackedImageManager;
    public GameObject RealEgg;
    public GameObject RottenEgg;
    // add more prefabs if needed

    // a dictionary to keep track of which images have been spawned
    private Dictionary<string, GameObject> m_InstantiatedObjects = new Dictionary<string, GameObject>();

    void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            //SpawnPrefab(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            //If trackedImage is updated to be tracked, spawn prefab
            if (trackedImage.trackingState == TrackingState.Tracking)
            {
                SpawnPrefab(trackedImage);
            }
        }

        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            // destroy the corresponding spawned prefab if needed - doesn't work
            
        }
    }

    void SpawnPrefab(ARTrackedImage trackedImage)
    {
        GameObject prefabToSpawn = null;

        if (trackedImage.referenceImage.name == "Real")
        {
            prefabToSpawn = RealEgg;
        }
        else if (trackedImage.referenceImage.name == "Rotten")
        {
            prefabToSpawn = RottenEgg;
        }
        // add more conditionals to handle more markers

        if (prefabToSpawn != null)
        {
            GameObject spawnedObject = Instantiate(prefabToSpawn, trackedImage.transform.position, trackedImage.transform.rotation);
            //Destroy after 3 seconds
            Destroy(spawnedObject, 2f);
            // add any additional behavior or components to the spawned object
        }
    }
}

