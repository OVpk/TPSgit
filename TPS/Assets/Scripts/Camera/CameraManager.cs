using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void SwitchCameraTo(CameraController newCamera)
    {
        if (Camera.main != null)
        {
            Camera.main.transform.parent.gameObject.SetActive(false);
        }
        newCamera.gameObject.SetActive(true);
    }
}
