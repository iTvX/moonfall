using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishScripts : MonoBehaviour
{
    
    public static bool GameIsFinshed = false;
    public GameObject FinishMenuUI;
    public GameObject GM;
    Vector2 pos;
    void Start()
    {
        GM = GameObject.Find("Game Master");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowFinishPanel(){

        FinishMenuUI.SetActive(true);
        Time.timeScale = 0f;
//        Debug.Log("finished");

    }


    public void FinishBackToMain(string sceneName){
        if (sceneName == "SampleScene")
        {

            pos.x = 957.21f;
            pos.y = -371.42f;
        }
        if (sceneName == "level3")
        {

            pos.x = 578.83f;
            pos.y = -362.75f;
        }
        if (sceneName == "IndustryScene")
        {

            pos.x = 958.3f;
            pos.y = -371.6f;
        }
        GM.GetComponent<CheckPoints>().lastCheckPointPos = pos;
        FinishMenuUI.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }


}
