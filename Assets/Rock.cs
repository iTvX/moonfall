using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    private Rigidbody2D rigidbody2D;
    
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(rigidbody2D.velocity.x);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            rigidbody2D.isKinematic = false;
            rigidbody2D.gravityScale = 1f;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && (System.Math.Abs(rigidbody2D.velocity.x) >= 5 || System.Math.Abs(rigidbody2D.velocity.y) >= 5))
        {
            
            GameObject.Find("FinishMenu").SendMessage("ShowFinishPanel");
        }
    }
}
