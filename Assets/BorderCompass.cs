using UnityEngine;
using UnityEngine.UI;

public class BorderCompass : MonoBehaviour
{
    public GameObject arrowPrefab;
    private GameObject arrowInstance;
    private Camera arCamera;
    private float angle = 0f;
    private RectTransform arrowRectTransform;

    void Start()
    {
        arCamera = Camera.main;

        // Create a Canvas if it doesn't exist
        GameObject canvasObj = GameObject.Find("NetworkCanvas");
        if (canvasObj == null)
        {
            canvasObj = new GameObject("Canvas");
            canvasObj.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
        }

        Canvas canvas = canvasObj.GetComponent<Canvas>();

        // Instantiate the arrowPrefab as a child of the Canvas
        arrowInstance = Instantiate(arrowPrefab, Vector3.zero, Quaternion.identity, canvas.transform);
        arrowRectTransform = arrowInstance.GetComponent<RectTransform>();
    }

    void Update()
    {
        Vector3 bet = transform.position - arCamera.transform.position;
        bet.y = 0f;

        Vector3 camForward = arCamera.transform.forward;
        camForward.y = 0f;

        angle = Vector3.SignedAngle(camForward, bet, Vector3.up);

        PositionArrowOnScreenBorder();
    }

    void PositionArrowOnScreenBorder()
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        float absScreenRatio = screenHeight / screenWidth;

        float screenAngleRad = angle * Mathf.Deg2Rad;
        float y = Mathf.Cos(screenAngleRad);
        float x = Mathf.Sin(screenAngleRad);

        Vector3 screenPoint = new Vector3(x, y, 0);
        Vector3 screenPointAdjusted;

        if (Mathf.Abs(screenPoint.x) > Mathf.Abs(screenPoint.y))
        {
            screenPointAdjusted = new Vector3(Mathf.Sign(screenPoint.x), screenPoint.y / Mathf.Abs(screenPoint.x), 0);
        }
        else
        {
            screenPointAdjusted = new Vector3(screenPoint.x / Mathf.Abs(screenPoint.y), Mathf.Sign(screenPoint.y), 0);
        }

        Vector3 normalizedScreenPosition = new Vector3((screenPointAdjusted.x + 1) / 2, (screenPointAdjusted.y + 1) / 2, 0);

        arrowRectTransform.anchorMin = normalizedScreenPosition;
        arrowRectTransform.anchorMax = normalizedScreenPosition;
        arrowRectTransform.pivot = new Vector2(0.5f, 0.5f);
        arrowRectTransform.anchoredPosition = Vector2.zero;
    }

}