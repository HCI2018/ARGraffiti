using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARTextureCapture : MonoBehaviour {
	public Camera cam;

	public Texture rt;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		rt = cam.GetComponent<Camera>().targetTexture;
		
	}
}
