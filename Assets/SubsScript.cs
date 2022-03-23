using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubsScript : MonoBehaviour
{
    bool timerstart = false;
    public string texts;
    public Text text;
    public GameObject player;
    public GameObject sprite;
    bool isPlaying = false;
    float currDist;
    
    float timer;
    public float interval = 5f;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        text.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
        if (timerstart)
        {
            timer += Time.deltaTime;
        }
        if (!isPlaying)
        {
        currDist = Vector3.Distance(sprite.transform.position, player.transform.position);
        if (currDist < 2)
            {
                timerstart = true;
                timer = 0;
                //print("subs now");
                isPlaying = true;
                EnableText();
                //StartCoroutine(TheSequence());

            }
        }
        
        if (timer >= interval)
        {
            //print(timer);
            //print(interval);
            //print("just disabled?");
            //text.gameObject.SetActive(false);
            text.gameObject.GetComponent<Text>().text ="";
            timerstart = false;
            timer = 0;
            //print("subs now");
            //isPlaying = false;
        }
    }

    public void EnableText()
    {
        text.gameObject.SetActive(true);
        text.gameObject.GetComponent<Text>().text = texts;
        float sprite_x = sprite.transform.position.x;
        float sprite_y = sprite.transform.position.y;
        text.transform.position = new Vector3(sprite_x, sprite_y - 4f, 1);
        //Destroy(text, time);
        
        
        

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
