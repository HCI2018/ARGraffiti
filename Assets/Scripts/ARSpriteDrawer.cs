﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;

struct ARRaycastHit
{
    public Vector3 position;
    public Quaternion rotation;
    public float distance;

    public ARRaycastHit(Vector3 position, Quaternion rotation, float distance)
    {
        this.position = position;
        this.rotation = rotation;
        this.distance = distance;
    }
}

public class ARSpriteDrawer : MonoBehaviour {
    public GameObject m_Brush;
    public float maxRayDistance = 30.0f;
    public LayerMask collisionLayer = 1 << 10;  //ARKitPlane layer

    public float interval = 0.1f;

    private void Start()
    {
        drawCoroutine = null;   
    }

    bool HitTestWithResultType(ARPoint point, ARHitTestResultType resultTypes, out ARRaycastHit hitInfo)
    {

        hitInfo = new ARRaycastHit();

        Ray ray = Camera.main.ViewportPointToRay(new Vector3((float)point.x, (float)point.y, 0));
        RaycastHit hit;

        //we'll try to hit one of the plane collider gameobjects that were generated by the plugin
        //effectively similar to calling HitTest with ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent

        if (Physics.Raycast(ray, out hit, maxRayDistance, collisionLayer))
        {
            //we're going to get the position from the contact point
            Vector3 position = hit.point;
            // Debug.Log(string.Format("x:{0:0.######} y:{1:0.######} z:{2:0.######}", position.x, position.y, position.z));

            //and the rotation from the transform of the plane collider
            Quaternion rotation = hit.transform.rotation;

            hitInfo = new ARRaycastHit(position, rotation, hit.distance);

            return true;
        }



        List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface().HitTest(point, resultTypes);

        Debug.Log(hitResults.Count);
        if (hitResults.Count > 0)
        { 
            foreach (var hitResult in hitResults)
            {
               //  Debug.Log("Got hit!");
                Vector3 position = UnityARMatrixOps.GetPosition(hitResult.worldTransform);
                Quaternion rotation = UnityARMatrixOps.GetRotation(hitResult.worldTransform);
             //   Debug.Log(string.Format("x:{0:0.######} y:{1:0.######} z:{2:0.######}", position.x, position.y, position.z));
                hitInfo = new ARRaycastHit(position, rotation, (float)hitResult.distance);
            }
        }
        return false;
    }

    Coroutine drawCoroutine;

    public void StartDrawing()
    {
        Debug.Log("Start Drawing!");
        if(drawCoroutine == null)
        {
            drawCoroutine = StartCoroutine(DrawSpriteIE(interval));
        }
    }

    public void StopDrawing()
    {
        Debug.Log("End Drawing!");
        if (drawCoroutine != null)
        {
            StopCoroutine(drawCoroutine);
            drawCoroutine = null;
        }
    }


    IEnumerator DrawSpriteIE(float interval)
    {
        while (true)
        {
            ARPoint point = new ARPoint { x = 0.5, y = 0.5 };
            ARRaycastHit hitInfo;
            if (HitTestWithResultType(point, ARHitTestResultType.ARHitTestResultTypeEstimatedVerticalPlane, out hitInfo))
            {
                Instantiate(m_Brush, hitInfo.position, hitInfo.rotation);
            }

            if (HitTestWithResultType(point, ARHitTestResultType.ARHitTestResultTypeExistingPlane, out hitInfo))
            {
                Instantiate(m_Brush, hitInfo.position, hitInfo.rotation);
            }

            yield return new WaitForSeconds(interval);
        }

    }
    

}
