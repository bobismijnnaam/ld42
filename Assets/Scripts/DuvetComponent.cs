using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuvetComponent : Activatable {

    enum State {
        SLEEPING,
        NOT_SLEEPING
    }

    public Player player;
    public Camera sleepCamera;
    public BlinkComponent blackSleepSurface;
    public AudioSource carTootToot;
    public SpareDeliverer spareDeliverer;
    public TextMesh deliveryTomorrowText;

    private State state = State.NOT_SLEEPING;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override string getDescription() {
        return "sleep";
    }

    public override void activate() {
        player.setViewWalkEnabled(false);
        // Look at point light so text disappears
        player.lookAt(new Vector3(0.82f, 1.38f, 4.12f));
        sleepCamera.enabled = true;
        blackSleepSurface.blinkOnce(7);
        Invoke("wakeUp", 5.5f);
        if (spareDeliverer.hasOrder()) {
            Invoke("triggerToot", 3.5f);
            deliveryTomorrowText.gameObject.SetActive(false);
        } 
    }

    public void wakeUp() {
        player.setViewWalkEnabled(true);
        sleepCamera.enabled = false;
        if (spareDeliverer.hasOrder()) {
            spareDeliverer.spawnDelivery();
            spareDeliverer.popOrder();
        }
    }

    public void triggerToot() {
        carTootToot.Play();
    }
}
