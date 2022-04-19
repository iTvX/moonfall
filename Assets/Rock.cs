using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rock : MonoBehaviour
{
    private Rigidbody2D rigidbody2D;
    private bool isTriggered;
    private float timer;
    bool freeze = false;
    public Collider2D collider;
    // Start is called before the first frame update
    void Start()
    {
        //transform.gameObject.layer = 7;
        rigidbody2D = GetComponent<Rigidbody2D>();
        isTriggered = false;
        timer = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isTriggered)
        {
            if (!freeze)
            {
                if ((System.Math.Abs(rigidbody2D.velocity.x) + System.Math.Abs(rigidbody2D.velocity.y)) == 0)
                {
                    rigidbody2D.isKinematic = true;
                    rigidbody2D.velocity = Vector2.zero;
                    rigidbody2D.freezeRotation = true;
                    

                }

                else if ((System.Math.Abs(rigidbody2D.velocity.x) + System.Math.Abs(rigidbody2D.velocity.y)) < 0.5)
                {
                    timer -= Time.deltaTime;
                    if (timer <= 0)
                    {
                        rigidbody2D.isKinematic = true;
                        
                        rigidbody2D.velocity = Vector2.zero;
                        rigidbody2D.freezeRotation = true;
                        freeze = true;
                    }
                }
            }
        }
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
                print("Disable");
                collider.enabled = false;
            }
        }

        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && (System.Math.Abs(rigidbody2D.velocity.x) + System.Math.Abs(rigidbody2D.velocity.y)) > 0.2)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
