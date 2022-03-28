using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ninjaGhost : MonoBehaviour
{
    public float ghostDelay;
    private float ghostDelaySeconds;
    public GameObject NinjaGhost;
    GameObject currentGhost;
    Sprite currentSprite;
    [SerializeField]
    private Transform ninjaTr;
    [SerializeField]
    private SpriteRenderer ninjaSpriteR;



    // Start is called before the first frame update
    void Awake()
    {
        ghostDelaySeconds = ghostDelay;
        if (ninjaTr == null)
            ninjaTr = transform.parent;
        if (ninjaSpriteR == null)
            ninjaSpriteR = transform.parent.gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gameObject.activeSelf)
        {
            if (ghostDelaySeconds > 0)
            {
                ghostDelaySeconds -= Time.deltaTime;
            }
            else
            {
                //This codes creates a ninja ghost object first, then changes each time it sprites to previous character sprite
                currentGhost = Instantiate(NinjaGhost, ninjaTr.position, transform.rotation);
                currentSprite = ninjaSpriteR.sprite;
                currentGhost.transform.localScale = ninjaTr.localScale;
                currentGhost.GetComponent<SpriteRenderer>().sprite = currentSprite;
                ghostDelaySeconds = ghostDelay;
                Destroy(currentGhost, 1.5f);
            }
        }     
    }
}
