using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;


[RequireComponent(typeof(ARPlaneManager))]
public class PlaceFloorOnPlane : MonoBehaviour
{
    [SerializeField] 
    private GameObject welcomePanel;

    [SerializeField]
    private GameObject floorPrefab;

    private GameObject floorPlaced;

    [SerializeField]
    private Button dismissButton;

    [SerializeField]
    private ARPlaneManager arPlaneManager;

    void Awake()
    {
        dismissButton.onClick.AddListener(Dismiss);
        arPlaneManager = GetComponent<ARPlaneManager>();
        arPlaneManager.planesChanged += OnPlanesChanged;
    }
    private void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        if(args.added != null && floorPlaced == null) {
            ARPlane arPlane = args.added[0];
            //Trying to place plane at world origin - then I should spawn at (-2,-2)
            Vector3 pos = new Vector3((float)0.0,arPlane.transform.position.y,(float)0.0);
            floorPlaced = Instantiate(floorPrefab, pos, Quaternion.identity);
        }
    }

    private void Dismiss() => welcomePanel.SetActive(false);

}
