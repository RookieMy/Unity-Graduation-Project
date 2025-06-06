using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pushBlock : MonoBehaviour
{
    public Transform hitPosition;
    public float pushSpeed=10f;
    public float backTimer = 1f;

    public float force=15f;

    private bool isPushing=false;
    private Vector3 startPosition;
    private Rigidbody rb;

    private void Start()
    {
        startPosition = transform.localPosition;
        rb = GetComponent<Rigidbody>();
    }
    public void triggerBlock()
    {
        rb.isKinematic = false;
        if (!isPushing)
            isPushing = true;
        
        StartCoroutine(backPosition());
    }

    private IEnumerator backPosition()
    {
        yield return new WaitForSeconds(backTimer);
        isPushing = false;
        rb.isKinematic = true;
        transform.localPosition = startPosition;
    }

    private void Update()
    {
        if (isPushing)
        {

            // Rigidbody ile hareket
            rb.MovePosition(Vector3.MoveTowards(transform.position, hitPosition.position, pushSpeed * Time.deltaTime));

            // Ýtme iþlemini sonlandýr
            if (Vector3.Distance(transform.position, hitPosition.position) < 0.1f)
            {
                isPushing = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            other.transform.GetComponent<Rigidbody>().AddForce(Vector3.right*force,ForceMode.Impulse);
        }
    }
}
