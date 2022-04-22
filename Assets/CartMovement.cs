using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public GameObject endPoint;
    public float speed = 0.1f;
    //public bool flag = true;
    public Vector2 endPoint_Pos;
    public float player_Pos;
   

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rb = GetComponent<Rigidbody2D>();
        endPoint_Pos = endPoint.transform.position;
      
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        
        if (collision.gameObject.tag == "Player")
        {
            //transform.position = Vector2.MoveTowards(transform.position, endPoint_Pos, speed * Time.deltaTime);         
            //player_Pos = collision.transform.position.y;
            //collision.transform.position = Vector2.MoveTowards(collision.transform.position, new Vector2(endPoint_Pos.x, player_Pos), speed * Time.deltaTime);

            if (System.Math.Abs(transform.position.x - endPoint_Pos.x) > 1)
            {
                transform.Translate(Vector3.left * speed * Time.deltaTime);
                collision.transform.Translate(Vector3.left * speed * Time.deltaTime);
            }

            //if (transform.position.x != endPoint_Pos.x)
            //{
            //    transform.Translate(Vector3.left * speed * Time.deltaTime);
            //    collision.transform.Translate(Vector3.left * speed * Time.deltaTime);
            //}


        }
        
    }

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    flag = false;
    //}

}
