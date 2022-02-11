using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grapplingHook : MonoBehaviour
{
	public LineRenderer line;
	DistanceJoint2D joint;
	Vector3 targetPos;
	RaycastHit2D hit;
	public float distance = 2f;
	public LayerMask mask;
	public float step = 0.04f;
	Vector2 tempPos;
	Vector3 currentPos;
	Vector2 anchorPos;
	Vector2 lookDirection;
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
			currentPos = GetComponent<Rigidbody2D>().transform.position;
			print("shooting hook");
			targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			targetPos.z = 0;
			print("target: "+targetPos);
			print("current:" + currentPos);
			print("dir: " + (targetPos - currentPos));
			targetPos.z = 0;
			//lookDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - currentPos;//transform.position;
			//tempPos = transform.position;
			//tempPos.x = tempPos.x * Mathf.Cos(transform.rotation.z);
			//tempPos.y = tempPos.y * Mathf.Sin(transform.rotation.z);
			//Debug.DrawLine(transform.position, lookDirection);
			hit = Physics2D.Raycast(transform.position, targetPos- currentPos, distance, mask);
			//targetPos - transform.position, distance, mask);

			if (hit.collider != null )

			{
				/*print(transform.position);
				print(currentPos);
				print("hit");*/
				print("hit point: "+hit.point);
				
				joint.enabled = true;
				
				anchorPos = new Vector2(hit.collider.gameObject.GetComponent<Rigidbody2D>().transform.position.x, hit.collider.gameObject.GetComponent<Rigidbody2D>().transform.position.y);
				print("anchor: " + anchorPos);
				Vector2 connectPoint = hit.point - anchorPos;
				connectPoint.x = connectPoint.x / hit.collider.transform.localScale.x;
				connectPoint.y = connectPoint.y / hit.collider.transform.localScale.y;
				Debug.Log(connectPoint);
				

				joint.connectedBody = hit.collider.gameObject.GetComponent<Rigidbody2D>();
				joint.distance = Vector2.Distance(currentPos, hit.point);
				joint.connectedAnchor = hit.collider.gameObject.GetComponent<Rigidbody2D>().transform.InverseTransformDirection(connectPoint);
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
		}


		if (Input.GetKeyUp(KeyCode.E))
		{
			
			joint.enabled = false;
			line.enabled = false;
		}

	}
}
