using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PipeButton : MonoBehaviour
{
    private RectTransform rectTransform;
    private Quaternion targetRotation;
    private bool isRotating = false;

    public int rotationState = 0; // 0, 90, 180, 270 derece
    public bool hasWater = false;

    public bool isButton = true;
    public float rotationSpeed = 200f;

    public Image image;

    public enum pipeType
    {
        Start,
        TypeL,
        TypeFlat,
        Type4,
        End
    };

    public pipeType type = pipeType.TypeFlat;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        targetRotation = rectTransform.rotation;

        if (isButton)
            GetComponent<Button>().onClick.AddListener(RotatePipe);
    }

    void Update()
    {
        if (hasWater)
            image.color = Color.cyan;
        else
            image.color = Color.white;

        if (isRotating)
        {
            rectTransform.rotation = Quaternion.RotateTowards(rectTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            if (Quaternion.Angle(rectTransform.rotation, targetRotation) < 0.1f)
            {
                rectTransform.rotation = targetRotation;
                isRotating = false;
            }
        }
    }

    public void RotatePipe()
    {
        if (isButton && !isRotating)
        {
            rotationState = (rotationState + 90) % 360;
            targetRotation = Quaternion.Euler(0, 0, rotationState);
            isRotating = true;
        }
    }
}
