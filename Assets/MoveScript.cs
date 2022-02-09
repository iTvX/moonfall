using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScript : MonoBehaviour
{
    public float moveSpeed = 5f;
    public bool isGrounded = false;

    public int jumpCount = 1;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Jump();
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
        transform.position += movement * Time.deltaTime * moveSpeed;
    }

    void Jump()
    {
        //print(isGrounded);
        if (Input.GetButtonDown("Jump") && jumpCount > 0)
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 5f), ForceMode2D.Impulse);
            jumpCount--;
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            jumpCount = 1;
        }
    }
}