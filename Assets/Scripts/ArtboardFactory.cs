﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;
using ARGraffiti.AR;

public class ArtboardFactory : MonoBehaviour {

	public RenderTexture artboardRT;
    public float maxRayDistance = 30.0f;
    public LayerMask collisionLayer = 1 << 10;  //ARKitPlane layer

	public GameObject artboardPrefab;
    Vector3 centerPoint;

    // Use this for initialization
    void Start () {
		centerPoint = new Vector3(0.5f, 0.5f, 0);
	}
	


	public void CreateArtboard()
	{
        Ray ray = Camera.main.ViewportPointToRay(centerPoint);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxRayDistance, collisionLayer))
		{
			// got a plane hit
			BoxCollider collider = hit.collider as BoxCollider;
			if(collider == null)
			{
				Debug.Log("InitArtboard(): unexpected collider type: " + collider.ToString());
				return;
			}

			Vector3 localCenter = collider.center;
			Vector3 localSize = collider.size;

			Vector3 worldCenter = collider.transform.TransformPoint(localCenter);
			Vector3 worldSize = collider.transform.TransformVector(localSize);

			Debug.Log(worldSize);

			Debug.DrawLine(worldCenter, worldCenter + worldSize / 2.0f);

			Vector3 scaledSize = Vector3.Scale(localSize, collider.transform.lossyScale);
			Vector2 xzSize = new Vector2(scaledSize.x, scaledSize.z);


			GameObject artboard = Instantiate(artboardPrefab);
			artboard.GetComponent<ArtboardManager>().InitArtboard(worldCenter, xzSize);
		}
		
	}

	// Update is called once per frame
	void Update () {

	}
}
