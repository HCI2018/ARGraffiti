using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorSelector : MonoBehaviour {

	Texture2D tex2d;
	public RawImage ri;

	int TexPixelLength = 256;
	Color[,] arrayColor;

	// Use this for initialization
	void Start () {
		arrayColor = new Color[TexPixelLength, TexPixelLength];
		tex2d = new Texture2D(TexPixelLength, TexPixelLength, TextureFormat.RGB24, true);
		ri.texture = tex2d;
	}


	
	// Update is called once per frame
	void Update () {
		
	}

	Color[] CalcArrayColor(Color endColor)
	{
        Color value = (endColor - Color.white) / (TexPixelLength - 1);
        for (int i = 0; i < TexPixelLength; i++)
        {
            arrayColor[i, TexPixelLength - 1] = Color.white + value * i;
        }
        for (int i = 0; i < TexPixelLength; i++)
        {
            value = (arrayColor[i, TexPixelLength - 1] - Color.black) / (TexPixelLength - 1);
            for (int j = 0; j < TexPixelLength; j++)
            {
                arrayColor[i, j] = Color.black + value * j;
            }
        }
        List<Color> listColor = new List<Color>();
        for (int i = 0; i < TexPixelLength; i++)
        {
            for (int j = 0; j < TexPixelLength; j++)
            {
                listColor.Add(arrayColor[j, i]);
            }
        }
        
        return listColor.ToArray();
    }

	public void SetColorPanel(Color endColor){
		Color[] CalcArray = CalcArrayColor(endColor);
		tex2d.SetPixels(CalcArray);
		tex2d.Apply();
	}
}
