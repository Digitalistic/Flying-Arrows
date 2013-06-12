using UnityEngine;
using System.Collections;

public class ArrowSticker : MonoBehaviour 
{
	Arrow a;
	
	// Use this for initialization
	void Start () 
	{
		a = transform.parent.GetComponent<Arrow>();
	}
	
	void OnCollisionEnter(Collision other)
	{
		a.Stick();	
	}
	
	void OnTriggerEnter(Collider other)
	{
		a.Stick();
	}
	
	void EnableCollider()
	{
		collider.enabled = true;
	}
}
