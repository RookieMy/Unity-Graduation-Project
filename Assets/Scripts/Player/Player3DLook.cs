using UnityEngine;

public class Player3DLook : MonoBehaviour
{
    public bool canMove = true;
    public Player3DController player;
    public GameObject cursor;

    [Range(50, 150)]
    public float sens = 100f;

    public Transform body;

    private float xRot = 0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (canMove && player != null && player.canMove)
        {
            float rotX = Input.GetAxis("Mouse X") * sens * Time.deltaTime;
            float rotY = Input.GetAxis("Mouse Y") * sens * Time.deltaTime;

            xRot -= rotY;
            xRot = Mathf.Clamp(xRot, -80f, 80f);

            transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
            body.Rotate(Vector3.up * rotX);
        }
    }

    public void SetInteraction(bool input)
    {
        canMove = input;
        player.canMove = input;
        cursor.SetActive(input);
    }

    void OnEnable()
    {
        GameManager.OnSensitivityChanged += UpdateSens;
    }

    void OnDisable()
    {
        GameManager.OnSensitivityChanged-= UpdateSens;
    }

    void UpdateSens(float value)
    {
        sens = value;
    }

}
