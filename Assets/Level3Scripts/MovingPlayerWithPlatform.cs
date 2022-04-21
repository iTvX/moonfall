using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem;

public class MovingPlayerWithPlatform : MonoBehaviour
{
    public GameObject rbody;
    private bool isOnPlatform;
    //public GameObject platformRBody;
    private void Start()
    {
        //platformRBody = GetComponent<GameObject>();
        rbody = GameObject.Find("Ninja");
    }

    void Update()
    {
        
        if (isOnPlatform)
        {
            rbody.transform.SetParent(gameObject.transform, true);
        }
        else
        {
            rbody.transform.parent = null;
        }
        
    }

    void OnCollisionStay2D(Collision2D col)
    {
        //if (col.gameObject.tag == "Player")
        //{
        col.transform.SetParent(gameObject.transform, true);
        isOnPlatform = true;
        //}
    }

    

    void OnCollisionExit2D(Collision2D col)
    {
        //if (col.gameObject.tag == "Player")
        //{
            //col.transform.parent = null;
            isOnPlatform = false;
        
            //platformRBody = null;
        //}
    }
}