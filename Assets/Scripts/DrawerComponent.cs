using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawerComponent : Activatable {

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

    public override string getDescription() {
        if (state == State.OPEN) {
            return "close";
        } else {
            return "open";
        }
    }

    public override void activate() {
        if (state == State.OPEN) {
            gameObject.transform.Translate(10, 0, 0);
            state = State.CLOSED;
        } else if (state == State.CLOSED) {
            gameObject.transform.Translate(-10, 0, 0);
            state = State.OPEN;
        }
    }
}
