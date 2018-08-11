using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaptopComponent : Activatable {

    public GameObject screenGameObject;

    enum State {
        OPEN,
        CLOSED
    }

    private State state = State.CLOSED;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void activate() {
        if (state == State.OPEN) {
            screenGameObject.transform.Rotate(0, 0, -117);
            state = State.CLOSED;
        } else {
            screenGameObject.transform.Rotate(0, 0, 117);
            state = State.OPEN;
        }
    }

    public override string getDescription() {
        if (state == State.OPEN) {
            return "close";
        } else {
            return "open";
        }
    }

}
