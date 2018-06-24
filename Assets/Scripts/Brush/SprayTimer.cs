using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class SprayTimer : MonoBehaviour {

	public ARArtboardDrawer drawer;

	public float minTotalTime;
	public float maxTotalTime;
	//[HideInInspector]
	public float normalizedTime;

	private float lastTimeDown;
	private float lastTimePressed;
	[HideInInspector]
	public float totalTimePressed;

	[HideInInspector]
	public BrushGenerator bg;

	// Use this for initialization
	void Start () {
		
	}

	void OnEnable(){
		drawer.OnStartDrawing.AddListener(PressTimer);
		drawer.OnStopDrawing.AddListener(TotalPressedTimer);
	}

	void OnDisable(){
		drawer.OnStartDrawing.RemoveListener(PressTimer);
		drawer.OnStopDrawing.RemoveListener(TotalPressedTimer);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void PressTimer()
	{
		Debug.Log (gameObject.name + "pressed");

		lastTimeDown = Time.time;
	}

	void TotalPressedTimer()
	{
		Debug.Log(gameObject.name + " released!");

		lastTimePressed = Time.time - lastTimeDown;
		totalTimePressed = totalTimePressed + lastTimePressed;
		TotalTimeCheck();

		Debug.Log ("total pressed time: " + totalTimePressed);

		normalizedTime = Total2Normalized(totalTimePressed);
		NormalizedTimeCheck();
	}

	public float Total2Normalized(float totalPressedTime){
		float normalizedTime = Mathf.InverseLerp(minTotalTime, maxTotalTime, totalPressedTime);
		return normalizedTime;
	}

	public float Normalized2Total(float normalizedTime){
		float totalPressedTime = Mathf.Lerp(minTotalTime, maxTotalTime, normalizedTime);
		return totalPressedTime;
	}


	public void TotalTimeCheck(){
		if (totalTimePressed > maxTotalTime) {
			totalTimePressed = maxTotalTime;
		}
		else if(totalTimePressed < minTotalTime){
			totalTimePressed = minTotalTime;
		}
		else{}
	}

	public void NormalizedTimeCheck(){
		if(normalizedTime < minTotalTime){
			normalizedTime = minTotalTime;
		}
		else if(normalizedTime > maxTotalTime){
			normalizedTime = maxTotalTime;
		}
		else{}
	}
}
