using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }
    CinemachineVirtualCamera cam;
    float ShakeTimer = 0;

    void Awake()
    {
        Instance = this;
        cam = GetComponent<CinemachineVirtualCamera>();
    }

    public void ShakeCamera(float Intensity, float Time)
    {
        GetCameraNoiseComponent().m_AmplitudeGain = Intensity;
        ShakeTimer = Time;
    }


    void Update()
    {
        if (ShakeTimer > 0)
        {
            ShakeTimer -= Time.deltaTime;
            if (ShakeTimer <= 0)
                GetCameraNoiseComponent().m_AmplitudeGain = 0f;
        }
    }

    CinemachineBasicMultiChannelPerlin GetCameraNoiseComponent()
    {
        return cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }
}
