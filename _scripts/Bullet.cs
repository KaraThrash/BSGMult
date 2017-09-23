﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    private Rigidbody rb;
    public int speed;
    public float lifeTime;
    public GameObject ownerObject;
    public int owner;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        
        rb.AddForce(transform.forward * (speed), ForceMode.Impulse);
    }
	
	// Update is called once per frame
	void Update () {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0) { Die(); }
	}
    public void OnCollisionEnter(Collision col)
    {

        if (ownerObject != null)
        {

        }
        Die();
        
    }
    public void Die() { Destroy(this.gameObject); }
}
