
using UnityEngine;
using TMPro;

public class SelectionLabelUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject labelContainer;
    public TMP_Text labelText;
    public Canvas canvas;
    public Camera mainCamera;

    [Header("Position Settings")]
    public Vector2 screenOffset = new Vector2(30f, 30f);

    private Transform selectedTarget;
    private RectTransform labelRect;
    private RectTransform canvasRect;

    void Start()
    {
        // Find the Main Camera if one hasn't been assigned
        if (mainCamera == null)
            mainCamera = Camera.main;

        // Get the RectTransforms of the label and Canvas
        labelRect =
            labelContainer.GetComponent<RectTransform>();

        canvasRect =
            canvas.GetComponent<RectTransform>();

        // Hide the label when the scene starts
        HideLabel();
    }

    void LateUpdate()
    {
        // Stop if no anatomical structure is selected
        if (selectedTarget == null)
            return;

        // Convert the 3D anchor position to screen coordinates
        Vector3 screenPosition =
            mainCamera.WorldToScreenPoint(
                selectedTarget.position
            );

        // Hide the label if the anchor is behind the camera
        if (screenPosition.z <= 0)
        {
            labelContainer.SetActive(false);
            return;
        }

        // Show the label when the anchor is in front
        labelContainer.SetActive(true);

        // Convert screen coordinates to Canvas coordinates
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPosition,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay
                ? null
                : canvas.worldCamera,
            out Vector2 localPoint
        );

        // Position the label with an adjustable offset
        labelRect.anchoredPosition =
            localPoint + screenOffset;
    }

    public void ShowLabel(string objectName, Transform target)
    {
        // Store the selected object's anchor
        selectedTarget = target;

        // Update the label text
        labelText.text = objectName;

        // Display the label
        labelContainer.SetActive(true);
    }

    public void HideLabel()
    {
        // Clear the selected target
        selectedTarget = null;

        // Hide the label
        labelContainer.SetActive(false);
    }
}
