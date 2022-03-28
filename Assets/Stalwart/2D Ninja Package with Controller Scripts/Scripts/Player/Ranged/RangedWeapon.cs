using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : MonoBehaviour
{
    public GameObject arrowPrefab;

    public Animator RangedProfileAnim;

    public Transform bulletPrefab;

    public ninja Ninja;

    float timeToSpawnEffect = 0;
    public float effectSpawnRate = 10;

    public float timeToFireMusket = 2;
    public Transform firePoint;

    private bool shooting;

    private float waitForAttack = 0.30f;

    [Header("Bow Ammo")]
    public int bArrowNumbers = 100;
    public int bArrowLimit = 100;
    public int fireArrowNumbers = 25;
    public int fireArrowLimit = 25;
    public int smokeArrowNumbers = 25;
    public int smokeArrowLimit = 25;
    [Header("Musket Ammo")]
    public int musketAmmo = 80;
    public int musketAmmoLimit = 80;

    // Use this for initialization
    void Awake ()
    {
        firePoint = transform.Find("FirePoint");
        if (firePoint == null)
        {
            Debug.Log("FirePoint not found");
        }
    }

    public void ShootBow(float rotationOffset)
    {
        if (bArrowNumbers > 0)
        {
            if (Time.time >= timeToSpawnEffect)
            {
                GameObject instantiatedArrow = Instantiate(arrowPrefab, firePoint.position, Quaternion.Euler(0, 0, rotationOffset));
                instantiatedArrow.GetComponent<arrowMoveTrail>().Initialize(rotationOffset);
                shooting = true;
                if (shooting)
                {
                    RangedProfileAnim.SetTrigger("shoot");
                    Ninja.anim.SetTrigger("shoot");
                    Ninja.audioSource.PlayOneShot(Ninja.sounds[5]);
                    shooting = false;
                    bArrowNumbers--;
                }
                timeToSpawnEffect = Time.time + 1 / effectSpawnRate;
            }
        }
    }

    public void ShootMusket(float rotationOffset)
    {
        if (Time.time >= timeToSpawnEffect)
        {
            if (musketAmmo > 0)
            {
                if (timeToFireMusket > 0)
                {
                    Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(0, 0, rotationOffset));
                    RangedProfileAnim.SetTrigger("shoot");
                    Ninja.anim.SetTrigger("shoot");
                    Ninja.audioSource.PlayOneShot(Ninja.sounds[6]);
                    timeToFireMusket--;
                    musketAmmo--;
                }
                else
                {
                    StartCoroutine(reloading());
                    timeToFireMusket = 2;
                    timeToSpawnEffect = Time.time + 2 / effectSpawnRate; // waiting for reload
                }
            }
            else
            {
                Ninja.audioSource.PlayOneShot(Ninja.sounds[8]);
                timeToSpawnEffect = Time.time + 1 / effectSpawnRate; // waiting for nothing
            }
        }
    }
    IEnumerator reloading()
    {
        yield return new WaitForSeconds(waitForAttack);
        RangedProfileAnim.SetTrigger("reload");
        yield return null;
    }
}
