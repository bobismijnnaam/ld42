using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageComponent : Activatable {

    public Player player;
    public int numContainedSpacebars = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override string getDescription() {
        return "pick up package";
    }

    public override void activate() {
        player.giveSpacebars(numContainedSpacebars);
        Destroy(gameObject);
    }
}
