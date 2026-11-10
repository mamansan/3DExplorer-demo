using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    [Header("Selection Settings")]
    public Color selectedColor = Color.yellow;
    
    [Header("Label Settings")]
    public Transform labelAnchor;
  
    private Renderer[] objectRenderers;
    private Color[] originalColors;

    void Start()
    {
        objectRenderers =
            GetComponentsInChildren<Renderer>();

        originalColors =
            new Color[objectRenderers.Length];

        for (int i = 0; i < objectRenderers.Length; i++)
        {
            originalColors[i] =
                objectRenderers[i].material.color;
        }
    }

    public void Select()
    {
        for (int i = 0; i < objectRenderers.Length; i++)
        {
            objectRenderers[i].material.color =
                selectedColor;
        }
    }

    public void Deselect()
    {
        for (int i = 0; i < objectRenderers.Length; i++)
        {
            objectRenderers[i].material.color =
                originalColors[i];
        }
    }
}