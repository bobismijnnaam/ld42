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
        NOT_WALKING
    }

	// Use this for initialization
	void Start () {
        fpController = GetComponent<FirstPersonController>();
        updateSpaceBarCount();
	}

    public int getNumSpacebars() {
        return numSpaceBars;
    }

    //bool lookingAtGameScreen() {
        //RaycastHit hit;
        //var wasHit = Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, 3f);
        //return wasHit && hit.collider.gameObject == gameScreenPlane;
    //}
	
	// Update is called once per frame
	void Update () {
        if (playerMode == PlayerMode.WALKING) {
            if (lookingAtSpaceBarDrop()) {
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
        } // else if (playerMode == PlayerMode.PLAYING) {
            //showFlavorText("Press F to leave");

            //if (Input.GetButtonDown("Fire1")) {
                //switchMode();
            //}
        //}
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

    public void setViewWalkEnabled(bool isEnabled) {
        if (isEnabled) {
            mainCamera.enabled = true;
            fpController.enabled = true;
            playerMode = PlayerMode.WALKING;
        } else {
            mainCamera.enabled = false;
            fpController.enabled = false;
            playerMode = PlayerMode.NOT_WALKING;
        }
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
