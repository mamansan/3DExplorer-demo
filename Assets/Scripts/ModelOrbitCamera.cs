using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class ModelOrbitCamera : MonoBehaviour
{
    public Transform target;

    [Header("Rotation")]
    public float rotationSpeed = 0.2f;
    public float minPitch = -80f;
    public float maxPitch = 80f;

    [Header("Zoom")]
    public float zoomSpeed = 0.005f;
    public float minDistance = 1f;
    public float maxDistance = 15f;

    [Header("Pan")]
    public float panSpeed = 0.002f;

    private float yaw;
    private float pitch;
    private float distance;

    private float initialYaw;
    private float initialPitch;
    private float initialDistance;

    private Quaternion initialRotation;
    private Vector3 initialOffset;

    private Vector3 focusPoint;
    private Vector3 initialFocusPoint;

    private bool draggingModel;
    private bool panningModel;

    void Start()
     // Initialize the camera's starting state
    {
        if (target == null)
        {
            Debug.LogError("Orbit target not assigned.");
            enabled = false;
            return;
        }

        focusPoint = target.position;
        initialFocusPoint = focusPoint;

        initialRotation = transform.rotation;

        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = Mathf.DeltaAngle(0f, angles.x);

        initialOffset = transform.position - focusPoint;
        distance = initialOffset.magnitude;

        initialYaw = yaw;
        initialPitch = pitch;
        initialDistance = distance;
    }

    void Update()
    // Read mouse input every frame
    // Calculate orbit, pan, and zoom
    {
        Mouse mouse = Mouse.current;
        if (mouse == null) return;

        bool overUI =
            EventSystem.current != null &&
            EventSystem.current.IsPointerOverGameObject();

        // LEFT MOUSE: Orbit
        if (mouse.leftButton.wasPressedThisFrame)
        {
            draggingModel = !overUI;
        }

        if (mouse.leftButton.wasReleasedThisFrame)
            draggingModel = false;

        // MIDDLE MOUSE: Pan
        if (mouse.middleButton.wasPressedThisFrame)
        {
            panningModel = !overUI;
        }

        if (mouse.middleButton.wasReleasedThisFrame)
            panningModel = false;

        Vector2 delta = mouse.delta.ReadValue();

        // Pan horizontally and vertically
        if (panningModel && mouse.middleButton.isPressed)
        {
            focusPoint -= transform.right *
                delta.x * panSpeed * distance;

            focusPoint -= transform.up *
                delta.y * panSpeed * distance;
        }

        // Orbit
        else if (draggingModel && mouse.leftButton.isPressed)
        {
            yaw += delta.x * rotationSpeed;
            pitch -= delta.y * rotationSpeed;

            pitch = Mathf.Clamp(
                pitch, minPitch, maxPitch
            );
        }

        // Scroll wheel: Zoom
        if (!overUI)
        {
            float scroll = mouse.scroll.ReadValue().y;

            distance -= scroll * zoomSpeed;

            distance = Mathf.Clamp(
                distance, minDistance, maxDistance
            );
        }

        UpdateCamera();
    }

    void UpdateCamera()
    {
        Quaternion orbitRotation =
            Quaternion.Euler(
                pitch - initialPitch,
                yaw - initialYaw,
                0f
            );

        Vector3 offset =
            orbitRotation *
            initialOffset.normalized *
            distance;

        transform.position = focusPoint + offset;
        transform.rotation = orbitRotation * initialRotation;
    }

    public void ResetView()
    {
        yaw = initialYaw;
        pitch = initialPitch;
        distance = initialDistance;
        focusPoint = initialFocusPoint;

        UpdateCamera();
    }
}