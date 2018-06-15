using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScaningButton : MonoBehaviour {

	//public GameObject planeGenerator;

	//public GameObject Scan;

	//public GameObject Lock;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ScanSwitch(){
		// if(planeGenerator.activeInHierarchy == true){
		// 	planeGenerator.SetActive(false);
		// 	//Scan.SetActive(true);
		// 	//Lock.SetActive(false);
		// }
		// else{
		// 	planeGenerator.SetActive(true);
		// 	//Scan.SetActive(false);
		// 	//Lock.SetActive(true);
		// }
	}

	public void Lock(GameObject planeGenerator){
		if(planeGenerator.activeInHierarchy == true){
			planeGenerator.SetActive(false);
		}
	}

}
