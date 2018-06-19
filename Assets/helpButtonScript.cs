using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class helpButtonScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void GotoMainScene()
	{
		SceneManager.LoadScene("scene1");
	}

	public void GotoMenuScene()
	{
		SceneManager.LoadScene("helpscene");
	}

}
