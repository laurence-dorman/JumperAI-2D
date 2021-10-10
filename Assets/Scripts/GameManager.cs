using System.Collections;
using UnityEngine;
    
public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerScript player;
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private GameObject deathZone;
    [SerializeField] private GameObject Wall_R;
    [SerializeField] private GameObject Wall_L;

    float deathZoneOffsetY;
    Vector3 initialPos;

    // Use this for initialization
    void Start()
    {
        deathZoneOffsetY = deathZone.transform.position.y;
        initialPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.y >= transform.position.y)
        {
            deathZone.transform.position = new Vector3(initialPos.x, transform.position.y + deathZoneOffsetY, 0.0f);
            Wall_R.transform.position = new Vector3(Wall_R.transform.position.x, player.transform.position.y, 0.0f);
            Wall_L.transform.position = new Vector3(Wall_L.transform.position.x, player.transform.position.y, 0.0f);
        }
    }

    public void RestartGame()
    {
        StartCoroutine("RestartGameCo");
    }

    public IEnumerator RestartGameCo()
    {
        yield return new WaitForSeconds(0.5f);

        deathZone.transform.position = new Vector3(initialPos.x, transform.position.y + deathZoneOffsetY, initialPos.z);
        Wall_R.transform.position = new Vector3(Wall_R.transform.position.x, 0.0f, 0.0f);
        Wall_L.transform.position = new Vector3(Wall_L.transform.position.x, 0.0f, 0.0f);

        player.ResetPlayer();
        cameraManager.ResetCameraManager();
    }
}