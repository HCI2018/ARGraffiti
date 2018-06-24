using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class CancelSetTankButton : MonoBehaviour {

	//public Vector3 targetPosition;


	//SetTankButton stb;

	RectTransform rt;

	// Use this for initialization
	void Start () {
		//stb = GetComponentInParent<SetTankButton>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void initCancelSet(){
		rt = GetComponent<RectTransform>();
	}

	
}
