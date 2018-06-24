using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehavior : MonoBehaviour {

	Rigidbody rb;

	public float velocityLimit = 1;

	ballSimulator sim;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		sim = GetComponentInParent<ballSimulator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	

	private void OnCollisionEnter(Collision other)
	{
		ContactPoint contact = other.contacts[0];

		float normalVelocity = Vector3.Dot(rb.velocity, contact.normal);
		
		//Debug.Log((int)(normalVelocity));

		if((int)(normalVelocity) >= velocityLimit || (int)(normalVelocity) <= -velocityLimit){
			sim.hitEfect();
		}
	}
}
