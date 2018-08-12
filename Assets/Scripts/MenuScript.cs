using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour {

    [NotNull]
    public Player player;
    private Camera menuCamera;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("f")) {
            player.gameObject.SetActive(true);
            Destroy(gameObject);
        }
	}
}
