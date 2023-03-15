using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class BeepOnProximityTrial : MonoBehaviour
{

    private AudioListener listener;
    private Camera polo;

    public float[] threatRadius;

    // Start is called before the first frame update
    void Start()
    {
        //Find camera
        polo = Camera.main;
        //Find audio source (attached to ARCamera)
        listener = this.GetComponent<AudioListener>();
    }

    // Update is called once per frame
    void Update()
    {
        float dis = XZDistance(transform.position, polo.transform.position);

        if (dis > threatRadius[0] ) {
            // To far away so should play the background sound
            SetMute(polo, new bool[]{true, true, true});
            //background.GetComponent<AudioSource>().mute = false;
            Debug.Log("Background Sound");
        } else {
            //background.GetComponent<AudioSource>().mute = true;
            makeThreatRadiusSound(polo, dis);
        }
    }

    void makeThreatRadiusSound(Camera polo, float distance) {
        
        if (distance < threatRadius[2]) {
            // Polo in the closest ring
            Debug.Log("polo in the 3 ring");
            SetMute(polo, new bool[]{true, true, false});
        } else if (distance < threatRadius[1]){
            // Polo in the second closest ring
            Debug.Log("polo in the 2 ring");
            SetMute(polo, new bool[]{true, false, true});

        } else {
            // Polo in the furthest ring
            Debug.Log("polo in the 1 ring");
            SetMute(polo, new bool[]{false, true, true});

        }
    }

    void SetMute(Camera polo, bool[] isMuted) {
        // Sets whether the child of the polo's audio sources are muted or not based on the bool array
        for (int i = 0 ; i < isMuted.Length; i++){
            polo.transform.GetChild(i).GetComponent<AudioSource>().mute = isMuted[i];
        }
    }

    float XZDistance(Vector3 v1, Vector3 v2) {
        return Mathf.Sqrt((v1.x - v2.x) * (v1.x - v2.x) + (v1.z - v2.z) * (v1.z - v2.z));
    }
}