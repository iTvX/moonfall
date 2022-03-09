using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubsScript : MonoBehaviour
{
    public Text text;
    public GameObject player;
    public GameObject sprite;
    public static bool isPlaying = false;
    public float currDist;
    public float time = 30;
    // Start is called before the first frame update
    void Start()
    {
        print("start is called");
        text.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
        if (!isPlaying)
        {
        currDist = Vector3.Distance(sprite.transform.position, player.transform.position);
        if (currDist < 2)
            {
                print("subs now");
                isPlaying = true;
                EnableText();
                //StartCoroutine(TheSequence());

            }
        }
    }

    public void EnableText()
    {
        text.gameObject.SetActive(true);
        text.gameObject.GetComponent<Text>().text = "It’s just a simple mission, climb. ";
        float sprite_x = sprite.transform.position.x;
        float sprite_y = sprite.transform.position.y;
        text.transform.position = new Vector3(sprite_x, sprite_y - 4f, 1);
        Destroy(text, time);


      
    }

    /**
    IEnumerator TheSequence()
    {

        //yield return new WaitForSeconds(1);
        print("subsing rn rn ");
        text.gameObject.SetActive(true);
        text.gameObject.GetComponent<Text>().text = "It’s just a simple mission, climb. ";
        
        yield return new WaitForSeconds(30);
        textBox.gameObject.GetComponent<Text>().text = "";
        textBox.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);



    }
    **/

}
