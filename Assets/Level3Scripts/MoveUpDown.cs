using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUpDown : MonoBehaviour
{
    public float min = 2f;
    public float max = 3f;
    public float minus = 0f;
    public float plus = 0f;
    public Rigidbody2D wheel;

    // Start is called before the first frame update
    void Start()
    {
        wheel = GetComponent<Rigidbody2D>();
        min = wheel.transform.position.y - minus;
        max = wheel.transform.position.y + plus;
    }

    // Update is called once per frame
    void Update()
    {
        wheel.transform.position = new Vector3(transform.position.x, Mathf.PingPong(Time.time * 2, max - min) + min, transform.position.z);
    }
}
