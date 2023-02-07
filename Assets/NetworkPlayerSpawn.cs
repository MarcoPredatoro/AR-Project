using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkPlayerSpawn : MonoBehaviourPunCallbacks
{

  private GameObject spawnedPlayerPrefab;

  public override void OnJoinedRoom()
  {
    base.OnJoinedRoom();
    //This sets the player's position to the position of the Network object
    spawnedPlayerPrefab = PhotonNetwork.Instantiate("Polo", transform.position, transform.rotation);
    Debug.Log("Player spawned");
  }

  public override void OnLeftRoom()
  {
    base.OnLeftRoom();
    PhotonNetwork.Destroy(spawnedPlayerPrefab);
    Debug.Log("Player destroyed");
  }




}
