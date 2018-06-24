using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ARArtboardDrawer : MonoBehaviour {

    public GameObject m_Brush;
    public float maxRayDistance = 30.0f;
    public LayerMask collisionLayer = 1 << 10;
    public float stepFactor = 0.25f;
    public float interval = 0.1f;

    public BrushGenerator brushGen;

    [HideInInspector]
    public UnityEvent OnStartDrawing;
    [HideInInspector]
    public UnityEvent OnStopDrawing;

//	public float minTotalTime;
//	public float maxTotalTime;
//	//[HideInInspector]
//	public float normalizedTime;
//
//	private float lastTimeDown;
//	private float lastTimePressed;
//	private float totalTimePressed;

    int currentOrder = 0;

    Transform brushCursor;
    
    private void Start()
    {
        drawCoroutine = null;
        
        brushCursor = brushGen.GenerateCursor(new RaycastHit());
        brushCursor.gameObject.SetActive(false);

        activeArtboardManagers = new HashSet<ArtboardManager>();
    }

    void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 1f));
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, maxRayDistance, collisionLayer))
        {
            Transform cursor = brushGen.GenerateCursor(hitInfo);
            cursor.position = hitInfo.point;
            cursor.forward = -hitInfo.normal;
            brushCursor.gameObject.SetActive(true);
        }
        else 
        {
            brushCursor.gameObject.SetActive(false);
        }
    }

    Coroutine drawCoroutine;
    HashSet<ArtboardManager> activeArtboardManagers;

    public void StartDrawing()
    {
        // Debug.Log("Start Drawing!");
        if (drawCoroutine == null)
        {
            drawCoroutine = StartCoroutine(DrawSpriteIE(interval));

            if(OnStartDrawing != null)
            {
                OnStartDrawing.Invoke();
            }
			//pressed timer
//			Debug.Log (gameObject.name + "pressed");
//			lastTimeDown = Time.time;
        }
    }

    public void StopDrawing()
    {
        // Debug.Log("End Drawing!");
        if (drawCoroutine != null)
        {
            StopCoroutine(drawCoroutine);

			//pressed timer
//			Debug.Log(gameObject.name + " released!");
//
//			lastTimePressed = Time.time - lastTimeDown;
//			totalTimePressed = totalTimePressed + lastTimePressed;
//			if (totalTimePressed > maxTotalTime) {
//				totalTimePressed = maxTotalTime;
//			}
//			Debug.Log ("total pressed time: " + totalTimePressed);
//			normalizedTime = Mathf.InverseLerp(minTotalTime, maxTotalTime, totalTimePressed);

            foreach (ArtboardManager manager in activeArtboardManagers)
            {
                manager.EndStoke();
            }
            
            activeArtboardManagers.Clear();
            
            if(OnStopDrawing != null)
            {
                OnStopDrawing.Invoke();
            }

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


        Transform brush = brushGen.GenerateBrush(hitInfo);

        brush.GetComponentInChildren<Renderer>().sortingLayerName = "Default";
        brush.GetComponentInChildren<Renderer>().sortingOrder
            = currentOrder++;

        ArtboardManager manager = hitInfo.transform.GetComponentInParent<ArtboardManager>();
        if(!activeArtboardManagers.Contains(manager))
        {
            activeArtboardManagers.Add(manager);
            manager.StartStoke();
        }

        manager.PlaceSprite(brush, hitInfo.point);

        // Instantiate(debugCube, hitInfo.position, Quaternion.identity);

        for (float distToCur = stepDistance; distToCur < curToLastDist;
            distToCur += stepDistance)
        {
            Vector3 newPos = curPos + curToLastDir * distToCur;
            brush = brushGen.GenerateBrush(hitInfo);

            brush.GetComponentInChildren<Renderer>().sortingLayerName = "Default";
            brush.GetComponentInChildren<Renderer>().sortingOrder
                = currentOrder++;

            manager.PlaceSprite(brush, newPos);
        }
    }


}
