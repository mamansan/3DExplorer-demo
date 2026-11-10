using UnityEngine;
using UnityEngine.UI;

public class VisibilityToggleUI : MonoBehaviour
{
    [System.Serializable]
    public class AnatomyToggle
    {
        public GameObject anatomyObject;
        public Toggle uiToggle;
    }

    public AnatomyToggle[] anatomyToggles;

    public ModelOrbitCamera orbitCamera;

    void Start()
    {
        foreach (AnatomyToggle item in anatomyToggles)
        {
            if (item.anatomyObject == null ||
                item.uiToggle == null)
                continue;

            GameObject targetObject = item.anatomyObject;

            item.uiToggle.SetIsOnWithoutNotify(
                targetObject.activeSelf);

            item.uiToggle.onValueChanged.AddListener(
                isVisible =>
                {
                    targetObject.SetActive(isVisible);
                });
        }
    }

    public void ResetCamera()
    {
        if (orbitCamera != null)
            orbitCamera.ResetView();
    }

    public void ShowAll()
    {
        foreach (AnatomyToggle item in anatomyToggles)
        {
            if (item.anatomyObject == null ||
                item.uiToggle == null)
                continue;

            item.anatomyObject.SetActive(true);
            item.uiToggle.SetIsOnWithoutNotify(true);
        }
    }
}