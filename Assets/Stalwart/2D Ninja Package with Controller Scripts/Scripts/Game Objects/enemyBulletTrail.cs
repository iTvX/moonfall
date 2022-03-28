using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBulletTrail : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private Rigidbody2D weaponrigidbody;

    [SerializeField]
    private GameObject groundParticles;

    [SerializeField]
    private GameObject bloodParticles;

    private Vector2 direction;

    void Start()
    {
        weaponrigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        weaponrigidbody.velocity = direction * speed;
    }

    public void Initialize(Transform target, Vector2 direction)
    {
        if (target != null)
        {
            Vector2 difference = target.GetComponent<Collider2D>().bounds.center - transform.position; // enemy shoots ninja colliders center, this for when ninja crouching.
            difference.Normalize();
            float pullingUpwardRatio = 0.2f;
            difference.y += pullingUpwardRatio;
            direction = difference;
        }
        this.direction = direction;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("ground") || other.gameObject.CompareTag("Wall"))
        {
            Instantiate(groundParticles, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        if (other.gameObject.CompareTag("Ninja"))
        {
            Instantiate(bloodParticles, transform.position, transform.rotation);
            Destroy(gameObject, 0.1f);
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
