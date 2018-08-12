using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionComponent : Activatable {

    AudioSource currentSource;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override string getDescription() {
        return "question";
    }

    public override void activate() {
        if (currentSource != null) {
            if (currentSource.isPlaying) {
                return;
            } else {
                currentSource = null;
            }
        }

        currentSource = getRandomAudioSource();
        currentSource.Play();
    }

    AudioSource getRandomAudioSource() {
        var sources = GetComponentsInChildren<AudioSource>();
        return sources[Random.Range(0, sources.Length)];
    }
}
