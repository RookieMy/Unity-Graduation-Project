using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCameraManager : MonoBehaviour
{
    public Camera[] portalCameras; // Her bölüm için bir portal kamera
    public RenderTexture portalTexture; // Tüm kameraların paylaşacağı RenderTexture

    private int currentActiveCameraIndex = -1;

    

    public void SetActiveCamera(int index)
    {
        

        // Yeni aktif kamerayı devreye al
        if (index >= 0 && index < portalCameras.Length)
        {
            currentActiveCameraIndex = index;
            portalCameras[currentActiveCameraIndex].targetTexture = portalTexture;
        }
    }
}
