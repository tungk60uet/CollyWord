using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.UI;
using UnityEngine.Networking;
using LitJson;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;
using System.IO;

public class RankManager : MonoBehaviour {

    // Use this for initialization
    public Sprite spr;
    public GameObject Friend;
    public GameObject LoggedInUI;
    public Animator RankMoveIn;
    public static string apiLink = "http://x3-cdva-zer.net/api/";
    List<Sprite> listAva = new List<Sprite>();
    Hashtable hash = new Hashtable();
    JsonData tk;
    bool done = false;
    
    void Start () {
        StartCoroutine(getUserRank());
    }
    public void backBtnClick()
    {
        SceneManager.LoadScene("StartMenu");
    }
    
    void CreateFriend(string rank, Sprite ava,string name,string point)
    {
        GameObject myFriend = Instantiate(Friend);
        myFriend.transform.localScale =new Vector3(1.0f, 1.0f, 1.0f);
        Transform parent = LoggedInUI.transform.Find("ListContainer").Find("List");
        myFriend.transform.SetParent(parent,false);
        Text[] li = myFriend.GetComponentsInChildren<Text>();
        li[1].text = rank;
        li[0].text = name;
        li[2].text = point;
        if (rank == "1") myFriend.GetComponentsInChildren<Image>()[0].color = MainGameManager.hexToColor("f1c50e");
        else
            if (rank == "2") myFriend.GetComponentsInChildren<Image>()[0].color = MainGameManager.hexToColor("12a787");
        else
            if (rank == "3") myFriend.GetComponentsInChildren<Image>()[0].color = MainGameManager.hexToColor("6a471d");
        
        myFriend.GetComponentsInChildren<Image>()[2].sprite = ava;
        
       //setImgByUrl(myFriend.GetComponentsInChildren<Image>()[1], ava);
       //FB.API("\api")

    }
    void Update()
    {
        if (tk != null)
            if (tk["status"].ToString() == "200")
                if (hash.Count == tk["listOfUserRank"].Count&&!done)
                {
                    for (int i = 0; i < tk["listOfUserRank"].Count; i++)
                    {
                        //  getImgByUrl(tk["listOfUserRank"][i]["userAvatarLink"].ToString());
                        CreateFriend(tk["listOfUserRank"][i]["userRankStand"].ToString(), hash[(i+1).ToString()] as Sprite, tk["listOfUserRank"][i]["userFullName"].ToString(), tk["listOfUserRank"][i]["userPoint"].ToString());
                    }
                    done = true;
                    RankMoveIn.Play("FadeInBoardRank");
                }
    } 

    IEnumerator getUserRank()
    {
        WWWForm form = new WWWForm();
        Debug.Log(Const.accessToken);
        form.AddField("accessToken", Const.accessToken);
        Debug.Log(Const.listFriend);
        form.AddField("fbFriendList",Const.listFriend);
        using (UnityWebRequest www = UnityWebRequest.Post(apiLink + "rank/colornotword", form))
        {
            yield return www.Send();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string json = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
                Debug.Log(json);
                tk = JsonMapper.ToObject(json);
                if (tk["status"].ToString() == "200")
                {
                    for (int i = 0; i < tk["listOfUserRank"].Count; i++)
                    {
                        Debug.Log("Getting!");
                        StartCoroutine(getImgByUrl(tk["listOfUserRank"][i]["userRankStand"].ToString(), tk["listOfUserRank"][i]["userAvatarLink"].ToString()));
                        //CreateFriend(tk["listOfUserRank"][i]["userRankStand"].ToString(), tk["listOfUserRank"][i]["userAvatarLink"].ToString(), tk["listOfUserRank"][i]["userFullName"].ToString(), tk["listOfUserRank"][i]["userPoint"].ToString());
                    }
                }
            }
        }
    }
    IEnumerator getImgByUrl(string rank,string url)
    {
        WWW www = new WWW(url);
        yield return www;
        Renderer renderer = GetComponent<Renderer>();
        hash.Add(rank, Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0.5f, 0.5f)));
        //listAva.Add();
        Debug.Log("Get xong!");
        //img.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0.5f, 0.5f));
    }
    
}
