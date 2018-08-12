using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundOnActivateComponent : Activatable {

    public string description;
    public AudioSource audioSource;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override string getDescription() {
        return description;
    }

    public override void activate() {
        audioSource.Play();
    }
}
