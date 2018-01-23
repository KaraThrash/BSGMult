using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : Photon.PunBehaviour
{
    public int hp;
    public int localHp; //for subsystems
    public int size;
    public GameObject myBody;
    public GameObject secondarySystem;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    [PunRPC]
    public void TakeDamage(int dmg, int byWho,int dmgSize)
    {
        if (dmgSize >= size)
        {
            if (localHp > 0)
            {

                if (secondarySystem != null)
                {
                    //secondarySystem.SendMessage("TakeDamage", 1);
                    secondarySystem.GetComponent<PhotonView>().RPC("Damaged", PhotonTargets.AllViaServer);
                }
                localHp -= dmg;
                myBody.SendMessage("TakeDamage", dmg);
            }

        }

    }
}
