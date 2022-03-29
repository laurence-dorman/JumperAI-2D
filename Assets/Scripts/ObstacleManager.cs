using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    [SerializeField] private PlayerScript player;
    [SerializeField] private GameObject prefab;
    [SerializeField] private GameObject spawnCollider;
    [SerializeField] private GameObject scoreCollider;
    [SerializeField] private CameraScript cameraScript;

    private Queue<System.Tuple<GameObject, GameObject, GameObject>> obstacles = new Queue<System.Tuple<GameObject, GameObject, GameObject>>();

    // Start is called before the first frame update
    void Start()
    {
        AddObstacle();
    }

    // Update is called once per frame
    void Update()
    {
        if (obstacles.Count != 0)
        {
            if (IsOffScreen(obstacles.Peek().Item1.transform.position))
            {
                RemoveObstacle();
            }
        }
    }

    public void AddObstacle()
    {
        float translation = Random.Range(-2f, 2f);
        float gap = Random.Range(-0.5f, 0.5f);
        Vector2 obstacleLPos = new Vector2((-3f + (gap * 0.5f)) + translation, player.transform.position.y + 7.5f);
        Vector2 obstacleRPos = new Vector2((3f - (gap * 0.5f)) + translation, player.transform.position.y + 7.5f);

        GameObject leftObstacle = Instantiate(prefab, obstacleLPos, Quaternion.identity);
        GameObject rightObstacle = Instantiate(prefab, obstacleRPos, Quaternion.identity);
        GameObject spawnColliderObject = Instantiate(spawnCollider, new Vector2(0f, leftObstacle.transform.position.y - 3f), Quaternion.identity);
        GameObject scoreColliderObject = Instantiate(scoreCollider, new Vector2(0f, player.transform.position.y + 7.5f), Quaternion.identity);

        obstacles.Enqueue(new System.Tuple<GameObject, GameObject, GameObject>(leftObstacle, rightObstacle, spawnColliderObject)); // add object to queue
        Debug.Log("bing");
    }

    public bool IsOffScreen(Vector3 pos)
    {
        if (Mathf.Abs(cameraScript.transform.position.y - pos.y) >= 6.0f)
        {
            return true;
        }
        return false;
    }

    public void RemoveObstacle()
    {
        if (obstacles.Count == 0) return;
        Destroy(obstacles.Peek().Item1); // destroy object
        Destroy(obstacles.Peek().Item2); // destroy object
        Destroy(obstacles.Peek().Item3); // destroy object
        obstacles.Dequeue(); // remove from queue
    }

    public void ResetObstacleManager()
    {
        foreach (var obstacle in obstacles)
        {
            Destroy(obstacle.Item1);
            Destroy(obstacle.Item2);
            Destroy(obstacle.Item3);
        }
        obstacles.Clear();

        Start();
    }
}
