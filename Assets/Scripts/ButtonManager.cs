using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ButtonManager : MonoBehaviour {

	[HideInInspector]
	public GameObject artBoard;

	StokeManager sm;

	public CameraPlaytest cameraPlaytest;
	Camera cam;

	public GameObject setBoardButton;

	public GameObject redoButton;

	Button redoB;

	public GameObject undoButton;

	Button undoB;

	public GameObject cancelSetButton;

	public GameObject cameraButton;

	Button camB;

	public Image selectButtonImage;

	public LayerMask showPlane;

	public LayerMask hidePlane;

	//public float flow;

	// Use this for initialization
	void Start () {
		redoB = redoButton.GetComponent<Button>();
		undoB = undoButton.GetComponent<Button>();
		camB = cameraButton.GetComponent<Button>();
		redoB.onClick.AddListener(redoButtonListener);
		undoB.onClick.AddListener(undoButtonListener);
		camB.onClick.AddListener(cameraButtonListener);

		cam = cameraPlaytest.activeCamera;
	}
	
	// Update is called once per frame
	void Update () {
		if(cancelSetButton.activeInHierarchy == false && setBoardButton.activeInHierarchy == false){
			cam.cullingMask = hidePlane;
		}
		else{
			cam.cullingMask = showPlane;
		}
		if(sm){
			if(sm.canUndo){
				undoB.interactable = true;
			}
			else{
				undoB.interactable = false;
			}

			if(sm.canRedo){
				redoB.interactable = true;
			}
			else{
				redoB.interactable = false;
			}
		}
		
	}

	void redoButtonListener(){
		redo();
	}
	void undoButtonListener(){
		undo();
	}

	void cameraButtonListener(){
		CaptureScreenshot();
	}


	public void setBoard(){
		sm = artBoard.GetComponent<StokeManager>();
		setBoardButton.SetActive(false);
		redoButton.SetActive(true);
		undoButton.SetActive(true);
	}

	public void undo(){
		if(sm.canUndo){
			sm.Undo();
			// redoButton.SetActive(true);
			// if(!sm.canUndo){
			// 	undoButton.SetActive(false);
			// }
		}
		else{
			Debug.Log("Invalid undo");
		}
	}

	public void redo(){
		if(sm.canRedo){
			sm.Redo();
			// undoButton.SetActive(true);
			// if(!sm.canRedo){
			// 	redoButton.SetActive(false);
			// }
		}
		else{
			Debug.Log("Invalid redo");
		}
	}

	void CaptureScreenshot(){
		// ScreenCapture.CaptureScreenshot("scrennshot.png");
		//Debug.Log("fuck!");
	}

	public void setDrawFilling(float amount){
		selectButtonImage.fillAmount = amount;
	}


}
