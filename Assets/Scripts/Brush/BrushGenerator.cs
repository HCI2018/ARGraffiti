﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrushGenerator : MonoBehaviour {
	public GameObject brushPrefab;

	public GameObject cursorPrefab;
	Transform cursor{
		get{
			if(_cursor == null){
				GameObject cursorGO = Instantiate(cursorPrefab);
				_cursor = cursorGO.transform;
                initCursorScale = _cursor.localScale;
			}
            return _cursor;
		}
	}
	Transform _cursor;
	Vector3 initCursorScale;

	public float minDistance;
	public float maxDistance;

	public float minSize;
	public float maxSize;

	[Range(0, 1)]
	public float minFlow;
	[Range(0, 1)]
	public float maxFlow; 


	public AnimationCurve flowCurve;

	// Use this for initialization
	void Start () {
		
	}



	public Transform GenerateBrush(RaycastHit hit)
	{
		float normalizedDist = 
			Mathf.InverseLerp(minDistance, maxDistance, hit.distance);
		float size = Mathf.Lerp(minSize, maxSize, normalizedDist);

		float negDist = 1.0f - normalizedDist;
		float flowFactor = flowCurve.Evaluate(negDist);

		float flow = Mathf.Lerp(minFlow, maxFlow, flowFactor);

//		Debug.Log("size: " + size.ToString() + ", flow: " + flow.ToString());

		GameObject brushGO = Instantiate(brushPrefab);
		Transform brush = brushGO.transform;
		Vector3 localScale = brush.localScale;
        brush.localScale = Vector3.Scale(localScale, new Vector3(size, size, size));
		Renderer renderer = brush.GetComponent<Renderer>();
		renderer.material.SetFloat("_Flow", flow);

		return brush;
	}

	public Transform GenerateCursor(RaycastHit hit)
	{
        float normalizedDist =
            Mathf.InverseLerp(minDistance, maxDistance, hit.distance);
        float size = Mathf.Lerp(minSize, maxSize, normalizedDist);

		if(cursor)
		{
			cursor.localScale = Vector3.Scale(initCursorScale, new Vector3(size, size, size));
		}

		return cursor;
	}
	
}