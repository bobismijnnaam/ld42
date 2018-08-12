using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpareDeliverer : MonoBehaviour {

    [NotNull]
    public Player player;
    [NotNull]
    public GameObject shrine;
    public GameObject packagePrefab;

    int numOrdered;
    bool shrineOrdered;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void addOrder() {
        numOrdered += 2;
    }

    public void addShrine() {
        shrineOrdered = true;
    }

    public bool hasOrder() {
        return numOrdered > 0 || shrineOrdered;
    }

    public int popOrder() {
        int no = numOrdered;
        numOrdered = 0;
        shrineOrdered = false;
        return no;
    }

    public void spawnDelivery() {
        var packageObj = Instantiate(packagePrefab);
        var packageComponent = packageObj.GetComponent<PackageComponent>();

        packageComponent.containsShrine = shrineOrdered;
        packageComponent.shrine = shrine;
        packageComponent.transform.position = new Vector3(-6.58f, 0.28f, 0.10f);
        packageComponent.player = player;
        packageComponent.numContainedSpacebars = numOrdered;
    }
}
