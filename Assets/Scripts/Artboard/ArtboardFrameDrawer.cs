using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtboardFrameDrawer : MonoBehaviour {

	public GameObject framePrefab;


    public float frameWidth = 0.05f;
	public float frameBleedLength = 0.05f;

	private GameObject topFrame;
    private GameObject bottomFrame;
    private GameObject leftFrame;
    private GameObject rightFrame;

	public void InitArtboardFrame(Transform display, Vector2 sizeInMeter)
	{
        Vector3 xOffset = display.right * (sizeInMeter.x / 2f + frameWidth / 2f);
		Vector3 yOffset = display.forward * (sizeInMeter.y / 2f + frameWidth / 2f);
		

		Vector3 topPos = display.position + yOffset;
		Vector3 bottomPos = display.position - yOffset;
		Vector3 rightPos = display.position + xOffset;
		Vector3 leftPos = display.position - xOffset;

		topFrame = GameObject.Instantiate(framePrefab, topPos, display.rotation);
        bottomFrame = GameObject.Instantiate(framePrefab, bottomPos, display.rotation);
        rightFrame = GameObject.Instantiate(framePrefab, rightPos, display.rotation);
        leftFrame = GameObject.Instantiate(framePrefab, leftPos, display.rotation);

		Transform leftFrameTrans = leftFrame.transform;
		leftFrameTrans.Rotate(0f, -90f, 0f);
		Transform rightFrameTrans = rightFrame.transform;
		rightFrameTrans.Rotate(0f, 90f, 0f);

		float horizontalLength = sizeInMeter.x + 2 * frameBleedLength;
        float verticalLength = sizeInMeter.y;

		topFrame.transform.localScale = Vector3.Scale(topFrame.transform.localScale,
                new Vector3(horizontalLength, 1f, frameWidth));
        bottomFrame.transform.localScale = Vector3.Scale(bottomFrame.transform.localScale,
                new Vector3(horizontalLength, 1f, frameWidth));

        leftFrame.transform.localScale = Vector3.Scale(leftFrame.transform.localScale,
                new Vector3(verticalLength, 1f, frameWidth));

        rightFrame.transform.localScale = Vector3.Scale(rightFrame.transform.localScale,
                new Vector3(verticalLength, 1f, frameWidth));

		
		Material horiMat = topFrame.GetComponent<Renderer>().material;
		float aspectRatio = horiMat.mainTexture.width / horiMat.mainTexture.height;
		Material vertMat = new Material(horiMat);
		
		float horiLengthDivHeight = horizontalLength / frameWidth;
		float vertiLengthDivHeight = verticalLength / frameWidth;

		horiMat.mainTextureScale = new Vector2(horiLengthDivHeight / aspectRatio, 1f);
		vertMat.mainTextureScale = new Vector2(vertiLengthDivHeight / aspectRatio, 1f);

		topFrame.GetComponent<Renderer>().material = horiMat;
        bottomFrame.GetComponent<Renderer>().material = horiMat;

		leftFrame.GetComponent<Renderer>().material = vertMat;
		rightFrame.GetComponent<Renderer>().material = vertMat;

		topFrame.transform.parent = display;
		bottomFrame.transform.parent = display;
		leftFrame.transform.parent = display;
		rightFrame.transform.parent = display;

    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
