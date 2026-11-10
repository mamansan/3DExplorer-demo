using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class HotspotManager : MonoBehaviour
{
    public Camera mainCamera;
    public LayerMask hotspotLayer;

    [Header("Click Settings")]
    public float dragThreshold = 5f;

    private Vector2 mouseDownPosition;
    private bool validClick;

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    void Update()
    {
        Mouse mouse = Mouse.current;

        if (mouse == null || mainCamera == null)
            return;

        if (mouse.leftButton.wasPressedThisFrame)
        {
            mouseDownPosition =
                mouse.position.ReadValue();

            validClick =
                EventSystem.current == null ||
                !EventSystem.current.IsPointerOverGameObject();
        }

        if (mouse.leftButton.isPressed && validClick)
        {
            float dragDistance = Vector2.Distance(
                mouseDownPosition,
                mouse.position.ReadValue()
            );

            if (dragDistance > dragThreshold)
                validClick = false;
        }

        if (mouse.leftButton.wasReleasedThisFrame)
        {
            if (validClick &&
                (EventSystem.current == null ||
                 !EventSystem.current.IsPointerOverGameObject()))
            {
                Ray ray = mainCamera.ScreenPointToRay(
                    mouse.position.ReadValue()
                );

                if (Physics.Raycast(
                    ray,
                    out RaycastHit hit,
                    Mathf.Infinity,
                    hotspotLayer
                ))
                {
                    HotspotInfo hotspot =
                        hit.collider.GetComponentInParent<HotspotInfo>();

                    if (hotspot != null)
                        hotspot.TogglePanel();
                }
            }

            validClick = false;
        }
    }
}