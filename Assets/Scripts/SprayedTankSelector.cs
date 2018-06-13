using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;

public struct SelectorResult{
	public Color color;
	public Color lastColor;
	public bool found;
}

public class SprayedTankSelector : MonoBehaviour {

    public float maxRayDistance = 30.0f;

	public LayerMask sprayedTankLayer = 1 << 11; //SprayedTank Layer

	[HideInInspector]
	public Camera cam;

	SelectorResult sr;

	void Start(){
		cam = GetComponentInParent<Camera>();
		sr = new SelectorResult();
		sr.lastColor = Color.white;
	}

	public SelectorResult HitTestInGameSpace(float maxRayDistance, LayerMask layer){
		Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
		RaycastHit hit;
		
		if(Physics.Raycast(ray, out hit, maxRayDistance, layer)){
			if(hit.transform.tag == "Box"){
				sr.found = false;
			}
			else{
				sr.found = true;
				sr.color = hit.transform.GetComponent<ColorInfo>().color;
				hit.transform.GetComponent<TankBehavior>().selected = true;
			}
		}
		else{
				sr.found = false;
				//sr.color = Color.white;
		}
		return sr;
	}

	public bool HitTestTank(float maxRayDistance, LayerMask layer){
		Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit, maxRayDistance, layer)){
			if(hit.transform.tag == "Box"){
				return false;
			}
			else{
				return true;
			}
		}
		else{
				return false;
		}
	}

	public void SetLastColor(Color color){
		sr.lastColor = color;
	}

}
