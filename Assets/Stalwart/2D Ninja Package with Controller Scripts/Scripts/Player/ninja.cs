using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public delegate void DeadEventHandler();
public class ninja : MonoBehaviour
{
    // this instance for find objects in this script 
    private static ninja instance;

    public static ninja Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<ninja>();
            }
            return instance;
        }
    }

    [SerializeField]
    private float Jumpspeed;

    [SerializeField]
    private float MoveSpeed;

    public int health;

    public float horizontal;

    [SerializeField]
    private float airSpeed;

    [SerializeField]
    private float stealthMoveSpeed = 10;

    [SerializeField]
    private float direction;

    private float buttonHorizontal;

    [SerializeField]
    private float climbdirection;

    private float buttonVertical;

    public bool Shooting;

    public int extraJumps = 2;

    public int jumpLimit;

    [SerializeField]
    private bool leaping;

    [SerializeField]
    public bool isCrouch;

    public GameObject rangedProfile;

    public Animator rangedAnimator;

    [SerializeField]
    private GameObject ninjastar;

    public GameObject shadowEffect;
    private bool shadowEffectIsActive = false;

    private Vector2 shadowEffectStealthPos = new Vector2(0f, -0.3f);

    public GameObject dustEffect;

    //public bool spawnDust;

    public bool ranged{ get; set; }

    public bool rangedRemind;

    public int weapontype;

    public bool IsInputEnabled = true; //reachable bool

    public Animator anim { get; set; }

    public Rigidbody2D rb { get; set; }

    public cameraTrack mainCamera;

    public Vector3 startSize { get; set; }
    public CapsuleCollider2D ninjaCollider;

    private Vector2 crouchSize;   // x = default, y = 0.9476676f
    private Vector2 crouchOffset;  // x = default, y = -0.4411662f

    private Vector2 standSize; // default x = 0.78, default y = 1.86
    private Vector2 standOffset; // default x = 0, default y = 0;

    [SerializeField]
    protected float speed;

    [SerializeField]
    protected bool facingRight = true;

    public bool FacingRight
    {
        get
        {
            return facingRight;
        }
    }

    public bool TakingDamage { get; set; }

    public bool Attacking { get; set; }
    public bool WallJumping { get; set; }

    private int randomizedAttack;

    private Vector3 startpos;
    public bool OnGround { get; set; }
    public bool OnWall { get; set; }
    private Collider2D lastTouchedWall;

    public AudioSource audioSource;

    // [0] and [1] is katana swing, [2] is throw, [3] is jumping sound, [4] is footstep, [5] is bow fire, [6] is rifle fire sound
    public AudioClip[] sounds;

    public RangedController rangedController;

    [SerializeField]
    private GameObject bloodParticles;

    [Header("Control Keys (Will be changed after game is start)")]
    public KeyCode attackKey;
    public KeyCode jumpKey;
    public KeyCode throwStarKey;
    public KeyCode changeWeaponKey;
    public KeyCode crouchKey;
    public KeyCode runLeftKey;
    public KeyCode runRightKey;
    [Space(10)]

    [SerializeField]
    private List<string> damageSources;

    [SerializeField]
    public Transform ninjastarpos;

    private Vector2 ninjaStarStealthPos = new Vector2(0.8f, 0f);

    private Vector2 ninjaStarDefaultPos = new Vector2(0.8f, 0.4f);

    [SerializeField]
    private GameObject Katana;

    [SerializeField]
    private Collider2D katanaInflictDamageArea;

    public Transform katanaEffectPos;

    public GameObject katanaFlashEffect;
    public event DeadEventHandler Dead;

    public void Start()
    {
        startpos = new Vector3(transform.position.x, transform.position.y + 10, transform.position.z);
        startSize = new Vector3(Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y), Mathf.Abs(transform.localScale.z));
        standSize = ninjaCollider.size;
        standOffset = ninjaCollider.offset;
        crouchSize = new Vector2(standSize.x, standSize.y / 1.498f); // crouch size and offset rates
        crouchOffset =  new Vector2(standOffset.x, crouchSize.y / -4.215279026431433f);
        ranged = false;  
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        facingRight = true;
        OnGround = true;
        Crouch();
        jumpLimit = extraJumps;
        airSpeed = speed / 1.5f; // air speed is 100/75 of movement speed for balance
        if (!shadowEffect.activeSelf)
            shadowEffect.SetActive(true);
        shadowEffect.GetComponent<Animator>().keepAnimatorControllerStateOnDisable = true; //  Controls the behaviour of the Animator component when a GameObject is disabled but unsupported by older than 2018.1 versions of unity. 
        shadowEffect.SetActive(false);
    }

    [ContextMenu("Set Default Inputs")]
    public void setDefaultInputs()
    {
        runLeftKey = KeyCode.A;
        runRightKey = KeyCode.D;
        jumpKey = KeyCode.Space;
        attackKey = KeyCode.E;
        throwStarKey = KeyCode.Q;
        crouchKey = KeyCode.LeftControl;
        changeWeaponKey = KeyCode.Alpha1;
    }

    // Update is called once per frame
     public void Update()
    {
        if (!TakingDamage && !IsDead)
        {
            inputs();
        }
    }
    private void Movement(float horizontal)
    {
        if (!ranged)
        {
            anim.SetFloat("speed", Mathf.Abs(rb.velocity.x));
        }
        else
        {
            if (facingRight)
            {
                anim.SetFloat("speed", rb.velocity.x);
            }
            else
            {
                anim.SetFloat("speed", -rb.velocity.x);
            }
        }
        if ((OnWall || !Attacking) && !leaping) // move horizontal freely independent from attacking when character on wall 
        {
            // I used to freeze position feature just because character slipping down on slopes or curved grounds.
            // So when character doesn't move (that means if horizontal == 0, horizontal value determining by movement keys), the position froze and character cant able to slipping down to slope.
            // moving with transform.Translate can be used and objects physics materials friction value can be set 
            // higher value to prevent this from happening as an alternative so it just slides from a certain height as
            // determined by rigidbodys custom physics material but the jump settings should be restructured at this point.
            if (horizontal != 0)
            {
                rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
                if ((rb.constraints & RigidbodyConstraints2D.FreezePositionX) == RigidbodyConstraints2D.FreezePositionX)  // runs the function if rigid bodys only rotation x is freezed because it doesnt work for all time instead of only work once.
                {
                    rb.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
                }  
            }
            else if (!OnWall) // if character freezez position x while onwall then character not automatically placed in the correct position when on wall
            {
                if ((rb.constraints & RigidbodyConstraints2D.FreezePositionX) != RigidbodyConstraints2D.FreezePositionX)
                {
                    rb.constraints |= RigidbodyConstraints2D.FreezePositionX;
                }
            }
        }
        if ((horizontal < 0 && rb.velocity.x > 0 || horizontal > 0 && rb.velocity.x < 0) && leaping == true)
        {
            leaping = false;
        } 
    }
    private void ReturnInput()
    {
        IsInputEnabled = true;
    }

    private void Flip (float horizontal)
    {
        if ((horizontal > 0 && !facingRight || horizontal < 0 && facingRight) && !ranged) // runs if character moving right or left and -facingRight bool is opposite of movement, character always turns while moving if facingRight bool is not added in this function
        {
            ChangeDirection();
        }
    }

    public void ChangeDirection()
    {
        facingRight = !facingRight;
        if (facingRight)
        {
            transform.localScale = startSize;
        }
        else
        {
            transform.localScale = new Vector3(-startSize.x, startSize.y, startSize.z);
        }
    }

    public void IsGrounded(Collider2D groundCollider) //check if Ninja touches ground collider, (if touches value turns true)
    {
        if (OnGround) // return move speeed to normal, and takeweapon already true and returns and return crouch if crouch value is true after fall on the ground from air if character on ground
        {
            anim.SetBool("onAir", false);
            anim.SetLayerWeight(2, 0);
            jumpLimit = extraJumps;
            leaping = false;
            Instantiate(dustEffect, transform.position, Quaternion.identity);
            mainCamera.mainCameraAnimator.SetTrigger("shake");
            audioSource.PlayOneShot(sounds[4]);
            Crouch();
            if (rangedRemind)
            {
                rangedAnimator.SetLayerWeight(1, 0);
                anim.SetLayerWeight(5, 0);
            }
        }
        else // if checked is not on ground, then crouch disabled for a while of course falls, take ranged weapons if already crounch and ranged weapons are true
        {
            speed = airSpeed;
            anim.SetBool("onAir", true);
            if (!ranged)
            {
                anim.SetLayerWeight(2, 1);
            }
            if (rangedRemind)
            {
                ranged = true;
                rangedWeapon();
            }
            if (isCrouch)
            {
                anim.SetLayerWeight(4, 0);
            }
        }
    }
    public void FixedUpdate()
    {
        if (!IsDead && !TakingDamage && IsInputEnabled)
        {
            //+ Custom horizontal axis input. Alternatively "horizontal = Input.GetAxis("Horizontal")" can be used but I believe this consumes a little less resources, i may be wrong so...
            if (Input.GetKey(runLeftKey) && horizontal > -1) // if horizontal is not less than -1
            {
                horizontal = Mathf.Clamp(horizontal - 0.1f, -1, 0); // horizontal lessing - 0.1f, every frame within positive lowest value(0)
            }
            if (Input.GetKey(runRightKey) && horizontal < 1)
            {
                horizontal = Mathf.Clamp(horizontal + 0.1f, 0, 1); // horizontal increasing increasing + 0.1f every frame, highest value is 1
            }
            if (!Input.GetKey(runLeftKey) && !Input.GetKey(runRightKey) && horizontal != 0)
            {
                horizontal = 0; // setting horizontal zero when move keys are not pressed. this line runs if horizontal is not zero, this makes it work once.
            }
            if (Input.GetKey(runLeftKey) && Input.GetKey(runRightKey) && horizontal != 0) // character stops when both button pressed
            {
                horizontal = 0;
            }
            //-
            Movement(horizontal);
            Flip(horizontal);
        }
        if (transform.position.y <= -14f) //death if out of area 
        {
            Respawn();
        }
        if (rb.velocity != Vector2.zero)
        {
            if (!shadowEffectIsActive)
            {
                shadowEffectIsActive = true;
                shadowEffect.SetActive(shadowEffectIsActive);
                // Animator shadowEffectAnim = shadowEffect.GetComponent<Animator>();  // moving the shadow effect to further down position while crouched, enable this if unity version older than 2018.1 because these older versions unsupport the Animator.keepAnimatorControllerStateOnDisable property.
                // if (isCrouch)
                //     shadowEffectAnim.SetBool("stealth", true);
                // else
                //     shadowEffectAnim.SetBool("stealth", false);
            }
        }
        else
        {
            if (shadowEffectIsActive)
            {
                shadowEffectIsActive = false;
                shadowEffect.SetActive(shadowEffectIsActive);
            }
        }
    }

    private void inputs() // return functions according to any control key pressed
    {
        if (IsInputEnabled)
        {
            if (Input.GetKeyDown(jumpKey) && jumpLimit > 0)
            {
                StartCoroutine(jumpSpringTimer());
                jumpButton();
            }
            if (Input.GetKeyDown(attackKey))
            {
                attackButton();
            }
            if (Input.GetKeyDown(throwStarKey))
            {
                starButton();
            }
            if (Input.GetKeyDown(crouchKey))
            {
                Stealth();
            }
            if (Input.GetKeyDown(changeWeaponKey))
            {
                changeWeapon(-1);
            }
        }
    }

    public void rangedWeapon()
    {
        if (ranged == true) // ranged profile object is activated and changes its animator controller according to changing weapon type integer
        {
            if (!OnWall)
                rangedProfile.SetActive(true);
            rangedController.changeCursor(); // reaches ranged controller script for changes cursor to musket or bow type of cursor
            if (weapontype == 1)
            {
                rangedController.anim.runtimeAnimatorController = rangedController.bowAnimatorController;
            }
            if (weapontype == 2)
            {
                rangedController.anim.runtimeAnimatorController = rangedController.musketAnimatorController;
            }
            anim.SetLayerWeight(1, 1);
            if (isCrouch == true && OnGround) // dont take any ranged weapon if ninja is crouched
            {
                ranged = false;
                rangedWeapon();
            }
            if (!OnGround && !OnWall) // change animator layers to including stand in the air with ranged weapon animations layers
            {
                anim.SetLayerWeight(2, 0);
                anim.SetLayerWeight(5, 1);
                rangedAnimator.SetLayerWeight(1, 1);
            }
        }
        else //deactive ranged profile
        {
            rangedProfile.SetActive(false);
            anim.SetLayerWeight(1, 0);
            if (!rangedRemind)
            {
                mainCamera.xOffset = 0;
                mainCamera.yOffset = 0;
            }
            if (weapontype == 0)
            {
                Cursor.SetCursor(default, default, default);
            } 
            if (!OnGround)
            {
                anim.SetLayerWeight(2, 1);
                anim.SetLayerWeight(5, 0);
            }
            else if (anim.GetLayerWeight(5) == 1)
            {
                anim.SetLayerWeight(5, 0);
            }
            if (isCrouch == true)
            {
                anim.SetLayerWeight(5, 0);
            }
        }
    }
    public void changeWeapon(int chooseWeapon) // increasing weapon type number and take a weapon for weapon type value and so change weapon type for every press change weapon button
    {
        if (!Attacking)
        {
            weapontype = chooseWeapon == -1 ? weapontype + 1 : chooseWeapon;
            switch (weapontype) //default is return non ranged because switch is dont return always zero without default case
            {
                case 0:
                    ranged = false;
                    rangedRemind = false;
                    break;
                case 1:
                    ranged = true;
                    rangedRemind = true;
                    break;
                case 2:
                    ranged = true;
                    rangedRemind = true;
                    break;
                default:
                    weapontype = 0;
                    ranged = false;
                    rangedRemind = false;
                    break;
            }
            rangedWeapon();
        }
    }

    private void Stealth() //stealth bool equals reverse every runs
    {
        isCrouch = !isCrouch;
        Crouch();
    }

    public void Crouch(bool ignoreOnGround = false)
    {
        if (OnGround || ignoreOnGround)
        {
            if (isCrouch)
            {
                if (horizontal == 0 && !Attacking)  // because if horizontal is not zero crouching triggered while running
                    anim.SetTrigger("crouch");
                anim.SetLayerWeight(4, 1);
                speed = stealthMoveSpeed;
                ninjaCollider.size = crouchSize; //change ninja collider size to crouch collider size and offset
                ninjaCollider.offset = crouchOffset;
                shadowEffect.GetComponent<Animator>().SetBool("stealth", true);
                ninjastarpos.localPosition = ninjaStarStealthPos;
                if (ranged) // disable ranged value if take ranged weapon when crouched, activates after stand up if ranged remind value is true
                {
                    ranged = false;
                    rangedWeapon();
                }
            }
            else
            {
                if (horizontal == 0 && !Attacking)
                    anim.SetTrigger("standup");
                anim.SetLayerWeight(4, 0);
                if (!OnWall)
                    speed = MoveSpeed;
                ninjaCollider.size = standSize; // collider size and offset sets to normal
                ninjaCollider.offset = standOffset;
                shadowEffect.GetComponent<Animator>().SetBool("stealth", false);
                ninjastarpos.localPosition = ninjaStarDefaultPos;
                if (!ranged && rangedRemind && !OnWall) // provite to take ranged weapon if ranged reminder bool is true 
                {
                    ranged = true;
                    rangedWeapon();
                }
            }
        }

    }

    public IEnumerator MeleeAttack()
    {
        if (Attacking)
        {
            yield return new WaitForSeconds(0.15f);
            Katana.GetComponent<Collider2D>().enabled = true;
            katanaInflictDamageArea.enabled = true;
            yield return new WaitForSeconds(0.15f);
            Katana.GetComponent<Collider2D>().enabled = false;
            if (Katana.GetComponent<katanaCollider>().sliding)
                yield return new WaitWhile(() => Katana.GetComponent<katanaCollider>().sliding);
            katanaInflictDamageArea.enabled = false;
        }
        Attacking = false;
    }
    public void MeleeAttackSound()
    {
        if (!OnWall)
        {
            int random = UnityEngine.Random.Range(1, 3);
            audioSource.PlayOneShot((random == 1) ? sounds[0] : sounds[1]);  // randomize melee attack sound
        }
    }
    public void MeleeAttackEffect()
    {
        if (!OnWall)
        {
            GameObject effect = Instantiate(katanaFlashEffect, katanaEffectPos.position, Quaternion.Euler(transform.localScale));
            effect.GetComponent<katanaFlashing>().Initialize(randomizedAttack, facingRight);
        }
    }

    public IEnumerator ThrowStar()
    {
        yield return new WaitForSeconds(0.2f);
        GameObject ninjaStr = Instantiate(ninjastar, ninjastarpos.position, ninjastarpos.rotation);
        if (!OnWall)
            ninjaStr.GetComponent<ninjaStar>().Initialize(facingRight ? Vector2.right : Vector2.left);
        else
            ninjaStr.GetComponent<ninjaStar>().Initialize(facingRight ? Vector2.left : Vector2.right);
        audioSource.PlayOneShot(sounds[2]);
        Attacking = false;
        yield return null;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (damageSources.Contains(other.tag)) // any object tagged by take able damage list
        {
            if (other.CompareTag("EnemyMelee"))
                TakeDamage(UnityEngine.Random.Range(1, 2), true); //damages random between 5 and 10 
            else
                TakeDamage(UnityEngine.Random.Range(1, 2), false);
        }
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            lastTouchedWall = collision.collider;
            if (!isCrouch)
            {
                OnWall = true;
                anim.Play("Wall Stand", 3);
                Crouch(true);
                Katana.transform.localScale = new Vector2(-1, 1);
                ninjastarpos.localPosition = new Vector2(-ninjastarpos.localPosition.x, ninjastarpos.localPosition.y);
            }
            if (collision.gameObject.CompareTag("Wall"))
                transform.position = new Vector3(transform.position.x, transform.position.y, startpos.z);
        }
    }
    public void OnCollisionStay2D(Collision2D collision2)
    {
        if (OnWall)
            if (collision2.gameObject.CompareTag("Wall"))
            {
                Collider2D collision = lastTouchedWall;  // this code for stay on only one wall while touching two or more walls so staying on first touched wall
                if (OnWall && !isCrouch)
                {
                    if ((rb.constraints & RigidbodyConstraints2D.FreezePositionY) != RigidbodyConstraints2D.FreezePositionY && !WallJumping)
                        rb.constraints |= RigidbodyConstraints2D.FreezePositionY;
                    if ((rb.constraints & RigidbodyConstraints2D.FreezePositionX) == RigidbodyConstraints2D.FreezePositionX)  // this for unfreeze rigid body pos x when character jumping through the wall and rb pos x, because if not, then character displaced when catching the wall
                        rb.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
                    if (jumpLimit != extraJumps)
                        jumpLimit = extraJumps;
                    if (leaping)
                        leaping = false;
                    if ((transform.position.x < collision.transform.position.x && !facingRight || transform.position.x > collision.transform.position.x && facingRight) && !Attacking)
                        ChangeDirection();
                    rb.velocity = new Vector2(collision.transform.position.x - transform.position.x, rb.velocity.y); //moves the character towards the wall as long as character on the sourface
                    float gripPointY = collision.transform.GetChild(0).position.y - (ninjaCollider.size.y / 2 * transform.localScale.y);  //in here, size,y multiples with characters local scale because, transforms scale affects the "real" collider size
                    if (transform.position.y > gripPointY && rb.velocity.y <= 0)
                        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, gripPointY, transform.position.z), 20 * Time.deltaTime);
                    float lowerGripPointY = collision.transform.GetChild(1).position.y + (ninjaCollider.size.y / 2 * transform.localScale.y);  //same situation as upper description
                    if (transform.position.y < lowerGripPointY && rb.velocity.y <= 0)
                        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, lowerGripPointY, transform.position.z), 20 * Time.deltaTime);
                    if (anim.GetLayerWeight(3) == 0 && transform.position.y <= gripPointY || transform.position.y >= lowerGripPointY)
                    {
                        anim.SetLayerWeight(3, 1);
                        if (ranged)
                        {
                            ranged = false;
                            rangedWeapon();
                        }
                    }
                }
                if (isCrouch)
                {
                    OnCollisionExit2D(collision2);
                }
            }
    }
    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") && !ninjaCollider.IsTouching(lastTouchedWall))
        {
            // rb.velocity = Vector2.zero;
            OnWall = false;
            if ((rb.constraints & RigidbodyConstraints2D.FreezePositionY) == RigidbodyConstraints2D.FreezePositionY)
                rb.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
            anim.SetLayerWeight(3, 0);
            Katana.transform.localScale = new Vector2(1, 1);
            ninjastarpos.localPosition = new Vector2(-ninjastarpos.localPosition.x, ninjastarpos.localPosition.y);
            if (!OnGround && (rb.velocity.x > 2 || rb.velocity.x < -2))
            {
                leaping = true;
            }
            if (rangedRemind)
            {
                ranged = true;
                rangedWeapon();
                if (mainCamera.mouseLocalPoint.x < transform.position.x && FacingRight || mainCamera.mouseLocalPoint.x > transform.position.x && !FacingRight) // cameraTrack already has this lines but without this lines character will turn a bit late to the mouse point while ranged
                {                                                                                                                                              // the reason for this situation is using fixedupdate in cameraTrack script instead of only update but fixed update is more optimized.
                    ChangeDirection();
                }
            }
        }
    }

    IEnumerator camYOffsetOnJump()
    {
        while (mainCamera.yOffset != 0)
        {
            mainCamera.yOffset = Mathf.MoveTowards(mainCamera.yOffset, 0, 8 * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }
    }
    
    IEnumerator wallJumpigDuration()
    {
        yield return new WaitUntil(() => rb.velocity.y < 0);
        WallJumping = false;
    }

    IEnumerator waitForActualizedJump()
    {
        yield return new WaitForSeconds(0.2f); // given time to jumping activity
        OnGround = false;
    }

    IEnumerator jumpSpringTimer()
    {
        float timer = 0;
        while (timer < 1)
        {
            timer += 0.15f;
            yield return new WaitForFixedUpdate();
        }
        if (Input.GetKey(jumpKey) && (Input.GetKey(runRightKey) || Input.GetKey(runLeftKey)))
        {
            leaping = true;
            rb.velocity = new Vector2(transform.localScale.x * MoveSpeed, rb.velocity.y);
        }
    }

    public void jumpButton()
    {
        if (jumpLimit > 0) // jump if limit is not end, value sets default after grounded
        {
            int random = UnityEngine.Random.Range(1, 3);
            if (!Attacking && !OnWall)
            {
                if (random == 1)
                {
                    anim.SetTrigger("jump");
                }
                else if (jumpLimit == extraJumps)
                {
                    anim.SetTrigger("jump2");
                }
            }
            if (rb.velocity.x != 0) // returns if rigid bodys velocity didnt increased and decreased too much
            {
                leaping = true; //character cant be controlled if character is jump far away 
            }
            if (!OnWall)
            {
                jumpLimit--;
            }
            else
            {
                WallJumping = true;
                if ((rb.constraints & RigidbodyConstraints2D.FreezePositionY) == RigidbodyConstraints2D.FreezePositionY)
                    rb.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
                StartCoroutine(wallJumpigDuration());
                anim.SetTrigger("jump"); // always play jump anim if character on wall
            }
            // Debug.Log(horizontal);
            // if (horizontal > 0.4f || horizontal < -0.4f)
            // {
            //     leaping = true;
            //     spring = true;
            // }
            rb.velocity = new Vector2(rb.velocity.x, Jumpspeed);
            Instantiate(dustEffect, transform.position, Quaternion.identity);
            audioSource.PlayOneShot(sounds[3]);
            if (!ranged)
            {
                anim.SetLayerWeight(2, 1);
            }
            else // activate air layer of ranged weapons
            {
                anim.SetLayerWeight(5, 1);
                rangedAnimator.SetLayerWeight(1, 1);
            }
            if (isCrouch)
            {
                anim.SetLayerWeight(4, 0);
            }
            if (transform.position.y < mainCamera.yMin)
            {
                if (OnGround)
                {
                    float camDiff = Mathf.Abs(mainCamera.transform.position.y - transform.position.y);
                    float yVal = camDiff < 2 ? camDiff : 2;
                    mainCamera.yOffset = yVal;
                }
                else
                {
                    StartCoroutine(camYOffsetOnJump());
                }
            }
            StartCoroutine(waitForActualizedJump());
        } 
    }
    public void attackButton()
    {
        if (!Attacking) // attack if character doesn't already attacking
        {
            Attacking = true;
            randomizedAttack = UnityEngine.Random.Range(1, 3);
            StartCoroutine(MeleeAttack());
            if (randomizedAttack == 1)
            {
                anim.SetTrigger("attack");
            }
            else
            {
                anim.SetTrigger("attack2");
            }
        }
    }
    public void starButton()
    {
        if (!Attacking && !ranged)
        {
            Attacking = true;
            StartCoroutine(ThrowStar());
            anim.SetTrigger("throw");
        }
    }

    public void TakeDamage(int damage, bool blood) // blood bool is for blood instantiated from where the bullets touch but this but this does not apply to melee and every ammo prefab have blood particle reference.
    {
        if (blood && !IsDead)
            Instantiate(bloodParticles, transform.position, transform.rotation);
        if (!IsDead)
        {
            health -= damage;
            if (!Attacking && !OnWall)
            {
                anim.SetTrigger("damage");
            }
        }
        if (IsDead) // else is not used so that it can be checked again when health is lower than or equal zero
        {
            anim.SetTrigger("die");
        }
    }

    public bool IsDead
    {
        get
        {
            if (health <= 0)
            {
                if (ranged == true)
                {
                    ranged = false;
                    weapontype = 0;
                    rangedWeapon();
                }
                if (OnWall)
                {
                    OnWall = false;
                    if ((rb.constraints & RigidbodyConstraints2D.FreezePositionY) == RigidbodyConstraints2D.FreezePositionY)
                        rb.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
                }
                anim.SetLayerWeight(1, 0);
                anim.SetLayerWeight(2, 0);
                anim.SetLayerWeight(3, 0);
                anim.SetLayerWeight(4, 0);
                anim.SetLayerWeight(5, 0);
                rb.velocity = Vector2.zero;
                mainCamera.enabled = false;
            }

            return health <= 0;

        }
    }
    public void Respawn() // resets character health values and position
    {    
        rb.velocity = Vector2.zero;
        anim.ResetTrigger("die");
        health = 100;
        transform.position = startpos;
        anim.SetTrigger("idle");
        mainCamera.enabled = true;
    }
}
