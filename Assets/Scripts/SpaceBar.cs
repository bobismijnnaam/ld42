using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceBar : MonoBehaviour {

    public const float SINK_PAUSE = 0.2f;

    enum SpaceBarState {
        AVAILABLE,
        SINKING,
        BROKEN
    }

    public GameObject brokenSpaceBar;

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

        if (numSinks == maxSinks) {
            state = SpaceBarState.BROKEN;
            renderer.enabled = false;
            brokenSpaceBar.SetActive(true);
        }
    }

    public void updateSpaceBarColor() {
        var startColor = new Color(245 / 255f, 245 / 255f, 220 / 255f);
        var endColor = new Color(1, 0, 0);
        
        var currentColor = Color.Lerp(startColor, endColor, (float) numSinks / maxSinks);

        renderer.material.color = currentColor;
    }

    public void repair() {
        numSinks = 0;
        updateSpaceBarColor();
        brokenSpaceBar.SetActive(false);
        renderer.enabled = true;
    }
}
