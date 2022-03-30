using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public GameObject endPoint;
    public float speed = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (rb.transform.position.x != endPoint.transform.position.x)
            {
                rb.transform.Translate(Vector3.left * speed * Time.deltaTime);
                collision.transform.Translate(Vector3.left * speed * Time.deltaTime);
                //rb.velocity = new Vector2(-speed, 0);
                //collision.rigidbody.velocity = new Vector2(-speed, 0);

            }

        }
        
    }
}
