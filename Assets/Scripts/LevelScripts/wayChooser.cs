using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wayChooser : MonoBehaviour
{
    public lavaPattern lp;
    public Transform tp;
    public bool makeActive = false;
    public bool isLava = false;
    public GameObject obj;
    public AudioClip music;
    public float volume;
    public int LevelSelection=0;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = tp.position;
            if (makeActive && obj != null)
                obj.SetActive(true);
            if (isLava)
            {
                other.GetComponent<Player3DController>().staminaGain = 75f;
                lp.SelectPlatforms();
            }

            GameManager.Instance.SetCurrentLevel(LevelSelection);

            AudioManager.Instance.PlayMusic(music,volume);
        }
    }
}
