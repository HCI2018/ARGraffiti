using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[RequireComponent(typeof(Camera))]
public class PrePostRender : MonoBehaviour {

	public UnityAction<Camera> onPreRender;
    public UnityAction<Camera> onPostRender;

	private Camera cam;

	void Start()
	{
		cam = GetComponent<Camera>();
	}

	// Use this for initialization
	void OnPreRender () {
		if(onPreRender != null)  onPreRender.Invoke(cam);
	}
	
	// Update is called once per frame
	void OnPostRender () {
		if(onPostRender != null) onPostRender.Invoke(cam);
	}
}
