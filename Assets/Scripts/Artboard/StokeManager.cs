using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ARGraffiti.Utilities;

public class StokeManager : MonoBehaviour {

	public int m_maxStoke = 10;
	public int m_maxRendererPerStoke = 200;


	#region StokeStacks
    CapStack<List<Renderer>> undoStack;
    CapStack<List<Renderer>> redoStack;
    RenderTexture backBuffer;
	RenderTexture frontBuffer;
	#endregion


	List<Renderer> activeStoke;
	public Camera backCamera;
	Camera frontCamera;

    const int backBrushLayer = 9;
	const int backBrushMask = 1 << backBrushLayer; // magic number, User Layer 9 is BackBrush

	const int frontBrushLayer = 8;
	const int frontBrushMask = 1 << frontBrushLayer; // magic number, User Layer 8 is FrontBrush



	public void InitStokeManager(ArtboardManager artboard)
	{
        undoStack = new CapStack<List<Renderer>>(m_maxStoke);
		redoStack = new CapStack<List<Renderer>>(m_maxStoke);

		frontBuffer = artboard.artboardRT;
		backBuffer = new RenderTexture(frontBuffer);
		backBuffer.name = frontBuffer.name + "_back";

		frontCamera = artboard.rigCamera;
		backCamera.CopyFrom(artboard.rigCamera);
		backCamera.targetTexture = backBuffer;
		backCamera.cullingMask = backBrushMask;

	}

	void ClearStackObjects(CapStack<List<Renderer>> stack)
	{
		while(stack.Count > 0)
		{
			List<Renderer> stroke = stack.Pop();
			foreach(Renderer renderer in stroke)
			{
				Destroy(renderer.gameObject);
			}
		}
	}

	public void StartStoke()
	{
		Debug.Assert(activeStoke == null);

		if(redoStack.Count > 0)
		{
            ClearStackObjects(redoStack);
		}

		activeStoke = new List<Renderer>(m_maxRendererPerStoke);
	}


	public void EndStoke()
	{
        Debug.Assert(activeStoke != null);
		List<Renderer> oldestStoke;

		if(undoStack.Push(activeStoke, out oldestStoke))
		{
			// exceeds undo stack
			StartCoroutine(RenderAndDisableIE(oldestStoke, backCamera, backBrushLayer, true));
		}

		Debug.Log(string.Format("Added Stoke! Now have {0} stokes in undo stack.", undoStack.Count));

		activeStoke = null;

	}

	IEnumerator RenderAndDisableIE(List<Renderer> stroke, Camera cam, int rendererLayer, bool willDestroy)
	{
        cam.clearFlags = CameraClearFlags.Nothing;
        cam.enabled = false;

        foreach (Renderer renderer in stroke)
        {
            renderer.gameObject.layer = rendererLayer;
            renderer.enabled = true;
        }

        cam.Render();

		// wait for the back camera finish rendering
		yield return new WaitForEndOfFrame();

		foreach(Renderer renderer in stroke)
		{
			renderer.enabled = false;
			if(willDestroy)  Destroy(renderer.gameObject);
		}
	}

	public void AddBrush(Renderer renderer)
	{
		Debug.Assert(activeStoke != null);
		if(activeStoke.Count >= m_maxRendererPerStoke)
		{
			EndStoke();
			StartStoke();
		}
		activeStoke.Add(renderer);
	}


    public void Undo()
    {
        List<Renderer> lastStroke = undoStack.Pop();
		foreach(Renderer renderer in lastStroke)
		{
			renderer.enabled = false;
		}

		List<Renderer>[] restStokes = undoStack.DumpElements();
		Graphics.CopyTexture(backBuffer, frontBuffer);

		StartCoroutine(RepaintWithStokes(restStokes, lastStroke));
    }

	public void Redo()
	{
		List<Renderer> lastRedoStroke = redoStack.Pop();
		StartCoroutine(RenderAndDisableIE(lastRedoStroke, frontCamera, frontBrushLayer, false));
		
		List<Renderer> oldestStroke;
		bool exceeded = undoStack.Push(lastRedoStroke, out oldestStroke);
		Debug.Assert(exceeded == false);
	}

    IEnumerator RepaintWithStokes(List<Renderer>[] stokes, List<Renderer> lastStoke)
    {
        foreach (List<Renderer> stoke in stokes)
        {
            foreach (Renderer renderer in stoke)
            {
                renderer.enabled = true;
            }
        }

        // frontCamera.clearFlags = CameraClearFlags.Color;
        // frontCamera.backgroundColor = new Color(0, 0, 0, 0);

        Graphics.Blit(backBuffer, frontBuffer);

        // yield return new WaitForEndOfFrame();
        // frontCamera.clearFlags = CameraClearFlags.Nothing;

        frontCamera.Render();

        yield return new WaitForEndOfFrame();

        foreach (List<Renderer> stoke in stokes)
        {
            foreach (Renderer renderer in stoke)
            {
                renderer.enabled = false;
            }
        }

        frontCamera.clearFlags = CameraClearFlags.Nothing;

        List<Renderer> oldestStoke;
        bool exceeded = redoStack.Push(lastStoke, out oldestStoke);
        Debug.Assert(exceeded == false);
    }


    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (canUndo) Undo();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (canRedo) Redo();
        }
#endif
    }
	public bool canRedo
	{
		get
		{
			return redoStack != null && redoStack.Count > 0;
		}
	}

	public bool canUndo
	{
		get
		{
			return undoStack != null && undoStack.Count > 0;
		}
	}


	void OnDestroy()
	{
		if(backBuffer != null)
		{
			backBuffer.Release();
			backBuffer = null;
		}
	}

}
