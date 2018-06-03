using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawerEditorTest : MonoBehaviour {

    public ARSpriteDrawer drawer;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            drawer.StartDrawing();
        }

        if (Input.GetMouseButtonUp(0))
        {
            drawer.StopDrawing();
        }
    }
}
