using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSetting : MonoBehaviour {

	public GameObject tankManager;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.transform.position = tankManager.transform.position;
		gameObject.transform.up = tankManager.transform.up;
		gameObject.transform.forward = tankManager.transform.forward;
		//gameObject.transform.right = tankManager.transform.right;
	}
}
