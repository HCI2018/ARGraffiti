using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;
using ARGraffiti.AR;

public class ARSpriteDrawer : MonoBehaviour {
    public GameObject m_Brush;
    public float maxRayDistance = 30.0f;
    public LayerMask collisionLayer = 1 << 10;  //ARKitPlane layer

    public float interval = 0.1f;

    private void Start()
    {
        drawCoroutine = null;
        
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
            if (ARRaycast.HitTestWithResultType(point, 
               ARHitTestResultType.ARHitTestResultTypeEstimatedVerticalPlane, 
               maxRayDistance, collisionLayer,
               out hitInfo))
            {
                Instantiate(m_Brush, hitInfo.position, hitInfo.rotation);
            }

            if (ARRaycast.HitTestWithResultType(point, 
               ARHitTestResultType.ARHitTestResultTypeExistingPlane,
               maxRayDistance, collisionLayer,
               out hitInfo))
            {
                Instantiate(m_Brush, hitInfo.position, hitInfo.rotation);
            }

            yield return new WaitForFixedUpdate();
        }

    }
    

}
