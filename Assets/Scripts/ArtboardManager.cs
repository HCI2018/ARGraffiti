using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtboardManager : MonoBehaviour {

	public float pixelPerMeter = 128f;
	public Vector3 artboardOffset;

	Camera artboardCam;
	public RenderTexture artboardRT;

	/// Initialize the artboard with the given center and size.
	/// realSize is the size of the artboard in meters
	public RenderTexture InitArtboard(Vector3 worldCenter, Vector2 realSize)
	{
		Vector2 pixelSizeF = realSize * pixelPerMeter;
		Vector2Int pixelSize = new Vector2Int(
			Mathf.RoundToInt(pixelSizeF.x), Mathf.RoundToInt(pixelSizeF.y)
		);

		artboardRT = new RenderTexture(pixelSize.x, pixelSize.y, 24, RenderTextureFormat.ARGB32);
		artboardRT.name = "ArtboardRT";

		artboardCam = GetComponentInChildren<Camera>();
		artboardCam.targetTexture = artboardRT;


		return artboardRT;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
