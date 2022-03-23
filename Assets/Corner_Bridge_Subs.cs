using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Corner_Bridge_Subs : MonoBehaviour
{
    bool timerstart = false;
    private List<string> texts = new List<string>();
    public Text text;
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

        texts.Add("See,  could be a little tighter than you think, those gaps in the path.");
        texts.Add(" It’s yet simple, you jump, or hook, easy to get up.");
        texts.Add("But why we get up?");
        texts.Add("Was it because I told you to climb? ");
        texts.Add("Was it because you wondered what’s up there?");
        texts.Add("Or was it just boredom?");
        texts.Add("Oh, or maybe, you were in a midterm, forced to play my game.");
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

            }

            if (timer >= interval)
            {
                if (i < texts.Count && timer > gap + interval)
                {
                    i++;
                    text.gameObject.GetComponent<Text>().text = "";
                    timer = 0;
                }
                text.gameObject.GetComponent<Text>().text = " ";
                //isPlaying = false;
            }
            if (i >= texts.Count) {
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
}
    