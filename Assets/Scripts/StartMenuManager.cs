using EaseTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using Facebook.Unity;
public class StartMenuManager : MonoBehaviour
{
    // Use this for initialization
    public Animator FadeImage;
    void Start()
    {
        Const.initGameData();
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    public void helpClick()
    {
        FadeImage.Play("FadeOut");
        StartCoroutine(ChangeScene("Help"));
    }
    public void startBtnClick()
    {
        FadeImage.Play("FadeOut");
        StartCoroutine(ChangeScene("play"));
    }
    public void rankBtnClick()
    {
        if (PlayerPrefs.GetString("list_friend","[]") != "[]") {
           
           
        } 
    }
    bool AnimatorIsPlaying()
    {
        return FadeImage.GetCurrentAnimatorStateInfo(0).length >
               FadeImage.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }
    bool AnimatorIsPlaying(string stateName)
    {
        return AnimatorIsPlaying() && FadeImage.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }
    IEnumerator ChangeScene(string s)
    {
        yield return new WaitForSeconds(FadeImage.GetCurrentAnimatorStateInfo(0).length + 0.1f);
        if(s.Equals("play"))
        SceneManager.LoadScene("MainGame");
        else
            if(s.Equals("Help"))
            SceneManager.LoadScene("Help");
    }
}
