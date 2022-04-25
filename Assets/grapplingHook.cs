using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grapplingHook : MonoBehaviour
{
    public Camera cam;
    public LineRenderer line;
    DistanceJoint2D joint;
    Vector3 targetPos;
    RaycastHit2D hit;
    public float distance = 2f;
    public LayerMask mask;
    public float step = 0.04f;
    GameObject hitCollide;
    Vector2 tempPos;
    Vector3 currentPos;
    Vector2 anchorPos;
    Vector2 lookDirection;
    Vector2 connectPoint;
    public bool hooking = false;

    // Use this for initialization
    void Start()
    {
        joint = GetComponent<DistanceJoint2D>();
        joint.enabled = false;
        transform.GetComponent<NinjaMove>().isHook = false;
        line.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (joint.distance > .5f && joint.connectedBody != null)
        {
            if (joint.connectedBody.tag != "wall")
            {
                joint.distance -= step;
            }
        }
        /*else
        {
         print("disable hook rn");
         line.enabled = false;
         joint.enabled = false;

        }*/


        if (Input.GetKeyDown(KeyCode.E))
        {
            
            currentPos = GetComponent<Rigidbody2D>().transform.position;
            // print("shooting hook");
            targetPos = cam.ScreenToWorldPoint(Input.mousePosition);
            targetPos.z = 0;
            // print("target: "+targetPos);
            // print("current:" + currentPos);
            // print("dir: " + (targetPos - currentPos));
            targetPos.z = 0;
            //lookDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - currentPos;//transform.position;
            //tempPos = transform.position;
            //tempPos.x = tempPos.x * Mathf.Cos(transform.rotation.z);
            //tempPos.y = tempPos.y * Mathf.Sin(transform.rotation.z);
            //Debug.DrawLine(transform.position, lookDirection);
            hit = Physics2D.Raycast(transform.position, targetPos - currentPos, distance, mask);
            //targetPos - transform.position, distance, mask);

            if (hit.collider != null)

            {
                print(transform.position);
                print(hit.point);
                print("hit");
                // print("hit point: "+hit.point);
                hooking = true;
                joint.enabled = true;
                transform.GetComponent<NinjaMove>().isHook = true;
                transform.parent = hit.collider.transform;
                anchorPos = new Vector2(hit.collider.gameObject.GetComponent<Rigidbody2D>().transform.position.x, hit.collider.gameObject.GetComponent<Rigidbody2D>().transform.position.y);
                print("anchor: " + anchorPos);
                hitCollide = hit.collider.gameObject;
                Vector2 tempConnect;
                connectPoint = hit.point - anchorPos;
                tempConnect = connectPoint;
                tempConnect.x = tempConnect.x / hit.collider.transform.localScale.x;
                tempConnect.y = tempConnect.y / hit.collider.transform.localScale.y;
                Debug.Log(connectPoint);


                joint.connectedBody = hit.collider.gameObject.GetComponent<Rigidbody2D>();
                joint.anchor = new Vector2(0, 0);
                joint.distance = Vector2.Distance(currentPos, hit.point);
                joint.connectedAnchor = hit.collider.gameObject.GetComponent<Rigidbody2D>().transform.InverseTransformDirection(tempConnect);
                line.enabled = true;
                line.SetPosition(0, transform.position);
                line.SetPosition(1, hit.point);

                //line.GetComponent<roperatio>().grabPos = hit.point;
                //line.SetPosition(1, joint.connectedBody.transform.TransformPoint(joint.connectedAnchor));

            }

        }

        if (Input.GetKey(KeyCode.E))
        {

            line.SetPosition(0, transform.position);
            if (joint.enabled)
            {
                Vector3 temp = hitCollide.transform.position;
                temp.x += connectPoint.x;
                temp.y += connectPoint.y;
                line.SetPosition(1, temp);
            }
        }


        if (Input.GetKeyUp(KeyCode.E))
        {
            transform.parent = null;
            joint.enabled = false;
            hooking = false;
            transform.GetComponent<NinjaMove>().isHook = false;
            line.enabled = false;
        }

    }



}