using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lever : MonoBehaviour
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
        
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            rigidbody2D.isKinematic = false;
            rigidbody2D.gravityScale = 1f;
        }
    }
}
