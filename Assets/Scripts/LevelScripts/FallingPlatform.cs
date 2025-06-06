using System.Collections;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public float delayBeforeFall = 3f;
    public float fallDistance = 2f;
    public float fallSpeed = 2f;
    public float returnDelay = 2f;
    public float returnSpeed = 2f;

    private Vector3 originalPosition;
    private Vector3 targetPosition;
    private bool isFalling = false;
    public bool randomPosition=false;

    public bool canFall = true;


    void Start()
    {
        if (randomPosition)
            transform.localPosition = new Vector3(Random.Range(transform.localPosition.x - .002f, transform.localPosition.x + .002f), transform.localPosition.y, transform.localPosition.z);
        originalPosition = transform.position;
        targetPosition = originalPosition + Vector3.down * fallDistance;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isFalling && collision.transform.CompareTag("Player") && canFall)
        {
            StartCoroutine(FallAndReturn());
        }
    }

    private IEnumerator FallAndReturn()
    {
        isFalling = true;

        yield return new WaitForSeconds(delayBeforeFall);

        // Aþaðý iner
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, fallSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(returnDelay);

        // Yukarý çýkar
        while (Vector3.Distance(transform.position, originalPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, originalPosition, returnSpeed * Time.deltaTime);
            yield return null;
        }

        isFalling = false;
    }
}
