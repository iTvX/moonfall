using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubsScript : MonoBehaviour
{
    public Text textBox;
    public GameObject body;
    public static bool isPlaying = false;
    // Start is called before the first frame update
    void Start()
    {
        print("start is called");
        
    }

    // Update is called once per frame
    void Update()
    {
        textBox.gameObject.transform.position = Camera.main.transform.position;
        //print((transform.position - body.transform.position).magnitude);
        if (!isPlaying)
        {
            if ((transform.position - body.transform.position).magnitude < 2)
            {
                print("subs now");
                isPlaying = true;
                
                StartCoroutine(TheSequence());
            }
        }
    }

    IEnumerator TheSequence()
    {

        //yield return new WaitForSeconds(1);
        print("subsing rn rn ");
        textBox.gameObject.SetActive(true);
        textBox.gameObject.GetComponent<Text>().text = "It’s just a simple mission, climb. ";
        
        yield return new WaitForSeconds(30);
        textBox.gameObject.GetComponent<Text>().text = "";
        textBox.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);



    }

}
