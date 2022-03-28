using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {
	



	public static Transform mTransform;
	public static Animator mAnimator;
	public static Rigidbody2D mRigidbody2D;

	public static float activeSpeed;

	public static float minSpeed = 1.0f;
	public static float maxSpeed = 12.0f;
	public static float playerHP;
	public static float jumpStamina;

	public static Transform modelTransform;
	
	public float addSpeed = 3.0f;
	public float boosterSpeed = 10.0f;
	public float jumpForce = 14000f;

	bool isMoveRight = true;

	private float jumpPenalty;
	private float jumpPower;
	private float realMaxSpeed;
	private float realActiveSpeed;
	private float realAddSpeed;
	private float realRegenJumpStamina;
	private float realRegenStaminaSec;
	private float realPlayerLossHP;
	private Vector3 mVector3;
	private bool isAccelerationON;
	private bool isSitChangeON;
	private int intAutoTimer;
	private bool isJumpKey;
	private bool isWalkKey;
	private bool isAutoSitSpeed;
	private bool isboosterON;

	private float modelScaleX;
	private float modelScaleY;
	private float modelScaleZ;

	void Awake () {
		mTransform = GetComponent<Transform>();
	}

	void Start () {
		modelTransform = GameObject.Find("Model").GetComponent<Transform>();

		mRigidbody2D = GetComponent<Rigidbody2D>();
		mAnimator = GameObject.Find ("player").transform.Find("Model").transform.GetChild(0).gameObject.GetComponent<Animator> ();
		realMaxSpeed = maxSpeed;
		activeSpeed = minSpeed;
		mAnimator.SetBool("IsEat", false);
		mAnimator.SetBool("IsWait", true);
		intAutoTimer = 2;
		StartCoroutine(AutoAnimation());
		modelScaleX = modelTransform.localScale.x;
		modelScaleY = modelTransform.localScale.y;
		modelScaleZ = modelTransform.localScale.z;
	}

	IEnumerator AutoAnimation(){
		yield return new WaitForSeconds (1.3f);
		if (!isAutoSitSpeed && !mAnimator.GetBool ("IsWalk") && !mAnimator.GetBool ("IsRun") && !mAnimator.GetBool ("IsJumpUp") && !mAnimator.GetBool ("IsJumpDown")) {
			intAutoTimer++;
			if(intAutoTimer >= 10){
				intAutoTimer = 1;
				mAnimator.SetBool("IsEat", true);
				mAnimator.SetBool("IsWait", false);
			}
			else if(intAutoTimer >= 7){
				mAnimator.SetBool("IsEat", false);
				mAnimator.SetBool("IsWait", true);
			}
			else if(intAutoTimer >= 5){
				mAnimator.SetBool("IsEat", true);
				mAnimator.SetBool("IsWait", false);
			}
			else if(intAutoTimer >= 2){
				mAnimator.SetBool("IsEat", false);
				mAnimator.SetBool("IsWait", true);
			}
		}
		else {
			AutoAnimationCancle();
		}
		StartCoroutine(AutoAnimation());
	}

	void AutoAnimationCancle(){
		mAnimator.SetBool("IsEat", false);
		mAnimator.SetBool("IsWait", false);
		intAutoTimer = 0;
	}

	void SetJumpSpeed(){
		if((activeSpeed * 0.8f) <= (realMaxSpeed * 0.5f)){
			realActiveSpeed = (realMaxSpeed * 0.5f);
		}
		else {
			realActiveSpeed = (activeSpeed * 0.8f);
		}
		realAddSpeed = 0.0f;;
	}


	void Update () {
		if (mRigidbody2D.velocity.y > 0.1f) {
			SetJumpSpeed();
			if(isAutoSitSpeed){
				mAnimator.SetBool("IsDive", true);
			}
			else {
				mAnimator.SetBool("IsDive", false);
				mAnimator.SetBool("IsJumpUp", true);
				mAnimator.SetBool("IsJumpDown", false);
			}
		}
		else if (mRigidbody2D.velocity.y < -0.1f) {
			SetJumpSpeed();
			if(isAutoSitSpeed){
				mAnimator.SetBool("IsDive", true);
			}
			else {
				mAnimator.SetBool("IsDive", false);
				mAnimator.SetBool("IsJumpUp", false);
				mAnimator.SetBool("IsJumpDown", true);
			}
		}
		else {
			mAnimator.SetBool("IsJumpUp", false);
			mAnimator.SetBool("IsJumpDown", false);
			mAnimator.SetBool("IsDive", false);
		}


		if (Input.GetKey(KeyCode.LeftShift)) {
			if (!Input.GetKey (KeyCode.LeftShift) && (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D))) {
				isWalkKey = false;
			} else if (!Input.GetKey (KeyCode.LeftShift) && (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A))) {
				isWalkKey = false;
			} else {
				isWalkKey = true;
			}
		}
		else {
			isWalkKey = false;
		}


		if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) {
			AutoAnimationCancle();
			if(!isJumpKey){
				mRigidbody2D.velocity = new Vector2 (mRigidbody2D.velocity.x,0.0f);
				mRigidbody2D.AddForce (new Vector2 (0.0f, jumpForce));
				isJumpKey = true;
			}
		}
		else {
			realActiveSpeed = activeSpeed;
			realAddSpeed = addSpeed;
			isJumpKey = false;
		}
		

		if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) {
			AutoAnimationCancle();
			isAutoSitSpeed = true;
			if (!isSitChangeON){
				StartCoroutine(SitChangeOFF());
				isSitChangeON = true;
				mAnimator.SetBool("IsSitdown", true);
				if(realActiveSpeed >= maxSpeed){
					isboosterON = true;
				}
			}

		}
		else {
			if (activeSpeed > maxSpeed) {
				mAnimator.SetBool("IsSitdown", true);
				isboosterON = false;
				isAutoSitSpeed = true;
			}
			else {
				mAnimator.SetBool("IsSitdown", false);
				isboosterON = false;
				isAutoSitSpeed = false;
			}
		}
		
		if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
			AutoAnimationCancle();
			ControlRight();
		}
		else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) {
			AutoAnimationCancle();
			ControlLeft();
		}
		else {
			mAnimator.SetBool("IsRun", false);
			mAnimator.SetBool("IsWalk", false);
			activeSpeed = minSpeed;
		}

	}
		

	IEnumerator SitChangeOFF(){
		yield return new WaitForSeconds (0.5f);
		isSitChangeON = false;
	}
	

	void ControlRight(){
		isMoveRight = true;
		mVector3 = Vector3.right;
		modelTransform.rotation = Quaternion.Euler(0, 0, 0);
		modelTransform.localScale = new Vector3(modelScaleX , modelScaleY, modelScaleZ);
		ControlMove ();
	}
	
	void ControlLeft(){
		isMoveRight = false;
		mVector3 = Vector3.left;
		modelTransform.rotation = Quaternion.Euler(0, 180f, 180f);
		modelTransform.localScale = new Vector3(-modelScaleX , -modelScaleY, -modelScaleY);
		ControlMove ();
	}


	void ControlMove(){
		if (!isAccelerationON){
			StartCoroutine(Acceleration());
			isAccelerationON = true;
		}

		mTransform.Translate( mVector3 * realActiveSpeed * Time.deltaTime);

		if (activeSpeed >= (realMaxSpeed*0.5)) {
			mAnimator.SetBool("IsRun", true);
			mAnimator.SetBool("IsWalk", false);
		}
		else {
			mAnimator.SetBool("IsRun", false);
			mAnimator.SetBool("IsWalk", true);
		}
	}

	IEnumerator Acceleration(){
		yield return new WaitForSeconds (0.1f);
		if (isboosterON) {
			realMaxSpeed = maxSpeed + boosterSpeed;
		}
		else {
			realMaxSpeed = realMaxSpeed - (realAddSpeed * 0.5f);
			realMaxSpeed = Mathf.Max(realMaxSpeed,maxSpeed);
		}
		if (isWalkKey) activeSpeed = minSpeed;
		activeSpeed = activeSpeed + realAddSpeed;
		if (activeSpeed >= realMaxSpeed) {
			activeSpeed = realMaxSpeed;
		}
		isAccelerationON = false;
	}
}
