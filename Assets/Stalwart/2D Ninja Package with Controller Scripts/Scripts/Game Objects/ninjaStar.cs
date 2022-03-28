using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class ninjaStar : MonoBehaviour {

    [SerializeField]
    private float speed;

    private Rigidbody2D weaponrigidbody;

    private Vector2 direction;

    [SerializeField]
    private GameObject groundParticles;

    [SerializeField]
    private GameObject bloodParticles;

    int randomizeRotation;

	void Start ()
    {
        weaponrigidbody = GetComponent<Rigidbody2D>();
        randomizeRotation = UnityEngine.Random.Range(1, 3);
        randomizeRotation = randomizeRotation == 1 ? 1 : -1;
        // direction = (transform.rotation.x < 0) ? Vector2.left : Vector2.right;
    }

    void FixedUpdate()
    {
        weaponrigidbody.velocity = direction * speed;
        transform.Rotate(Vector3.forward * randomizeRotation * UnityEngine.Random.Range(20, 100));
    }

    public void Initialize(Vector2 direction)
    {
        this.direction = direction;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("enemy"))
        {
            Instantiate(bloodParticles, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        if (other.gameObject.CompareTag("ground") || other.gameObject.CompareTag("Wall"))
        {
            Instantiate(groundParticles, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
