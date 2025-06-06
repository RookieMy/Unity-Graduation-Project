using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameManager : MonoBehaviour
{
    public bool isSolved=false;
    public Transform gamePlacement;
    public GameObject gamePlace;
    [Header("Refferences")]
    public GameObject game;
    public Transform wheelFocusPoint;

    [Header("Settings")]
    public float interactionDistance = 3f;
    public float focusDuration = 1.5f;

    private Camera mainCamera;
    private bool isFocusing = false;
    private bool isInRange = false;

    private Vector3 originalCamPos;
    private Quaternion originalCamRot;
    public GameObject exit;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (!isFocusing)
        {
            float distance = Vector3.Distance(mainCamera.transform.position, transform.position);
            if (distance <= interactionDistance)
            {
                if (!isInRange)
                {
                    isInRange = true;
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    StartCoroutine(FocusOnWheel());
                }
            }
            else
            {
                if (isInRange)
                {
                    isInRange = false;
                }
            }
        }
    }

    private IEnumerator FocusOnWheel()
    {
        isFocusing = true;
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
        
        if (game != null)
        {
            Cursor.lockState = CursorLockMode.None;
            game.SetActive(true);
            game.transform.GetChild(0).GetComponent<PipeGamePanel>().StartGame();
            mainCamera.GetComponent<Player3DLook>().SetInteraction(false);
            
            
        }

    }

    public void CloseWheelUI()
    {
        // Arayüzü kapat
        if (game != null)
            game.SetActive(false);

        mainCamera.GetComponent<Player3DLook>().SetInteraction(true);
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
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ExitGame(bool input)
    {
        Cursor.lockState = CursorLockMode.Locked;
        if (input)
            WinGame();
        CloseWheelUI();
    }

    void WinGame()
    {
        if (exit != null) exit.SetActive(true);

        StartCoroutine(Dissolve());
    }

    private IEnumerator Dissolve()
    {
        float t = 0f;
        while (t < 5f)
        {
            t += Time.deltaTime;
            float ratio = t / focusDuration;

            // Lerp pozisyon
            gamePlace.transform.position = Vector3.Lerp(gamePlace.transform.position, gamePlacement.position, ratio);
            // Slerp rotasyon
            gamePlace.transform.rotation = Quaternion.Slerp(gamePlace.transform.rotation, gamePlacement.rotation, ratio);

            yield return null;
        }

        gamePlace.SetActive(false);
    }
}
