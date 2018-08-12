using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpareDeliverer : MonoBehaviour {

    public Player player;
    public GameObject packagePrefab;

    int numOrdered;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void addOrder() {
        numOrdered += 10;
    }

    public bool hasOrder() {
        return numOrdered > 0;
    }

    public int popOrder() {
        int no = numOrdered;
        numOrdered = 0;
        return no;
    }

    public void spawnDelivery() {
        var packageObj = Instantiate(packagePrefab);
        var packageComponent = packageObj.GetComponent<PackageComponent>();

        packageComponent.transform.position = new Vector3(-6.58f, 0.28f, 0.10f);
        packageComponent.player = player;
        packageComponent.numContainedSpacebars = numOrdered;
    }
}
