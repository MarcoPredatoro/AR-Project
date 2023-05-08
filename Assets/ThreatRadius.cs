using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

//This script is attached to Marco's head, and it will play the sound based on the distance between Marco and Kinect tracked Polo
public class ThreatRadius : MonoBehaviour
{

    private AudioListener listener;
    private GameObject polo;
    private Camera cam;

    public float threatRadius;

    // Start is called before the first frame update
    void Start()
    {
        //Still keep audio listener on Camera
        //Find camera
        cam = Camera.main;
        //Find audio listener (attached to ARCamera)
        listener = cam.GetComponent<AudioListener>();
    }

    // Update is called once per frame
    void Update()
    {
        //Find polo-with-bones
        if (polo == null) {
            //SetMute(true);
            SetMute(true,1.0f);
            polo = GameObject.Find("polo-with-bones(Clone)"); //This is the polo that is spawned in
            Debug.Log("Polo found");
        }
        //If polo is found, then check distance
        if (polo != null) {
            float dis = XZDistance(transform.position, polo.transform.position);
            Debug.Log("Distance: " + dis);
            if (dis > threatRadius) {
                // To far away so should play the background sound
                //SetMute(true);
                SetMute(true, 1.0f);
                //background.GetComponent<AudioSource>().mute = false;
                Debug.Log("Background Sound");
            } else {
                //background.GetComponent<AudioSource>().mute = true;
                makeThreatRadiusSound(dis);
            }
        }
    }

    void makeThreatRadiusSound(float distance) {
        
        if (distance < threatRadius) {
            Debug.Log("polo in the ring");
            //SetMute(false); //If polo is in the threat radius, then play the sound
            SetMute(false, 2.0f);
        }
        /*
        if (distance < threatRadius[2]) {
            // Polo in the closest ring
            Debug.Log("polo in the 3 ring");
            SetMute(new bool[]{true, true, false});
        } else if (distance < threatRadius[1]){
            // Polo in the second closest ring
            Debug.Log("polo in the 2 ring");
            SetMute(new bool[]{true, false, true});

        } else {
            // Polo in the furthest ring
            Debug.Log("polo in the 1 ring");
            SetMute(new bool[]{false, true, true});

        }
        */
    }

    /*
    void SetMute(bool isMuted) {
        // Sets whether the child of the polo's audio sources are muted or not based on the bool array
        if (transform.childCount > 0) {
            this.gameObject.transform.GetChild(0).GetChild(0).GetComponent<AudioSource>().mute = isMuted;
        }
    }
    */
    IEnumerator FadeOut(AudioSource audioSource, float duration)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / duration;
            yield return null;
        }

        audioSource.mute = true;
        audioSource.volume = startVolume;
    }

    void SetMute(bool isMuted, float fadeDuration)
    {
        if (transform.childCount > 0)
        {
            AudioSource audioSource = this.gameObject.transform.GetChild(0).GetChild(0).GetComponent<AudioSource>();

            if (isMuted)
            {
                StartCoroutine(FadeOut(audioSource, fadeDuration));
            }
            else
            {
                audioSource.mute = false;
            }
        }
    }



    float XZDistance(Vector3 v1, Vector3 v2) {
        return Mathf.Sqrt((v1.x - v2.x) * (v1.x - v2.x) + (v1.z - v2.z) * (v1.z - v2.z));
    }
}