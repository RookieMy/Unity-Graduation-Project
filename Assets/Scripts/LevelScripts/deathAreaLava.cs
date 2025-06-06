using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deathAreaLava : MonoBehaviour
{
    public Transform checkpoint;
    public FlameThrowerController controller;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            controller.ResetFlames();
            other.transform.position = checkpoint.position;
            other.GetComponent<Player3DController>().OnDeath();
        }
    }
}
