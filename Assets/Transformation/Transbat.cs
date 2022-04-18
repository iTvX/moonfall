using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Transbat : MonoBehaviour
{
    // Start is called before the first frame update
    
    [SerializeField] CinemachineVirtualCamera NinjaCam;
    [SerializeField] CinemachineVirtualCamera BatCam;
 
    public GameObject Ninja;
    public GameObject Bat;
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
            Bat.transform.position = trackposition;
            Ninja.gameObject.SetActive(false);
            Bat.gameObject.SetActive(true);
            NinjaCam.gameObject.SetActive(false);
            BatCam.gameObject.SetActive(true);
            Destroy(gameObject);
        }
    }
}
