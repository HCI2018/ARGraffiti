using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class SprayTimer : MonoBehaviour {

	public ButtonInterface buttonInterface;

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
		buttonInterface.OnButtonDown.AddListener(PressTimer);
		buttonInterface.OnButtonUp.AddListener(TotalPressedTimer);
	}

	void OnDisable(){
		buttonInterface.OnButtonDown.RemoveListener(PressTimer);
		buttonInterface.OnButtonUp.RemoveListener(TotalPressedTimer);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void PressTimer(Button button)
	{
		Debug.Log (gameObject.name + "pressed");

		lastTimeDown = Time.time;
	}

	void TotalPressedTimer(Button button)
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
