using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishScripts : MonoBehaviour
{
    
    public static bool GameIsFinshed = false;
    public GameObject FinishMenuUI;

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
        FinishMenuUI.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }


}
