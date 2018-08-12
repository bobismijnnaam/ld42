using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionComponent : Activatable {

    [NotNull]
    public Player player;

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

        if (Random.value < 0.2) {
            player.doShakeFadeOut(2.5f);
        }
    }

    AudioSource getRandomAudioSource() {
        var sources = GetComponentsInChildren<AudioSource>();
        return sources[Random.Range(0, sources.Length)];
    }
}
