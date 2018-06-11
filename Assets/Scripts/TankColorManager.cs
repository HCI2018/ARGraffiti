using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[RequireComponent(typeof(ColorInfo))]
public class TankColorManager : MonoBehaviour {

	ColorInfo ci;

	MaterialPropertyBlock mpb;

	Renderer rr;

	// Use this for initialization
	void Start () {
		ci = GetComponentInParent<ColorInfo>();
		mpb = new MaterialPropertyBlock();
		rr = gameObject.GetComponent<Renderer>();
		SetColor();
	}

	void SetColor(){
		rr.GetPropertyBlock(mpb);
		mpb.SetColor("_Color", ci.color);
		rr.SetPropertyBlock(mpb);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
