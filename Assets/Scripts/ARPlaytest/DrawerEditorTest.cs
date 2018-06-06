using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DrawerEditorTest : MonoBehaviour {

    public ARSpriteDrawer drawer;
    bool drawing = false;
    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            drawer.StartDrawing();
            drawing = true;
        }

        if (Input.GetMouseButtonUp(0) && drawing)
        {
            drawer.StopDrawing();
            drawing = false;
        }
    }
}
