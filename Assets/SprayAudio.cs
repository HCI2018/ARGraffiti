using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SprayAudio : MonoBehaviour {

	public ButtonInterface buttonInterface;
	private AudioSource audioSource;

	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnEnable(){
		buttonInterface.OnButtonDown.AddListener(SoundOn);
		buttonInterface.OnButtonUp.AddListener(SoundOff);
	}

	void OnDisable(){
		buttonInterface.OnButtonDown.RemoveListener(SoundOn);
		buttonInterface.OnButtonUp.RemoveListener(SoundOff);
	}

	void SoundOn(Button button)
	{
		audioSource.Play ();
	}

	void SoundOff(Button button)
	{
		audioSource.Stop ();
	}
}
