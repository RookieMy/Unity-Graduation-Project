using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BestTimeScript : MonoBehaviour
{
    public TextMeshProUGUI text;
    public enum Type
    {
        MazeEasy,
        MazeMid,
        MazeHard,
        PlatformEasy,
        PlatformHard
    };

    public Type type;

    private void Start()
    {
        GameData data = DataManager.Instance.LoadData();

        switch(type)
        {
            case Type.MazeEasy:
                text.SetText("Best Score:" + data.MazeLevelEasy.ToString() + "s");
                break;
            case Type.MazeMid:
                text.SetText("Best Score:" + data.MazeLevelMid.ToString() + "s");
                break;
            case Type.MazeHard:
                text.SetText("Best Score:" + data.MazeLevelHard.ToString() + "s");
                break;
            case Type.PlatformEasy:
                text.SetText("Best Score:\n" + data.PlatformEasyLevel.ToString() + "s");
                break;
            case Type.PlatformHard:
                text.SetText("Best Score:\n" + data.PlatformHardLevel.ToString() + "s");
                break;
        }
    }
}
