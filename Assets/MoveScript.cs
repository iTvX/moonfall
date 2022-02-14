using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private float timer;


    // Start is called before the first frame update
    void Start()
    {
        startpoint = transform.position;
        lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
        Jump();
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
        transform.position += movement * Time.deltaTime * moveSpeed;
        //算总行程
        Vector3 Julicha = transform.position - lastPosition;
        float distancethisframe = Julicha.magnitude;
        totalDistance += distancethisframe;
        int intDistance = (int)totalDistance;
        lastPosition = transform.position;
        //算当前高度
        Vector3 Gaoducha = transform.position - startpoint;
        float heightcount = Gaoducha.magnitude;
        currentheight = (int)heightcount;
        //timer
        timer += Time.deltaTime;
        int intTimer = (int)timer;

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
    
}