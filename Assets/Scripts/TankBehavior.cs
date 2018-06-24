using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankBehavior : MonoBehaviour {

	public float moveDistance;

	public bool selected = false;

	public Vector3 oringinPos;

	TankManager tm;

	Ray ray;

	RaycastHit hit;

	GameObject selector;

	SprayedTankSelector sts;

	Camera cam;

	bool set = false;

	// Use this for initialization
	void Start () {
		//initTank();
	}
	
	// Update is called once per frame
	void Update () {
		if(set == false){
			initTank();
			set = true;
		}
		

		Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
		
		if(selected && Physics.Raycast(ray, out hit, sts.maxRayDistance, sts.sprayedTankLayer)){
			if(hit.transform.gameObject == gameObject){
				TankSelected();
			}
			else{
				//selected = false;
				TankCanceled();
			}
		}
		//else if(selected == false && transform.localPosition == (oringinPos + transform.up * moveDistance)){
		else{
			TankCanceled();
		}

	}

	public void initTank(){
		//Debug.Log("hit");
		transform.localPosition = oringinPos;
		tm = GetComponentInParent<TankManager>();
		selector = tm.selector;
		sts = selector.GetComponent<SprayedTankSelector>();
		cam = sts.cam;
	}

	public void TankSelected(){
		//StopAllCoroutines();
		//StartCoroutine(TankSelect());
		transform.localPosition = Vector3.Lerp(transform.localPosition, oringinPos + transform.up * moveDistance, 0.1f);
	}

	public void TankCanceled(){
		//StopAllCoroutines();
		//StartCoroutine(TankCancel());
		transform.localPosition = Vector3.Lerp(transform.localPosition, oringinPos, 0.1f);
	}

	IEnumerator TankSelect(){
		transform.localPosition = Vector3.Lerp(transform.localPosition, oringinPos + transform.up * moveDistance, 0.1f);
		yield return new WaitForSeconds(0.0f);
	}

	IEnumerator TankCancel(){
		transform.localPosition = Vector3.Lerp(transform.localPosition, oringinPos, 0.1f);
		yield return new WaitForSeconds(0.0f);
	}

}
