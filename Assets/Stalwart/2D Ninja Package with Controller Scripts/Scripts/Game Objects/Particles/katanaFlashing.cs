using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class katanaFlashing : MonoBehaviour
{
    [SerializeField]
    private Sprite flashing01;

    [SerializeField]
    private Sprite flashing02;

    public float flashingSize = 0.1f;
    private float increasingRate = 25f;
    public bool directionIsRight = true;
    public int randomizedAttack = 0;

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector2(directionIsRight ? 0.1f : -0.1f, 0.1f);
        GetComponent<SpriteRenderer>().sprite = randomizedAttack == 1 ? flashing01 : flashing02;
        increasingRate = directionIsRight ? 25f : -25f;
        Destroy(gameObject, 0.1f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        flashingSize = Mathf.Clamp(flashingSize += increasingRate * Time.deltaTime, -2f, 2f);
        transform.localScale = new Vector3(flashingSize, Mathf.Abs(flashingSize), 1);
    }

    public void Initialize(int randomizedAttack, bool directionIsRight)
    {
        this.randomizedAttack = randomizedAttack;
        this.directionIsRight = directionIsRight;
    }
}
