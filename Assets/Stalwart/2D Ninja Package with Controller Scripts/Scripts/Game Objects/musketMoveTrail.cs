using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musketMoveTrail : MonoBehaviour
{
    public int moveSpeed = 50;
    public bool moving;

    [SerializeField]
    private GameObject groundParticles;

    [SerializeField]
    private GameObject bloodParticles;

    void Awake()
    {
        moving = true;
    }

    void FixedUpdate()
    {
        if (moving == true)
        {
            transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
        }
        else
        {
            transform.Translate(Vector3.zero);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("ground") || other.gameObject.CompareTag("Wall"))
        {
            moving = false;
            Instantiate(groundParticles, transform.position, transform.rotation);
            gameObject.tag = "Untagged";
            Destroy(gameObject);
        }
        if (other.gameObject.CompareTag("enemy"))
        {
            moving = false;
            Instantiate(bloodParticles, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
