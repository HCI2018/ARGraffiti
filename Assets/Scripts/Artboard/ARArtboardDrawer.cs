using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARArtboardDrawer : MonoBehaviour {

    public GameObject m_Brush;
    public float maxRayDistance = 30.0f;
    public LayerMask collisionLayer = 1 << 10;
    public float stepFactor = 0.25f;
    public float interval = 0.1f;

    int currentOrder = 0;

    private void Start()
    {
        drawCoroutine = null;

    }
    Coroutine drawCoroutine;

    public void StartDrawing()
    {
        // Debug.Log("Start Drawing!");
        if (drawCoroutine == null)
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
			Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 1f));
            RaycastHit hitInfo;

            Debug.DrawLine(ray.origin, ray.origin + ray.direction * maxRayDistance);

            if (Physics.Raycast(ray, out hitInfo, maxRayDistance, collisionLayer))
            {
                if (!lastPosValid)
                {
                    lastPosValid = true;
                    lastPos = hitInfo.point;
                }
//                Debug.Log(hitInfo.textureCoord);
                PaintSprite(hitInfo, lastPos);
				

                lastPos = hitInfo.point;

            } else {
                Debug.Log("Hit nothing!");
            }

            yield return new WaitForSeconds(interval);
        }

    }

    void PaintSprite(RaycastHit hitInfo, Vector3 lastPos)
    {
        float stepDistance = stepFactor * 0.1f;

        float curToLastDist = Vector3.Distance(lastPos, hitInfo.point);
        Vector3 curPos = hitInfo.point;
        Vector3 curToLastDir = (lastPos - curPos);
        curToLastDir.Normalize();

//        Debug.Log(curToLastDist);

        GameObject brushGO =
                    Instantiate(m_Brush, hitInfo.point, Quaternion.identity);
        brushGO.GetComponentInChildren<SpriteRenderer>().sortingOrder
            = currentOrder++;

        hitInfo.transform.GetComponentInParent<ArtboardManager>().
            PlaceSprite(brushGO.transform, hitInfo.point, new Vector2(1, 1));

        // Instantiate(debugCube, hitInfo.position, Quaternion.identity);

        for (float distToCur = stepDistance; distToCur < curToLastDist;
            distToCur += stepDistance)
        {
            Vector3 newPos = curPos + curToLastDir * distToCur;
            brushGO =
                    Instantiate(m_Brush, newPos, Quaternion.identity);
            brushGO.GetComponentInChildren<SpriteRenderer>().sortingOrder
                = currentOrder++;

            hitInfo.transform.GetComponentInParent<ArtboardManager>().
            PlaceSprite(brushGO.transform, newPos, new Vector2(1, 1));
        }
    }


}
