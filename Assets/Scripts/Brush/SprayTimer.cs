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
	private float totalTimePressed;

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
		if (totalTimePressed > maxTotalTime) {
			totalTimePressed = maxTotalTime;
		}
		Debug.Log ("total pressed time: " + totalTimePressed);
		normalizedTime = Mathf.InverseLerp(minTotalTime, maxTotalTime, totalTimePressed);
	}
}
