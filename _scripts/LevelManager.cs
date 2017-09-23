using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Application.LoadLevel("Game");
        if (Input.GetKeyDown(KeyCode.E)) { Application.LoadLevel("Game"); }

    }
}
