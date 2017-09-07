
using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using Facebook.Unity;
using System;

public class ShareImageCanvas : MonoBehaviour
{
    // public
    // private
    private bool isProcessing = false;
    public Image buttonShare;
   
    public string mensaje;

    //function called from a button
    public void ButtonShare()
    {

        if (!isProcessing)
        {
            //CanvasShareObj.SetActive(true);
            StartCoroutine(ShareScreenshot());
        }
    }
    public IEnumerator ShareScreenshot()
    {
        isProcessing = true;
        // wait for graphics to render
        yield return new WaitForEndOfFrame();
      

        yield return new WaitForSecondsRealtime(0.3f);
        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- PHOTO
        // create the texture
        Texture2D screenTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, true);

        // put buffer into texture
        screenTexture.ReadPixels(new Rect(0f, 0f, Screen.width, Screen.height), 0, 0);
        // apply
        screenTexture.Apply();

        //check.sprite = Sprite.Create(screenTexture, new Rect(0, 0, screenTexture.width, screenTexture.height), new Vector2(0.5f, 0.5f));
        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- PHOTO
        byte[] dataToSave = screenTexture.EncodeToJPG(80);

        string destination = Path.Combine(Application.persistentDataPath, System.DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + ".jpg");
        File.WriteAllBytes(destination, dataToSave);
        if (!Application.isEditor)
        {
            AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
            AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
            intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
            AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
            AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", "file://" + destination);
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"),
                uriObject);
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"),
                "Can you beat my score?");
            intentObject.Call<AndroidJavaObject>("setType", "image/jpeg");
            AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject chooser = intentClass.CallStatic<AndroidJavaObject>("createChooser",
                intentObject, "Share your new score");
            currentActivity.Call("startActivity", chooser);

            yield return new WaitForSecondsRealtime(1);
        }
        isProcessing = false;
        buttonShare.enabled = true;
    }
}