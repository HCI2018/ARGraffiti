using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCancelButton : MonoBehaviour {

	[HideInInspector]
	public GameObject selectBtn;

	ColorButton cb;

	// Use this for initialization
	void Start () {
		//cb = colorBtn.GetComponent<ColorButton>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void cancelSelect(){
		cb = selectBtn.GetComponent<ColorButton>();
		cb.SelectCancel();
		gameObject.SetActive(false);
	}
}
