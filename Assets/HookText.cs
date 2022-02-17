using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HookText : MonoBehaviour

{
    public GameObject body;
    public Text t_left;
    public Text t_right;
    public Text t_hook;
    public Text t_jump;
    public bool e_pressed = false;

    public void OnCollisionEnter2D(Collision2D other)
    {
        if(e_pressed == false && other.transform.name == "first_collide")
        {
            float body_x = body.transform.position.x;
            float body_y = body.transform.position.y;
            float text_x = body_x;
            float text_y = body_y + 1f;
        
            t_hook.transform.position = new Vector3(text_x, text_y, 1);
        
            t_hook.text = "Move the mouse to where you want to grapple hook and Press E";
            t_hook.fontSize = 10;
            t_hook.GetComponent<Text>().color = Color.white;
        
            t_hook.gameObject.SetActive(true);
        }
        
    }

    



    // Start is called before the first frame update
    void Start()
    {
        t_left = GameObject.Find("Text (left)").GetComponent<Text>();
        t_right = GameObject.Find("Text (right)").GetComponent<Text>();
        t_jump = GameObject.Find("Text (jump)").GetComponent<Text>();
        t_hook = GameObject.Find("Text (hook)").GetComponent<Text>();

        t_left.gameObject.SetActive(false);
        t_right.gameObject.SetActive(false);
        t_jump.gameObject.SetActive(false);
        //t_hook.gameObject.SetActive(false);

        t_left.fontSize = 10;
        t_right.fontSize = 10;
        t_jump.fontSize = 10;
        t_left.text = "Press A to move left";
        t_right.text = "Press D to move right";
        t_jump.text = "Press space to jump";
        t_hook.text = "";
        t_left.GetComponent<Text>().color = Color.white;
        t_right.GetComponent<Text>().color = Color.white;
        t_jump.GetComponent<Text>().color = Color.white;
        float body_x = body.transform.position.x;
        float body_y = body.transform.position.y;
        
        t_left.transform.position = new Vector3(body_x, body_y+1.6f, 1);
        t_right.transform.position = new Vector3(body_x, body_y+1f, 1);
        t_jump.transform.position = new Vector3(body_x, body_y - 1f, 1);

        t_left.gameObject.SetActive(true);
        t_right.gameObject.SetActive(true);
        t_jump.gameObject.SetActive(true);








    }

    // Update is called once per frame
    void Update()
    {
        
        
        if(Input.GetKey(KeyCode.A))
        {
            t_left.gameObject.SetActive(false);
            
        }
        if (Input.GetKey(KeyCode.D)){
            t_right.gameObject.SetActive(false);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            t_jump.gameObject.SetActive(false);
        }


        if (Input.GetKey(KeyCode.E))
        {
            e_pressed = true;
            t_hook.gameObject.SetActive(false);
        }
        

        
    }
}
