using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitPuzzleManager : MonoBehaviour
{
    public pushBlock[] blocks;
    public float startTimer = 2f;
    public float timeBetweenTrigger = 1f;

    private Coroutine currentState = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player") && currentState == null)
        {
            currentState=StartCoroutine(startPuzzle());
            
        }
    }

    private IEnumerator startPuzzle()
    {
        yield return new WaitForSeconds(startTimer);
        Debug.Log("Hit");
        StartCoroutine(blockTimer());
    }

    private void OnTriggerExit(Collider other)
    {
        GetComponent<Collider>().isTrigger = false;
    }

    private IEnumerator blockTimer()
    {
        Debug.Log("Hit2");
        foreach (pushBlock block in blocks)
        {
            Debug.Log("Hit3");
            block.triggerBlock();
            yield return new WaitForSeconds(timeBetweenTrigger);
        }
        GetComponent<Collider>().isTrigger = true;
        currentState = null;
    }
}
