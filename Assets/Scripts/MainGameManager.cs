using EaseTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using UnityEngine.Advertisements;

[RequireComponent(typeof(AudioSource))]
public class MainGameManager : MonoBehaviour {
    string adID = "ca-app-pub-1589360682424634/3180373670";
    string testDeviceID;
    InterstitialAd interstitial;

    const int START = 0,PLAYING = 1, GAMEOVER = 2,WRONG=3;

    public Button btnTL, btnTR, btnBL, btnBR,btnMute;
    public Image background;
    public Text txtColor, txtScore,txtOverScore,txtHighScore;
    public EaseUI txtColorTrans,muteAnm,scoreAnm;
    public Animator anmOver;
    public Slider sliderTime,sliderSkip;
    public ParticleSystem stars;
 //   public GameObject gameOverMenu;
    int idColor, correctAws;
    int status = START;
    int score = 0, highScore = 0, dieCount = 0;
    bool adsClicked = false;
    AudioSource[] audio;
    // Use this for initialization
    void Start()
    {
        testDeviceID = SystemInfo.deviceUniqueIdentifier;
        RequestInterstitialAds();
        if (PlayerPrefs.GetInt("mute", 0) == 0) AudioListener.volume = 1.0f ; else AudioListener.volume=0;
        if (AudioListener.volume == 0)
        {
            btnMute.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/mute");
        }
        highScore =PlayerPrefs.GetInt("highscore", highScore);
        status = START;
        audio = GetComponents<AudioSource>();
        changeColor();
        txtColor.text = Const.colorName[Random.Range(0, Const.colorHex.Count)];
        txtColor.color = hexToColor(Const.colorHex[idColor]);
    }
    private void showBannerAd()
    {
    
        //***For Testing in the Device***
        AdRequest request = new AdRequest.Builder()
       .AddTestDevice(AdRequest.TestDeviceSimulator)       // Simulator.
       .AddTestDevice(testDeviceID)  // My test device.
       .Build();

        //***For Production When Submit App***
        //AdRequest request = new AdRequest.Builder().Build();

        BannerView bannerAd = new BannerView(adID, AdSize.SmartBanner, AdPosition.Top);
        bannerAd.LoadAd(request);
    }
    private void RequestInterstitialAds()
    {

#if UNITY_ANDROID
        string adUnitId = adID;
#elif UNITY_IOS
        string adUnitId = adID;
#else
        string adUnitId = adID;
#endif

        // Initialize an InterstitialAd.
        interstitial = new InterstitialAd(adUnitId);

        //***Test***
        AdRequest request = new AdRequest.Builder()
       .AddTestDevice(AdRequest.TestDeviceSimulator)       // Simulator.
       .AddTestDevice(testDeviceID)  // My test device.
       .Build();
        //***Production***
        //AdRequest request = new AdRequest.Builder().Build();

        //Register Ad Close Event
        interstitial.OnAdClosed += Interstitial_OnAdClosed;

        // Load the interstitial with the request.
        interstitial.LoadAd(request);

        Debug.Log("AD LOADED XXX");

    }
    public void ShowRewardedAd()
    {
        const string RewardedPlacementId = "rewardedVideo";

#if UNITY_ADS
        if (!Advertisement.IsReady(RewardedPlacementId))
        {
            Debug.Log(string.Format("Ads not ready for placement '{0}'", RewardedPlacementId));
            return;
        }

        var options = new ShowOptions { resultCallback = HandleShowResult };
        Advertisement.Show(RewardedPlacementId, options);
#endif
    }

#if UNITY_ADS
    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");
                //
                // YOUR CODE TO REWARD THE GAMER
                // Give coins etc.
                sliderTime.value = sliderTime.maxValue;
                //txtScore.text = score.ToString();
                status = START;
                //changeColor();
                //txtColor.text = Const.colorName[Random.Range(0, Const.colorHex.Count)];
                //txtColor.color = hexToColor(Const.colorHex[idColor]);
                anmOver.Play("OverNext");
                break;
            case ShowResult.Skipped:
                //Debug.Log("The ad was skipped before reaching the end.");
                GameOver();
                break;
            case ShowResult.Failed:
                //Debug.LogError("The ad failed to be shown.");
                GameOver();
                break;
        }
    }

#endif
    public void skipAdsClick()
    {
        //anmOver.Play("watchAdsOut");
        //anmOver.Play("OverIn");
        GameOver();
    }
    public void adsClick()
    {
        anmOver.Play("watchAdsOut");
        ShowRewardedAd();
    }
    public void showInterstitialAd()
    {
        //Show Ad
        if (interstitial.IsLoaded())
        {
            interstitial.Show();

            //Stop Sound
            //

            Debug.Log("SHOW AD XXX");
        }

    }
    
    //Ad Close Event
    private void Interstitial_OnAdClosed(object sender, System.EventArgs e)
    {
        RequestInterstitialAds();
        //Resume Play Sound
    }
    void changeColor()
    {
        if (score >= 1000)
        {
            sliderTime.maxValue = 0.5f;
        }
        else if (score >= 100)
        {
            sliderTime.maxValue = 1.0f;
        }
        else
        {
            sliderTime.maxValue = (1500 + ((1000 - score) * 1.0f / 10000))*1.0f/1000;
        }

        sliderTime.value = sliderTime.maxValue;
        txtScore.text = score.ToString();
        
        idColor = Random.Range(0, Const.colorHex.Count);
        correctAws = Random.Range(0, 4);
      
        background.color= hexToColor(Const.colorBgr[idColor]);


        string s = idColor + " ";
        int idx;
        idx = Random.Range(0, Const.colorHex.Count);
        while (s.IndexOf(idx.ToString()) != -1) idx = (idx + 1) % Const.colorHex.Count;
        s += idx + " ";
        btnTL.GetComponentInChildren<Text>().text = Const.colorName[idx];

        idx = Random.Range(0, Const.colorHex.Count);
        while (s.IndexOf(idx.ToString()) != -1) idx = (idx + 1) % Const.colorHex.Count;
        s += idx + " ";
        btnTR.GetComponentInChildren<Text>().text = Const.colorName[idx];

        idx = Random.Range(0, Const.colorHex.Count);
        while (s.IndexOf(idx.ToString()) != -1) idx = (idx + 1) % Const.colorHex.Count;
        s += idx + " ";
        btnBL.GetComponentInChildren<Text>().text = Const.colorName[idx];

        idx = Random.Range(0, Const.colorHex.Count);
        while (s.IndexOf(idx.ToString()) != -1) idx = (idx + 1) % Const.colorHex.Count;
        s += idx + " ";
        btnBR.GetComponentInChildren<Text>().text = Const.colorName[idx];

        btnTL.GetComponentInChildren<Text>().color = hexToColor(Const.colorHex[Random.Range(0, Const.colorHex.Count)]);
        btnTR.GetComponentInChildren<Text>().color = hexToColor(Const.colorHex[Random.Range(0, Const.colorHex.Count)]);
        btnBL.GetComponentInChildren<Text>().color = hexToColor(Const.colorHex[Random.Range(0, Const.colorHex.Count)]);
        btnBR.GetComponentInChildren<Text>().color = hexToColor(Const.colorHex[Random.Range(0, Const.colorHex.Count)]);

        switch (correctAws)
        {
            case 0:
                btnTL.GetComponentInChildren<Text>().text = Const.colorName[idColor];
                break;
            case 1:
                btnTR.GetComponentInChildren<Text>().text = Const.colorName[idColor];
                break;
            case 2:
                btnBL.GetComponentInChildren<Text>().text = Const.colorName[idColor];
                break;
            case 3:
                btnBR.GetComponentInChildren<Text>().text = Const.colorName[idColor];
                break;
            default:
                break;
        }
    }
    // Update is called once per frame
    void Update()
    {
        switch (status)
        {
            case PLAYING:
                {
                    if((!txtColorTrans.IsMoving())&&txtColor.rectTransform.anchoredPosition.x !=0)
                    {
                        txtColorTrans.StartPosition.x = -txtColorTrans.StartPosition.x;
                        txtColorTrans.MoveIn();
                        txtColor.text = Const.colorName[Random.Range(0, Const.colorHex.Count)];
                        //txtColor.text = Application.systemLanguage.ToString();
                        txtColor.color = hexToColor(Const.colorHex[idColor]);
                    }
                    sliderTime.value -= Time.deltaTime;
                    if (sliderTime.value <= 0) StartCoroutine(wrongAws());
                }
                break;
            case WRONG:
                {
                    sliderSkip.value -= Time.deltaTime;
                    if (sliderSkip.value <= 0) GameOver();
                }
                break;
            default:
                break;
        }

    }
    IEnumerator wrongAws()
    {
        if (status != WRONG) {
            if(dieCount==2)
                Invoke("showInterstitialAd", 0);
            status = WRONG;
            sliderSkip.value = sliderSkip.maxValue;
            anmOver.Play("Shake");
            audio[1].Play();
            yield return new WaitForSeconds(anmOver.GetCurrentAnimatorStateInfo(0).length);
            if (score >= 3 && !adsClicked)
            {
                anmOver.Play("watchAdsIn");
                adsClicked = true;
            }
                
            else
                GameOver();
        }
    }
   
    void GameOver()
    {
        if (status != GAMEOVER)
        {
            status = GAMEOVER;
            dieCount = (dieCount + 1) % 3;
            //audio[1].Play();
            if (score > highScore)
            {
                highScore = score;
                PlayerPrefs.SetInt("highscore", highScore);
                StartCoroutine(update());
            }
            txtOverScore.text = score.ToString();
            txtHighScore.text = highScore.ToString();
            anmOver.Play("OverIn");
        }

    }
    IEnumerator update()
    {
        WWWForm form = new WWWForm();
        form.AddField("accessToken", Const.accessToken);
        form.AddField("newLevel", 0);
        form.AddField("newPoint", PlayerPrefs.GetInt("highscore", 0));
        form.AddField("newGold", 0);
        using (UnityWebRequest www = UnityWebRequest.Post(AcountManager.apiLink + "update", form))
        {
            yield return www.Send();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                //string json = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
            }
        }
    }
    public void Retry()
    {
        anmOver.Play("OverOut");
        StartCoroutine(WaitRetry());

    }
    IEnumerator WaitRetry()
    {
        yield return new WaitForSeconds(anmOver.GetCurrentAnimatorStateInfo(0).length);
        adsClicked = false;
        sliderTime.value = sliderTime.maxValue;
        score = 0;
        txtScore.text = score.ToString();
        status = START;
        changeColor();
        txtColor.text = Const.colorName[Random.Range(0, Const.colorHex.Count)];
        txtColor.color = hexToColor(Const.colorHex[idColor]);
        anmOver.Play("OverNext");
    }
    
    public void Quit()
    {
        Debug.Log("Thoát game");
        SceneManager.LoadScene("StartMenu");
    }

    public void btnMuteClick()
    {
        if (AudioListener.volume==0)
        {
            AudioListener.volume = 1.0f;
            btnMute.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/speaker");
            PlayerPrefs.SetInt("mute", 0);
        }
        else
        {
            AudioListener.volume = 0;
            btnMute.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/mute");
            PlayerPrefs.SetInt("mute", 1);
        }
        muteAnm.ScaleIn();

    }
    public void btnClick(int id)
    {
        if (id == correctAws && (status == PLAYING|| status == START))
        {
            // anmTxtColor.Play("txtColorOut");
            status = PLAYING;
            score++;
            if (score == highScore + 1)
            {
                audio[2].Play();
                stars.Play();
            }
           
            else audio[0].Play();
            scoreAnm.ScaleIn();
            changeColor();
            sliderTime.value = sliderTime.maxValue;
            txtColorTrans.StartPosition.x = -txtColorTrans.StartPosition.x;
            txtColorTrans.MoveOut();
        }
        else
        {
            StartCoroutine(wrongAws());
        }
    }
    public static Color hexToColor(string hex)
    {
        hex = hex.Replace("0x", "");//in case the string is formatted 0xFFFFFF
        hex = hex.Replace("#", "");//in case the string is formatted #FFFFFF
        byte a = 255;//assume fully visible unless specified in hex
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        //Only use alpha if the string has enough characters
        if (hex.Length == 8)
        {
            a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
        }
        return new Color32(r, g, b, a);
    }

}
