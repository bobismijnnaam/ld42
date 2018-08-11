using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceGameController : MonoBehaviour {

    public Player player;
    public SpaceBar spaceBar;
    public TextMesh spaceCountLabel;
    public GameObject noSpaceBarText;
    public GameObject shopReticules;

    public float approachSpeedPercentage = 0.20f;

    private int spaceCount;

    private float shopReticuleYTarget = 1.13f;
    

	// Use this for initialization
	void Start () {
        
	}
	
    void spaceBarPressed() {
        if (spaceBar.isAvailable()) {
            spaceBar.doSink();
            spaceCount += 1;
            updateSpaceCountLabel();
        }
    }

    void updateSpaceCountLabel() {
        spaceCountLabel.text = spaceCount + "";
    }

	// Update is called once per frame
	void Update () {
        if (player.isPlaying()) {
            // Game logic
            if (Input.GetKeyDown("space")) {
                spaceBarPressed();
            }

            if (spaceBar.isBroken() && Input.GetKeyDown("r")) {
                if (player.getNumSpacebars() > 0) {
                    spaceBar.repair();
                    player.takeASpaceBar();
                } else {
                    noSpaceBarText.GetComponent<BlinkComponent>().blinkOnce(1f);
                }
            }

            if (Input.GetKeyDown("up")) {
                shopReticuleYTarget += 0.53f;
            }
            if (Input.GetKeyDown("down")) {
                shopReticuleYTarget -= 0.53f;
            }
            if (Input.GetKeyDown("enter")) {
                
            }

        }	

        updateShopReticule();
	}

    void updateShopReticule() {
        shopReticuleYTarget = Mathf.Min(1.13f, Mathf.Max(shopReticuleYTarget, -0.4f));

        var currentPosition = shopReticules.transform.position;
        currentPosition.y = currentPosition.y + (shopReticuleYTarget - currentPosition.y) * approachSpeedPercentage * Time.deltaTime;
        shopReticules.transform.position = currentPosition;
    }
}
