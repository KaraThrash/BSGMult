using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMain : MonoBehaviour {
    //Parent script for each player
    public GameObject myCamera;
    public GameObject currentController;
    public GameObject humanoidObject;
    public bool atBattleStation;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //if (Input.GetKey(KeyCode.Backspace) && m_PhotonView.isMine == true) {
        //    humanoidObject.GetComponent<RPGMovement>().canMove = true;
        //    myCamera.active = true;
        //}
    }

}
