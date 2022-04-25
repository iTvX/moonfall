using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
#if ENABLE_CLOUD_SERVICES_ANALYTICS
using UnityEngine.Analytics;
#endif
public class SkeletonMove : MonoBehaviour
{
    
    public Animator animator;
    private BoxCollider2D character;
    private Rigidbody2D rigidbody2D;
    private bool facingRight = true;
    Vector3 currentPos;
    Vector3 targetPos;
    public float mouseFacX;
    public float mouseFacY;
    private Transform proj;
    private GameObject projG;
    public Camera cam;
    public float moveSpeed = 5f;
    public float climbSpeed = 0.5f;
    public float playerGravity;
    
    public bool isGrounded = false;
    private Vector3 lastPosition;
    private float totalDistance;
    public int jumpCount = 1;
    //public Text distance;
    //public Text height;
    //public Text Timecount;
    private Vector3 startpoint;
    private Vector3 lastfallpoint;
    private int currentheight = 0;
    private int lastTimeSent = -1;
    private int lastSecondHeight = 0;
    private bool inFalling = false;

    private float timer;
    private bool inThrowing = false;
    private bool isJump = false;
    // private int numJump = 8; // compute distance after 8 frames delay
    private Vector3 tempJumpPosition;
    private float jumpDistance;


    private bool isHook = false;
    // private int numHook = 8;
    private Vector3 tempHookPosition;
    private float hookDistance;


    private bool isOnLadder;
    private bool isClimbingLadder;


    // Start is called before the first frame update
    async void Start()
    {
        proj = this.gameObject.transform.GetChild(1);
        //projG = transform.GetChild("proj").transform.position;
        lastfallpoint = transform.position;
        startpoint = transform.position;
        lastPosition = transform.position;

        character = GetComponent<BoxCollider2D>();
        rigidbody2D = GetComponent<Rigidbody2D>();

        playerGravity = rigidbody2D.gravityScale;
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckLadder();
        
        timer += Time.deltaTime;
        int intTimer = (int)timer;

        Jump(intTimer);
        ClimbLadder();
        
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
        
        if (!inThrowing)
        {
            animator.SetFloat("speed", Mathf.Abs(movement.x));

            transform.position += movement * Time.deltaTime * moveSpeed;
            if (movement.x > 0 && facingRight)
            {
                Flip();
            }
            else if (movement.x < 0 && !facingRight)
            {
                Flip();
            }
        }
        //�����г�
        Vector3 Julicha = transform.position - lastPosition;
        // if the user falls, stop tracking last fall position.
        if (Julicha.y > 0)
        {
            lastfallpoint = transform.position;
        }
        
        float distancethisframe = Julicha.magnitude;
        totalDistance += distancethisframe;
        int intDistance = (int)totalDistance;
        lastPosition = transform.position;
        
        Vector3 Gaoducha = transform.position - startpoint;
        float heightcount = Gaoducha.magnitude;
        currentheight = (int)heightcount;
        //timer
        

       

 
        if (intTimer == lastTimeSent + 1) {
            RecordDistanceAndHeightWithTime(intDistance, currentheight, intTimer);
            lastTimeSent = intTimer;

            lastSecondHeight = currentheight;
        }

        if(Input.GetKeyDown(KeyCode.E)){

            if (!inThrowing)
            {
                throwing();
                animator.SetBool("isThrowing", true);
                inThrowing = true;
            }
        }
        if(!inThrowing)
        {
            Vector3 tempPos = transform.position;
            tempPos.y = tempPos.y - 0.86f;
            proj.position = tempPos;
            proj.GetComponent<Rigidbody2D>().freezeRotation = true;
        }
        if (inThrowing)
        {
            if(proj.GetComponent<Rigidbody2D>().velocity.magnitude < 0.05)
            {
                Vector3 tempPos = proj.position;
                tempPos.y = tempPos.y + 1.2f;
                transform.position = tempPos;
                inThrowing = false;
                animator.SetBool("isThrowing", false);
            }
        }
        if (isJump && (lastSecondHeight > currentheight))
        {

            // Debug.Log("jump-fall!");
            isJump = false;
            float distanceDiff = (transform.position - tempJumpPosition).magnitude;
            jumpDistance += distanceDiff;
            jumpDistance = (float)Math.Round(jumpDistance, 2);
            RecordJumpDistance(jumpDistance, intTimer);


        }

        if (isHook && (lastSecondHeight > currentheight))
        {

            // Debug.Log("hook-fall!");
            isHook = false;
            float distanceDiff = (transform.position - tempHookPosition).magnitude;
            hookDistance += distanceDiff;
            hookDistance = (float)Math.Round(hookDistance, 2);
            RecordHookDistance(hookDistance, intTimer);


        }

        if (lastSecondHeight >= currentheight + 5 && !inFalling) {
            RecordFall(intTimer);
            inFalling = true;   // Set inFalling flag to true, so a fall will only be recorded once
        } else if (lastSecondHeight < currentheight + 5 && inFalling) {
            inFalling = false;  // Set inFalling flag to false, so we can record next fall
        }


    }
    private void throwing()
    {
        
        proj.GetComponent<Rigidbody2D>().freezeRotation = false;
        
        currentPos = proj.GetComponent<Rigidbody2D>().transform.position;
        print("shooting hook");
        targetPos = cam.ScreenToWorldPoint(Input.mousePosition);
        targetPos.z = 0;
        float fx = -(currentPos.x - targetPos.x)/mouseFacX;
        float fy = -(currentPos.y - targetPos.y)/ mouseFacY;
        print("forces used: ");
        print((currentPos.x - targetPos.x) / 5);
        print((currentPos.y - targetPos.y) / 5);
        if (fx > 0 && facingRight)
        {
            Flip();
        }
        else if (fx < 0 && !facingRight)
        {
            Flip();
        }
        proj.GetComponent<Rigidbody2D>().AddForce(new Vector2(fx, fy ), ForceMode2D.Impulse);

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground" || collision.collider.tag == "wall")
        {
            isGrounded = true;

        }

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground" || collision.collider.tag == "wall")
        {
            isGrounded = false;


        }
    }

    void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    void Jump(int timer)
    {
        //print(isGrounded);
        if (Input.GetKeyDown("space") && jumpCount > 0)
        {
            if (!isJump)
            {
                isJump = true;
                tempJumpPosition = transform.position;
            }

            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 2f), ForceMode2D.Impulse);
            jumpCount--;
            animator.SetBool("isground", false);
        }

        if (isGrounded)
        {
            jumpCount = 1;
            animator.SetBool("isground", true);
        }
    }

    void CheckLadder()
    {
        isOnLadder = character.IsTouchingLayers(LayerMask.GetMask("Ladder"));
    }

    void ClimbLadder()
    {
        if (isOnLadder)
        {
            float moveY = Input.GetAxis("Vertical");
            rigidbody2D.gravityScale = 0.0f;
            rigidbody2D.velocity = new Vector2(0.0f, moveY * climbSpeed);
        }

        else
        {
            rigidbody2D.gravityScale = playerGravity;
        }
    }

    void RecordDistanceAndHeightWithTime(int intDistance, int currentheight, int intTimer) {
        // Send custom event
        Analytics.CustomEvent("motionTrail", new Dictionary<string, object>()
        {
            { "distancePlayerGoes", intDistance },
            { "heightPlayerGoes", currentheight },
            { "time", intTimer },
        });
    }

    void RecordFall(int intTimer) {
        
        Analytics.CustomEvent("fall", new Dictionary<string, object>()
        {
            { "time", intTimer },
            { "xPosition", lastfallpoint.x},
            { "yPosition", lastfallpoint.y}
        });

       
        //Events.CustomData("fall", parameters); 
    }

    void RecordSwing(int intTimer){

        Analytics.CustomEvent("NumOfSwing", new Dictionary<string, object>()
        {
            { "time", intTimer},
            {"userLevel",1}
        });
        
    }
    void RecordJump(int intTimer)
    {

        Analytics.CustomEvent("NumOfJump", new Dictionary<string, object>()
        {
            { "time", intTimer},
            {"userLevel",1}
        });
       
    }


    void RecordJumpDistance(float jumpDistance, int intTimer)
    {
        // Debug.Log("Jump Distance: " + jumpDistance);

        Analytics.CustomEvent("TraveledThroughJump", new Dictionary<string, object>()
        {
            { "time", intTimer},
            { "DistanceThroughJump", jumpDistance},
            {"userLevel",1}
        });
    }

    void RecordHookDistance(float hookDistance, int intTimer)
    {
        //Debug.Log("Hook Distance: " + hookDistance);

        Analytics.CustomEvent("TraveledThroughHook", new Dictionary<string, object>()
        {
            { "time", intTimer},
            { "DistanceThroughHook", hookDistance},
            {"userLevel",1}
        });
    }


}