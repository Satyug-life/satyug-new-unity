using Cinemachine;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera[] virtualCameras;

    private void Awake()
    {
        ChangeCameraPriority(0);
    }

    public void ChangeCameraPriority(int selectedCameraIndex)
    {
        for (int i = 0; i < virtualCameras.Length; i++)
        {
            virtualCameras[i].Priority = 0;

            if(i == selectedCameraIndex) 
            {
                virtualCameras[selectedCameraIndex].Priority = 11; break;
            }
        }
    }
}
