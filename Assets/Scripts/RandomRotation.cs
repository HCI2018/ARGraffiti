using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotation : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Vector3 eulerAngles = transform.rotation.eulerAngles;

		Quaternion rot = Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.forward);

		// eulerAngles.y = Random.Range(0f, 360f);

		// Quaternion newRot = Quaternion.Euler(eulerAngles);
		transform.rotation *= rot ;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
