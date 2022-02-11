using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grapplingHook : MonoBehaviour
{
	public LineRenderer line;
	DistanceJoint2D joint; // check Max Distance Only, we can get close to the object, otherwise the distance stays the max
	Vector3 targetPos;
	RaycastHit2D hit;
	public float distance = 20f; // the max position hook can go
	public LayerMask mask;
	public float step = 0.02f;

	// Use this for initialization
	void Start()
	{
		joint = GetComponent<DistanceJoint2D>();
		joint.enabled = false;
		line.enabled = false;
	}

	// Update is called once per frame
	void Update()
	{

		if (joint.distance > .5f)
			joint.distance -= step;
		else
		{
			line.enabled = false;
			joint.enabled = false;

		}


		if (Input.GetKeyDown(KeyCode.E))
		{
			targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			targetPos.z = 0;
			print("target position:"+targetPos);
			// use beam to detect collider
			hit = Physics2D.Raycast(transform.position, targetPos - transform.position, distance, mask); // origin, direction(vector), distance, layer
			print(hit);
			if (hit.collider != null && hit.collider.gameObject.GetComponent<Rigidbody2D>() != null)

			{
				joint.enabled = true;
				//	Debug.Log (hit.point - new Vector2(hit.collider.transform.position.x,hit.collider.transform.position.y);
				Vector2 connectPoint = hit.point - new Vector2(hit.collider.transform.position.x, hit.collider.transform.position.y);
				connectPoint.x = connectPoint.x / hit.collider.transform.localScale.x;
				connectPoint.y = connectPoint.y / hit.collider.transform.localScale.y;
				Debug.Log(connectPoint);
				
				// the connect anchor is not the center of the collider, but the surface
				joint.connectedAnchor = connectPoint;

				joint.connectedBody = hit.collider.gameObject.GetComponent<Rigidbody2D>();
				//		joint.connectedAnchor = hit.point - new Vector2(hit.collider.transform.position.x,hit.collider.transform.position.y);
				joint.distance = Vector2.Distance(transform.position, hit.point);

				line.enabled = true;
				line.SetPosition(0, transform.position);
				//line.SetPosition(1, hit.point);
				line.SetPosition(1, joint.connectedBody.transform.TransformPoint(joint.connectedAnchor));
				//line.GetComponent<roperatio>().grabPos = hit.point;


			}
			

		}


		else if (Input.GetKey(KeyCode.E))
		{

			line.SetPosition(0, transform.position);
		}


		if (Input.GetKeyUp(KeyCode.E))
		{
			joint.enabled = false;
			line.enabled = false;
		}

	}
}
