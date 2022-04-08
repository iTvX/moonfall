using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlayerWithPlatform : MonoBehaviour
{
    public GameObject platform;
    

    void Start()
    {
        
    }

    void Update()
    {
       
    }



    void OnCollisionEnter2D(Collision2D col)
    {
        col.gameObject.transform.SetParent(platform.gameObject.transform, true);
    }
    void OnCollisionExit2D(Collision2D col)
    {
        col.gameObject.transform.parent = null;
    }

}
