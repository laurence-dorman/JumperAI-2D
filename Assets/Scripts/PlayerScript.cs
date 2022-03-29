using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float deathTimerTotal = 3.0f;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private ObstacleManager obstacleManager;

    private Rigidbody2D rigidBody;
    private Vector3 startingPos;
    private Quaternion startingRotation;
    private float deathTimer;
    private bool alive = true;
    private bool onRightWall = false;
    private bool onLeftWall = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        startingPos = transform.position;
        startingRotation = transform.rotation;
        deathTimer = deathTimerTotal;
    }

    // Update is called once per frame
    void Update()
    {
        if (alive)
        {
            if (Input.GetKeyDown("a") || Input.GetKeyDown("left") && !onLeftWall)
            {
                rigidBody.velocity = new Vector2(-speed * 0.5f, speed);
            }
            if (Input.GetKeyDown("d") || Input.GetKeyDown("right") && !onRightWall)
            {
                rigidBody.velocity = new Vector2(speed * 0.5f, speed);
            }
        }
        else
        {
            if (deathTimer > 0.0f)
            {
                deathTimer -= Time.deltaTime;
                transform.Rotate(new Vector3(0.0f, 0.0f, transform.rotation.z + (500 * Time.deltaTime)));
            }
            else
            {
                gameManager.RestartGame();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Deadly") && alive)
        {
            gameManager.cameraManager.setShaking(true);
            rigidBody.velocity = new Vector2(0.0f, -10.0f); // fall
            alive = false;
        }

        if (other.gameObject.CompareTag("Respawn") && alive)
        {
            Destroy(other.gameObject);
            obstacleManager.AddObstacle();
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Right Wall"))
        {
            onRightWall = true;
        }
        else if (collision.gameObject.CompareTag("Left Wall"))
        {
            onLeftWall = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Right Wall"))
        {
            onRightWall = false;
        }
        else if (collision.gameObject.CompareTag("Left Wall"))
        {
            onLeftWall = false;
        }
    }

    public void ResetPlayer()
    {
        deathTimer = deathTimerTotal;
        rigidBody.velocity = new Vector2(0.0f, 0.0f);
        rigidBody.angularVelocity = 0.0f;
        transform.SetPositionAndRotation(startingPos, startingRotation);
        alive = true;
        rigidBody.Sleep();
    }
}
