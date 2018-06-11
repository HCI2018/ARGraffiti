using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEditor;
using OpenCVForUnity;
using System.IO;


public class OpenCvTest : MonoBehaviour {
	 private List<Vector2> corners = new List<Vector2>();  
    
    [HideInInspector]
	//public string pathName;
    //public RenderTexture rt;

    //public GameObject testObject;

    private Texture2D inputTexture;	

    private Texture2D outputTexture;
	// Use this for initialization  

    void Start(){
    }

    public void PerspectiveTransformNew(RenderTexture inputRT, Vector2[] points){
        inputTexture = new Texture2D(inputRT.width, inputRT.height);
        RenderTexture.active = inputRT;
        inputTexture.ReadPixels(new UnityEngine.Rect(0, 0, inputRT.width, inputRT.height), 0, 0);
        inputTexture.Apply();

        Mat inputMat = new Mat(inputTexture.height, inputTexture.width, CvType.CV_8UC4);  
        Mat outputMat = new Mat(inputTexture.height, inputTexture.width, CvType.CV_8UC4);  
        Utils.texture2DToMat(inputTexture, inputMat);  
  
        Imgproc.cvtColor(inputMat, outputMat, Imgproc.COLOR_RGB2GRAY);  
          
        Mat lines = new Mat();  
        
        //calculate for matrix
        Vector2 tl = points[0];  
        Vector2 tr = points[1];  
        Vector2 bl = points[2];  
        Vector2 br = points[3];  

        Debug.Log(inputRT.width + " " + inputRT.height + tl + tr + bl + br);
  
        Mat srcRectMat = new Mat(4, 1, CvType.CV_32FC2);  
        Mat dstRectMat = new Mat(4, 1, CvType.CV_32FC2);  
  
        srcRectMat.put(0, 0, tl.x, tl.y, tr.x, tr.y, bl.x, bl.y, br.x, br.y);  
        dstRectMat.put(0, 0, 
                        0.0, inputMat.rows(), 
                        inputMat.cols(), inputMat.rows(), 
                        0.0, 0.0, 
                        inputMat.cols(), 0);  
  
        Mat perspectiveTransform = Imgproc.getPerspectiveTransform(srcRectMat, dstRectMat);  

        Mat outputMat0 = inputMat.clone();  
  
        //perspective transform 
        Imgproc.warpPerspective(inputMat, outputMat0, perspectiveTransform, new Size(inputMat.cols(), inputMat.rows()));  
  
        Texture2D outputTexture = new Texture2D(outputMat0.cols(), outputMat0.rows(), TextureFormat.RGBA32, false);  

        Utils.matToTexture2D(outputMat0, outputTexture);  

        // outputRT = new RenderTexture(inputRT.width, inputRT.height, 0);
        // RenderTexture.active = outputRT;
        // Graphics.Blit(outputTexture, outputRT);

        Debug.Log("Finished");
        //testObject.GetComponent<Renderer>().sharedMaterial.mainTexture = outputTexture;
        SaveTextureToFile(outputTexture, "testSave");
        Debug.Log("Save Finished");
    }

    // public void PerspectiveTransform () { 
    //     Debug.Log("hit 1"); 
    //     //Texture2D inputTexture = Resources.Load(pathName) as Texture2D;  
    //     //Texture2D inputTexture = rt;
    //     inputTexture = new Texture2D(rt.width, rt.height);
    //     Debug.Log(inputTexture.height + " " + inputTexture.width);
    //     RenderTexture.active = rt;
    //     inputTexture.ReadPixels(new UnityEngine.Rect(0, 0, rt.width, rt.height), 0, 0);
    //     inputTexture.Apply();

    //     Mat inputMat = new Mat(inputTexture.height, inputTexture.width, CvType.CV_8UC4);  
    //     Mat outputMat = new Mat(inputTexture.height, inputTexture.width, CvType.CV_8UC4);  
    //     Utils.texture2DToMat(inputTexture, inputMat);  
  
    //     Imgproc.cvtColor(inputMat, outputMat, Imgproc.COLOR_RGB2GRAY);  
         
    //     Imgproc.Canny(outputMat, outputMat, 100, 150);  
          
    //     Mat lines = new Mat();  
         
    //     Imgproc.HoughLinesP(outputMat, lines, 1, Mathf.PI / 180, 60, 50, 10);  
    //     //calculate intersect points
    //     int[] linesArray = new int[lines.cols() * lines.rows() * lines.channels()];  
    //     lines.get(0, 0, linesArray);  
    //     Debug.Log("length of lineArray " + linesArray.Length);  
    //     List<int> a = new List<int>();  
    //     List<int> b = new List<int>();  
    //     // for (int i = 0; i < linesArray.Length -4; i = i + 4)  
    //     // {  
    //     //     //Imgproc.line(inputMat, new Point(linesArray[i + 0], linesArray[i + 1]), new Point(linesArray[i + 2], linesArray[i + 3]), new Scalar(255, 255, 255), 2);  
    //     // }  
    //     for (int i = 0; i < linesArray.Length; i = i+4)  
    //     {  
    //         a.Add(linesArray[i + 0]);  
    //         a.Add(linesArray[i + 1]);  
    //         a.Add(linesArray[i + 2]);  
    //         a.Add(linesArray[i + 3]);  
    //         for (int j = i+4; j < linesArray.Length; j= j + 4)  
    //         {  
    //             b.Add(linesArray[j + 0]);  
    //             b.Add(linesArray[j + 1]);  
    //             b.Add(linesArray[j + 2]);  
    //             b.Add(linesArray[j + 3]);  
  
    //             Vector2 temp = ComputeIntersect(a, b);  
    //             b.Clear();  
  
    //             if (temp.x > 0 && temp.y > 0 && temp.x < 1000 && temp.y < 1000)  
    //             {  
    //                 corners.Add(temp);  
    //             }  
    //         }  
    //         a.Clear();  
    //     }  
    //     //culling
    //     CullIllegalPoint(ref corners,20);  
    //     if (corners.Count != 4)  
    //     {  
    //         Debug.Log("The object is not quadrilateral  " + corners.Count);  
    //     }  
    //     Vector2 center = Vector2.zero;  
    //     for (int i = 0; i < corners.Count; i++)  
    //     {  
    //         center += corners[i];  
    //     }  
    //     center *= 0.25f;  
    //     SortCorners(ref corners, center);  
  
    //     //calculate for matrix
    //     Vector2 tl = corners[0];  
    //     Vector2 tr = corners[1];  
    //     Vector2 br = corners[2];  
    //     Vector2 bl = corners[3];  
  
    //     Mat srcRectMat = new Mat(4, 1, CvType.CV_32FC2);  
    //     Mat dstRectMat = new Mat(4, 1, CvType.CV_32FC2);  
  
    //     srcRectMat.put(0, 0, tl.x, tl.y, tr.x, tr.y, bl.x, bl.y, br.x, br.y);  
    //     dstRectMat.put(0, 0, 0.0, inputMat.rows(), inputMat.cols(), inputMat.rows(), 0.0, 0.0, inputMat.rows(), 0);  
  
    //     Mat perspectiveTransform = Imgproc.getPerspectiveTransform(srcRectMat, dstRectMat);  
    //     Mat outputMat0 = inputMat.clone();  
  
    //     //perspective transform 
    //     Imgproc.warpPerspective(inputMat, outputMat0, perspectiveTransform, new Size(inputMat.rows(), inputMat.cols()));  
  
    //     Texture2D outputTexture = new Texture2D(outputMat0.cols(), outputMat0.rows(), TextureFormat.RGBA32, false);  
    //     Utils.matToTexture2D(outputMat0, outputTexture);  
  
    //     //testObject.GetComponent<Renderer>().sharedmaterial.mainTexture = outputTexture;

    //     SaveTextureToFile(outputTexture, "testSave");

    //     testObject.GetComponent<Renderer>().sharedMaterial.mainTexture = outputTexture;

    //     //Debug.Log("hit 2"); 

    //     //gameObject.GetComponent<Renderer>().material.mainTexture = outputTexture;  
    // }  
  
    private Vector2 ComputeIntersect(List<int> a, List<int> b)  
    {  
        int x1 = a[0], y1 = a[1], x2 = a[2], y2 = a[3];  
        int x3 = b[0], y3 = b[1], x4 = b[2], y4 = b[3];  
        float d = ((float)(x1 - x2) * (y3 - y4)) - (x3 - x4) * (y1 - y2);  
        Vector2 temp = Vector2.zero;  
        if (d == 0)  
        {  
            temp.x = -1;  
            temp.y = -1;  
        }  
        else  
        {  
            temp.x = ((x1 * y2 - y1 * x2) * (x3 - x4) - (x1 - x2) * (x3 * y4 - y3 * x4)) / d;  
            temp.y = ((x1 * y2 - y1 * x2) * (y3 - y4) - (y1 - y2) * (x3 * y4 - y3 * x4)) / d;  
        }  
        return temp;  
    }  
  
    private void CullIllegalPoint(ref List<Vector2> corners,float minDis)  
    {  
        Vector2 a = Vector2.zero;  
        Vector2 b = Vector2.zero;  
        List<Vector2> removeList = new List<Vector2>();  
        for (int i = 0; i < corners.Count; i++)  
        {  
            a = corners[i];  
            for (int j = i + 1; j < corners.Count; j++)  
            {  
                b = corners[j];  
                if (Vector2.Distance(a, b) < minDis)  
                {  
                    removeList.Add(b);  
                }  
            }  
        }  
        for (int i = 0; i < removeList.Count; i++)  
        {  
            corners.Remove(removeList[i]);  
        }  
    }  
  
    private void SortCorners( ref List<Vector2> corners, Vector2 center)  
    {  
        List<Vector2> top = new List<Vector2>();  
        List<Vector2> bot = new List<Vector2>();  
        for (int i = 0; i < corners.Count; i++)  
        {  
             
            if (corners[i].y > center.y)  
                top.Add(corners[i]);  
            else  
                bot.Add(corners[i]);  
        }  
        if (top.Count < 2)  
        {  
            Vector2 temp = GetMaxFromList(bot);  
            top.Add(temp);  
            bot.Remove(temp);  
        }  
        if (top.Count > 2)  
        {  
            Vector2 temp = GetMinFromList(top);  
            top.Remove(temp);  
            bot.Add(temp);  
        }  
        Vector2 tl = top[0].x > top[1].x ? top[1]: top[0];  
        Vector2 tr = top[0].x > top[1].x ? top[0] : top[1];  
        Vector2 bl = bot[0].x > bot[1].x ? bot[1] : bot[0];  
        Vector2 br = bot[0].x > bot[1].x ? bot[0] : bot[1];  
        corners.Clear();  
        corners.Add(tl);  
        corners.Add(tr);  
        corners.Add(br);  
        corners.Add(bl);  
    }  
  
    private Vector2 GetMaxFromList(List<Vector2> list)  
    {  
        Vector2 temp = list[0];  
        for (int i = 0; i < list.Count; i++)  
        {  
            if (list[i].y > temp.y)  
            {  
                temp = list[i];  
            }  
        }  
        return temp;  
    }  
  
    private Vector2 GetMinFromList(List<Vector2> list)  
    {  
        Vector2 temp = list[0];  
        for (int i = 0; i < list.Count; i++)  
        {  
            if (list[i].y < temp.y)  
            {  
                temp = list[i];  
            }  
        }  
        return temp;  
    }  

    public void SaveTextureToFile(Texture2D texture, string fileName){
        byte[] bytes = texture.EncodeToPNG();
        if (bytes != null && bytes.Length > 0) {  
                File.WriteAllBytes (Application.dataPath + "/" + fileName + ".png", bytes);  
                Debug.Log("finished");
                //Debug.Log ("转换PNG成功：" + AssetDatabase.GetAssetPath (mt.mainTexture) + "-->" + savedTexture.format.ToString ());  
        }  
    } 
}
