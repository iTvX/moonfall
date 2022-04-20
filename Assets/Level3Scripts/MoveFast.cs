using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFast : MonoBehaviour
{

    public float min = 2f;
    public float max = 3f;
    public GameObject platform;

    // Start is called before the first frame update
    void Start()
    {
        min = platform.transform.position.x;
        max = platform.transform.position.x + 8f;
    }

   
    // Update is called once per frame
    void Update()

    {
        platform.transform.position = new Vector3(Mathf.PingPong(Time.time * 4f, max - min) + min, transform.position.y, transform.position.z);
    }
}
