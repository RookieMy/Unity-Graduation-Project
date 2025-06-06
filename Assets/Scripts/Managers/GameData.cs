[System.Serializable]
public class GameData
{
    public float MazeLevelHard;
    public float MazeLevelEasy;
    public float MazeLevelMid;
    public float PlatformHardLevel;
    public float PlatformEasyLevel;
    public float volumeMaster;
    public float volumeSFX;
    public float volumeMusic;
    public float volumeAmbience;
    public float sensitivity;

    
    public GameData()
    {
        MazeLevelHard = MazeLevelEasy = MazeLevelMid = PlatformHardLevel = PlatformEasyLevel = 0f;
        volumeAmbience = 1f;
        volumeMaster = 1f;
        volumeMusic = 1f;
        volumeSFX = 1f;
        sensitivity = 80;
    }
}
