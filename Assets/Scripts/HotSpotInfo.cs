using UnityEngine;

public class HotspotInfo : MonoBehaviour
{
    [Header("Information Panel")]
    public GameObject infoPanel;

    public void TogglePanel()
    {
        if (infoPanel == null) return;

        bool isOpen = infoPanel.activeSelf;

        HotspotInfo[] allHotspots =
            FindObjectsByType<HotspotInfo>(
                FindObjectsSortMode.None
            );

        foreach (HotspotInfo hotspot in allHotspots)
        {
            if (hotspot.infoPanel != null)
                hotspot.infoPanel.SetActive(false);
        }

        infoPanel.SetActive(!isOpen);
    }

    public void ClosePanel()
    {
        if (infoPanel != null)
            infoPanel.SetActive(false);
    }
}