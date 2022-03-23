using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flower_Subs : MonoBehaviour
{
    bool timerstart = false;
    private List<string> texts = new List<string>();
    public Text text ;
    public GameObject player;
    public GameObject sprite;
    public GameObject canvas;
    bool isPlaying = false;
    public float gap;
    float currDist;
    int i;
    float timer;
    public float interval = 5f;
    // Start is called before the first frame update
    void Start()
    {
       
        texts.Add("See the flower there, the one that is all black? ");
        texts.Add("It could be called “my unseeable hope of relationship” with 4.4 star yelp reviews.");
        texts.Add("Not many would come here, but it’s a unique scene, isn’t it?");
        i = 0;
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
            if (currDist < 2 && text.gameObject.GetComponent<Text>().text == "")
            {
                timerstart = true;
                timer = 0;
                //print("subs now");
                isPlaying = true;

            }
        }
        if (isPlaying)
        {

            if (i < texts.Count && timer < interval)
            {

                EnableText(texts[i]);
                //StartCoroutine(TheSequence());


            }



            if (timer >= interval)
            {
                if (i < texts.Count && timer > gap + interval)
                {
                    i++;
                    text.gameObject.GetComponent<Text>().text = "";
                    timer = 0;
                }


                //print(timer);
                //print(interval);
                //print("just disabled?");
                //text.gameObject.SetActive(false);
                text.gameObject.GetComponent<Text>().text = " ";



                //print("subs now");
                //isPlaying = false;
            }
            if (i >= texts.Count)
            {
                text.gameObject.GetComponent<Text>().text = "";
            }
        }
    
    }

    public void EnableText(string toshow)
    {
        text.gameObject.SetActive(true);
        
        text.gameObject.GetComponent<Text>().text = toshow;
        float sprite_x = canvas.transform.position.x;
        float sprite_y = canvas.transform.position.y;
        text.transform.position = new Vector3(sprite_x, sprite_y - 8f, 1);
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
