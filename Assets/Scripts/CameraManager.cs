using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraManager : MonoBehaviour
{

    public CameraScript mainCamera;

    public bool shaking;
    public float totalShakeTime = 0.5f;
    public float shakeAmount = 1.0f;

    float shakeTimer;
    Vector3 startingPos;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = mainCamera.GetComponent<CameraScript>();

        startingPos = mainCamera.transform.position;
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
}
