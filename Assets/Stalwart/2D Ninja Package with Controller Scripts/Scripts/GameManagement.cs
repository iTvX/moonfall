using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagement : MonoBehaviour
{
    public List<Sprite> preloadSprites;

    // Start is called before the first frame update
    void Awake()
    {
        Application.targetFrameRate = 101;
        StartCoroutine(operation());
    }

    public IEnumerator operation()
    {
        for (int i = 0; i < preloadSprites.Count; i++)
        {
            GameObject preloadedObject =  new GameObject("preloadObjects", typeof(SpriteRenderer));
            preloadedObject.GetComponent<SpriteRenderer>().sprite = preloadSprites[i];
            preloadedObject.transform.position = new Vector3(-1000,-1000,-1000);
            yield return new WaitForEndOfFrame();
            yield return new WaitForFixedUpdate();
            Destroy(preloadedObject);
        }
        Debug.Log("sprites preloaded");
        yield return new WaitForFixedUpdate();
        yield return new WaitForEndOfFrame();
        Destroy(gameObject);
    }
}
