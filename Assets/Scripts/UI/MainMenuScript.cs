using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
    public void OnPlayButton()
    {
        GameManager.Instance.PlayGame();
    }

    public void OnExitButton()
    {
        Application.Quit();
    }

    public void settingsButton()
    {
        GameManager.Instance.SettingsMenu();
    }
}
