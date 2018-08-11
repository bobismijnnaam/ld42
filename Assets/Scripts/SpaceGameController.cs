using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceGameController : MonoBehaviour {

    public Player player;
    public SpaceBar spaceBar;
    public TextMesh spaceCountLabel;
    public GameObject noSpaceBarText;

    private int spaceCount;

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
        }	
	}
}
