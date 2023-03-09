using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class CompassBehaviour : MonoBehaviour
{   

    private Texture2D arrow;
    public Camera arCamera;
    public GameObject Marco;
    // PhotonView MarcoView;
    private float angle = 0f;
    
    void Start() {
        //Create a new texture for the arrow
        arrow = new Texture2D(1, 1);
        // Load the PNG file as a byte array
        byte[] imageBytes = File.ReadAllBytes("Assets/kisspng-computer-icons-automotive-navigation-system-5-arrow-5b28aacfb73147.7266884915293918237504.png");
        // Load the byte array into a texture
        arrow.LoadImage(imageBytes);
    }

    void Update() {
        // Get the PhotonView component of Marco if hasn't been done already
        // if (!MarcoView) {
        //     MarcoView = Marco.GetComponent<PhotonView>();
        // } 
        //Find vector from camera to Marco
        Vector3 bet = Marco.transform.position - arCamera.transform.position;
        bet.y = 0f; //Ignore vertical component of vector

        //Find forward vector of camera
        Vector3 camForward = arCamera.transform.forward;
        camForward.y = 0f; //Ignore vertical component of vector

        //Update angle between camera forward vector and vector from camera to Marco, about y axis
        angle = Vector3.SignedAngle(camForward, bet, Vector3.up); //Returns angle in degrees between -180 and 180
        Debug.Log("Angle: " + angle);
       
    }

    private void OnGUI() {
        //Draw arrow on screen
        GUIUtility.RotateAroundPivot(angle, new Vector2(Screen.width/2, Screen.height/2));
        GUI.DrawTexture(new Rect(Screen.width/2 - arrow.width/2, Screen.height/2 - arrow.height/2, arrow.width, arrow.height), arrow);
    }

}





