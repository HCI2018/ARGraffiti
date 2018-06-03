using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlaytest : MonoBehaviour {

	public Transform arCamera;
	public Transform editorCamera;

	public Transform mountToCamera;

    public bool enableEditorCamera = false;

	// bool updated = false;

	// Use this for initialization
	void Awake () {
#if !UNITY_EDITOR
        InitArCamera();
#else
        // when we are in editor...
        if (enableEditorCamera)
            InitEditorCamera();
        else
            InitArCamera();
#endif

	}

    void InitArCamera()
    {
        // when we are on the iOS device...
        arCamera.gameObject.SetActive(true);
        editorCamera.gameObject.SetActive(false);
        if (mountToCamera != null)
        {
            Transform[] childs = new Transform[mountToCamera.childCount];
            for (int i = 0; i < mountToCamera.childCount; ++i)
            {
                childs[i] = mountToCamera.GetChild(i);
            }
            foreach (Transform child in childs)
            {
                child.SetParent(arCamera);
            }
        }
    }

    void InitEditorCamera()
    {
        editorCamera.gameObject.SetActive(true);
        arCamera.gameObject.SetActive(false);
        if (mountToCamera != null)
        {
            Transform[] childs = new Transform[mountToCamera.childCount];
            for (int i = 0; i < mountToCamera.childCount; ++i)
            {
                childs[i] = mountToCamera.GetChild(i);
            }
            foreach (Transform child in childs)
            {
                child.SetParent(editorCamera);
            }
        }
    }

    //	// Update is called once per frame
    //	void Update () {
    //		
    //	}
}
