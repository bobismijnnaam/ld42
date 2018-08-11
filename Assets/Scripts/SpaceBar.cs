using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceBar : MonoBehaviour {

    public const float SINK_PAUSE = 0.2f;

    enum SpaceBarState {
        AVAILABLE,
        SINKING
    }

    private Renderer renderer;
    private SpaceBarState state;
    private float sinkingStart;
    private int numSinks;
    private int maxSinks;

	// Use this for initialization
	void Start () {
		maxSinks = 5;
        renderer = gameObject.GetComponent<Renderer>();
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

        updateSpaceBarColor();
	}

    public bool isAvailable() {
        return state == SpaceBarState.AVAILABLE;
    }

    public void doSink() {
        state = SpaceBarState.SINKING;
        sinkingStart = Time.time;
        numSinks += 1;

        Debug.Log("Started sinking");
    }

    public void updateSpaceBarColor() {
        var startColor = new Color(245 / 255f, 245 / 255f, 220 / 255f);
        var endColor = new Color(1, 0, 0);
        
        float p = (float) numSinks / maxSinks;
        var currentColor = Color.Lerp(startColor, endColor, p);
        Debug.Log(p);
        Debug.Log(currentColor);

        renderer.material.color = currentColor;
    }
}
