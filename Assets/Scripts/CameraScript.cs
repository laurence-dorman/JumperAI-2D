using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    public GameObject player;
    public GameObject deathZone;

    float deathZoneOffsetY;

    Vector3 initialPos;

    // Start is called before the first frame update
    void Start()
    {
        initialPos = transform.position;

        deathZoneOffsetY = deathZone.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.y >= transform.position.y)
        {
            transform.position = new Vector3(0.0f, player.transform.position.y, -10.0f);
            deathZone.transform.position = new Vector3(0.0f, transform.position.y + deathZoneOffsetY, 0.0f);
        }
    }

    public void ResetCamera()
    {
        transform.position = initialPos;
        deathZone.transform.position = new Vector3(initialPos.x, transform.position.y + deathZoneOffsetY, initialPos.z);
    }

}
