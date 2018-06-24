using System.Collections;
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
	//public AnimationCurve flowVolumeCurve;

	public Color tintColor;

	//[HideInInspector]
	public SprayTimer timer;

	public ButtonManager bm;

	[HideInInspector]
	public float flow = 1;

	float flowFactor = 0;

	float flowVolume = 1;

	// Use this for initialization
	void Start () {
		// tintColor = Color.white;
		timer.bg = gameObject.GetComponent<BrushGenerator>();
	}

	private void Update()
	{
		Debug.Log(flowVolume);
		bm.setDrawFilling(1 - timer.normalizedTime);
	}

	public void AddFlowVolume(float amount){
		
		// add flowVolume by amount
		// Debug.Log(flowVolume);
		// flowVolume += amount;
		// CheckFlowVolume();

		// //float volume = getFlowVolume(flow);

		// timer.normalizedTime = Volume2NormalizedTime(flowVolume);
		timer.normalizedTime -= amount;
		timer.NormalizedTimeCheck();

		timer.totalTimePressed = timer.Normalized2Total(timer.normalizedTime);
		timer.TotalTimeCheck();
	}

	// get volume from flow
    float getFlowVolume(float flow)
    {
        float flowVolume = Mathf.InverseLerp(minFlow, maxFlow, flow);
        flowVolume = flowVolume / flowFactor;
        return flowVolume;
    }


	// volume to normalizedTime
	float Volume2NormalizedTime(float flowVolume){
		return (1 - flowVolume);
	}

	// normalizedTime to volume
	float NormalizedTime2Volume(float normalizedTime){
		return (1 - normalizedTime);
	}


	public Transform GenerateBrush(RaycastHit hit)
	{
		float normalizedDist = 
			Mathf.InverseLerp(minDistance, maxDistance, hit.distance);
		float size = Mathf.Lerp(minSize, maxSize, normalizedDist);

		float negDist = 1.0f - normalizedDist;
		flowFactor = flowCurve.Evaluate(negDist);

		// // volume decreasing with the pressed time
		// float normalizedTotalTime = timer.normalizedTime;

		// //bm.setDrawFilling(normalizedTotalTime);

		// float negVolume = 1.0f - normalizedTotalTime;
		// // currentVolume = Mathf.Lerp(minVolume, maxVolume, negVolume);
		// float flowVolume = flowVolumeCurve.Evaluate(negVolume);

		Debug.Log("here");
		flowVolume = NormalizedTime2Volume(timer.normalizedTime);

		//Debug.Log(flowFactor);

		//Debug.Log(flowVolume);

		flow = Mathf.Lerp(minFlow, maxFlow, flowFactor * flowVolume);

	
		//Debug.Log(flowVolume);

//		Debug.Log("size: " + size.ToString() + ", flow: " + flow.ToString());

		GameObject brushGO = Instantiate(brushPrefab);
		Transform brush = brushGO.transform;
		Vector3 localScale = brush.localScale;
        brush.localScale = Vector3.Scale(localScale, new Vector3(size, size, size));
		Renderer renderer = brush.GetComponent<Renderer>();
		renderer.material.SetFloat("_Flow", flow);  
		renderer.material.SetColor("_TintColor", tintColor);

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

	public void SetColor(Color newColor)
	{
		tintColor = newColor;
	}

	// check and set for flow range
    public void CheckFlow()
    {
        if (flow > maxFlow)
        {
            flow = maxFlow;
        }
        else if (flow < minFlow)
        {
            flow = minFlow;
        }
        else { }
    }
	
	public void CheckFlowVolume()
    {
        if (flowVolume > 1)
        {
            flowVolume = 1;
        }
        else if (flowVolume < 0)
        {
            flowVolume = 0;
        }
        else { }
    }


}