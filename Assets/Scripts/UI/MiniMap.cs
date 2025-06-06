using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    public float height = 100f;
    public Transform player;

    private void LateUpdate()
    {
        if(player!=null)
        {
            Vector3 newPosition = player.position;
            newPosition.y += height;
            transform.position = newPosition;

            transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);

        }
    }
}
