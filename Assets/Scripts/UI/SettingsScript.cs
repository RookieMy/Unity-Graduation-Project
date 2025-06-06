using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingsScript : MonoBehaviour
{
    GameData currentSettings;
    public Slider masterVolume,musicVolume,SFXVolume,ambienceVolume, sens;

    private void Start()
    {
        currentSettings = DataManager.Instance.LoadData();
    }

    public void SaveSettings()
    {
        currentSettings.sensitivity = (float)Math.Round(sens.value, 1);
        currentSettings.volumeMaster = (float)Math.Round(masterVolume.value, 1);
        currentSettings.volumeMusic = (float)Math.Round(musicVolume.value, 1);
        currentSettings.volumeSFX = (float)Math.Round(SFXVolume.value, 1);
        currentSettings.volumeAmbience = (float)Math.Round(ambienceVolume.value, 1);
        DataManager.Instance.SaveData(currentSettings);
        gameObject.SetActive(false);
    }

    public void CancelSettings()
    {
        masterVolume.value = currentSettings.volumeMaster;
        musicVolume.value = currentSettings.volumeMusic;
        SFXVolume.value = currentSettings.volumeSFX;
        ambienceVolume.value = currentSettings.volumeAmbience;
        sens.value = currentSettings.sensitivity;

        AudioManager.Instance.SetMasterVolume(masterVolume.value);
        AudioManager.Instance.SetSFXVolume(SFXVolume.value);
        AudioManager.Instance.SetMusicVolume(musicVolume.value);
        AudioManager.Instance.SetAmbienceVolume(ambienceVolume.value);

        GameManager.Instance.SetSensitivity(sens.value);

        gameObject.SetActive(false);
    }

    public void OpenPanel()
    {
        masterVolume.value = currentSettings.volumeMaster;
        musicVolume.value = currentSettings.volumeMusic;
        SFXVolume.value = currentSettings.volumeSFX;
        ambienceVolume.value = currentSettings.volumeAmbience;
        sens.value = currentSettings.sensitivity;
    }
}
