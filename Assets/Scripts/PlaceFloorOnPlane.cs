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

            // Add an ARAnchor component if it doesn't have one already - should stop it moving
            if (floorPlaced.GetComponent<ARAnchor>() == null)
            {
                floorPlaced.AddComponent<ARAnchor>();
            }
            //Disable plane detection after floor is placed
            arPlaneManager.enabled = !arPlaneManager.enabled;
            //Disable plane visuals
            foreach (var plane in arPlaneManager.trackables)
            {
                plane.gameObject.SetActive(false);
            }

        }
    }

    private void Dismiss() => welcomePanel.SetActive(false);

}
