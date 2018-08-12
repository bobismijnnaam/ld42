using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpareDeliverer : MonoBehaviour {

    int numOrdered;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void addOrder() {
        numOrdered += 100;
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
        Debug.Log("Not implemented!");
    }
}
