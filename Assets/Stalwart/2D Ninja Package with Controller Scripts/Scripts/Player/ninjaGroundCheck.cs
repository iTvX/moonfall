using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ninjaGroundCheck : MonoBehaviour
{
    private ninja ninja;
    [SerializeField]
    private List<string> groundTags;
    // Start is called before the first frame update
    void Awake()
    {
        ninja = GetComponentInParent<ninja>();
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (!ninja.OnGround)
        {
            if (groundTags.Contains(other.gameObject.tag))
            {
                ninja.OnGround = true;
                ninja.IsGrounded(other);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (groundTags.Contains(other.gameObject.tag))
        {
            if (ninja.OnGround == true)
            {
                ninja.OnGround = false;
                ninja.IsGrounded(other);
            }
        }
    }
}