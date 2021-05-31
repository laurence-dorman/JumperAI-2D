using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    public GameObject player;
    public GameObject deathZone;
    public GameObject Wall_R;
    public GameObject Wall_L;

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
            transform.position = new Vector3(initialPos.x, player.transform.position.y, initialPos.z);
            deathZone.transform.position = new Vector3(initialPos.x, transform.position.y + deathZoneOffsetY, 0.0f);
            Wall_R.transform.position = new Vector3(Wall_R.transform.position.x, player.transform.position.y, 0.0f);
            Wall_L.transform.position = new Vector3(Wall_L.transform.position.x, player.transform.position.y, 0.0f);
        }
    }

    public void ResetCamera()
    {
        transform.position = initialPos;
        deathZone.transform.position = new Vector3(initialPos.x, transform.position.y + deathZoneOffsetY, initialPos.z);
        Wall_R.transform.position = new Vector3(Wall_R.transform.position.x, 0.0f, 0.0f);
        Wall_L.transform.position = new Vector3(Wall_L.transform.position.x, 0.0f, 0.0f);
    }

}
