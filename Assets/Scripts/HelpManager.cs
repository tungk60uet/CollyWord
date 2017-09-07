using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HelpManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void backClick()
    {
        Debug.Log("Thoát game");
        SceneManager.LoadScene("StartMenu");
    }
}
