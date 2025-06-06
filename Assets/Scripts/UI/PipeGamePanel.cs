using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeGamePanel : MonoBehaviour
{
    public PipeGridManager game;
    public MinigameManager manager;

    private bool isCreated = false;
    public void StartGame()
    {
        for(int i =0;i<transform.childCount;i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }

        if (!isCreated)
        {
            game.StartGame();
            isCreated = true;
        }
        else
            game.ResumeGame();
    }

    public void ExitGame(bool input)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        manager.ExitGame(input);
    }

    public void FinishGame()
    {
        ExitGame(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("A");
            StartGame();
        }
    }
}
