using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ColorButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler {

	public GameObject selector;

	public GameObject tankManager;

	public float factor;

	Image image;

	Button button;

	SprayedTankSelector sts;

	SelectorResult sr;

	Vector3 selectorPos;

	// Use this for initialization
	void Start () {
		image = GetComponent<Image>();
		sts = selector.GetComponent<SprayedTankSelector>();
		sr = new SelectorResult();
		selectorPos = new Vector3();
	}
	
	// Update is called once per frame
	void Update () {
		selectorPos = selector.transform.position;
        if (isDown) {  
			if(tankManager.activeInHierarchy == false){
				tankManager.transform.position = selectorPos + selector.transform.forward * factor;
				Debug.Log("set");
				tankManager.SetActive(true);
			}
            if (Time.time - lastIsDownTime > delay) {  
                Debug.Log("长按");  
                lastIsDownTime = Time.time;  
				RaycastAndSet();
            }  
        }  
	}

	void RaycastAndSet(){
		if(selector != null){
			sr = sts.HitTestInGameSpace(sts.maxRayDistance, sts.sprayedTankLayer);
			if(sr.found == true){
				image.color = sr.color;
			}
			else{
				image.color = sr.lastColor;
			}
		}
	}

    private float delay = 0.2f;  

    private bool isDown = false;  
  
    private float lastIsDownTime;  

	public void OnPointerDown (PointerEventData eventData)  
    {  
        isDown = true;  
        lastIsDownTime = Time.time;  
    }  
  
    public void OnPointerUp (PointerEventData eventData)  
    {  
        isDown = false;  
		
		if(sts.HitTestTank(sts.maxRayDistance, sts.sprayedTankLayer)){
			image.color = sr.color;
			sts.SetLastColor(sr.color);
		}
		else{
			image.color = sr.lastColor;
		}

		tankManager.SetActive(false);
    }  
}
