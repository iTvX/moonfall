using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemysight : MonoBehaviour
{

    [SerializeField]
    private enemy enemy;

    public Transform sightStart, sightEnd;

    //what will hit the sight line
    public LayerMask targetLayers;

    private void Start()
    {
        // Physics2D.queriesStartInColliders = false;
    }

    private void FixedUpdate()
    {
        if (enemy.Renderer.isVisible && !enemy.IsDead)
        {
            RaycastHit2D hitInfo = Physics2D.Linecast(sightStart.position, sightEnd.position, targetLayers);
            if (hitInfo.collider != null && hitInfo.collider.CompareTag("Ninja"))
            {
                Debug.DrawLine(sightStart.position, hitInfo.point, Color.red);
                if (hitInfo.collider.GetComponent<ninja>().IsDead == true)
                {
                    enemy.RemoveTarget();
                }
                else
                {
                    setPlayerToDetected(hitInfo.collider.GetComponent<ninja>().gameObject);
                }
            }
            if (hitInfo.collider != null && !hitInfo.collider.CompareTag("Ninja") || hitInfo.collider == null)
            {
                enemy.Target = null;
                Debug.DrawLine(sightStart.position, sightEnd.position, Color.green);
            }
        }
        else
        {
            enemy.Target = null;
        }
    }
    public void setPlayerToDetected(GameObject target)
    {
        enemy.Target = target;
    }
}
