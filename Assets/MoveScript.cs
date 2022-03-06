using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Analytics;
using Unity.Services.Core;

public class MoveScript : MonoBehaviour
{
    public float moveSpeed = 5f;
    public bool isGrounded = false;
    private Vector3 lastPosition;
    private float totalDistance;
    public int jumpCount = 1;
    public Text distance;
    public Text height;
    public Text Timecount;
    private Vector3 startpoint;
    private int currentheight = 0;
    private int lastTimeSent = -1;
    private int lastSecondHeight = 0;
    private bool inFalling = false;

    private float timer;


    // Start is called before the first frame update
    async void Start()
    {
        startpoint = transform.position;
        lastPosition = transform.position;
        
        await UnityServices.InitializeAsync();
        List<string> consentIdentifiers = await Events.CheckForRequiredConsents();
    }

    // Update is called once per frame
    void Update()
    {
        
        Jump();
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
        transform.position += movement * Time.deltaTime * moveSpeed;
        //�����г�
        Vector3 Julicha = transform.position - lastPosition;
        float distancethisframe = Julicha.magnitude;
        totalDistance += distancethisframe;
        int intDistance = (int)totalDistance;
        lastPosition = transform.position;
        //�㵱ǰ�߶�
        Vector3 Gaoducha = transform.position - startpoint;
        float heightcount = Gaoducha.magnitude;
        currentheight = (int)heightcount;
        //timer
        timer += Time.deltaTime;
        int intTimer = (int)timer;

       

 
        if (intTimer == lastTimeSent + 1) {
            RecordDistanceAndHeightWithTime(intDistance, currentheight, intTimer);
            lastTimeSent = intTimer;

            lastSecondHeight = currentheight;
        }

        if(Input.GetKeyDown(KeyCode.E)){
 //           print("reocord swing");
            RecordSwing(intTimer);
            
        }

        if (lastSecondHeight >= currentheight + 5 && !inFalling) {
            RecordFall(intTimer);
            inFalling = true;   // Set inFalling flag to true, so a fall will only be recorded once
        } else if (lastSecondHeight < currentheight + 5 && inFalling) {
            inFalling = false;  // Set inFalling flag to false, so we can record next fall
        }

        distance.text = "Total Distance: " + intDistance.ToString() + "m";
        height.text = "Height: " + currentheight.ToString() + "m";
        Timecount.text = "Time: " + intTimer.ToString() + "s";

    }

    void Jump()
    {
        //print(isGrounded);
        if (Input.GetKeyDown("space") && jumpCount > 0)
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 5f), ForceMode2D.Impulse);
            jumpCount--;
        }

        if (isGrounded)
        {
            jumpCount = 1;
        }
    }

    void RecordDistanceAndHeightWithTime(int intDistance, int currentheight, int intTimer) {
        // Send custom event
        Dictionary<string, object> parameters = new Dictionary<string, object>()
        {
            { "distancePlayerGoes", intDistance },
            { "heightPlayerGoes", currentheight },
            { "time", intTimer },
        };
        Events.CustomData("motionTrail", parameters); 
        // Events.Flush();
    }

    void RecordFall(int intTimer) {
        Dictionary<string, object> parameters = new Dictionary<string, object>()
        {
            { "time", intTimer },
        };
        Events.CustomData("fall", parameters); 
    }

    void RecordSwing(int intTimer){

        Dictionary<string, object> parameters = new Dictionary<string, object>()
        {
            { "time", intTimer},
            {"userLevel",1}
        };
        Events.CustomData("NumOfSwing", parameters); 

    }
}