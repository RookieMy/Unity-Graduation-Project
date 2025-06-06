using UnityEngine;
using TMPro;

public class LevelSelectUI : MonoBehaviour
{
    [Header("Instances")]
    public WheelInteract wheel;
    public GameObject portalDoor;
    public GameObject portalPlane;
    public GameObject[] levels;
    public PortalCameraManager cameraManager;
    public Material[] skyboxes;
    public GameObject selectionPanel;

    public GameObject scorePanel;
    public TextMeshProUGUI scoreText;


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (selectionPanel.activeSelf)
                selectionPanel.SetActive(false);
            wheel.CloseWheelUI();
        }
    }

    
    public void SelectLevel(int selection)
    {
        if (selection == 1)
        {
            selectionPanel.SetActive(true);
        }
        for (int i = 0; i < levels.Length; i++)
            if (i != 0)
                levels[i].SetActive(false);
        levels[selection].SetActive(true);
        Debug.Log(selection);
        cameraManager.SetActiveCamera(selection);
        portalDoor.SetActive(false);
        RenderSettings.skybox = skyboxes[selection - 1];
        portalPlane.SetActive(true);
        if (selection != 1)
            wheel.CloseWheelUI();
    }
}
