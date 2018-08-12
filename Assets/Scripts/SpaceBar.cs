using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceBar : MonoBehaviour {

    public const float SINK_PAUSE = 0.3f;

    enum SpaceBarState {
        AVAILABLE,
        BROKEN
    }

    enum AnimState {
        STILL,
        SINKING
    }

    public GameObject brokenSpaceBar;
    public GameObject brokenText;
    public float sinkDepth;

    private Renderer myRenderer;
    private SpaceBarState state;
    private AnimState animState;
    private float sinkingStart;
    private int numSinks;
    private int maxSinks;
    private Vector3 startPos;
    private Vector3 lowPos;

	// Use this for initialization
	void Start () {
		maxSinks = 150;
        myRenderer = gameObject.GetComponent<Renderer>();
        startPos = gameObject.transform.position;
        lowPos = startPos - new Vector3(0, sinkDepth, 0);
        animState = AnimState.STILL;
	}
	
	// Update is called once per frame
	void Update () {
        if (animState == AnimState.SINKING) {
            var dt = Time.time - sinkingStart;
            if (dt >= SINK_PAUSE) {
                animState = AnimState.STILL;
                gameObject.transform.position = startPos;
                Debug.Log("Finished sinking");
            } else {
                // Between 0 and 1
                var p = dt / SINK_PAUSE;
                // Between 0 and 1 and 0 again
                p = Mathf.Sin(p * Mathf.PI);
                gameObject.transform.position = Vector3.Lerp(startPos, lowPos, p);
            }
        }

        updateSpaceBarColor();
	}

    public bool isAvailable() {
        return state == SpaceBarState.AVAILABLE;
    }

    public bool isBroken() {
        return state == SpaceBarState.BROKEN;
    }

    public void doSink() {
        animState = AnimState.SINKING;
        sinkingStart = Time.time;
        numSinks += 1;
        gameObject.transform.position = startPos;

        Debug.Log("Started sinking");

        if (numSinks == maxSinks) {
            state = SpaceBarState.BROKEN;
            myRenderer.enabled = false;
            brokenSpaceBar.SetActive(true);
            brokenText.SetActive(true);
        }
    }

    public void updateSpaceBarColor() {
        var startColor = new Color(245 / 255f, 245 / 255f, 220 / 255f);
        var endColor = new Color(1, 0, 0);
        
        var currentColor = Color.Lerp(startColor, endColor, (float) numSinks / maxSinks);

        myRenderer.material.color = currentColor;
    }

    public void repair() {
        numSinks = 0;
        updateSpaceBarColor();
        brokenSpaceBar.SetActive(false);
        myRenderer.enabled = true;
        brokenText.SetActive(false);
        state = SpaceBarState.AVAILABLE;
    }

    public void setMaxSinks(int maxSinks) {
        this.maxSinks = maxSinks;
    }
}
