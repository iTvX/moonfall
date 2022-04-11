using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Check : MonoBehaviour
{
    // Start is called before the first frame update
    private CheckPoints cp;
    void Start()
    {
        cp = GameObject.FindGameObjectWithTag("CP").GetComponent<CheckPoints>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player"){
            cp.lastCheckPointPos = transform.position;
        }
    }
}
