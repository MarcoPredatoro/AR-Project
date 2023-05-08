using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class NetworkManager : MonoBehaviourPunCallbacks
{
    public GameObject joinButton;
    public GameObject leaveButton;
    // Start is called before the first frame update
    void Start()
    {
        ConnecttoServer();
        PhotonNetwork.NickName = "ARPhone";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ConnecttoServer()
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Connecting to server....");
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("Connected to master server");
        joinButton.SetActive(true);
        
    }

    public void OnJoinButtonClicked()
    {
        joinButton.SetActive(false);
        leaveButton.SetActive(true);
        Debug.Log("Join button clicked");

        //Create or join a room
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        PhotonNetwork.JoinOrCreateRoom("Room1", roomOptions, TypedLobby.Default);
    }

    public void OnLeaveButtonClicked()
    {
        leaveButton.SetActive(false);
        joinButton.SetActive(true);

        //Leave room
        Debug.Log("Leave button clicked");
        PhotonNetwork.LeaveRoom();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room");
        base.OnJoinedRoom();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("New Player entered room");
        base.OnPlayerEnteredRoom(newPlayer);
    }
}