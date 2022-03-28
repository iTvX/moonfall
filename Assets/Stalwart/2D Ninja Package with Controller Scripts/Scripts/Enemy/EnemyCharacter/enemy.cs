using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{

    private IEnemyState currentState;

    public GameObject Target { get; set; }

    [SerializeField]
    private GameObject bullet;

    private static enemy instance;

    public static enemy Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<enemy>();
            }
            return instance;
        }
    }

    public SpriteRenderer Renderer;

    public Rigidbody2D rb { get; set; }
    public Collider2D thisEnemyCollider;

    [SerializeField]
    protected float speed;

    [SerializeField]
    protected bool facingRight;

    private Vector3 startpos;

    public bool TakingDamage { get; set; }

    private bool damageableFromKatana = true;

    public bool Attack { get; set; }

    public bool isThrowing { get; set; }

    public Animator anim { get; set; }

    [SerializeField]
    private float throwRange;

    [SerializeField]
    private float meleeRange;

    [SerializeField]
    public float idleduration;

    [SerializeField]
    public float walkingduration;

    [SerializeField]
    private Transform leftTurnBackPoint;

    [SerializeField]
    private Transform rightTurnBackPoint;

    [SerializeField]
    protected enemyHealthStat enemyHealthStat;

    public Canvas healthCanvas;

    public AudioSource audioSource;

    public AudioClip[] sounds;

    [SerializeField]
    private GameObject bloodParticles;

    [SerializeField]
    protected Transform bulletStartPos;

    [SerializeField]
    private BoxCollider2D meleeCollider;

    public BoxCollider2D MeleeCollider
    {
        get
        {
            return meleeCollider;
        }
    }

    public bool InMeleeRange
    {
        get
        {
            if (Target != null)
            {
                return Vector2.Distance(transform.position, Target.transform.position) <= meleeRange;

            }

            return false;
        }
    }

    public bool InThrowRange
    {
        get
        {
            if (Target != null)
            {
                return Vector2.Distance(transform.position, Target.transform.position) <= throwRange;

            }

            return false;
        }
    }

    public bool IsDead
    {
        get
        {
            return enemyHealthStat.CurrentVal <= 0;
        }
    }

    // Use this for initialization
    public void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        enemyHealthStat.Initialize();
        facingRight = true;
        ninja.Instance.Dead += new DeadEventHandler(RemoveTarget);
        ChangeState(new IdleState());
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        startpos = transform.position;
    }
	

	// Update is called once per frame
	public void Update ()
    {
        if (!IsDead && Renderer.isVisible)
        {
            if (!TakingDamage)
            {
                currentState.Execute();
            }
            LookAtTarget();
        }
    }

    public void RemoveTarget()
    {
        Target = null;
        ChangeState(new IdleState());
    }

    private void LookAtTarget()
    {
        if (Target != null)
        {
            float xDir = Target.transform.position.x - transform.position.x;

            if (xDir < 0 && facingRight || xDir > 0 && !facingRight)
            {
                ChangeDirection();
            }
        }
    }

    public void ChangeState(IEnemyState newState)
    {
        //if (currentState != null)  // all kinds of things can be placed to exit state of interfaces.
        //{
        //    currentState.Exit();
        //}

        currentState = newState;

        currentState.Enter(this);
    }

    public void Move()
    {
        if (!Attack)
        {
            if ((GetDirection().x > 0 && transform.position.x < rightTurnBackPoint.position.x) || (GetDirection().x < 0 && transform.position.x > leftTurnBackPoint.position.x))
            {
                anim.SetFloat("speed", 1);

                transform.Translate(GetDirection() * (speed * Time.deltaTime));
            }
            else if (currentState is PatrolState)
            {
                ChangeDirection();
            }
            else if (currentState is RangedState) // dont try run move animation the patrolling border while attacking if this enemy has not reached the border
            {
                anim.SetFloat("speed", 0);
            }
        }
    }
    public void ChangeDirection()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);

    }

    public Vector2 GetDirection()
    {
        return facingRight ? Vector2.right : Vector2.left;
    }

    public void Fire(int value)
    {
        audioSource.PlayOneShot(sounds[0]);
        if (facingRight)
        {
            GameObject tmp = (GameObject)Instantiate(bullet, bulletStartPos.position, Quaternion.Euler(new Vector3(0, 0, 0)));
            tmp.GetComponent<enemyBulletTrail>().Initialize(Target != null ? Target.transform : null, Vector2.right);
        }
        else
        {
            GameObject tmp = (GameObject)Instantiate(bullet, bulletStartPos.position, Quaternion.Euler(new Vector3(0, 0, 0)));
            tmp.GetComponent<enemyBulletTrail>().Initialize(Target != null ? Target.transform : null, Vector2.left);
        }
    }

    public void MeleeAttack()
    {
        MeleeCollider.enabled = true;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ninja"))
        {
            if (!collision.collider.GetComponent<ninja>().IsDead)
                StrikeBack();
        }
        if (collision.gameObject.CompareTag("arrow"))
        {
            TakeDamage(UnityEngine.Random.Range(10, 20), false, false);
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("ninjastar"))
        {
            TakeDamage(UnityEngine.Random.Range(5, 10), false, false);
        }
        if (other.gameObject.CompareTag("musketFire"))
        {
            TakeDamage(30, false, false);
        }
        if (other.gameObject.CompareTag("Katana"))
        {
            if (damageableFromKatana)
            {
                if (Target == null)
                {
                    TakeDamage(UnityEngine.Random.Range(100, 150), false, true);
                }
                else
                {
                    TakeDamage(30, false, true);
                }
                damageableFromKatana = false;
                StartCoroutine(katanaDamageableReactivation(other));
            }
        }
    }
    private void StrikeBack()
    {     
        if (ninja.Instance.transform.position.x < transform.position.x && facingRight || ninja.Instance.transform.position.x > transform.position.x && !facingRight)
        {
            ChangeDirection();
        }
        ChangeState(new RangedState());
    }

    IEnumerator enablingMove()
    {
        yield return new WaitForSeconds(15f);
        anim.SetTrigger("reset");
    }

    IEnumerator katanaDamageableReactivation(Collider2D katanaCollider)
    {
        yield return new WaitUntil(() => katanaCollider.enabled == false);
        damageableFromKatana = true;
    }

    public void TakeDamage(int damage, bool silentDamageSource, bool blood) // blood bool is for blood instantiated from where the bullets touch but this but this does not apply to melee and every ammo prefab have blood particle reference.
    {
        if (!healthCanvas.isActiveAndEnabled)
        {
            healthCanvas.enabled = true;
        }
        if (enemyHealthStat.CurrentVal > 0)
            enemyHealthStat.CurrentVal -= damage;
        if (blood)
            Instantiate(bloodParticles, transform.position, transform.rotation);
        enemyHealthStat.HandleBar();
        if (!IsDead)
        {
            anim.SetTrigger("damage");
            if (!silentDamageSource)
            {
                StrikeBack();
            }
        }
        else
        {
            anim.ResetTrigger("damage");
            anim.SetTrigger("die");
            StartCoroutine(Death(2));
        }
    }

    public IEnumerator Death(int deathTime)
    {
        rb.simulated = false;
        RemoveTarget();
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name.Contains("arrowPrefab"))
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
        yield return new WaitForSecondsRealtime(deathTime);
        healthCanvas.enabled = false;
        StartCoroutine(respawn());
    }

    public IEnumerator respawn()
    {
        foreach (var parameter in anim.parameters)
        {
            anim.ResetTrigger(parameter.name);
        }
        int respawnTime = 0;
        while (respawnTime < 1)
        {
            respawnTime++;
            yield return new WaitForSecondsRealtime(1);
        }
        anim.SetTrigger("reset");
        rb.simulated = true;
        enemyHealthStat.CurrentVal = 100;
        transform.position = new Vector3(startpos.x, startpos.y + 40, startpos.z);
    }
}
