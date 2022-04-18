using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TransSkel : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera NinjaCam;
    [SerializeField] CinemachineVirtualCamera SkelCam;

    public GameObject Ninja;
    public GameObject Skel;
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
            Skel.transform.position = trackposition;
            Ninja.gameObject.SetActive(false);
            Skel.gameObject.SetActive(true);
            NinjaCam.gameObject.SetActive(false);
            SkelCam.gameObject.SetActive(true);
            Destroy(gameObject);
        }
    }
}
