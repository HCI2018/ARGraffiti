using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtboardManager : MonoBehaviour {

	public float pixelsPerMeter = 128f;
	public float unitsPerMeter = 1f;


	public Transform rig;
	public Transform display;


	Camera rigCamera;
	Renderer displayRenderer;
	Vector2 sizeInMeter;
	Material displayMat;
	public RenderTexture artboardRT;

	Vector3 worldAnchor;
	Vector3 worldToHereOffset{
		get{
			Vector3 offset =  rig.position - worldAnchor;
			offset.z = 0;
			return offset;
		}
	}

    void Start()
    {
		worldAnchor = Vector3.zero;
        sizeInMeter = new Vector2(1, 1);
    }

	/// Initialize the artboard with the given center and size.
	/// realSize is the size of the artboard in meters
	public RenderTexture InitArtboard(Vector3 worldCenter, Quaternion worldRotation, Vector2 sizeInMeter)
	{
		Vector2 pixelSizeF = sizeInMeter * pixelsPerMeter;
		Vector2Int pixelSize = new Vector2Int(
			Mathf.RoundToInt(pixelSizeF.x), Mathf.RoundToInt(pixelSizeF.y)
		);

		artboardRT = new RenderTexture(pixelSize.x, pixelSize.y, 24, RenderTextureFormat.ARGB32);
		artboardRT.name = "ArtboardRT";
		artboardRT.Create();

        rigCamera = rig.GetComponentInChildren<Camera>();
		

        rigCamera.orthographicSize = sizeInMeter.y / 2.0f * unitsPerMeter;
        this.sizeInMeter = sizeInMeter;
        rigCamera.targetTexture = artboardRT;
		worldAnchor = worldCenter;

		// set up display
		display.SetPositionAndRotation(worldCenter, worldRotation);
		display.localScale = Vector3.Scale(display.localScale, new Vector3(sizeInMeter.x, 1f, sizeInMeter.y));

		displayRenderer = display.GetComponentInChildren<Renderer>();

        

		displayMat = new Material(displayRenderer.material);
		displayMat.mainTexture = artboardRT;

		displayRenderer.sharedMaterial = displayMat;

        // displayRenderer.sharedMaterial.EnableKeyword("_METALLICGLOSSMAP");
        // displayRenderer.sharedMaterial.EnableKeyword("_ALPHABLEND_ON");
        // displayRenderer.sharedMaterial.mainTexture = artboardRT;
		// displayRenderer.sharedMaterial.SetTexture("_MetallicGlossMap", Texture2D.blackTexture);
		

        foreach (string keyword in displayRenderer.material.shaderKeywords)
        {
            Debug.Log(keyword);
        }

		return artboardRT;
	}

	public void PlaceSprite(Transform sprite, Vector3 worldPos, Vector2 realSize)
	{
		// Vector2 offsetUv = uvPos - new Vector2(0.5f, 0.5f);
		// Vector2 unitOffset = Vector2.Scale(offsetUv, artboardExtent);
		
		sprite.parent = null;
		// sprite.position = transform.position + new Vector3(unitOffset.x, unitOffset.y ,0f);
		// sprite.position = worldPos + worldToHereOffset;
		Vector2 offset = ArtboardInverseTransformPoint(worldPos);
		sprite.position = rig.position + new Vector3(offset.x, offset.y, 0f);

		// TODO: implement scale
		// sprite.localScale = realSize;
		sprite.parent = rig;
	}


	Vector2 ArtboardInverseTransformPoint(Vector3 worldPos)
	{
		Vector3 posInDisplay3d = display.InverseTransformPoint(worldPos);
		Debug.Log(posInDisplay3d);


		Vector2 posInDisplay = new Vector2(posInDisplay3d.x, posInDisplay3d.z);

		Vector2 offsetInMeter = Vector2.Scale(posInDisplay, sizeInMeter);
		return offsetInMeter;
	}


	void OnDestroy()
	{

	}
}