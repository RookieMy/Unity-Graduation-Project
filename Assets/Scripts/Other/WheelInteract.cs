using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelInteract : MonoBehaviour
{
    [Header("Refferences")]
    public Transform wheelFocusPoint;
    public GameObject interactPrompt;
    public GameObject levelSelectionUI;

    [Header("Settings")]
    public float interactionDistance = 3f;
    public float focusDuration = 1.5f;

    private Camera mainCamera;
    private bool isFocusing = false;
    private bool isInRange = false;

    private Vector3 originalCamPos;
    private Quaternion originalCamRot;

    private void Start()
    {
        mainCamera = Camera.main;
        if(interactPrompt!=null)
        {
            interactPrompt.SetActive(false);
        }
    }

    private void Update()
    {
        if(!isFocusing)
        {
            float distance = Vector3.Distance(mainCamera.transform.position, transform.position);
            if(distance<=interactionDistance)
            {
                if(!isInRange)
                {
                    isInRange = true;
                    if (interactPrompt != null)
                        interactPrompt.SetActive(true);
                }

                if(Input.GetKeyDown(KeyCode.E))
                {
                    StartCoroutine(FocusOnWheel());
                }
            }
            else
            {
                if(isInRange)
                {
                    isInRange = false;
                    if (interactPrompt != null)
                        interactPrompt.SetActive(false);
                }
            }
        }
    }

    private IEnumerator FocusOnWheel()
    {
        isFocusing = true;
        if (interactPrompt != null)
            interactPrompt.SetActive(false);

        originalCamPos = mainCamera.transform.position;
        originalCamRot = mainCamera.transform.rotation;

        mainCamera.GetComponent<Player3DLook>().SetInteraction(false);

        float t = 0f;

        while (t < focusDuration)
        {
            t += Time.deltaTime;
            float ratio = t / focusDuration;

            // Lerp pozisyon
            mainCamera.transform.position = Vector3.Lerp(originalCamPos, wheelFocusPoint.position, ratio);
            // Slerp rotasyon
            mainCamera.transform.rotation = Quaternion.Slerp(originalCamRot, wheelFocusPoint.rotation, ratio);

            yield return null;
        }

        if(levelSelectionUI!=null)
        {
            levelSelectionUI.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
        }

    }

    public void CloseWheelUI()
    {
        // Arayüzü kapat
        if (levelSelectionUI != null)
            levelSelectionUI.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        // Kamerayý eski pozisyonuna döndür
        StartCoroutine(ReturnToOriginalPosition());
    }

    private IEnumerator ReturnToOriginalPosition()
    {
        isFocusing = true;

        Vector3 startPos = mainCamera.transform.position;
        Quaternion startRot = mainCamera.transform.rotation;

        float t = 0f;
        while (t < focusDuration)
        {
            t += Time.deltaTime;
            float ratio = t / focusDuration;

            // Lerp pozisyon
            mainCamera.transform.position = Vector3.Lerp(startPos, originalCamPos, ratio);
            // Slerp rotasyon
            mainCamera.transform.rotation = Quaternion.Slerp(startRot, originalCamRot, ratio);

            yield return null; // Bir sonraki frame'e geç
        }

        // Kamerayý tam eski pozisyonuna ayarla
        mainCamera.transform.position = originalCamPos;
        mainCamera.transform.rotation = originalCamRot;

        isFocusing = false;

        mainCamera.GetComponent<Player3DLook>().SetInteraction(true);
    }


}
