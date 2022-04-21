using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TransBackNinja : MonoBehaviour
{
    public GameObject player1, player2, player3, player4;
    [SerializeField] CinemachineVirtualCamera cam1;
    [SerializeField] CinemachineVirtualCamera cam2;
    [SerializeField] CinemachineVirtualCamera cam3;
    [SerializeField] CinemachineVirtualCamera cam4;

    private Vector3 trackposition;

    private int whichcharacter;




    void Start()
    {
        

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Swap();
            Destroy(gameObject);
        }
    }

    // Update is called once per frame

    public void Swap()
    {
        if (player2.gameObject.activeInHierarchy == true)
        {

            trackposition = player2.transform.position;
            player1.transform.position = trackposition;
            player1.gameObject.SetActive(true);
            player2.gameObject.SetActive(false);
            cam1.gameObject.SetActive(true);
            cam2.gameObject.SetActive(false);

        }
        else if (player3.gameObject.activeInHierarchy == true)
        {

            trackposition = player3.transform.position;
            player1.transform.position = trackposition;
            player1.gameObject.SetActive(true);
            player3.gameObject.SetActive(false);
            cam1.gameObject.SetActive(true);
            cam3.gameObject.SetActive(false);

        }
        else if (player4.gameObject.activeInHierarchy == true)
        {

            trackposition = player4.transform.position;
            player1.transform.position = trackposition;
            player1.gameObject.SetActive(true);
            player4.gameObject.SetActive(false);
            cam1.gameObject.SetActive(true);
            cam4.gameObject.SetActive(false);

        }
    }
}
