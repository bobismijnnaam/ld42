using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityStandardAssets.Characters.FirstPerson;

public class Player : MonoBehaviour {

    public GameObject gameScreenPlane;
    public Camera screenCamera;

    public FirstPersonController fpController;
    public Camera mainCamera;

    private PlayerMode playerMode = PlayerMode.WALKING;

    enum PlayerMode {
        WALKING,
        PLAYING
    }

	// Use this for initialization
	void Start () {
        fpController = GetComponent<FirstPersonController>();
	}
	
	// Update is called once per frame
	void Update () {
        RaycastHit hit;
        var wasHit = Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, 2f);
        if (wasHit && hit.collider.gameObject == gameScreenPlane && Input.GetButtonDown("Fire1")) {
            switchMode();
        }
	}

    void switchMode() {
        if (playerMode == PlayerMode.WALKING) {
            mainCamera.enabled = false;
            screenCamera.enabled = true;
            fpController.enabled = false;
            playerMode = PlayerMode.PLAYING;
        } else {
            mainCamera.enabled = true;
            screenCamera.enabled = false;
            fpController.enabled = true;
            playerMode = PlayerMode.WALKING;
        }
     }   

    public bool isPlaying() {
        return playerMode == PlayerMode.PLAYING;
    }
}
