using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deathArea : MonoBehaviour
{
    public Transform checkpoint;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.transform.position = checkpoint.position;
            other.GetComponent<Player3DController>().OnDeath();
        }
    }
}
