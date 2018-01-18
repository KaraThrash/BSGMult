using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    private Rigidbody rb;
    public int speed;
    public float lifeTime;
    public int damage = 1;
    public GameObject ownerObject;
    public int owner;
    public GameObject explosion;
    public bool large;
    public GameObject intialExplosion;
    public bool aiBullet;
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        if (large == true) { Instantiate(intialExplosion, transform.position, transform.rotation); }
        rb.AddForce(transform.forward * (speed), ForceMode.Impulse);
       
    }
	
	// Update is called once per frame
	void Update () {
       // Debug.Log(rb.velocity.magnitude + "bullet Magnitude");
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0) { Die(); }
	}
    public void OnCollisionEnter(Collision col)
    {
        GetComponent<Collider>().enabled = false;
        if (explosion != null)
        {
            Instantiate(explosion, transform.position, transform.rotation);
        }
     
        Die();
        
    }
    public void Die() {

        
        
        Destroy(this.gameObject);

    }
}
