using UnityEngine;

public class Instructions : MonoBehaviour
{
    public GameObject instructionsPanel;
    public void OpenPanel()
    {
        instructionsPanel.SetActive(true);
    }

    public void ClosePanel()
    {
        instructionsPanel.SetActive(false);
    }
}
