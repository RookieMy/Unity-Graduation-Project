using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCameraManager : MonoBehaviour
{
    public Camera[] portalCameras; 
    public RenderTexture portalTexture;

    private int currentActiveCameraIndex = -1;

    

    public void SetActiveCamera(int index)
    {
        

        
        if (index >= 0 && index < portalCameras.Length)
        {
            currentActiveCameraIndex = index;
            portalCameras[currentActiveCameraIndex].targetTexture = portalTexture;
        }
    }
}
