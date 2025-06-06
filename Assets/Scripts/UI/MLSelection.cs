using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MLSelection : MonoBehaviour
{
    public MazeGenerator maze;
    public WheelInteract wheel;
    public GameObject panel;

    public void EasyLevel()
    {
        Debug.Log("Easy");
        GameManager.Instance.SetCurrentLevel(1);
        maze.ClearMaze();
        maze.rows = 15;
        maze.columns = 15;
        maze.StartSpawn();
        wheel.CloseWheelUI();
        panel.SetActive(false);
    }

    public void MiddleLevel()
    {
        Debug.Log("Mid");
        GameManager.Instance.SetCurrentLevel(2);
        maze.ClearMaze();
        maze.rows = 20;
        maze.columns = 20;
        maze.StartSpawn();
        wheel.CloseWheelUI();
        panel.SetActive(false);
    }

    public void HardLevel()
    {
        Debug.Log("Hard");
        GameManager.Instance.SetCurrentLevel(3);
        maze.ClearMaze();
        maze.rows = 25;
        maze.columns = 25;
        maze.StartSpawn();
        wheel.CloseWheelUI();
        panel.SetActive(false);
    }
}
