using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMeleeCollider : MonoBehaviour
{
    [SerializeField]
    private string targetTag;

    private BoxCollider2D weaponCollider;

    private void Start()
    {
        weaponCollider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == targetTag)
        {
            weaponCollider.enabled = false;
        }
    }


}
