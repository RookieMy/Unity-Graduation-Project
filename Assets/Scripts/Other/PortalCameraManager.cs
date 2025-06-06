using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCameraManager : MonoBehaviour
{
    public Camera[] portalCameras; // Her b�l�m i�in bir portal kamera
    public RenderTexture portalTexture; // T�m kameralar�n payla�aca�� RenderTexture

    private int currentActiveCameraIndex = -1;

    

    public void SetActiveCamera(int index)
    {
        

        // Yeni aktif kameray� devreye al
        if (index >= 0 && index < portalCameras.Length)
        {
            currentActiveCameraIndex = index;
            portalCameras[currentActiveCameraIndex].targetTexture = portalTexture;
        }
    }
}
