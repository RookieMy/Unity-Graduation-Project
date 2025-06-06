using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class lavaPattern : MonoBehaviour
{
    public List<    FallingPlatform> platforms;
    public TextMeshProUGUI text;
    public void SelectPlatforms()
    {
        int[] index=new int[8];
        index[0] = Random.Range(0, 5);
        for (int i = 1; i < 8; i++)
        {
            if (index[i - 1] % 5 == 0)
                index[i] = Random.Range(0, 2) + (i * 5);
            else if (index[i - 1] % 5 == 4)
                index[i] = Random.Range(3, 5) + (i * 5);
            else
                index[i] = Random.Range((index[i - 1] % 5) - 1, (index[i - 1] % 5) + 2) + (i * 5);
        }

        text.SetText("");

        foreach(int a in index)
        {
            Debug.Log(a.ToString());
            text.SetText(text.text + ((a%5)+1).ToString() + " - ");
            platforms[a].canFall = false;
        }

        text.SetText(text.text.Substring(0, text.text.Length - 3));

    }
}
