using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Attach to Marco prefab -> Head
public class BeepOnProximity : MonoBehaviour
{

    AudioSource Beep;
    Camera arCamera;
    int circle; //used to prevent new coroutine from being started every frame if Marco is in the same proximity circle

    void Start() {
        //Find camera
        arCamera = Camera.main;
        //Find audio source (attached to ARCamera)
        Beep  = arCamera.GetComponent<AudioSource>();
    }

    void Update()
    {
            // Get the position of the Marco & phone
            Vector3 marcoPosition = transform.position;
            Vector3 phonePosition = arCamera.transform.position; //Script is attached to ARCamera so this shld be the phone's position
            //Debug.Log("Marco position: " + marcoPosition);
            //Debug.Log("Phone position: " + phonePosition);
            float distance = Vector3.Distance(marcoPosition, phonePosition);
            Debug.Log("Distance: " + distance);

            //Define concentric circles around the camera
            //If Marco is within the inner circle, buzz at a high frequency
            //First circle: 0.5m
            //Second circle: 1.5m
            //Third circle: 3.0m
            if (distance <= 0.5f && circle != 1) {
                circle = 1;
                StopCoroutine("StartBuzz");
                StartCoroutine(StartBuzz(0.25f));
            } else if (distance <= 1.5f && circle != 2) {
                circle = 2;
                StopCoroutine("StartBuzz");
                StartCoroutine(StartBuzz(1.0f));
            } else if (distance <= 3.0f && circle != 3) {
                circle = 3;
                StopCoroutine("StartBuzz");
                StartCoroutine(StartBuzz(3.0f));
            } else if (circle != 4) {
                //If Marco is in the outer circle, stop buzzing
                circle = 4;
                StopCoroutine("StartBuzz");
            }
        
    }

    IEnumerator StartBuzz(float duration)
    {
        while(true) {
            //Handheld.Vibrate();
            Beep.Play(0); //Play sound effect
            Debug.Log("Beep");
            yield return new WaitForSeconds(Beep.clip.length + duration);
        }
    }

    //Don't need if script is attached to Marco prefab
    // private void findMarco() {
    //     HashSet<GameObject> foundObjects = PhotonNetwork.FindGameObjectsWithComponent(typeof(PhotonView));
    //     foreach (GameObject obj in foundObjects) {
    //         if (obj.name == "Marco(Clone)") {
    //             Marco = obj;
    //             Debug.Log("Marco found");
    //             break;
    //         }
    //     }
    // }   

}
