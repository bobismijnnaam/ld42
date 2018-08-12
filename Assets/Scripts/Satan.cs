using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Satan : Activatable {

    [NotNull]
    public Light[] lights;
    [NotNull]
    public Player player;

    private int lightIndex = 0;

	// Use this for initialization
	void Start () {
		Assert.AreEqual(lights.Length, 4);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override string getDescription() {
        return "offer a space";
    }

    public override void activate() {
        if (lightIndex < 4) {
            if (player.getNumSpacebars() > 0) {
                player.takeASpaceBar();
                lights[lightIndex].color = new Color(1, 0, 0);
                lightIndex += 1;
            }
        }
    }
}
