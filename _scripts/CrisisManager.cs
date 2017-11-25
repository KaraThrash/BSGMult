using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrisisManager : Photon.PunBehaviour
{

    public GameObject activeCylonFleet;

    public GameObject npcBaseStar;
    public GameObject spawnSpot1;
    public GameObject spawnSpot2;
    public GameObject spawnSpot3;

    public GameObject crisis1;
    public GameObject crisis2;
    public GameObject crisis3;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    //[PunRPC]
    public void AttackingBaseStar()
    {
        if (photonView.isMine == true)
        {
            if (crisis1 == null)
            {
                crisis1 = PhotonNetwork.Instantiate("BaseStar_New", spawnSpot1.transform.position, spawnSpot1.transform.rotation, 0, null);
                crisis1.transform.parent = activeCylonFleet.transform;

            }
            else if (crisis2 == null)
            {
                crisis2 = PhotonNetwork.Instantiate("BaseStar_New", spawnSpot2.transform.position, spawnSpot2.transform.rotation, 0, null);
                crisis2.transform.parent = activeCylonFleet.transform;

            }
            else if (crisis3 == null)
            {
                crisis3 = PhotonNetwork.Instantiate("BaseStar_New", spawnSpot3.transform.position, spawnSpot3.transform.rotation, 0, null);
                crisis3.transform.parent = activeCylonFleet.transform;
            }
            else
            {
                //buff  base star
            }
        }
     

    }
}
