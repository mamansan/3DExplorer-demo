using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class MouseSelection : MonoBehaviour
{
    [Header("References")]
    public Camera mainCamera;
    public SelectionLabelUI selectionLabelUI;

    // NEW: Choose which layers can be selected
    public LayerMask selectableLayers;

    [Header("Click Settings")]
    public float dragThreshold = 5f;

    private SelectableObject selectedObject;
    private Vector2 mouseDownPosition;
    private bool validClick;

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (selectionLabelUI != null)
            selectionLabelUI.HideLabel();
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
            if (validClick)
            {
                if (EventSystem.current == null ||
                    !EventSystem.current.IsPointerOverGameObject())
                {
                    CheckSelection(mouse.position.ReadValue());
                }
            }

            validClick = false;
        }
    }

    void CheckSelection(Vector2 mousePosition)
    {
        Ray ray =
            mainCamera.ScreenPointToRay(mousePosition);

        SelectableObject clickedObject = null;

        // NEW: Raycast only against selectable layers
        if (Physics.Raycast(
            ray,
            out RaycastHit hit,
            Mathf.Infinity,
            selectableLayers))
        {
            clickedObject =
                hit.collider.GetComponentInParent<SelectableObject>();
        }

        if (clickedObject == selectedObject)
        {
            if (selectedObject != null)
                selectedObject.Deselect();

            selectedObject = null;

            if (selectionLabelUI != null)
                selectionLabelUI.HideLabel();

            return;
        }

        if (selectedObject != null)
            selectedObject.Deselect();

        if (selectionLabelUI != null)
            selectionLabelUI.HideLabel();

        selectedObject = clickedObject;

        if (selectedObject != null)
        {
            selectedObject.Select();

            if (selectionLabelUI != null)
            {
                Transform labelTarget =
                    selectedObject.labelAnchor != null
                    ? selectedObject.labelAnchor
                    : selectedObject.transform;

                selectionLabelUI.ShowLabel(
                    selectedObject.gameObject.name,
                    labelTarget
                );
            }
        }
    }
}