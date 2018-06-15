using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetTankButton : MonoBehaviour {

	[HideInInspector]
	public GameObject selectBtn;

	//public GameObject setText;

	public GameObject cancelSetButton;

	public Sprite originSprite;

	public Sprite setSprite;

	public bool setted;

	public Vector3 targetPos;

	public float timeOfTravel = 5.0f;

	float currentTime = 0;
	float normalizedValue = 0;

	//public GameObject settedText;

	Image image;

	GameObject tankManager;

	TankManager tm;

	SprayedTankSelector sts; 

	Camera cam;

	Vector3 temp = new Vector3();

	LayerMask arPlaneLayer = 1 << 10; //ARPlane Layer

	ColorButton cb;

	Vector3 originPos;

	Quaternion originRot;

	RectTransform rt;

	Vector3 originRectPos;

	private void Start()
	{
		rt = GetComponent<RectTransform>();
		originRectPos = rt.anchoredPosition3D;
		image = GetComponent<Image>();
		image.sprite = setSprite;
		MoveToPosition(targetPos);
	}

	void Update()
	{
		SetTank();
		if(setted == false){
			RealTimeMovingTank(sts.maxRayDistance, arPlaneLayer);	
		}
		else{

		}
		
	}

	public void initSetTankButton(){
		cb = selectBtn.GetComponent<ColorButton>();
		tm = cb.tm;
		tankManager = cb.tankManager;
		sts = cb.selector.GetComponent<SprayedTankSelector>();
		
	}

	public void SetTank(){
		sts.updateCam();
		cam = sts.cam;

	

		//SetTankReal(sts.maxRayDistance, arPlaneLayer);
	}

	void SetTankReal(float maxRayDistance, LayerMask layer){
		Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
		RaycastHit hit;
		
		if(Physics.Raycast(ray, out hit, maxRayDistance, layer)){
			if(testPlaneHorizontal(hit.transform.gameObject)){
				//Debug.Log(hit.transform.position);
				//tm.transform.position = hit.transform.position;

				tankManager.transform.position = hit.point;

				temp = (cam.transform.position - tankManager.transform.position);

				tankManager.transform.forward = new Vector3(-temp.x, 0, -temp.z);
			}
		}
	}


	void RealTimeMovingTank(float maxRayDistance, LayerMask layer){
		Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
		RaycastHit hit;
		
		if(Physics.Raycast(ray, out hit, maxRayDistance, layer)){
			if(testPlaneHorizontal(hit.transform.gameObject)){
				//Debug.Log("hori");
				//Debug.Log(hit.transform.position);
				//tm.transform.position = hit.transform.position;

				tankManager.transform.position = hit.point;

				temp = (cam.transform.position - tankManager.transform.position);

				tankManager.transform.forward = new Vector3(-temp.x, 0, -temp.z);
			}
			else{
				tankManager.transform.position = cam.transform.position + cam.transform.forward * 1f;
				temp = (cam.transform.position - tankManager.transform.position);
				tankManager.transform.forward = new Vector3(-temp.x, 0, -temp.z);
			}
		}
		else{
			//Debug.Log("no");
			tankManager.transform.position = cam.transform.position + cam.transform.forward * 1f;
			temp = (cam.transform.position - tankManager.transform.position);
			tankManager.transform.forward = new Vector3(-temp.x, 0, -temp.z);
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

	public void setModeSwitch(){
		if(setted == true){
			originPos = tankManager.transform.position;
			originRot = tankManager.transform.rotation;
			image.sprite = setSprite;
			cancelSetButton.SetActive(true);
			cancelSetButton.GetComponent<CancelSetTankButton>().initCancelSet();
			MoveToPosition(targetPos);
		}
		else{
			image.sprite = originSprite;
			cancelSetButton.SetActive(false);
			MoveToPosition(originRectPos);
		}
		setted ^= true; 
	}


	public void SetOriginPos(){
		tankManager.transform.position = originPos;
		tankManager.transform.rotation = originRot;
		setted = true;
		image.sprite = originSprite;
		cancelSetButton.SetActive(false);
		MoveToPosition(originRectPos);
	}

	public void textSwitch(){
		StartCoroutine(SetSwitch());
	}

	IEnumerator SetSwitch(){
		//settedText.SetActive(true);
		//setText.SetActive(false);
		yield return new WaitForSeconds(1.0f);
		//settedText.SetActive(false);
		//setText.SetActive(true);

	}

	public void MoveToPosition(Vector3 targetPos){
		StopAllCoroutines();
		currentTime = 0;
		StartCoroutine(moving(targetPos));
	}


	IEnumerator moving(Vector3 targetPos){
		while(currentTime < timeOfTravel){
			currentTime += Time.deltaTime;
			normalizedValue = currentTime / timeOfTravel;
			rt.anchoredPosition3D = Vector3.Lerp(rt.anchoredPosition3D, targetPos, normalizedValue);
			yield return null;
		}
	} 

}
