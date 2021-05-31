using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraManager : MonoBehaviour
{
    [SerializeField] private CameraScript mainCamera;

    [SerializeField] private float totalShakeTime = 0.5f;
    [SerializeField] private float shakeAmount = 1.0f;

    private bool shaking;
    private float shakeTimer;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = mainCamera.GetComponent<CameraScript>();

        StopShake();
    }

    // Update is called once per frame
    void Update()
    {
        if (shaking)
        {
            if (shakeTimer >= 0.0f)
            {
                shakeTimer -= Time.deltaTime;
                Shake();
            }
            else
            {
                StopShake();
            }
        }
    }

    void Shake()
    {
        Vector3 rotationAmount = Random.insideUnitSphere * shakeTimer * shakeAmount;
        rotationAmount.z = 0;

        mainCamera.transform.localRotation = Quaternion.Euler(rotationAmount);
    }

    private void StopShake()
    {
        mainCamera.transform.localRotation = Quaternion.Euler(0, 0, 0);
        shakeTimer = totalShakeTime;
        shaking = false;
    }
    public void ResetCameraManager()
    {
        StopShake();
        mainCamera.ResetCamera();
    }

    public void setShaking(bool b)
    {
        shaking = b;
    }
}
