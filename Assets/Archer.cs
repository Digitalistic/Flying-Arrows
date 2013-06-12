using UnityEngine;
using System.Collections;

public class Archer : MonoBehaviour 
{
	bool hasArrow = true;
	float pullAmount = 0;
	
	public GameObject arrowPrefab;
	GameObject arrow;
	
	Transform arrowStart, arrowEnd;
	
	public float shotForce;
	
	Transform bow;
	
	public float runSpeed, strafeSpeed;
	Vector3 moveForward, moveStrafe;
	Vector3 myVelo;
	
	Vector3 myRot;
	public float rotSpeed;
	public float jumpSpeed;
	
	// Use this for initialization
	void Start () 
	{
		//bow model was originally offset by 0.08806594 on x
		bow = transform.Find("Bow");
		arrow = bow.Find("Arrow").gameObject;
		arrowStart = bow.transform.Find("ArrowStart");
		arrowEnd = bow.transform.Find("ArrowEnd");
		
		Screen.lockCursor = true;
		Screen.showCursor = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		ProcessMovement();
		ProcessShooting();
	}
	
	void ProcessMovement()
	{
		moveForward = Input.GetAxis("Vertical") * transform.forward * runSpeed;
		moveStrafe = Input.GetAxis("Horizontal") * transform.right * strafeSpeed;
		myVelo = moveForward + moveStrafe;
		
		if(Input.GetKeyDown(KeyCode.Space))
		{
			myVelo.y = jumpSpeed;
		}
		
		rigidbody.velocity = myVelo;
		
		myRot = transform.eulerAngles;
		myRot.y += Input.GetAxis("Mouse X") * rotSpeed;
		transform.eulerAngles = myRot;
		
		myRot = bow.eulerAngles;
		myRot.x += Input.GetAxis("Mouse Y") * -rotSpeed;
		bow.eulerAngles = myRot;
	}
	
	void ProcessShooting()
	{
		if(hasArrow)
		{
			if(Input.GetMouseButton(0))
			{
				if(pullAmount < 1)
				{
					pullAmount += (Time.deltaTime * 2);
					arrow.transform.position = Vector3.Lerp(arrowStart.position, arrowEnd.position, pullAmount);
				}
			}
			
			if(Input.GetMouseButtonUp(0))
			{
				arrow.rigidbody.useGravity = true;
				arrow.rigidbody.isKinematic = false;
				arrow.transform.parent = null;
				arrow.rigidbody.velocity = (arrow.transform.forward * shotForce * pullAmount) + rigidbody.velocity;
				arrow.SendMessage("Fire");
				Invoke("SetNewArrow", 1.0f);
				
				hasArrow = false;
			}
		}
	}
	
	void SetNewArrow()
	{
		arrow = (GameObject)Instantiate(arrowPrefab, arrowStart.position, arrowStart.rotation);
		arrow.transform.parent = bow;
		
		pullAmount = 0;
		hasArrow = true;
	}
}
