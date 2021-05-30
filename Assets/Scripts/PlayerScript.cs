using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    public CameraManager cameraManager;

    public float speed = 5.0f;
    public float deathTimer = 3.0f;

    private bool alive = true;
    private bool goingRight = false;
    private Vector3 startingPos;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        cameraManager = cameraManager.GetComponent<CameraManager>();

        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (alive)
        {
            checkBounds();

            if (Input.GetKeyDown("a") || Input.GetKeyDown("left"))
            {
                goingRight = false;
                rigidBody.velocity = new Vector2(-speed * 0.5f, speed);
            }
            if (Input.GetKeyDown("d") || Input.GetKeyDown("right"))
            {
                goingRight = true;
                rigidBody.velocity = new Vector2(speed * 0.5f, speed);
            }
        }
        else
        {
            if (deathTimer > 0.0f)
            {
                deathTimer -= Time.deltaTime;
                rigidBody.velocity = new Vector2(0.0f, -10.0f); // fall
            }
            else
            {
                ResetPlayer();
            }
        }
    }

    private void checkBounds()
    {
        if (Mathf.Abs(transform.position.x - 0.0f) > 2.494f) // if we hit edge of screen
        {
            rigidBody.velocity = new Vector2(0.0f, -5.0f); // slide down
            transform.position = new Vector2(goingRight ? 2.494f : -2.494f, transform.position.y); // set position depending on which way player is moving
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Deadly" && alive)
        {
            cameraManager.shaking = true;
            alive = false;
        }
    }

    private void ResetPlayer()
    {
        cameraManager.ResetCameraManager();

        deathTimer = 3.0f;
        alive = true;
        goingRight = false;
        transform.position = startingPos;

        rigidBody.Sleep();
    }
}
