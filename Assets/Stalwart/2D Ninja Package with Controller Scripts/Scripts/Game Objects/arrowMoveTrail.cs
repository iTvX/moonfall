using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrowMoveTrail : MonoBehaviour
{
    public int moveSpeed = 25;

    // public Transform overlapareastartpos;
    // public Transform overlapareaendpos;
    // public LayerMask whatIsTarget;

    public Rigidbody2D arrowRb;
    private Collider2D ninjaCollider;

    [SerializeField]
    private GameObject bloodParticles;

    // Update is called once per frame
    void Awake()
    {
        ninjaCollider = GameObject.FindGameObjectWithTag("Ninja").GetComponent<Collider2D>();
    }

    public void Initialize(float rotation)
    {
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), ninjaCollider);
        Vector2 direction = new Vector2(Mathf.Cos(rotation * Mathf.Deg2Rad), Mathf.Sin(rotation * Mathf.Deg2Rad));
        arrowRb.velocity = direction * moveSpeed;
    }
    
    // void Update()
    // {
    //     Collider2D detection = Physics2D.OverlapArea(overlapareastartpos.position, overlapareaendpos.position, whatIsTarget);
    //     if (detection != null)
    //     {
    //         if (detection.CompareTag("Wall") || detection.CompareTag("ground"))
    //         {
    //             moving = false;
    //             arrowRb.velocity = Vector2.zero;
    //             Destroy(gameObject, 4);
    //             gameObject.tag = "Untagged";

    //         }
    //         if (detection.CompareTag("enemy"))
    //         {
    //             moving = false;
    //             arrowRb.velocity = Vector2.zero;
    //             Destroy(gameObject, 0.4f);
    //         }
    //     }
	// }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground") || collision.gameObject.CompareTag("Wall"))
        {
            arrowRb.velocity = Vector2.zero;
            arrowRb.constraints = RigidbodyConstraints2D.FreezeAll;
            Destroy(gameObject, 15);
            gameObject.tag = "Untagged";
            Destroy(this);
        }
        if (collision.gameObject.CompareTag("enemy"))
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.collider);
            arrowRb.constraints = RigidbodyConstraints2D.FreezeAll;
            arrowRb.velocity = Vector2.zero;
            transform.SetParent(collision.transform, true);
            transform.localPosition = new Vector3(transform.localPosition.x - transform.localPosition.x / 1.5f, transform.localPosition.y, 0); // transform position decreased towards zero, so that the arrow is closer to the mid point.
            GetComponent<SpriteRenderer>().sortingOrder = 1;
            Instantiate(bloodParticles, transform.position, transform.rotation);
            Destroy(gameObject, 3);
            Destroy(this);
        }
    }
    
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
