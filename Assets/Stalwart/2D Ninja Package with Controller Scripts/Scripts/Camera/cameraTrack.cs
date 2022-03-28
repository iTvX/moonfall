using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraTrack : MonoBehaviour
{
    private static cameraTrack instance;

    public static cameraTrack Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<cameraTrack>();
            }
            return instance;
        }
    }

    public Camera mainCamera;
    public RectTransform cameraRuinedCanvas;
    public Animator mainCameraAnimator;

    public ninja Ninja;
    private Transform trackingObject;

    private Vector3 playerPosition;
    private Vector3 CatchThePlayer;

    public float xOffset, yOffset;
    public float cameraSpeed = 5f;
    public float cameraRotationSpeed = 3.5f;

    [Space]
    public Vector3 mouseLocalPoint;
    public bool cursorInScreen = false;
    private Vector3 mouseViewportPoint;
    float attackTimer = 0;

    [Space]
    public float xMax, xMin, yMax, yMin;

    private IEnumerator MoveCameraXRotation;

    private void Awake()
    {
        trackingObject = Ninja.transform;
    }

    void FixedUpdate() // ninja shakes if LateUpdate because camera follows lag behind while lateupdate
    {
        //calculates where mouse is on the screen
        mouseLocalPoint = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraRuinedCanvas.position.z));

        playerPosition = new Vector3(Mathf.Clamp(trackingObject.position.x + xOffset, xMin, xMax), Mathf.Clamp(trackingObject.position.y + yOffset, yMin, yMax), transform.position.z);
        CatchThePlayer = Vector3.Lerp(transform.position, playerPosition, cameraSpeed * Time.deltaTime); // catch the player slowly
        transform.position = CatchThePlayer;

        // if cursor is in the screen 
        mouseViewportPoint = mainCamera.ScreenToViewportPoint(Input.mousePosition);
        cursorInScreen = mouseViewportPoint.x > 0 && mouseViewportPoint.x < 1 && mouseViewportPoint.y > 0 && mouseViewportPoint.y < 1;
        if (((Ninja.rangedRemind && !Ninja.isCrouch) || (Ninja.horizontal == 0 && attackTimer <= 0)))
        {
            if (cursorInScreen && !Ninja.OnWall)
            {
                if (Ninja.rangedRemind) // camera moves with cursor if ninja has ranged weapon
                {
                    xOffset = mouseLocalPoint.x - CatchThePlayer.x; // camera offset changes according to mouse position
                    yOffset = mouseLocalPoint.y - CatchThePlayer.y;
                }

                //Flip Ninja above cursor
                if (mouseLocalPoint.x < Ninja.transform.position.x && Ninja.FacingRight || mouseLocalPoint.x > Ninja.transform.position.x && !Ninja.FacingRight)
                {
                    Ninja.ChangeDirection();
                }
            }
        }
        directionAttackTimer();
    }
    private void directionAttackTimer()
    {
        if (Input.GetKey(Ninja.attackKey) && (Input.GetKey(Ninja.runLeftKey) || Input.GetKey(Ninja.runRightKey)) || Ninja.OnWall)
        {
            attackTimer = 2;
        }
        if (Input.GetAxis("Mouse X") > 0.1f || Input.GetAxis("Mouse Y") > 0.1f) //  input settings must have these entries! if not, create a Mouse X and Mouse Y axes and set these type as "Mouse Movement" from Edit > Project Settings > Input.
            attackTimer = 0;
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
            if (Input.GetKey(Ninja.attackKey))
                attackTimer = 2;
        }
    }
}