using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityStandardAssets.Characters.FirstPerson;

public class Player : MonoBehaviour {

    public GameObject gameScreenPlane;
    public Camera screenCamera;
    public Camera mainCamera;
    public Text flavorText;
    public Text spaceBarCountText;

    private FirstPersonController fpController;
    private PlayerMode playerMode = PlayerMode.WALKING;
    private int numSpaceBars = 0;

    enum PlayerMode {
        WALKING,
        PLAYING
    }

	// Use this for initialization
	void Start () {
        fpController = GetComponent<FirstPersonController>();
        updateSpaceBarCount();
	}

    public int getNumSpacebars() {
        return numSpaceBars;
    }

    bool lookingAtGameScreen() {
        RaycastHit hit;
        var wasHit = Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, 2f);
        return wasHit && hit.collider.gameObject == gameScreenPlane;
    }
	
	// Update is called once per frame
	void Update () {
        if (playerMode == PlayerMode.WALKING) {
            if (lookingAtGameScreen()) {
                showFlavorText("Press F to play");
                if (Input.GetButtonDown("Fire1")) {
                    switchMode();
                }
            } else if (lookingAtSpaceBarDrop()) {
                showFlavorText("Press F to pick up spare space bar");
                if (Input.GetButtonDown("Fire1")) {
                    deleteSpaceBarDrop();
                    numSpaceBars += 1;
                    updateSpaceBarCount();
                }
            } else if (lookingAtActivatable()) {
                Activatable acti = getActivatableInFront();   
                showFlavorText("Press F to " + acti.getDescription());
                if (Input.GetButtonDown("Fire1")) {
                    acti.activate();
                }
            } else {
                flavorText.enabled = false;
            }
        } else if (playerMode == PlayerMode.PLAYING) {
            showFlavorText("Press F to leave");

            if (Input.GetButtonDown("Fire1")) {
                switchMode();
            }
        }
	}

    GameObject getSpaceBarInFront() {
        RaycastHit hit;
        var wasHit = Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, 10f);
        if (wasHit && hit.collider.gameObject.GetComponent<SpaceBarDrop>() != null) {
            return hit.collider.gameObject;
        }
        return null;
    }

    Activatable getActivatableInFront() {
        RaycastHit hit;
        var wasHit = Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, 10f);
        if (wasHit) {
            return hit.collider.gameObject.GetComponentInParent<Activatable>();
        }
        return null;
    }

    bool lookingAtActivatable() {
        return getActivatableInFront() != null;
    }

    bool lookingAtSpaceBarDrop() {
        return getSpaceBarInFront() != null;
    }

    void updateSpaceBarCount() {
        spaceBarCountText.text = numSpaceBars + "";
    }

    void deleteSpaceBarDrop() {
        var possibleSpaceBar = getSpaceBarInFront();
        if (possibleSpaceBar != null) {
            Destroy(possibleSpaceBar);
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

    public void showFlavorText(string txt) {
        flavorText.text = txt;
        flavorText.enabled = true;
    }

    public void takeASpaceBar() {
        numSpaceBars -= 1;
        updateSpaceBarCount();
    }
}
