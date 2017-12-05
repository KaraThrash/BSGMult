using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrisisManager : Photon.PunBehaviour
{

    public GameObject activeCylonFleet;
    public GameObject[] hiddenNukeSpots;
    public GameObject npcBaseStar;
    public GameObject hiddenNuke;

    public GameObject spawnSpot1;
    public GameObject spawnSpot2;
    public GameObject spawnSpot3;

    public GameObject crisis1;
    public GameObject crisis2;
    public GameObject crisis3;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    //[PunRPC]
    public void AttackingBaseStar()
    {
        if (photonView.isMine == true)
        {
            if (crisis1 == null)
            {
                crisis1 = PhotonNetwork.Instantiate("BaseStar_New_HeavyRaider", spawnSpot1.transform.position, spawnSpot1.transform.rotation, 0, null);
                crisis2 = PhotonNetwork.Instantiate("BaseStar_New_Missile", spawnSpot2.transform.position, spawnSpot2.transform.rotation, 0, null);
                crisis3 = PhotonNetwork.Instantiate("BaseStar_New", spawnSpot3.transform.position, spawnSpot3.transform.rotation, 0, null);
                crisis1.transform.parent = activeCylonFleet.transform;
                crisis2.transform.parent = activeCylonFleet.transform;
                crisis3.transform.parent = activeCylonFleet.transform;
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

    public void HiddenBomb()
    {
        if (photonView.isMine == true)
        {
            if (crisis1 == null)
            {
                hiddenNuke = PhotonNetwork.Instantiate("HiddenNuke", hiddenNukeSpots[0].transform.position, hiddenNukeSpots[0].transform.rotation, 0, null);
                hiddenNuke.GetComponent<Bomb>().target = 0;

            }
            else if (crisis2 == null)
            {
                hiddenNuke = PhotonNetwork.Instantiate("HiddenNuke", hiddenNukeSpots[1].transform.position, hiddenNukeSpots[1].transform.rotation, 0, null);
                hiddenNuke.GetComponent<Bomb>().target = 1;

            }
            else if (crisis3 == null)
            {
                hiddenNuke = PhotonNetwork.Instantiate("HiddenNuke", hiddenNukeSpots[2].transform.position, hiddenNukeSpots[2].transform.rotation, 0, null);
                hiddenNuke.GetComponent<Bomb>().target = 2;
            }
            else
            {
                hiddenNuke = PhotonNetwork.Instantiate("HiddenNuke", hiddenNukeSpots[3].transform.position, hiddenNukeSpots[3].transform.rotation, 0, null);
                hiddenNuke.GetComponent<Bomb>().target = 3;
            }
            hiddenNuke.GetComponent<Bomb>().crisisManager = this.gameObject;
        }
    }

    [PunRPC]
    public void NukeDetonated(int location)
    {
        hiddenNukeSpots[location].GetComponent<PartOfShip>().DealDamage(1);
        if (photonView.isMine == true)
        {
            PhotonNetwork.Destroy(hiddenNuke);
        }
    }
    [PunRPC]
    public void NukeDefused(int location)
    {
        
        if (photonView.isMine == true)
        {
            PhotonNetwork.Destroy(hiddenNuke);
        }
    }
}
