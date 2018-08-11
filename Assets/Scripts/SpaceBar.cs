using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceBar : MonoBehaviour {

    public const float SINK_PAUSE = 0.2f;

    enum SpaceBarState {
        AVAILABLE,
        SINKING
    }

    private SpaceBarState state;
    private float sinkingStart;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (state == SpaceBarState.AVAILABLE) {
            // Nothing
        } else if (state == SpaceBarState.SINKING) {
            if (Time.time - sinkingStart >= SINK_PAUSE) {
                state = SpaceBarState.AVAILABLE;
                Debug.Log("Finished sinking");
            }
        }
	}

    public bool isAvailable() {
        return state == SpaceBarState.AVAILABLE;
    }

    public void doSink() {
        state = SpaceBarState.SINKING;
        sinkingStart = Time.time;

        Debug.Log("Started sinking");
    }
}
