using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    private Rigidbody2D rigidbody2D;
    private bool isTriggered;
    private float timer;
    
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        isTriggered = false;
        timer = 3.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            rigidbody2D.isKinematic = false;
            rigidbody2D.gravityScale = 1f;
            if ((System.Math.Abs(rigidbody2D.velocity.x) + System.Math.Abs(rigidbody2D.velocity.y)) > 1 && !isTriggered)
            {
                isTriggered = true;
            }
        }

        if ((System.Math.Abs(rigidbody2D.velocity.x) + System.Math.Abs(rigidbody2D.velocity.y)) == 0 && isTriggered)
        {
            rigidbody2D.isKinematic = true;
            rigidbody2D.velocity = Vector2.zero;
            rigidbody2D.freezeRotation = true;
        }

        else if ((System.Math.Abs(rigidbody2D.velocity.x) + System.Math.Abs(rigidbody2D.velocity.y)) < 0.5 && isTriggered)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                rigidbody2D.isKinematic = true;
                rigidbody2D.velocity = Vector2.zero;
                rigidbody2D.freezeRotation = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && (System.Math.Abs(rigidbody2D.velocity.x) + System.Math.Abs(rigidbody2D.velocity.y)) > 0.2)
        {
            GameObject.Find("FinishMenu").SendMessage("ShowFinishPanel");
        }
    }
}
