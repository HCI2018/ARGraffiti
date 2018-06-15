using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrushSetter : MonoBehaviour {

	BrushGenerator brushGenerator;

	public GameObject selectButton;

	Image image;

	// Use this for initialization
	void Start () {
		brushGenerator = GetComponent<BrushGenerator>();
		image = selectButton.GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
		brushGenerator.SetColor(image.color);
	}
}
