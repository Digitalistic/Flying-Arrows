using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour 
{
	Vector3 myRot;
	bool isAlive = false;
	
	float rotY, rotZ;
	Vector3 eulers;
	
	//TrailRenderer trail;
	ParticleSystem particles;
	
	void Start()
	{
		//rigidbody.centerOfMass = new Vector3(0, 0, 0.3f);
		//trail = transform.Find("Trail").GetComponent<TrailRenderer>();
		particles = transform.Find("Particle System").particleSystem;
		//particles.Play();
	}
	
	void OnCollisionEnter(Collision other)
	{
		/*rigidbody.velocity = Vector3.zero;
		rigidbody.Sleep();
		rigidbody.isKinematic = true;*/
		
		isAlive = false;
		Invoke("KillTrail", 0.5f);
		Destroy(gameObject, 10.0f);
	}
	
	void Update()
	{
		if(isAlive)
		{
			rotY = transform.eulerAngles.y;
			rotZ = transform.eulerAngles.z;
			
			myRot = transform.position + rigidbody.velocity.normalized;
			transform.LookAt(myRot);
			
			eulers = transform.eulerAngles;
			eulers.y = rotY;
			eulers.z = rotZ;
			transform.eulerAngles = eulers;
		}
	}
	
	void Fire()
	{
		isAlive = true;
		//Invoke("StartTrail", 0.1f);
		StartTrail();
	}
	
	void StartTrail()
	{
		//trail.enabled = true;
		particles.Play();
	}
	
	void KillTrail()
	{
		//trail.enabled = false;
		particles.Stop();
	}
}
