using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Assertions;

public class Satan : Activatable {

    [NotNull]
    public Light[] lights;
    [NotNull]
    public Player player;
    [NotNull]
    public AudioSource endOfDays;

    private bool musicFadeIn;
    private float musicFadeInStart;
    private float musicFadeInDuration;

    private int lightIndex = 0;

	// Use this for initialization
	void Start () {
		Assert.AreEqual(lights.Length, 4);
	}
	
	// Update is called once per frame
	void Update () {
        if (musicFadeIn) {
            var dt = Time.time - musicFadeInStart;
            if (dt >= musicFadeInDuration) {
                musicFadeIn = false;
                endOfDays.Stop();
            } else {
                endOfDays.volume = dt / musicFadeInDuration;
            }
        }
	}

    public override string getDescription() {
        return "offer a space";
    }

    public override void activate() {
        if (lightIndex < 4) {
            if (player.getNumSpacebars() > 0) {
                player.takeASpaceBar();
                lights[lightIndex].color = new Color(1, 0, 0);
                lightIndex += 1;

                if (lightIndex == 4) {
                    startSFFade(5);
                    player.doShakeFadeIn(5);
                    Invoke("switchScenes", 5);
                }
            }
        } else {
            if (player.getNumSpacebars() > 0) {
                startSFFade(5);
                player.doShakeFadeIn(5);
            }
        }
    }

    void startSFFade(float t) {
        endOfDays.volume = 0;
        musicFadeIn = true;
        musicFadeInStart = Time.time;
        musicFadeInDuration = t;
        endOfDays.Play();
    }
    
    void switchScenes() {
        SceneManager.LoadScene("BlackScene");
    }
}
