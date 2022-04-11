using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class CharacterSwitch : MonoBehaviour
{
    public GameObject player1, player2, player3;
    [SerializeField] CinemachineVirtualCamera follow1;
    [SerializeField] CinemachineVirtualCamera follow2;
    [SerializeField] CinemachineVirtualCamera follow3;

    private Vector3 trackposition;

    private int whichcharacter;



    
    void Start()
    {
        player1.gameObject.SetActive(true);
        player2.gameObject.SetActive(false);
        player3.gameObject.SetActive(false);
        follow1.gameObject.SetActive(true);
        follow2.gameObject.SetActive(false);
        follow3.gameObject.SetActive(false);
        whichcharacter = 1;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Swap();
        }
    }
    public void Swap()
    {
        if(player2.gameObject.activeInHierarchy == true)
        {
            
            trackposition = player2.transform.position;
            player1.transform.position = trackposition;
            player1.gameObject.SetActive(true);
            player2.gameObject.SetActive(false);
            follow1.gameObject.SetActive(true);
            follow2.gameObject.SetActive(false);

        }
        else if (player3.gameObject.activeInHierarchy == true)
        {
            
            trackposition = player3.transform.position;
            player1.transform.position = trackposition;
            player1.gameObject.SetActive(true);
            player3.gameObject.SetActive(false);
            follow1.gameObject.SetActive(true);
            follow3.gameObject.SetActive(false);

        }
    }
}
