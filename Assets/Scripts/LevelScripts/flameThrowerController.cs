using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrowerController : MonoBehaviour
{
    public List<FlameThrower> flameThrowers = new List<FlameThrower>();
    public float delayBetweenActivations = 3f;

    private bool isRunning = false;

    private void OnTriggerExit(Collider other)
    {
        if (!isRunning)
            StartCoroutine(ActivateFlamesInOrder());
    }

    private IEnumerator ActivateFlamesInOrder()
    {
        isRunning = true;

        for (int i = 0; i < flameThrowers.Count; i++)
        {
            yield return new WaitForSeconds(delayBetweenActivations);
            flameThrowers[i].SetActive(true);
            
        }

        isRunning = false;
    }

    public void ResetFlames()
    {
        StopAllCoroutines();
        isRunning = false;
        foreach (FlameThrower ft in flameThrowers)
            ft.SetActive(false);
    }
}
