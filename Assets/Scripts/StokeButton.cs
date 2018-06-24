using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//static GameObject 

public class StokeButton : MonoBehaviour {

	bool found = false;



	// Use this for initialization
	void Start () {
				
	}
	
	// Update is called once per frame
	void Update () {
		GameObject.FindGameObjectWithTag("ArtBoard");
	}
}
