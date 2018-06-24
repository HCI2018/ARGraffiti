using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeDetector : MonoBehaviour {

	GameObject realCam;
	Camera cam;

	// Use this for initialization
	void Start () {
		cam = GetComponentInParent<Camera>();
		realCam = cam.gameObject;
		Debug.Log(realCam);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
