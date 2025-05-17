using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    public CameraController cameraToDisplay;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            CameraManager.Instance.SwitchCameraTo(cameraToDisplay);
        }
    }
}
