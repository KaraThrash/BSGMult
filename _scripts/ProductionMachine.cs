using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductionMachine : MonoBehaviour {
    public bool activated;
    public float timeCost;
    public float interactTimer;
    public string objectToSpawn;
    public GameObject objectToSpawnPrefab;
    public GameObject whereToSpawn;
    public GameObject loadingObject;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (activated == true)
        {

            loadingObject.active = true;
            interactTimer += Time.deltaTime;
            if (interactTimer >= timeCost) {
                interactTimer = 0;
                PhotonNetwork.Instantiate(objectToSpawn, whereToSpawn.transform.position, whereToSpawn.transform.rotation, 0, null);
                activated = false;
                loadingObject.active = false;
                GetComponent<PhotonView>().RPC("ToggleOnOff", PhotonTargets.AllBufferedViaServer); 
            }
        }
	}
    public void Manned()
    {
        loadingObject.active = true;
        activated = true;
       
    }
    public void NotManned()
    {
     
       
        activated = false;
        
    }
    public void Repair()
    {

    }

    public void Sabotage()
    {
        if (interactTimer > -10)
        {
            interactTimer--;
        }
    }

}
