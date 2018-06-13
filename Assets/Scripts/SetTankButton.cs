using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetTankButton : MonoBehaviour {

	[HideInInspector]
	public GameObject selectBtn;

	GameObject tm;

	SprayedTankSelector sts; 

	Camera cam;

	Vector3 temp = new Vector3();

	LayerMask arPlaneLayer = 1 << 10; //ARPlane Layer

	//float maxRayDistance;

	// Use this for initialization
	// void Start () {
	// 	tm = selectBtn.GetComponent<ColorButton>().tankManager;
	// 	sts = selectBtn.GetComponent<ColorButton>().selector.GetComponent<SprayedTankSelector>();
	// 	//maxRayDistance = sts.maxRayDistance;
	// 	//cam = sts.cam;
	// }
	
	// Update is called once per frame
	void Update () {
		
	}

	public void initSetTankButton(){
		tm = selectBtn.GetComponent<ColorButton>().tankManager;
		sts = selectBtn.GetComponent<ColorButton>().selector.GetComponent<SprayedTankSelector>();
	}

	public void SetTank(){
		sts.updateCam();
		cam = sts.cam;
		SetTankReal(sts.maxRayDistance, arPlaneLayer);
	}

	void SetTankReal(float maxRayDistance, LayerMask layer){
		Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
		RaycastHit hit;
		
		if(Physics.Raycast(ray, out hit, maxRayDistance, layer)){
			if(testPlaneHorizontal(hit.transform.gameObject)){
				tm.transform.position = hit.transform.position;

				temp = (cam.transform.position - tm.transform.position);

				tm.transform.forward = new Vector3(-temp.x, 0, -temp.z);
			}
		}
	}

	bool testPlaneHorizontal(GameObject plane){

		float angle = Vector3.Angle (plane.transform.up, new Vector3(0, 1, 0));

		if(angle <= 30 && angle >= -30){
			return true;
		}
		else{
			return false;
		}

	}

}
