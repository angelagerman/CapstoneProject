using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public void SwapActiveCamera(Camera currentCamera, Camera newCamera)
    {
        if (currentCamera != null)
        {
            currentCamera.gameObject.SetActive(false);
        }
        
        if (newCamera != null)
        {
            newCamera.gameObject.SetActive(true);
        }
    }
}
