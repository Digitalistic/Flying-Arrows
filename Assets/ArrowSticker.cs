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
	
	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
			a.Stick(other.transform, true);
		else
			a.Stick(other.transform, false);
	}
	
	void EnableCollider()
	{
		collider.enabled = true;
	}
}
