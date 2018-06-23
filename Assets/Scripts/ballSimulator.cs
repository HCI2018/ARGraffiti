using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballSimulator : MonoBehaviour {

	public GameObject ballTracker;

	public GameObject cam;

	public GameObject ball;

	public Vector3 gravityVec ;

	public float gravityForce = 10;

	Vector3 currentPos;

	Vector3 lastPos;

	Vector3 distance;

	Rigidbody ballRb;

	// Use this for initialization
	void Start () {
		ballRb = ball.GetComponent<Rigidbody>();
		lastPos = cam.transform.position;
	}
	
	// Update is called once per frame
	private void FixedUpdate()
	{
		inverseMove();	
		
		gravityVec = cam.transform.InverseTransformVector(0, -1, 0);

		ballRb.AddForce(gravityVec.x * gravityForce, gravityVec.y * gravityForce, gravityVec.z * gravityForce);

	}

	void inverseMove(){
		currentPos =  cam.transform.position;
		distance = currentPos - lastPos;
		//Debug.Log("camDis: " + distance);
		distance = -transform.InverseTransformVector(distance);
		//Debug.Log("ballDis: " + distance);
		ballRb.position += distance;
		lastPos = currentPos;
	}
	

}
