using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TransRabbit : MonoBehaviour
{
    // Start is called before the first frame update
    
    [SerializeField] CinemachineVirtualCamera NinjaCam;
    [SerializeField] CinemachineVirtualCamera RabbitCam;
    public GameObject Ninja;
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
           
            NinjaCam.gameObject.SetActive(false);
            RabbitCam.gameObject.SetActive(true);
            Destroy(gameObject);

        }
    }
}
