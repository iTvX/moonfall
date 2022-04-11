using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TransRabbit : MonoBehaviour
{
    // Start is called before the first frame update
    
    [SerializeField] CinemachineVirtualCamera follow1;
    [SerializeField] CinemachineVirtualCamera follow2;
    [SerializeField] CinemachineVirtualCamera follow3;
    public GameObject Ninja;
    public GameObject Bat;
    public GameObject Rabbit;
    private Vector3 trackposition;

    private int whichcharacter;
    void Start()
    {
        

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            trackposition = Ninja.transform.position;
            Rabbit.transform.position = trackposition;
            Ninja.gameObject.SetActive(false);
            Rabbit.gameObject.SetActive(true);
            Bat.gameObject.SetActive(false);
            follow1.gameObject.SetActive(false);
            follow3.gameObject.SetActive(true);
            follow2.gameObject.SetActive(false);
            
        }
    }
}