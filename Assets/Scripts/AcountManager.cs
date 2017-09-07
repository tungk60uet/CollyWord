using Facebook.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using LitJson;

public class AcountManager : MonoBehaviour
{

    public Button btnLogin, btnLogout;
    public GameObject LogoutPanel;
    public Text txtName;
    public Image imgAva;
    public Animator FadeImage;
    bool rankClicked = false;
    public static string apiLink = "http://x3-cdva-zer.net/api/";
    void Start()
    {
        if(Const.accessToken=="")
            StartCoroutine(GetToken());
        
        // txtDebug.text = FB.IsInitialized + " " + FB.IsLoggedIn;
    }

    IEnumerator setImgByUrl(Image img, string url)
    {
       
        // Start a download of the given URL
        WWW www = new WWW(url);
       
        // Wait for download to complete
        yield return www;

        // assign texture
        //Renderer renderer = GetComponent<Renderer>();
        img.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0.5f, 0.5f));
        //txtName.text += " ";
        //StartCoroutine(setImgByUrl(imgAva, "https://graph.facebook.com/" + PlayerPrefs.GetString("fb_id", "0") + "/picture?type=square"));
    }
    IEnumerator GetToken()
    {
        WWWForm form = new WWWForm();
        form.AddField("deviceId", SystemInfo.deviceUniqueIdentifier);
        Debug.Log("AccessToken "+ SystemInfo.deviceUniqueIdentifier);
        using (UnityWebRequest www = UnityWebRequest.Post(apiLink + "token", form))
        {
            yield return www.Send();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string json = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
                JsonData tk = JsonMapper.ToObject(json);
                Const.accessToken = tk["accessToken"].ToString();
                Debug.Log("AccessToken " + Const.accessToken);
                if(PlayerPrefs.GetString("fb_id", "0")!="0")
                    StartCoroutine(loginByFacebook());
            }
        }
    }

    IEnumerator loginByFacebook()
    {
        WWWForm form = new WWWForm();
        form.AddField("appname", "colornotword");
        form.AddField("accessToken", Const.accessToken);
        form.AddField("facebookId", PlayerPrefs.GetString("fb_id", "0"));
        form.AddField("userGold", 0);
        form.AddField("userLevel", 0);
        form.AddField("userPoint", PlayerPrefs.GetInt("highscore", 0));
        form.AddField("userAvatarLink", "https://graph.facebook.com/" + PlayerPrefs.GetString("fb_id", "0") + "/picture?type=square");
        form.AddField("userFullName", PlayerPrefs.GetString("fb_name", "0"));
        Debug.Log(Const.accessToken+" "+ PlayerPrefs.GetString("fb_id", "0")+" "+ PlayerPrefs.GetInt("highscore", 0)+" "+ PlayerPrefs.GetString("fb_name", "0"));
        using (UnityWebRequest www = UnityWebRequest.Post(apiLink + "login_facebook", form))
        {
            yield return www.Send();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {

                string json = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
                JsonData tk = JsonMapper.ToObject(json);
                if (tk["status"].ToString() == "200")
                {
                    int n = Int32.Parse(tk["userPoint"].ToString());
                    if (n > PlayerPrefs.GetInt("highscore")) PlayerPrefs.SetInt("highscore", n);
                    Debug.Log("FBLOG" + json);
                    //if(tk["status"].ToString()=="500") StartCoroutine(GetToken());
                }
            }
        }
    }
    IEnumerator logoutfb()
    {
        WWWForm form = new WWWForm();
        form.AddField("accessToken", Const.accessToken);
        using (UnityWebRequest www = UnityWebRequest.Post(apiLink + "logout", form))
        {
            yield return www.Send();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string json = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
                Debug.Log("logout"+json);
            }
        }
    }
    IEnumerator update()
    {
        WWWForm form = new WWWForm();
        form.AddField("accessToken", Const.accessToken);
        form.AddField("newLevel", 0);
        form.AddField("newPoint", PlayerPrefs.GetInt("highscore", 0));
        form.AddField("newGold", 0);
        using (UnityWebRequest www = UnityWebRequest.Post(apiLink + "update", form))
        {
            yield return www.Send();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string json = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
                Debug.Log("Update" + json);
            }
        }
    }
    // Awake function from Unity's MonoBehavior
    void Awake()
    {
        if (PlayerPrefs.GetString("fb_id", "0").Equals("0")) btnLogin.gameObject.SetActive(true);
        else
        {
           
           // txtName.text += PlayerPrefs.GetString("fb_name", "noname");
            btnLogin.gameObject.SetActive(false);
            LogoutPanel.SetActive(true);
            StartCoroutine(WriteText(PlayerPrefs.GetString("fb_name", "noname"), txtName));
            StartCoroutine(setImgByUrl(imgAva, "https://graph.facebook.com/" + PlayerPrefs.GetString("fb_id", "0") + "/picture?type=normal"));
        }
        
        if (!FB.IsInitialized)
        {
            FB.Init(InitCallback, OnHideUnity);
        }
        else
        {
            // Already initialized, signal an app activation App Event
            FB.ActivateApp();

        }

    }
    private IEnumerator WriteText(string textString, Text textComponent)
    {
        textComponent.text = " ";
        // write title one letter at a time
        foreach (char c in textString)
        {
            textComponent.text += c;
            yield return new WaitForSeconds(0.03f);
        }
    }
    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            FB.ActivateApp();

        }
        else
        {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
        if (FB.IsLoggedIn)
        {
            FB.Mobile.RefreshCurrentAccessToken();
        }
    }
    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            // Pause the game - we will need to hide
            Time.timeScale = 0;
        }
        else
        {
            // Resume the game - we're getting focus again
            Time.timeScale = 1;
        }
    }

    public void LogIn()
    {
        var perms = new List<string>() { "user_friends" };
        FB.LogInWithReadPermissions(perms, OnLogIn);
    }

    public void LogOut()
    {
        FB.LogOut();
       // StartCoroutine(logoutfb());
        PlayerPrefs.SetString("fb_id", "0");
        PlayerPrefs.SetString("fb_name", "noname");
        PlayerPrefs.SetString("list_friend", "[]");
        LogoutPanel.SetActive(false);
        btnLogin.gameObject.SetActive(true);

    }
    public void OnLogIn(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            txtName.text = "";
            AccessToken token = AccessToken.CurrentAccessToken;
            btnLogin.gameObject.SetActive(false);
            LogoutPanel.SetActive(true);
            PlayerPrefs.SetString("fb_id", token.UserId);
            FB.API("/me", HttpMethod.GET, GetUserInfoCallback);
            //FB.API("/me/friends", HttpMethod.GET, GetFriendList);
            StartCoroutine(setImgByUrl(imgAva, "https://graph.facebook.com/" + token.UserId + "/picture?type=normal"));

        }
        else
        {
            Debug.Log("Cancel");
        }
    }

    public void rankClick()
    {
        if (FB.IsLoggedIn)
        {
            rankClicked = true;
            FB.API("/me/friends", HttpMethod.GET, GetFriendList);
        }   
    }
    private void GetFriendList(IGraphResult result)
    {
        IDictionary<string, object> data = result.ResultDictionary;
        List<object> friends = (List<object>)data["data"];

        String res = "[\"" + PlayerPrefs.GetString("fb_id", "0") + "\"";
        foreach (object obj in friends)
        {
            Dictionary<string, object> dictio = (Dictionary<string, object>)obj;
            res += ",\"" + dictio["id"].ToString() + "\"";
            //  Debug.Log(dictio["id"].ToString());
        }
        res += "]";
        //PlayerPrefs.SetString("list_friend", res);
        Const.listFriend = res;
        FadeImage.Play("FadeOut");
        StartCoroutine(ChangeToRankScene());
    }
    IEnumerator ChangeToRankScene()
    {
        yield return new WaitForSeconds(FadeImage.GetCurrentAnimatorStateInfo(0).length + 0.1f);
        SceneManager.LoadScene("Rank");
    }
    private void GetUserInfoCallback(IGraphResult result)
    {
        if (result.Error != null)
        {
            print(result.Error);
        }
        PlayerPrefs.SetString("fb_name", result.ResultDictionary["name"].ToString());
        StartCoroutine(WriteText(PlayerPrefs.GetString("fb_name", "noname"), txtName));
        StartCoroutine(loginByFacebook());
        
    }
}
