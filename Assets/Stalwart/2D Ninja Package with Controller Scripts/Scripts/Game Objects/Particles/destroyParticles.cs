using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyParticles : MonoBehaviour
{
    public float destroyTime;
    void Update()
    {
        Destroy(gameObject, destroyTime);
    }
}
