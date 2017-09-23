using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempDisplayObjects : MonoBehaviour {
    public float HowLongActive;
    public float timer;
	// Use this for initialization
	void Start () {
		
	}
    void OnEnable()
    {
        timer = HowLongActive;
    }
    // Update is called once per frame
    void Update () {
        if (timer > 0) { timer -= Time.deltaTime; }
        if (timer <= 0) { this.gameObject.active = false; }
	}
}
