using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ballSimulator : MonoBehaviour {

	//public GameObject ballTracker;

	public CameraPlaytest cameraPlaytest;

	GameObject cam;

	public GameObject ball;

	public Vector3 gravityVec;

	public float gravityForce = 10;

	public float distFactor = 1f;

	Vector3 currentPos;

	Vector3 lastPos;

	Vector3 distance;

	Rigidbody ballRb;

	Vector3 originPos;
	
	bool rendererIsInsideTheBox;

	Bounds bounds;

	[HideInInspector]
	public BoxCollider boxCol;

	public event EventHandler ballHit;

	AudioSource audioS;

	public BrushGenerator bg;

	public float addVolume = 0.05f;

	public float transportFactor = 0.5f;

	// Use this for initialization
	void Start () {

		audioS = GetComponent<AudioSource>();

		ballRb = ball.GetComponent<Rigidbody>();
		originPos = ball.transform.position;
		boxCol = GetComponent<BoxCollider>();
		bounds = boxCol.bounds;

		cam = cameraPlaytest.activeCamera.gameObject;
		lastPos = cam.transform.position;
	}
	
	// Update is called once per frame
	private void FixedUpdate()
	{
		inverseMove();	
		
		gravityVec = cam.transform.InverseTransformVector(0, -1, 0);

		//Debug.Log(gravityVec.x * gravityForce + " " + gravityVec.y * gravityForce + " " +gravityVec.z * gravityForce);

		ballRb.AddForce(gravityVec.x * gravityForce, gravityVec.y * gravityForce, gravityVec.z * gravityForce);

		//ballRb.AddForce(0, -1f, 0);

		rendererIsInsideTheBox = bounds.Contains(ball.transform.position);
		//Debug.Log(rendererIsInsideTheBox);
		if(!rendererIsInsideTheBox){
			ballRb.position = originPos;
			ballRb.velocity *= transportFactor;
		}

	}

	void inverseMove(){
		currentPos =  cam.transform.position;
		distance = currentPos - lastPos;
		//Debug.Log("camDis: " + distance);
		distance = -transform.InverseTransformVector(distance);
		//Debug.Log("ballDis: " + distance);
		Vector3 estimated = ballRb.position + distance * distFactor;
		ballRb.MovePosition(estimated);

		lastPos = currentPos;
	}
	
	private void OnTriggerExit(Collider other)
	{
		other.gameObject.GetComponent<Rigidbody>().position = originPos;
		hitEfect();
	}

	public void hitEfect(){
		audioS.Play();
	
		iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.ImpactHeavy);
		flowAdd();
	}

	void flowAdd(){
		bg.AddFlowVolume(addVolume);
		//Debug.Log("flowAdd");
	}

}