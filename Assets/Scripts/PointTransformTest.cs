using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(OpenCvTest))]
public class PointTransformTest : MonoBehaviour {

	[HideInInspector]
	public GameObject debugPlane;

	//[HideInInspector]
	public RenderTexture inputRT;

	//public RenderTexture outputRT;

	Vector3 posTest;

	public Camera cam;

	BoxCollider boxCollider;
	// Use this for initialization

	[HideInInspector]
	public Vector2[] transformedVertices;

	OpenCvTest cv;

	void Start () {
		//rt = cam.targetTexture;
		transformedVertices = new Vector2[4];
		cv = GetComponent<OpenCvTest>();
	}

	public void initialize(){
		debugPlane = GameObject.Find("Plane");
		if(debugPlane == null){
			Debug.Log("Plane not found");
		}
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void test(){
		initialize();
		GetCollider();
		pointTransform(GetPlaneVertices());
		cv.PerspectiveTransformNew(inputRT, transformedVertices);
	}

	public void GetCollider(){
		boxCollider = debugPlane.GetComponentInChildren<BoxCollider>();
	}

	public Vector3[] GetPlaneVertices (){
		return GetPlaneVertexPositions(boxCollider);
	}

	private Vector3[] GetPlaneVertexPositions (BoxCollider boxCollider){
		var vertices = new Vector3[4];
		vertices[0] = boxCollider.transform.TransformPoint(boxCollider.center + new Vector3(boxCollider.size.x, 0,-boxCollider.size.z) * 0.5f);
		vertices[1] = boxCollider.transform.TransformPoint(boxCollider.center + new Vector3(boxCollider.size.x, 0, boxCollider.size.z) * 0.5f);
		vertices[2] = boxCollider.transform.TransformPoint(boxCollider.center + new Vector3(-boxCollider.size.x, 0, -boxCollider.size.z) * 0.5f);
		vertices[3] = boxCollider.transform.TransformPoint(boxCollider.center + new Vector3(-boxCollider.size.x, 0, boxCollider.size.z) * 0.5f);
		return vertices;
	}

	public void TestClipPosition(Camera camera){
		float nearHalfHeight = camera.nearClipPlane * Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
        float nearHalfWidth = nearHalfHeight * camera.aspect;
        float farHalfHeight = camera.farClipPlane * Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
	    float farHalfWidth = farHalfHeight * camera.aspect;		
	}

	public void pointTransform(Vector3[] originVertices){
		Vector3[] vertices = new Vector3[4];
		for (int i = 0; i < 4; i ++){
			//Debug.Log(originVertices[i]);
			vertices[i] = cam.WorldToScreenPoint(originVertices[i]);	
			transformedVertices[i] = new Vector2(vertices[i].x, vertices[i].y);
			//Debug.Log(transformedVertices[i]);
		}
	}
}
