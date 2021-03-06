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

    public Vector2 holePos;

    private List<System.Tuple<GameObject, GameObject, GameObject>> obstaclesList = new List<System.Tuple<GameObject, GameObject, GameObject>>();

    // Start is called before the first frame update
    void Start()
    {
        AddObstacle();
    }

    // Update is called once per frame
    void Update()
    {
        if (obstaclesList.Count != 0)
        {
            if (IsOffScreen(obstaclesList[0].Item1.transform.position))
            {
                RemoveObstacle();
            }

            holePos = GetTargetHole();
        }
    }

    Vector2 GetTargetHole()
    {
        var bestObstacle = obstaclesList[0];
        for (int i = 0; i < obstaclesList.Count; i++)
        {
            if (player.transform.position.y >= obstaclesList[i].Item1.transform.position.y + 0.5f)
            {
                bestObstacle = obstaclesList[i + 1];
            }
        }
        return (bestObstacle.Item1.transform.position + bestObstacle.Item2.transform.position) / 2.0f;
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

        obstaclesList.Add(new System.Tuple<GameObject, GameObject, GameObject>(leftObstacle, rightObstacle, spawnColliderObject)); // add object to list
        
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
        if (obstaclesList.Count == 0) return;
        Destroy(obstaclesList[0].Item1); // destroy object
        Destroy(obstaclesList[0].Item2); // destroy object
        Destroy(obstaclesList[0].Item3); // destroy object
        obstaclesList.RemoveAt(0); // remove from queue
    }

    public void ResetObstacleManager()
    {
        foreach (var obstacle in obstaclesList)
        {
            Destroy(obstacle.Item1);
            Destroy(obstacle.Item2);
            Destroy(obstacle.Item3);
        }
        obstaclesList.Clear();

        Start();
    }
}
