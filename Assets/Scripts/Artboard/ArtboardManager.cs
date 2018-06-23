using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtboardManager : MonoBehaviour {

	public float pixelsPerMeter = 128f;
	public float unitsPerMeter = 1f;


	public Transform rig;
	public Transform display;


	public Camera rigCamera;
	Renderer displayRenderer;
	Vector2 sizeInMeter;
	Material displayMat;
	public RenderTexture artboardRT;

	public bool noClearMode = true;

	Vector3 worldAnchor;
	Vector3 worldToHereOffset{
		get{
			Vector3 offset =  rig.position - worldAnchor;
			offset.z = 0;
			return offset;
		}
	}

	StokeManager stokeManager;
	ArtboardFrameDrawer frameDrawer;

    void Start()
    {
		// commented because the following 2 lines of code
		// create race condition with InitArtboard()

		// worldAnchor = Vector3.zero;
        // sizeInMeter = new Vector2(1, 1);

		dirtySprites = new List<Transform>();
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

		if(noClearMode)
		{
            rigCamera.GetComponent<PrePostRender>().onPostRender += CleanSpriteCallback;
		}
		

        rigCamera.orthographicSize = sizeInMeter.y / 2.0f * unitsPerMeter;
        this.sizeInMeter = sizeInMeter;
        rigCamera.targetTexture = artboardRT;
		worldAnchor = worldCenter;

		Debug.Log(sizeInMeter);

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
		
		// init StokeManager
		stokeManager = GetComponent<StokeManager>();
		stokeManager.InitStokeManager(this);


        frameDrawer = GetComponent<ArtboardFrameDrawer>();
		frameDrawer.InitArtboardFrame(display, sizeInMeter);

		return artboardRT;
	}

	public void StartStoke()
	{
		stokeManager.StartStoke();
	}

	public void EndStoke()
	{
		stokeManager.EndStoke();
	}

	public void PlaceSprite(Transform sprite, Vector3 worldPos)
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

		if(noClearMode)
			dirtySprites.Add(sprite);


		// tell stoke manager about the brush
		Renderer renderer = sprite.GetComponentInChildren<Renderer>();
		stokeManager.AddBrush(renderer);
	}

//	bool isDirty = false;
	List<Transform> dirtySprites;
	void LateUpdate()
	{	
		if(!noClearMode)  return;

		if(dirtySprites != null && dirtySprites.Count > 0)
		{
            rigCamera.clearFlags = CameraClearFlags.Nothing;
            rigCamera.enabled = false;
            rigCamera.Render();
		}
		
	}

	void CleanSpriteCallback(Camera cam)
	{
		if(cam != rigCamera) return;
		foreach(Transform sprite in dirtySprites)
		{
			sprite.GetComponentInChildren<Renderer>().enabled = false;
		}
		dirtySprites.Clear();
	}


	Vector2 ArtboardInverseTransformPoint(Vector3 worldPos)
	{
		Vector3 posInDisplay3d = display.InverseTransformPoint(worldPos);
 		


		Vector2 posInDisplay = new Vector2(posInDisplay3d.x, posInDisplay3d.z);
        

		Vector2 offsetInMeter = Vector2.Scale(posInDisplay, sizeInMeter);

        // Debug.Log(offsetInMeter);
		return offsetInMeter;
	}

	void OnDisable()
	{
		if(rigCamera)
		{
            rigCamera.GetComponent<PrePostRender>().onPostRender -= CleanSpriteCallback;
		}
	}

	void OnDestroy()
	{
		if(artboardRT != null)
		{
			artboardRT.Release();
			artboardRT = null;
		}
	}
	
}