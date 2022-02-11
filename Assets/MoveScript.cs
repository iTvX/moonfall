using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScript : MonoBehaviour
{
    public float moveSpeed = 5f; 
    public bool isGrounded = false;
    public GameObject topRightLimitGameObject;
    public GameObject bottomLeftLimitGameObject;

    private Vector3 topRightLimit;
    private Vector3 bottomLeftLimit;

    private float objectWidth;
    private float objectHeight;

 


    // Start is called before the first frame update
    void Start()
    {
        topRightLimit = topRightLimitGameObject.transform.position;
        bottomLeftLimit = bottomLeftLimitGameObject.transform.position;
        objectWidth = transform.GetComponent<SpriteRenderer>().bounds.size.x/2;
        objectHeight = transform.GetComponent<SpriteRenderer>().bounds.size.y/2;



    }

    // Update is called once per frame
    void Update()
    {

        Jump();


        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f,0f);


        //if ((transform.position.x <= bottomLeftLimit.x&& horizontalmove==-1) || (transform.position.x >= topRightLimit.x&&horizontalmove==1))
        if (transform.position.x-objectWidth <= bottomLeftLimit.x)
        {
            print("left boundary");
            transform.position = new Vector3(bottomLeftLimit.x+objectWidth, transform.position.y, transform.position.z);
            
        }
        if (transform.position.x+objectWidth >= topRightLimit.x)
        {
            print("right boundary");
            transform.position = new Vector3(topRightLimit.x-objectWidth, transform.position.y, transform.position.z);

        }
        if (transform.position.y-objectHeight <= bottomLeftLimit.y)
        {
            print("bottom");
            transform.position = new Vector3(transform.position.x, bottomLeftLimit.y+objectHeight, transform.position.z);
            
        }

        /*if (transform.position.y <= bottomLeftLimit.y)
        {
            transform.position.y = bottomLeftLimit.y;
        }*/
        
        transform.position += movement * Time.deltaTime * moveSpeed;

    }
    void Jump(){
        //print(isGrounded);
        //&& isGrounded == true
        if (Input.GetButtonDown("Jump") ){
	gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0,5f),ForceMode2D.Impulse);
	}
    }
}
