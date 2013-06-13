using UnityEngine;
using System.Collections;

public class Archer : MonoBehaviour 
{
	bool hasArrow = false;
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
	public float jumpForce;
	public float hoverForce;
	public float burstForce;
	
	ParticleSystem hoverParts;
	public GameObject burstPrefab;
	Transform burst1, burst2;
	
	// Use this for initialization
	void Start () 
	{
		//bow model was originally offset by 0.08806594 on x
		bow = transform.Find("Bow");
		//arrow = bow.Find("Arrow").gameObject;
		arrowStart = bow.transform.Find("ArrowStart");
		arrowEnd = bow.transform.Find("ArrowEnd");
		
		hoverParts = transform.Find("HoverParts").particleSystem;
		hoverParts.enableEmission = false;
		hoverParts.Play();
		
		burst1 = transform.Find("burst1pos");
		burst2 = transform.Find("burst2pos");
		
		Screen.lockCursor = true;
		Screen.showCursor = false;
		
		SetNewArrow();
	}
	
	// Update is called once per frame
	void Update () 
	{
		ProcessMovement();
		ProcessShooting();
	}
	
	void ProcessMovement()
	{
		float myY = rigidbody.velocity.y;
		
		moveForward = Input.GetAxis("Vertical") * transform.forward * runSpeed;
		moveStrafe = Input.GetAxis("Horizontal") * transform.right * strafeSpeed;
		myVelo = moveForward + moveStrafe;
		myVelo.y = myY;
		
		if(Input.GetKeyDown(KeyCode.LeftShift))
		{
			//rigidbody.AddForce(Vector3.up * burstForce);
			myVelo.y = burstForce;
			GameObject parts1 = (GameObject)Instantiate(burstPrefab, burst1.position, burstPrefab.transform.rotation);
			GameObject parts2 = (GameObject)Instantiate(burstPrefab, burst2.position, burstPrefab.transform.rotation);
			parts1.transform.parent = transform;
			parts2.transform.parent = transform;
			Destroy(parts1, 1.0f);
			Destroy(parts2, 1.0f);
		}
		
		rigidbody.velocity = myVelo;
		
		myRot = transform.eulerAngles;
		myRot.y += Input.GetAxis("Mouse X") * rotSpeed;
		transform.eulerAngles = myRot;
		
		myRot = bow.eulerAngles;
		myRot.x += Input.GetAxis("Mouse Y") * -rotSpeed;
		bow.eulerAngles = myRot;
		
		if(Input.GetKeyDown(KeyCode.Space))
		{
			//myVelo.y = jumpSpeed;
			rigidbody.AddForce(Vector3.up * jumpForce);
		}
		
		if(Input.GetMouseButton(1))
		{
			rigidbody.AddForce(Vector3.up * hoverForce);
		}
		
		if(Input.GetMouseButtonDown(1))
			hoverParts.enableEmission = true;
		else if(Input.GetMouseButtonUp(1))
			hoverParts.enableEmission = false;
	}
	
	void ProcessShooting()
	{
		if(hasArrow)
		{
			if(Input.GetMouseButton(0) || Input.GetKey(KeyCode.LeftControl))
			{
				if(pullAmount < 1)
				{
					pullAmount += (Time.deltaTime * 2);
					arrow.transform.position = Vector3.Lerp(arrowStart.position, arrowEnd.position, pullAmount);
				}
			}
			
			if(Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.LeftControl))
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
