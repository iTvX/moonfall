using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStart : MonoBehaviour
{
    Vector2 pos;
    public GameObject GM;
    // Start is called before the first frame update
    void Start()
    {
        GM = GameObject.Find("Game Master");
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void ChangeScene(string scenename)
    {
        //        Application.LoadLevel(scenename);
        
        if (scenename == "SampleScene")
        {
            
            pos.x = 957.21f;
            pos.y = -371.42f;
        }
        if (scenename == "level3")
        {

            pos.x = 581.83f;
            pos.y = -362.75f;
        }
        if (scenename == "IndustryScene")
        {

            pos.x = 958.3f;
            pos.y = -371.6f;
        }
        GM.GetComponent<CheckPoints>().lastCheckPointPos  = pos;
        SceneManager.LoadScene(scenename);
    }

    public void QuitGame(){
        Application.Quit();
    }
}
