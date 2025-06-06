using UnityEngine;
using System.Collections.Generic;

public class PipeGridManager : MonoBehaviour
{
    public PipeGamePanel panel;
    public List<PipeButton> pipes = new List<PipeButton>();
    public PipeButton startPipe;
    public PipeButton endPipe;
    public PipeButton entry;
    public PipeButton exit;

    public GameObject[] pipePreFabs;

    public void StartGame()
    {
        
        PipeButton[] temp = new PipeButton[72];
        temp[0] = startPipe;
        temp[1] = entry;
        temp[70] = exit;
        temp[71] = endPipe;
        for (int i = 2; i < 70; i++)
        {
            GameObject newPipe = Instantiate(pipePreFabs[Random.Range(0,2)], transform.parent);
            temp[i] = newPipe.GetComponent<PipeButton>();

            newPipe.transform.SetSiblingIndex(i);
        }

        pipes = new List<PipeButton>(temp);
        InvokeRepeating("CheckWaterFlow", 1f, 0.5f);
        
    }

    public void ResumeGame()
    {
        InvokeRepeating("CheckWaterFlow", 1f, 0.5f);
    }

    void CheckWaterFlow()
    {
        foreach (PipeButton pipe in pipes)
        {
            pipe.hasWater = false; // Önce tüm borularý sýfýrla
        }

        

        if (startPipe != null)
        {
            FlowThroughPipe(startPipe,0,0);
        }

        if (endPipe.hasWater)
        {
            CancelInvoke("CheckWaterFlow");
            panel.FinishGame();
        }
    }

    void FlowThroughPipe(PipeButton pipe, int input, int prevInput)
    {
        Debug.Log(input);
        pipe.hasWater = true;

        if (pipe.type == PipeButton.pipeType.End)
            return;

        object nextInput;
 
        int direction=prevInput-input;
        if (pipe.type == PipeButton.pipeType.Start)
        {
            nextInput = new int();

            switch (pipe.rotationState)
            {
                case 0:
                    if (input + 1 < pipes.Count)
                        nextInput = input + 1;
                    break;
                case 90:
                    if (input - 12 >= 0)
                        nextInput = input - 12;
                    break;
                case 180:
                    if (input - 1 >= 0)
                        nextInput = input - 1;
                    break;
                case 270:
                    if (input + 12 < pipes.Count)
                        nextInput = input + 12;
                    break;
            }


            Debug.Log("Next:" + (int)nextInput);
            if (CheckRotation(pipes[(int)nextInput], input, (int)nextInput)&& !pipes[(int)nextInput].hasWater)
                FlowThroughPipe(pipes[(int)nextInput], (int)nextInput, input);
        }
        else if (pipe.type == PipeButton.pipeType.TypeFlat)
        {
            nextInput = new int();
            nextInput = input;
            if (input-direction < pipes.Count && input-direction >= 0)
                nextInput = input-direction;

            if (CheckRotation(pipes[(int)nextInput], input, (int)nextInput) && !pipes[(int)nextInput].hasWater)
                FlowThroughPipe(pipes[(int)nextInput], (int)nextInput, input);
        }
        else if(pipe.type == PipeButton.pipeType.TypeL)
        {
            int state = pipe.rotationState;
            nextInput = new int();
            nextInput = input;
            switch(direction)
            {
                case -1:
                    if (state == 90 && input - 12 >= 0)
                        nextInput = input - 12;
                    else if (state == 180 && input + 12 < pipes.Count)
                        nextInput = input + 12;
                    break;
                case 1:
                    if (state == 0 && input - 12 >= 0)
                        nextInput = input - 12;
                    else if (state == 270 && input + 12 < pipes.Count)
                        nextInput = input + 12;
                    break;
                case -12:
                    if (state == 90 && input - 1 >= 0)
                        nextInput = input - 1;
                    else if (state == 0 && input + 1 < pipes.Count)
                        nextInput = input + 1;
                    break;
                case 12:
                    if (state == 180 && input - 1 >= 0)
                        nextInput = input - 1;
                    else if (state == 270 && input + 1 < pipes.Count)
                        nextInput = input + 1;
                    break;
            }

            if (CheckRotation(pipes[(int)nextInput], input, (int)nextInput) && !pipes[(int)nextInput].hasWater)
                FlowThroughPipe(pipes[(int)nextInput], (int)nextInput, input);
        }

        else if(pipe.type==PipeButton.pipeType.Type4)
        {
            int[] temp=new int[4];
            temp[0] = input - 1;
            temp[1] = input + 1;
            temp[2] = input - 12;
            temp[3] = input + 12;

            for(int i = 0;i<4;i++)
            { 
                if (temp[i] < 0 || temp[i] >= pipes.Count)
                    temp[i] = input;
            }

            foreach(int i in temp)
            {
                if(CheckRotation(pipes[i], input, i) && !pipes[i].hasWater)
                {
                    FlowThroughPipe(pipes[i], i, input);
                }
            }

            
        }
    }

    bool CheckRotation(PipeButton pipe,int input,int nextInput)
    {
        int state = pipe.rotationState;
        bool check = false;
        if (input == nextInput)
            check = false;

        if(pipe.type==PipeButton.pipeType.TypeFlat)
        {
            if ((input - nextInput == 1 || input - nextInput == -1) && (state == 90 || state == 270))
                check = true;
            else if ((input - nextInput == 12 || input - nextInput == -12) && (state == 0 || state == 180))
                check = true;

            else
                check = false;
        }
        else if (pipe.type == PipeButton.pipeType.TypeL)
        {
            if (input - nextInput == -1 && (state == 90 || state == 180))
                check = true;
            else if (input - nextInput == 1 && (state == 0 || state == 270))
                check = true;
            else if (input - nextInput == -12 && (state == 0 || state == 90))
                check = true;
            else if (input - nextInput == 12 && (state == 180 || state == 270))
                check = true;
            else
                check = false;
        }

        else if (pipe.type == PipeButton.pipeType.Type4)
            check = true;

        else if(pipe.type==PipeButton.pipeType.End)
        {
            if (input - nextInput == -1)
                check = true;
        }

        return check;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            panel.ExitGame(false);

            
    }
}
