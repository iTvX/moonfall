using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class katanaCollider : MonoBehaviour
{
    [SerializeField]
    private List<string> targetTags;
    private Transform Target;
    private Vector3 proximityLimit;

    [SerializeField]
    private GameObject katanaDash; // dashing and spawn ghosts effect
    private Collider2D weaponCollider;

    [HideInInspector]
    public bool sliding;

    [SerializeField]
    private ninja Ninja;
    private Transform ninjaTransform;
    private Animator ninjaAnimator;
    private Animator cameraAnimator;
    void Awake()
    {
        weaponCollider = GetComponent<Collider2D>();
        sliding = false;
        ninjaTransform = ninja.Instance.transform;
        ninjaAnimator = ninja.Instance.GetComponent<Animator>();
        cameraAnimator = cameraTrack.Instance.mainCameraAnimator;
        if (Ninja == null)
            Ninja = transform.parent.gameObject.GetComponent<ninja>();
    }

    void FixedUpdate()
    {
        if (sliding == true)
        {
            Sliding();
        }
    }

    void Sliding() // sliding to the target
    {
        if (Target != null)
        {
            if (Vector3.Distance(ninjaTransform.position, Target.position) >= 1.5f) // checking if distance between ninja and target is close
            {
                ninjaTransform.position = Vector3.MoveTowards(ninjaTransform.position, Target.position + proximityLimit, 20 * Time.deltaTime);
            }
            else
            {
                ninjaTransform.position = Vector3.MoveTowards(ninjaTransform.position, Target.position + proximityLimit, 120 * Time.deltaTime);
            }
            if (Vector3.Distance(ninjaTransform.position, Target.position) <= 2f) // stops if character moves to near of target
            {
                sliding = false;
                Invoke("deactiveSlide", 0.3f); // late call dashing effect
            }
        }
        else
        {
            sliding = false;
            Invoke("deactiveSlide", 0.3f);
        }
    }
    private void deactiveSlide()
    {
        katanaDash.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (targetTags.Contains(other.gameObject.tag)) // stops all when reach target
        {
            weaponCollider.enabled = false;
            Target = other.transform;
            sliding = true;
            if (ninjaTransform.position.x < Target.position.x)
            {
                proximityLimit = Vector3.left * 1.7f;     // this for stay in a certain position during the attack, don't get any closer unnecessarily
                if (!Ninja.FacingRight)
                    Ninja.ChangeDirection();  // colliderler arka arkaya döndüğü için 2 kere çalışıyor
            }
            else
            {
                proximityLimit = Vector3.right * 1.7f;
                if (Ninja.FacingRight)
                    Ninja.ChangeDirection();
            }
            katanaDash.SetActive(true);
            cameraAnimator.SetTrigger("shake2");
        }
    }
}
