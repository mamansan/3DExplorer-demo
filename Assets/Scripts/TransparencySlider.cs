using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TransparencySlider : MonoBehaviour
{
    [Header("UI")]
    public Slider slider;

    [Header("Transparent Anatomy")]
    public GameObject headObject;

    private List<Material> materials =
        new List<Material>();

    void Start()
    {
        if (slider == null || headObject == null)
        {
            Debug.LogWarning(
                "Assign the Slider and Head Object."
            );
            return;
        }

        // Collect only the Head's materials
        Renderer[] renderers =
            headObject.GetComponentsInChildren<Renderer>();

        foreach (Renderer rend in renderers)
        {
            foreach (Material mat in rend.materials)
            {
                if (mat.HasProperty("_BaseColor"))
                    materials.Add(mat);
            }
        }

        slider.minValue = 0f;
        slider.maxValue = 1f;

        slider.onValueChanged.AddListener(SetOpacity);

        // Apply the starting slider value
        SetOpacity(slider.value);
    }

    public void SetOpacity(float opacity)
    {
        foreach (Material mat in materials)
        {
            Color color = mat.GetColor("_BaseColor");
            color.a = opacity;

            mat.SetColor("_BaseColor", color);
        }
    }
}