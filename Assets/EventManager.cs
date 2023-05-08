using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;
using UnityEngine.UI;
using System;

public class EventManager : MonoBehaviourPun
{
    private const byte RFID_POINTS_EVENT = 1;
    private const byte MARCO_STAB_EVENT = 2;
    private const byte RESET_POINTS_EVENT = 3;
    //Powerup events
    private const byte BLIND_EVENT = 4;
    private const byte DECOY_EVENT = 5;
    private const byte EGG_TIMER_EVENT = 6;
    private const byte START_GAME = 8;

    private int points ;
    public Text pointsText;
    public Text blindText;
    public Text decoyText;
    public GameObject BlindMessage;
    public GameObject DecoyMessage;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }
    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }

    private void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == RFID_POINTS_EVENT)
        {
            //Removing points count on AR
            //int value = (int)photonEvent.CustomData;
            //updatePoints(value);
        }
        else if (photonEvent.Code == MARCO_STAB_EVENT)
        {
            Handheld.Vibrate();  
            //Removing points count on AR
            //int value = (int)photonEvent.CustomData;
            //updatePoints(-value);          
        }
        else if (photonEvent.Code == START_GAME)
        {
            //Reset AR markers
            GameObject.Find("AR Session Origin").GetComponent<TrackedImageSpawner>().ResetARMarkers();
            //Reset powerup counts
            blindText.text = "3";
            decoyText.text = "3";

        }
    }

    public void updatePoints(int value) {
        points += value;
        pointsText.text = points.ToString();
    }

    public void sendEggTimerUp(string eggCode)
    {
        Debug.Log("sent: " + eggCode);
        RaiseEventOptions options = RaiseEventOptions.Default;
        options.Receivers = ReceiverGroup.All;
        PhotonNetwork.RaiseEvent(EGG_TIMER_EVENT, eggCode, options, SendOptions.SendReliable);
    }

    //make RESET MARKERS on RFID side!!!!
    /*
    public void sendResetMarkers(string eggCode)
    {
        Debug.Log("sent: " + eggCode);
        RaiseEventOptions options = RaiseEventOptions.Default;
        options.Receivers = ReceiverGroup.All;
        PhotonNetwork.RaiseEvent(RESET_MARKER_EVENT, eggCode, options, SendOptions.SendReliable);
    }
    */

    //Powerup events
    public void SendMarcoBlind()
    {
        if (blindText.text == "0")
        {
            Debug.Log("No blind powerups left");
        } else {
            //Update the number of blind powerups left
            blindText.text = (Int32.Parse(blindText.text) - 1).ToString();

            //Send the event
            Debug.Log("Sending blind collision");
            RaiseEventOptions options = RaiseEventOptions.Default;
            options.Receivers = ReceiverGroup.All;
            PhotonNetwork.RaiseEvent(BLIND_EVENT, 0, options, SendOptions.SendReliable); //What should we put in content?

            //Tell the player that they have used a decoy
            GameObject spawnedObject = Instantiate(BlindMessage);
            //Destroy after 3 seconds
            Destroy(spawnedObject, 2f);
        }  
    }

    public void SendDecoyPolo()
    {
        if (decoyText.text == "0")
        {
            Debug.Log("No decoy powerups left");
        } else {
            //Update the number of blind powerups left
            decoyText.text = (Int32.Parse(decoyText.text) - 1).ToString();

            //Send the event
            Debug.Log("Sending decoy collision");
            RaiseEventOptions options = RaiseEventOptions.Default;
            options.Receivers = ReceiverGroup.All;
            PhotonNetwork.RaiseEvent(DECOY_EVENT, 0, options, SendOptions.SendReliable); //What should we put in content?

            //Tell the player that they have used a decoy
            GameObject spawnedObject = Instantiate(DecoyMessage);
            //Destroy after 3 seconds
            Destroy(spawnedObject, 2f);
        }  
    }

    // Coroutine for flickering the compass square
    private IEnumerator Flicker(GameObject compassSquare, float duration)
    {
        float startTime = Time.time;
        while (Time.time - startTime <= duration)
        {
            compassSquare.GetComponent<Image>().enabled = !compassSquare.GetComponent<Image>().enabled;
            yield return new WaitForSeconds(0.1f); // flicker interval
        }
        compassSquare.GetComponent<Image>().enabled = true;
        compassSquare.GetComponent<Image>().color = Color.white;
    }
}
