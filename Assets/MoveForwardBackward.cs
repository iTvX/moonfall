using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class MoveForwardBackward : MonoBehaviour
{

    public float min = 2f;
    public float max = 3f;
    public GameObject wheel;

    // Start is called before the first frame update
    void Start()
    {
        min = wheel.transform.position.x;
        max = wheel.transform.position.x + 5f;
    }

    // Update is called once per frame
    void Update()
    {
        wheel.transform.position = new Vector3(Mathf.PingPong(Time.time * 2, max - min) + min, transform.position.y, transform.position.z);
    }
}



