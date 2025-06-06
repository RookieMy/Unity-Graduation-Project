using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turnBlock : MonoBehaviour
{
    public float speed;
    private void LateUpdate()
    {
        transform.Rotate(new Vector3( Time.deltaTime * speed,0));
    }
}
