using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections;
using UnityEngine.UI;

//BIG BUG: Infinite powerups spawn when you scan the same image twice
public class TrackedImageSpawner : MonoBehaviour
{
    public ARTrackedImageManager trackedImageManager;
    public GameObject RealEgg;
    public GameObject RottenEgg;
    
    // add more prefabs if needed
    public GameObject BlindMessage;
    public GameObject DecoyMessage;
    public GameObject events;

    public Text blindText;
    public Text decoyText;
    public GameObject timerTextPrefab;
    private int BlindCount = 0;
    private int DecoyCount = 0;


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

        if (trackedImage.referenceImage.name.Contains("Real"))
        {
            //If trackedImage has not been spawned yet, spawn prefab
            if (!m_InstantiatedObjects.ContainsKey(trackedImage.referenceImage.name))
            {
                //just for debugging
                //blindText.text = trackedImage.referenceImage.name;
                prefabToSpawn = RealEgg;
                m_InstantiatedObjects.Add(trackedImage.referenceImage.name, prefabToSpawn);

                //Start timer - need to update timer ui also
                StartCoroutine(StartCountdown(5, trackedImage.referenceImage.name));
            }
        }
        else if (trackedImage.referenceImage.name.Contains("Rot"))
        {
            prefabToSpawn = RottenEgg;
        }

        //Powerup events
        else if (trackedImage.referenceImage.name.Contains("Blind"))
        {
            //If marker has been seen before, don't start timer again
            if (!m_InstantiatedObjects.ContainsKey(trackedImage.referenceImage.name))
            {
                prefabToSpawn = BlindMessage;
                m_InstantiatedObjects.Add(trackedImage.referenceImage.name, prefabToSpawn);
                //Add to inventory count 
                BlindCount += 1;
                blindText.text = BlindCount.ToString();
            }

        }
        else if (trackedImage.referenceImage.name.Contains("Decoy"))
        {
            //If trackedImage has not been spawned yet, spawn prefab
            if (!m_InstantiatedObjects.ContainsKey(trackedImage.referenceImage.name))
            {
                prefabToSpawn = DecoyMessage;
                m_InstantiatedObjects.Add(trackedImage.referenceImage.name, prefabToSpawn);
                //Add to inventory count
                DecoyCount += 1;
                decoyText.text = DecoyCount.ToString();
            }
                
        }
        // add more conditionals to handle more markers
        
        if (prefabToSpawn != null)
        {
            GameObject spawnedObject = Instantiate(prefabToSpawn, trackedImage.transform.position, trackedImage.transform.rotation);
            //Destroy after 3 seconds
            Destroy(spawnedObject, 1f);
            // add any additional behavior or components to the spawned object
        }
    }

    //Timer for egg
    IEnumerator StartCountdown(int countdownValue, string referenceImageName)
    {
        GameObject timer = Instantiate(timerTextPrefab);
        Text timerText = timer.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();

        while (countdownValue > 0)
        {
            timerText.text = countdownValue.ToString();
            yield return new WaitForSeconds(1);
            countdownValue--;
        }
        timerText.text = "Egg rotten!";

        // When timer is up, send event and destroy timer text
        events.GetComponent<EventManager>().sendEggTimerUp(referenceImageName);
        Destroy(timer);
    }
}

