using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grounded : MonoBehaviour
{
    GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        Player = gameObject.transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        print("grounded");
    }
    private void OnCollisionEnter2D(Collision2D collision){
	if (collision.collider.tag == "Ground"||collision.collider.tag == "wall"){
            print("colliding");
	    Player.GetComponent<NinjaMove>().isGrounded = true;
	
	}

    }
    private void OnCollisionExit2D(Collision2D collision){
        print("not colliding");
        if (collision.collider.tag == "Ground"||collision.collider.tag == "wall"){
	    Player.GetComponent<NinjaMove>().isGrounded = false;

	
	}

    }

}
