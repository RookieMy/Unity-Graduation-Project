using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatfromScript : MonoBehaviour
{
    public enum MoveDirection
    {
        Back,
        Front,
        Up,
        Down,
        Right,
        Left
    };

    public MoveDirection direction = MoveDirection.Right;

    public bool isBreakable = false;
    public float breakTimer = 2f;

    public float range = 2f;
    public float speed = 2f;

    public Vector3 target;
    public Vector3 startPosition;

    private Vector3 previousPosition;
    [HideInInspector] public Vector3 deltaPos;
    private void Start()
    {
        startPosition = transform.position;
    }
    private void Update()
    {

        switch(direction)
        {
            case MoveDirection.Right:
                target = new Vector3(startPosition.x + range, startPosition.y,startPosition.z);
                transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
                if (transform.position == target)
                    direction = MoveDirection.Left;
                break;
            case MoveDirection.Left:
                target = new Vector3(startPosition.x - range, startPosition.y, startPosition.z);
                transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
                if (transform.position == target)
                    direction = MoveDirection.Right;
                break;
            case MoveDirection.Up:
                target = new Vector3(startPosition.x, startPosition.y + range, startPosition.z);
                transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
                if (transform.position == target)
                    direction = MoveDirection.Down;
                break;
            case MoveDirection.Down:
                target = new Vector3(startPosition.x, startPosition.y - range, startPosition.z);
                transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
                if (transform.position == target)
                    direction = MoveDirection.Up;
                break;
            case MoveDirection.Front:
                target = new Vector3(startPosition.x, startPosition.y, startPosition.z + range);
                transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
                if (transform.position == target)
                    direction = MoveDirection.Back;
                break;
            case MoveDirection.Back:
                target = new Vector3(startPosition.x, startPosition.y, startPosition.z - range);
                transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
                if (transform.position == target)
                    direction = MoveDirection.Front;
                break;

        }

        // Bu karedeki delta hesaplamasý:
        deltaPos = transform.position - previousPosition;
        previousPosition = transform.position;
    }

    private IEnumerator BreakTime()
    {
        yield return new WaitForSeconds(breakTimer);
        GetComponent<Collider>().enabled = false;
        GetComponent<MeshRenderer>().enabled=false;
        StartCoroutine(RegenTime());
    }

    private IEnumerator RegenTime()
    {
        yield return new WaitForSeconds(breakTimer);
        GetComponent<Collider>().enabled = true;
        GetComponent<MeshRenderer>().enabled = true;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player") && isBreakable)
        {
            Debug.Log("A");
            StartCoroutine(BreakTime());
        }
    }
    
}
