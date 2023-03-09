using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

//Play with the frequency of the buzzing according to gameplay
//Possible to replace attach script to Marco prefab and have Marco buzz the phone? Try this if isnt working
//Could also use sound effects instead of vibration 
public class BuzzOnProximity : MonoBehaviour
{
    public GameObject Marco;
    PhotonView MarcoView;
    int circle; //used to prevent new coroutine from being started every frame if Marco is in the same proximity circle

    void Update()
    {
        // Get the PhotonView component of Marco if hasn't been done already
        if (!MarcoView) {
            MarcoView = Marco.GetComponent<PhotonView>();
        }

        // Get the position of the Marco & phone
        //Vector3 gameObjectPosition = MarcoView.transform.position;
        Vector3 gameObjectPosition = Marco.transform.position;
        Vector3 phonePosition = transform.position; //Script is attached to ARCamera so this shld be the phone's position
        Debug.Log("Marco position: " + gameObjectPosition);
        Debug.Log("Phone position: " + phonePosition);
        float distance = Vector3.Distance(gameObjectPosition, phonePosition);
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
            StartCoroutine(StartBuzz(0.5f));
        } else if (distance <= 3.0f && circle != 3) {
            circle = 3;
            StopCoroutine("StartBuzz");
            StartCoroutine(StartBuzz(1.0f));
        } else if (circle != 4) {
            //If Marco is in the outer circle, stop buzzing
            circle = 4;
            StopCoroutine("StartBuzz");
        }
    }

    IEnumerator StartBuzz(float duration)
    {
        while(true) {
            Handheld.Vibrate();
            yield return new WaitForSeconds(duration);
        }
    }
}

