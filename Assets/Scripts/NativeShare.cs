using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
public class NativeShare : MonoBehaviour
{
    public string subject, ShareMessage, url;
    private bool isProcessing = false;
    public string ScreenshotName = "screenshot.png";
    public void ShareScreenshotWithText()
    {
        // Share();
    }
    public void Share()
    {
#if UNITY_IOS
        if (!isProcessing)
            StartCoroutine(CallSocialShareRoutine());
#else
Debug.Log("No sharing set up for this platform.");
#endif
    }
#if UNITY_IOS
    public struct ConfigStruct
    {
        public string title;
        public string message;
    }
    [DllImport("__Internal")] private static extern void showAlertMessage(ref ConfigStruct conf);
    public struct SocialSharingStruct
    {
        public string text;
        public string url;
        public string image;
        public string subject;
    }
    [DllImport("__Internal")] private static extern void showSocialSharing(ref SocialSharingStruct conf);
    public void CallSocialShare(string title, string message)
    {
        ConfigStruct conf = new ConfigStruct();
        conf.title = title;
        conf.message = message;
        showAlertMessage(ref conf);
        isProcessing = false;
    }
    public static void CallSocialShareAdvanced(string defaultTxt, string subject, string url, string img)
    {
        SocialSharingStruct conf = new SocialSharingStruct();
        conf.text = defaultTxt;
        conf.url = url;
        conf.image = img;
        conf.subject = subject;
        showSocialSharing(ref conf);
    }
    IEnumerator CallSocialShareRoutine()
    {
        isProcessing = true;
        string screenShotPath = Path.Combine(Application.persistentDataPath, ScreenshotName);
        
        if(File.Exists(screenShotPath)) File.Delete(screenShotPath);

        //Application.CaptureScreenshot(ScreenshotName);
        ScreenCapture.CaptureScreenshot(ScreenshotName);

        Debug.Log(screenShotPath);

        while(!File.Exists(screenShotPath)){
            yield return new WaitForSeconds(.05f);
        }

        Debug.Log(screenShotPath);
        CallSocialShareAdvanced(ShareMessage, subject, url, screenShotPath);

        isProcessing = false;
    }
#endif
}

