using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResetARSession: MonoBehaviour {

    [SerializeField]
    private Button resetButton;

    [SerializeField]
    private GameObject welcomePanel;

    [SerializeField]
    private ARSession _arSession;

    public void ResetSpace() {
        Debug.Log("Resetting AR Session...");
        _arSession.Reset();
        //SceneManager.LoadScene("AR_WIP");
        //Debug.Log("Loading Start Scene...");
    }

    public void OnResetButtonClicked() {
      Debug.Log("Reset Button Pressed");
      ResetSpace();
      //welcomePanel.SetActive(true);
    }
}