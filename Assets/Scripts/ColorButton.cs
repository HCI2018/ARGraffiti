using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ColorButton : MonoBehaviour/*, IPointerUpHandler, IPointerDownHandler*/ {

	public GameObject selector;

	public GameObject tankManager;

	public GameObject box;

	RectTransform rt;

	TankManager tm;

	Image image;

	Button button;

	SprayedTankSelector sts;

	SelectorResult sr;

	Vector3 selectorPos;

	public bool isSelecting = false;

	public GameObject setTankBtn;

	SetTankButton stb;

	public GameObject cancelBtn;

	SelectCancelButton scb;

	Vector3 originPos;

	Vector3 selectedPos;

	float currentTime = 0;

	float timeOfTravel = 1;

	float normalizedValue;

	// Use this for initialization
	void Start () {
		rt = GetComponent<RectTransform>();
		originPos = rt.anchoredPosition3D;
		selectedPos = new Vector3(0, originPos.y, 0);
		image = GetComponent<Image>();
		sts = selector.GetComponent<SprayedTankSelector>();
		tm = tankManager.GetComponent<TankManager>();
		sr = new SelectorResult();
		selectorPos = new Vector3();
		button = GetComponent<Button>();
		button.onClick.AddListener(SelectTask);
		scb = cancelBtn.GetComponent<SelectCancelButton>();
		scb.selectBtn = gameObject;

		stb = setTankBtn.GetComponent<SetTankButton>();
		stb.selectBtn = gameObject;
		stb.initSetTankButton();
	}
	
	// Update is called once per frame
	void Update () {

		//Debug.Log(originPos);
		//Debug.Log(selectedPos);
		//Debug.Log(selectorPos);
		selectorPos = selector.transform.position;
        if (isSelecting) {  
			// if(tankManager.activeInHierarchy == false){
			// 	tankManager.transform.position = selectorPos;
				
			// 	tankManager.SetActive(true);
			// 	box.SetActive(true);
			// 	tm.initTankManager();
			// 	tm.SortTank(tm.sortType);
			// }
			RaycastAndSet();
            // if (Time.time - lastIsDownTime > delay) {   
            //     lastIsDownTime = Time.time;  
			// 	RaycastAndSet();
            // }  
        }  
	}

	void RaycastAndSet(){
		if(selector != null){
			sr = sts.HitTestInGameSpace(sts.maxRayDistance, sts.sprayedTankLayer);
			if(sr.found == true){
				image.color = sr.color;
			}
			else{
				if(sr.lastColor == new Color(0,0,0,0)){
					image.color = Color.white;
				}
				else{
					image.color = sr.lastColor;
				}
				
			}
		}
	}

    private float delay = 0.2f;  

    private bool isDown = false;  
  
    private float lastIsDownTime;  

	// public void OnPointerDown (PointerEventData eventData)  
    // {  
    //     isDown = true;  
    //     lastIsDownTime = Time.time;  
    // }  
  
    // public void OnPointerUp (PointerEventData eventData)  
    // {  
    //     isDown = false;  
		
	// 	if(sts.HitTestTank(sts.maxRayDistance, sts.sprayedTankLayer)){
	// 		image.color = sr.color;
	// 		sts.SetLastColor(sr.color);
	// 	}
	// 	else{
	// 		image.color = sr.lastColor;
	// 	}

	// 	tankManager.SetActive(false);
	// 	box.SetActive(false);
    // }  

	void SelectTask(){
		if(isSelecting){
			if(sts.HitTestTank(sts.maxRayDistance, sts.sprayedTankLayer)){
				image.color = sr.color;
				sts.SetLastColor(sr.color);
			}
			else{
				image.color = sr.lastColor;
			}	
			tankManager.SetActive(false);
			box.SetActive(false);
			cancelBtn.SetActive(false);
			MoveToPosition(originPos);
		}
		else{
			if(tankManager.activeInHierarchy == false){
				MoveToPosition(selectedPos); 
				
				Debug.Log("hit");
				cancelBtn.SetActive(true);
				cancelBtn.GetComponent<RectTransform>().anchoredPosition3D = originPos;
				
				tankManager.SetActive(true);
				box.SetActive(true);
				tm.initTankManager();
				tm.SortTank(tm.sortType);
			}
		}
		isSelecting ^= true;
	}

	public void SelectCancel(){
		if(isSelecting){
			image.color = sr.lastColor;
		}
		tankManager.SetActive(false);
		box.SetActive(false);
		MoveToPosition(originPos);
		isSelecting ^= true;
	}

	
	void MoveToPosition(Vector3 targetPos){
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
