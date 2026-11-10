using UnityEngine;

public class HotspotOcclusion : MonoBehaviour
{
    [Header("References")]
    public Camera mainCamera;
    public LayerMask anatomyLayer;

    [Header("Opacity Settings")]
    [Range(0f, 1f)]
    public float headHiddenOpacity = 0.8f;

    [Range(0f, 1f)]
    public float otherHiddenOpacity = 0.15f;

    [Range(0f, 1f)]
    public float visibleOpacity = 1f;

    [Header("Fade Settings")]
    public float fadeSpeed = 8f;

    private Renderer hotspotRenderer;
    private Material hotspotMaterial;
    private float currentOpacity;
    private bool isHeadHotspot;

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        hotspotRenderer = GetComponent<Renderer>();

        if (hotspotRenderer == null)
        {
            Debug.LogWarning(
                "HotspotOcclusion requires a Renderer."
            );
            enabled = false;
            return;
        }

        hotspotMaterial = hotspotRenderer.material;

        // Check whether this hotspot belongs to Head
        Transform parent = transform.parent;

        while (parent != null)
        {
            if (parent.gameObject.name == "Head")
            {
                isHeadHotspot = true;
                break;
            }

            parent = parent.parent;
        }

        currentOpacity = visibleOpacity;
    }

    void LateUpdate()
    {
        if (mainCamera == null || hotspotMaterial == null)
            return;

        Vector3 direction =
            transform.position - mainCamera.transform.position;

        float distance = direction.magnitude;

        // Detect anatomy between camera and hotspot
        bool isHidden = Physics.Raycast(
            mainCamera.transform.position,
            direction.normalized,
            distance,
            anatomyLayer
        );

        float targetOpacity = visibleOpacity;

        if (isHidden)
        {
            targetOpacity = isHeadHotspot
                ? headHiddenOpacity
                : otherHiddenOpacity;
        }

        // Smoothly transition to target opacity
        currentOpacity = Mathf.MoveTowards(
            currentOpacity,
            targetOpacity,
            fadeSpeed * Time.deltaTime
        );

        // Update the custom shader's tint alpha
        Color color =
            hotspotMaterial.GetColor("_BaseColor");

        color.a = currentOpacity;

        hotspotMaterial.SetColor(
            "_BaseColor",
            color
        );
    }
}