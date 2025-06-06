using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static Action<float> OnSensitivityChanged;
    public GameObject settingsMenu;
    public void SetSensitivity(float value)
    {
        OnSensitivityChanged?.Invoke(value);
    }


    private float sens=90f;
    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        SetSensitivity(DataManager.Instance.LoadData().sensitivity);
    }

    public string[] levels;
    public string CurrentLevel;

    public enum GameState
    {
        InGame,
        Paused,
        MainMenu
    };

    public GameState gameState;

    public void SettingsMenu()
    {
        settingsMenu.SetActive(true);
        settingsMenu.GetComponent<SettingsScript>().OpenPanel();
    }
    public void SetCurrentLevel(int input)
    {
        if (input < levels.Length && input >= 0)
            CurrentLevel = levels[input];

        Debug.Log("Level Set:"+CurrentLevel);
    }

    public string GetCurrentLevel()
    {
        return CurrentLevel;
    }

    public void PlayGame()
    {
        gameState = GameState.InGame;
        FadeManager.Instance.FadeOutToIn(() =>
        {
            SceneManager.LoadScene(1);
        });

    }

    public void MainMenu()
    {
        gameState = GameState.MainMenu;
        AudioManager.Instance.PlayMusic(AudioManager.Instance.mainTheme, .5f);
        FadeManager.Instance.FadeOutToIn(() =>
        {
            SceneManager.LoadScene(0);
        });
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Update()
    {
        switch(gameState)
        {
            case GameState.InGame:
                Time.timeScale = 1f;
                break;
            case GameState.MainMenu:
                Time.timeScale = 1f;
                break;
            case GameState.Paused:
                Time.timeScale = 0f;
                break;
        }
    }
}
