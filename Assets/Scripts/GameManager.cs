using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
    
public class GameManager : MonoBehaviour
{
    [SerializeField] public PlayerScript player;
    [SerializeField] private GameObject deathZone;
    [SerializeField] private GameObject Wall_R;
    [SerializeField] private GameObject Wall_L;
    [SerializeField] private ObstacleManager obstacleManager;

    public CameraManager cameraManager;

    private float deathZoneOffsetY;
    private int score = 0;

    // Use this for initialization
    void Start()
    {
        deathZoneOffsetY = deathZone.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.y >= cameraManager.mainCamera.transform.position.y)
        {
            deathZone.transform.position = new Vector3(cameraManager.mainCamera.initialPos.x, cameraManager.mainCamera.transform.position.y + deathZoneOffsetY, 0.0f);
            Wall_R.transform.position = new Vector3(Wall_R.transform.position.x, player.transform.position.y, 0.0f);
            Wall_L.transform.position = new Vector3(Wall_L.transform.position.x, player.transform.position.y, 0.0f);
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
    }
    public int GetScore()
    {
        return score;
    }

    public void SetScore(int score)
    {
        this.score = score;
    }
    public void RestartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
}