using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasestarNpc : MonoBehaviour
{
   
    public GameObject activeCylonFleet;
    public GameObject target;
    public float timer;
    public int spawnCoolDown;
    public string spawnObject;
    public GameObject spawnSpot;
    public int maxSpawn;
    public int hp;
    public int currentSpawn;
    public GameObject explosion;
    public int spawntype; // 0:raiders 1:missiles, 2:heavy raiders etc
    // Use this for initialization
    void Start()
    {
        target = GameObject.Find("GalacticaShip");
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.parent == null)
        {
            activeCylonFleet = GameObject.Find("ActiveCylonFleet");

            if (activeCylonFleet != null)
            {
                transform.parent = activeCylonFleet.transform;
            }
        }
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = spawnCoolDown;
            Spawn();
        }


    }
   
    public void Spawn()
    {
        if (currentSpawn < maxSpawn)
        {
            currentSpawn++;
            GameObject clone = PhotonNetwork.Instantiate(spawnObject, spawnSpot.transform.position, spawnSpot.transform.rotation, 0, null);
            // GameObject clone = Instantiate(spawnObject, spawnSpot.transform.position, spawnSpot.transform.rotation) as GameObject;
            if (spawntype == 0) { clone.GetComponent<FighterWing>().roundManager = GameObject.Find("RoundManager"); }
            if (spawntype == 1 && target != null) { clone.GetComponent<Missile>().target = target; }
        }
    }
    //public void TakeDamage()
    //{
    //    if (hp > 0)
    //    {
    //        hp--;

    //        if (hp <= 0)
    //        {
    //            Instantiate(explosion, transform.position, transform.rotation);
    //            Destroy(this.gameObject);

    //        }
            
    //        Debug.Log("hit");
    //    }
    //}
}
