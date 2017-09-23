using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffects : MonoBehaviour {
    public int defaultDepthOfField;
    public int ftlDepthOfField;
    public Vector3 defaultPosition;
    public Vector3 ftlPosition;
    public bool ftlEffect;
    public GameObject cam;
    public float ftlDuration;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (ftlDuration > 0) {
            ftlDuration -= Time.deltaTime;
            
            cam.GetComponent<Camera>().fieldOfView += 0.5f;
            cam.transform.localPosition = Vector3.MoveTowards( cam.transform.localPosition, ftlPosition, 0.01f);
        } else { cam.GetComponent<Camera>().fieldOfView = defaultDepthOfField;
            cam.transform.localPosition = defaultPosition;
        }
	}
    public void StartFTLEffect() {
        ftlDuration = 2;
        GetComponent<RPGCamera>().ftlCameraEffect = 2;
    }
}
