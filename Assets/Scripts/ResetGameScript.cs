using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetGameScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	    Invoke("switchScenes", 3);	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void switchScenes() {
        SceneManager.LoadScene("MainScene");
    }
}
