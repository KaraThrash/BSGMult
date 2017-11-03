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
    public GameObject ftlEffectObject;
	// Use this for initialization
	void Start () {
        defaultPosition = transform.localPosition;
        ftlPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + 3);
    }
	
	// Update is called once per frame
	void Update () {
        if (ftlDuration > 0) {
            ftlDuration -= Time.deltaTime;
            
            cam.GetComponent<Camera>().fieldOfView += 0.5f;
            cam.transform.localPosition = Vector3.MoveTowards( cam.transform.localPosition, ftlPosition, 0.03f);
        } else { cam.GetComponent<Camera>().fieldOfView = defaultDepthOfField;
            cam.transform.localPosition = defaultPosition;
        }
	}
    public void StartFTLEffect() {
        ftlDuration = 2;
        GameObject clone = Instantiate(ftlEffectObject, transform.position,transform.rotation) as GameObject;
        clone.transform.parent = this.transform;
       // GetComponent<RPGCamera>().ftlCameraEffect = 2;
    }
}
