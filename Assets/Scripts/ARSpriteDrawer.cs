using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;
using ARGraffiti.AR;

public class ARSpriteDrawer : MonoBehaviour {
    public GameObject m_Brush;
    public float maxRayDistance = 30.0f;
    public LayerMask collisionLayer = 1 << 10;  //ARKitPlane layer

    /// stepFactor * diameter = min brush step
    public float stepFactor = 0.25f;

    public float interval = 0.1f;

    public GameObject debugCube;

    int currentOrder = 0;

    private void Start()
    {
        drawCoroutine = null;
        
    }
    Coroutine drawCoroutine;

    public void StartDrawing()
    {
        // Debug.Log("Start Drawing!");
        if(drawCoroutine == null)
        {
            drawCoroutine = StartCoroutine(DrawSpriteIE(interval));
        }
    }

    public void StopDrawing()
    {
        // Debug.Log("End Drawing!");
        if (drawCoroutine != null)
        {
            StopCoroutine(drawCoroutine);
            drawCoroutine = null;
        }
    }


    IEnumerator DrawSpriteIE(float interval)
    {
        bool lastPosValid = false;
        Vector3 lastPos = transform.position;


        while (true)
        {
            ARPoint point = new ARPoint { x = 0.5, y = 0.5 };
            ARRaycastHit hitInfo;
            if (ARRaycast.HitTestWithResultType(point, 
               ARHitTestResultType.ARHitTestResultTypeEstimatedVerticalPlane, 
               maxRayDistance, collisionLayer,
               out hitInfo))
            {
                if(!lastPosValid)
                {
                    lastPosValid = true;
                    lastPos = hitInfo.position;
                }

                PaintSprite(hitInfo, lastPos);
                lastPos = hitInfo.position;
                
                
            }

            // if (ARRaycast.HitTestWithResultType(point, 
            //    ARHitTestResultType.ARHitTestResultTypeExistingPlane,
            //    maxRayDistance, collisionLayer,
            //    out hitInfo))
            // {

            //     if (!lastPosValid)
            //     {
            //         lastPosValid = true;
            //         lastPos = hitInfo.position;
            //     }

            //     PaintSprite(hitInfo, lastPos);
            //     lastPos = hitInfo.position;
                
            // }

            yield return new WaitForSeconds(interval);
        }

    }

    void PaintSprite(ARRaycastHit hitInfo, Vector3 lastPos)
    {
        float stepDistance = stepFactor * 0.1f;

        float curToLastDist = Vector3.Distance(lastPos, hitInfo.position);
        Vector3 curPos = hitInfo.position;
        Vector3 curToLastDir = (lastPos - curPos);
        curToLastDir.Normalize();

//        Debug.Log(curToLastDist);

        GameObject brushGO =
                    Instantiate(m_Brush, hitInfo.position, hitInfo.rotation);
        brushGO.GetComponentInChildren<SpriteRenderer>().sortingOrder
            = currentOrder++;

        // Instantiate(debugCube, hitInfo.position, Quaternion.identity);

        for(float distToCur = stepDistance; distToCur < curToLastDist; 
            distToCur += stepDistance)
        {   
            Vector3 newPos = curPos + curToLastDir * distToCur;
            brushGO =
                    Instantiate(m_Brush, newPos, hitInfo.rotation);
            brushGO.GetComponentInChildren<SpriteRenderer>().sortingOrder
                = currentOrder++;
        }
    }

    
    

}
