using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FLS;
using FLS.Rules;
using FLS.MembershipFunctions;

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

    private float moveTime;
    private const float totalMoveTime = 0.3f;

    private IFuzzyEngine engine;
    private LinguisticVariable distance;
    private LinguisticVariable velocity;

    public bool failsafe;

    public enum ControlState
    {
        Manual = 0,
        RBS    = 1,
        Fuzzy  = 2
    }

    public ControlState controlState;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        startingPos = transform.position;
        startingRotation = transform.rotation;
        deathTimer = deathTimerTotal;
        controlState = ControlState.Manual;

        moveTime = totalMoveTime;

        // Set up fuzzy inference system
        distance = new LinguisticVariable("distance");
        var right = distance.MembershipFunctions.AddTrapezoid("right", -5, -5, -5, -0.1);
        var none = distance.MembershipFunctions.AddTrapezoid("none", -0.5, -0.05, 0.05, 0.5);
        var left = distance.MembershipFunctions.AddTrapezoid("left", 0.1, 5, 5, 5);

        velocity = new LinguisticVariable("velocity");
        var v_right = velocity.MembershipFunctions.AddTrapezoid("right", -5, -5, -5, -0.1);
        var v_none = velocity.MembershipFunctions.AddTrapezoid("none", -0.5, -0.05, 0.05, 0.5);
        var v_left = velocity.MembershipFunctions.AddTrapezoid("left", 0.1, 5, 5, 5);

        engine = new FuzzyEngineFactory().Default();

        var rule1 = Rule.If(distance.Is(right)).Then(velocity.Is(v_left));
        var rule2 = Rule.If(distance.Is(left)).Then(velocity.Is(v_right));
        var rule3 = Rule.If(distance.Is(none)).Then(velocity.Is(v_none));

        engine.Rules.Add(rule1, rule2, rule3);
    }

    // Update is called once per frame
    void Update()
    {
        

        if (alive)
        {
            //Debug.Log("Y Delta: " + (transform.position.y - obstacleManager.holePos.y));
            //Debug.Log("Relative X: " + ((double)transform.position.x - (double)obstacleManager.holePos.x));

            switch (controlState) // switch between control states
            {
                case ControlState.Manual:
                {
                    ManualControl();
                    break;
                }
                case ControlState.RBS:
                {
                    RBSControl();
                    break;
                }
                case ControlState.Fuzzy:
                {
                    FuzzyControl();
                    break;
                }
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
                deathTimer = deathTimerTotal;
                gameManager.RestartGame();
            }
        }
    }

    private void ManualControl()
    {
        double relativeXPos = (double)transform.position.x - (double)obstacleManager.holePos.x;

        if ((Mathf.Abs(transform.position.y - obstacleManager.holePos.y) < 1.5f) && (Mathf.Abs((float)relativeXPos) >= 0.5f) && failsafe)
        {
            // might collide
            moveTime = totalMoveTime;
            return;
        }

        if (!onLeftWall && (Input.GetKeyDown("a") || Input.GetKeyDown("left")))
        {
            rigidBody.velocity = new Vector2(-speed * 0.5f, speed);
        }
        if (!onRightWall && (Input.GetKeyDown("d") || Input.GetKeyDown("right")))
        {
            rigidBody.velocity = new Vector2(speed * 0.5f, speed);
        }
    }

    private void RBSControl()
    {
        moveTime -= Time.deltaTime;

        if (moveTime <= 0.0f)
        {
            moveTime = totalMoveTime;

            // do move

            double relativeXPos = (double)transform.position.x - (double)obstacleManager.holePos.x;

            if ((Mathf.Abs(transform.position.y - obstacleManager.holePos.y) < 1.5f) && (Mathf.Abs((float)relativeXPos) >= 0.5f) && failsafe)
            {
                // might collide
                moveTime = totalMoveTime;
                return;
            }
        }
    }

    private void FuzzyControl()
    {
        moveTime -= Time.deltaTime;

        if (moveTime <= 0.0f)
        {
            moveTime = totalMoveTime;

            // do move

            // get position relative to hole

            double relativeXPos = (double)transform.position.x - (double)obstacleManager.holePos.x;

            if ((Mathf.Abs(transform.position.y - obstacleManager.holePos.y) < 1.5f) && (Mathf.Abs((float)relativeXPos) >= 0.5f) && failsafe)
            {
                // might collide
                moveTime = totalMoveTime;
                return;
            }

            rigidBody.velocity = new Vector2((float)engine.Defuzzify(new { distance = relativeXPos }), speed);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Deadly") && alive)
        {
            gameManager.cameraManager.setShaking(true);
            rigidBody.velocity = new Vector2(0.0f, -10.0f); // fall
            alive = false;
        }

        if (collision.gameObject.CompareTag("Respawn") && alive)
        {
            Destroy(collision.gameObject);
            obstacleManager.AddObstacle();
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Score") && alive)
        {
            Destroy(collision.gameObject);
            gameManager.AddScore(1);
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
