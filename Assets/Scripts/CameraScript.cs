using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    [SerializeField] private GameObject player;

    Vector3 initialPos;

    // Start is called before the first frame update
    void Start()
    {
        initialPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.y >= transform.position.y)
        {
            transform.position = new Vector3(initialPos.x, player.transform.position.y, initialPos.z);
           
        }
    }

    public void ResetCamera()
    {
        transform.position = initialPos;
    }

}
