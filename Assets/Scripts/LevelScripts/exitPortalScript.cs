using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class exitPortalScript : MonoBehaviour
{
    public Transform portalDoor;
    public GameObject currentLevel;
    public Transform portalPosition;
    public GameObject portalPlane;

    public bool isLava;
    public bool isWater;
    public GameObject water;
    public GameObject Lava;
    public GameObject plat;
    private Transform cam;
    private void Start()
    {
        cam = Camera.main.transform;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Player"))
        {
            collision.transform.rotation=new Quaternion(0, 180,0,0);
            collision.transform.position = portalPosition.position;
            collision.transform.GetComponent<Player3DController>().SetMenuActive(false);
            collision.transform.GetComponent<Player3DController>().ExitLevel();
            collision.transform.GetComponent<Player3DController>().staminaGain = 50f;
            portalDoor.gameObject.SetActive(true);
            RenderSettings.skybox = null;
            if (isLava || isWater)
                plat.SetActive(false);
            if (currentLevel.GetComponent<MazeGenerator>() != null)
                currentLevel.GetComponent<MazeGenerator>().ClearMaze();
            else if (isWater)
                water.SetActive(false);
            else if (isLava)
                Lava.SetActive(false);
            portalPlane.SetActive(false);
            currentLevel.SetActive(false);
            AudioManager.Instance.PlayMusic(AudioManager.Instance.mainTheme, .5f);
        }
    }

    private void Update()
    {
        Vector3 lookDirection = cam.position - transform.position;
        lookDirection.y = 0;

        if (lookDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            Vector3 currentEuler = transform.rotation.eulerAngles;
            Vector3 targetEuler = targetRotation.eulerAngles;

            transform.rotation = Quaternion.Euler(currentEuler.x, targetEuler.y, currentEuler.z);
        }
    }
}
