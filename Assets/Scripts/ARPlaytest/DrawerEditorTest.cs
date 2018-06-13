using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DrawerEditorTest : MonoBehaviour {

    public ARSpriteDrawer spriteDrawer;
    public ARArtboardDrawer artboardDrawer;

    bool drawing = false;
    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            spriteDrawer.StartDrawing();
            artboardDrawer.StartDrawing();
            drawing = true;
        }

        if (Input.GetMouseButtonUp(0) && drawing)
        {
            spriteDrawer.StopDrawing();
            artboardDrawer.StopDrawing();
            drawing = false;
        }
    }
}