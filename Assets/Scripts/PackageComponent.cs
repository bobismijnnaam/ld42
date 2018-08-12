using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageComponent : Activatable {

    public Player player;
    public GameObject shrine;
    public int numContainedSpacebars = 0;
    public bool containsShrine = true;

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
        shrine.SetActive(true);
        Destroy(gameObject);
    }
}
