using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkComponent : MonoBehaviour {

    private TextMesh text;
    private State state;
    private float start;
    private float duration;

    enum State {
        OFF,
        LAST
    }

	// Use this for initialization
	void Start () {
        if (GetComponent<TextMesh>() != null) {
            text = GetComponent<TextMesh>();
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (state == State.LAST) {
            if ((Time.time - start) >= duration) {
                state = State.OFF;
                setOpacity(0);
            } else {
                setOpacity(Mathf.PingPong((Time.time - start) / duration * 2, 1));
            }
        }
	}

    void setOpacity(float o) {
        Debug.Log("Setting setOpacity at " + o);
        if (text != null) {
            var clr = text.color;
            clr.a = o;
            text.color = clr;
        }
    }

    public void blinkOnce(float duration) {
        setOpacity(0);
        start = Time.time;
        this.duration = duration;
        state = State.LAST;
    }
}
