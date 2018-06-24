using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SprayAudio : MonoBehaviour {

	//public ButtonInterface buttonInterface;
	private AudioSource audioSource;

	public ARArtboardDrawer drawer;

	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnEnable(){
		drawer.OnStartDrawing.AddListener(SoundOn);
		drawer.OnStopDrawing.AddListener(SoundOff);
	}

	void OnDisable(){
		drawer.OnStartDrawing.RemoveListener(SoundOn);
		drawer.OnStopDrawing.RemoveListener(SoundOff);
	}

	void SoundOn()
	{
		audioSource.Play ();
	}

	void SoundOff()
	{
		audioSource.Stop ();
	}
}
