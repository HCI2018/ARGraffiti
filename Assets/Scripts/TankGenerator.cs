using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEditor;
using System;

public class TankGenerator : MonoBehaviour {

	[HideInInspector]
	public BoxCollider boxCol;

	public float tankThickness = 1;

	Vector3 center;

	public GameObject tankBound;

	Vector3[] vertices;

 	public GameObject ball;

	Rigidbody ballRb;

	
 
	// Use this for initialization
	void Start () {
	}

    Vector3[] GetBoxColliderVertexPositions(BoxCollider boxcollider)
    {
        var vertices = new Vector3[6];
        vertices[0] = boxcollider.transform.TransformPoint(boxcollider.center + new Vector3(boxcollider.size.x, 0, 0) * 0.5f);
        vertices[1] = boxcollider.transform.TransformPoint(boxcollider.center + new Vector3(-boxcollider.size.x, 0, 0) * 0.5f);
        vertices[2] = boxcollider.transform.TransformPoint(boxcollider.center + new Vector3(0, boxcollider.size.y, 0) * 0.5f);
        vertices[3] = boxcollider.transform.TransformPoint(boxcollider.center + new Vector3(0, -boxcollider.size.y, 0) * 0.5f);
        vertices[4] = boxcollider.transform.TransformPoint(boxcollider.center + new Vector3(0, 0, boxcollider.size.z) * 0.5f);
        vertices[5] = boxcollider.transform.TransformPoint(boxcollider.center + new Vector3(0, 0, -boxcollider.size.z) * 0.5f);
        return vertices;
    }

	public void InitVertices(){	
		boxCol = GetComponent<BoxCollider>();
		vertices = GetBoxColliderVertexPositions(boxCol);
	}

	public void GenerateTank(){
		for(int i = 0; i < 6; i++){
			GameObject child = Instantiate(tankBound,vertices[i],tankBound.transform.rotation, transform);
			Debug.Log("hit");
			SetChildSize(child, boxCol);
		}
	}

	void SetChildSize(GameObject go, BoxCollider boxCol){
		//Debug.Log("fuck");
		if(go.transform.localPosition.x == 0){
			go.transform.transform.localScale += new Vector3(boxCol.size.x, 0, 0);
		}
		else{
			go.transform.transform.localScale += new Vector3(tankThickness, 0, 0);
		}


		if(go.transform.localPosition.y == 0){
			go.transform.transform.localScale += new Vector3(0, boxCol.size.y, 0);
		}
		else{
			go.transform.transform.localScale += new Vector3(0, tankThickness, 0);
		}

		if(go.transform.localPosition.z == 0){
			go.transform.transform.localScale += new Vector3(0, 0, boxCol.size.z);
		}
		else{
			go.transform.transform.localScale += new Vector3(0, 0, tankThickness);
		}

	}



}
