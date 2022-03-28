using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedController : MonoBehaviour
{
    private int flipRotationOffset = 180;
    
    private bool cursorImageIsCrosshair = false;

    public Animator anim;
    public RuntimeAnimatorController bowAnimatorController;
    public RuntimeAnimatorController musketAnimatorController;
    public ninja ninja;
    public Transform diffStartPos;

    public RangedWeapon rangedWeapon;

    public cameraTrack mainCamera;
    private Vector2 difference;

    public Texture2D musketCrosshair;
    public Texture2D bowCrosshair;
    public Texture2D cursorTexture
    {
        get
        {
            return (ninja.weapontype == 1) ? bowCrosshair : (ninja.weapontype == 2) ? musketCrosshair : null;
        }
    }
    public Vector2 cursorHotSpot
    {
        get
        {
            return new Vector2(bowCrosshair.height / 2, bowCrosshair.width / 2);
        }
    }
    CursorMode cursorMode = CursorMode.ForceSoftware;

    public RectTransform blockingCursorArea;
    public RectTransform blockingCursorArea02;

    void Update()
    {
        //Use "RectTransformUtility.RectangleContainsScreenPoint(RectTransform rect, Vector2 screenPoint, Camera cam)" in these areas with that "Camera" parameter if UI Canvas Render Mode is relative to Camera.
        if (!RectTransformUtility.RectangleContainsScreenPoint(blockingCursorArea, Input.mousePosition) && !RectTransformUtility.RectangleContainsScreenPoint(blockingCursorArea02, Input.mousePosition) && mainCamera.cursorInScreen)
        {
            difference = mainCamera.mouseLocalPoint - diffStartPos.position;
            difference.Normalize(); //normalizing vector, equals to 1
            //Limited rotation and rotate ninja to difference from above
            float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg; // converts difference values angle to degree
            rotZ += (rotZ < 90) ? 270 : -90; // makes degree to counterclockwise

            if (ninja.FacingRight) 
            {
                transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Clamp(rotZ, 175, 340) + 90);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Clamp(rotZ, 20, 185) + 90 + flipRotationOffset);
            }

            //Weapon input
            if (Input.GetMouseButtonUp(0) && !ninja.Attacking)
            {
                if (ninja.weapontype == 1)
                {
                    rangedWeapon.ShootBow(rotZ + 90);

                }
                else if (ninja.weapontype == 2)
                {
                    rangedWeapon.ShootMusket(rotZ + 90);
                }
            }

            if (!cursorImageIsCrosshair)
            {
                changeCursor();
            }
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            if (cursorImageIsCrosshair)
            {
                cursorImageIsCrosshair = false;
                Cursor.SetCursor(default, default, default);
            }
        }
        //Animator
        if (ninja.FacingRight)
        {
            anim.SetFloat("speed", ninja.rb.velocity.x);
        }
        else
        {
            anim.SetFloat("speed", -ninja.rb.velocity.x); // setting negative speed value for walking to back in ninja animator controller 
        }
    }
    public void changeCursor()
    {
        try
        {
            if (!RectTransformUtility.RectangleContainsScreenPoint(blockingCursorArea, Input.mousePosition) && !RectTransformUtility.RectangleContainsScreenPoint(blockingCursorArea02, Input.mousePosition))
            {
                Cursor.SetCursor(cursorTexture, cursorHotSpot, cursorMode); // cursorTexture value always return according to ninjas weaponType integer
                cursorImageIsCrosshair = true;
            }
        }
        catch
        {
            Cursor.SetCursor(bowCrosshair, cursorHotSpot, CursorMode.Auto);
            Debug.Log("error: cursormode cant be changed to software mode");
        }
    }
    private void OnEnable()
    {
        if (cursorTexture == null)
        {
            changeCursor();
            Debug.Log("Ranged Weapons actived but cursor has not been changed or weaponType integer still zero");
        }
    }

    private void OnDisable()
    {
        if (ninja.weapontype == 0)
        {
            Cursor.SetCursor(default, default, default);
        }
    }
}
