using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceTerminal : Photon.PunBehaviour
{
    public GameObject fleet;
    public bool foodTerminal;
    public GameObject myResource;
    public bool countedForFleetStats;
    public int resourceInMachine;
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.GetComponent<SupplyCrate>() != null)
        {
            if (foodTerminal == true && col.gameObject.GetComponent<SupplyCrate>().food == true)
            { fleet.GetComponent<Fleet>().UpdateResources(col.gameObject.GetComponent<SupplyCrate>().quantity, 0, 0, 0, 0);
                GetComponent<PhotonView>().RPC("UpdateTotal", PhotonTargets.AllBufferedViaServer, col.gameObject.GetComponent<SupplyCrate>().quantity);
                Destroy(col.gameObject);
                return;
            }

            if (foodTerminal == false && col.gameObject.GetComponent<SupplyCrate>().food == false)
            { fleet.GetComponent<Fleet>().UpdateResources(0, col.gameObject.GetComponent<SupplyCrate>().quantity, 0, 0, 0);
                GetComponent<PhotonView>().RPC("UpdateTotal", PhotonTargets.AllBufferedViaServer, col.gameObject.GetComponent<SupplyCrate>().quantity);
                Destroy(col.gameObject);
                return;
            }

        }

    }
    public void Interact(GameObject whoUsedMe)
    {
        if (resourceInMachine > 0)
        {
            if (whoUsedMe.GetComponent<PlayerCharacter>().carriedObject == null)
            {
                if (countedForFleetStats == true)
                {
                    if (foodTerminal == true)
                    {
                        if (fleet.GetComponent<Fleet>().food > 0) { fleet.GetComponent<Fleet>().UpdateResources(-1, 0, 0, 0, 0); }
                        else { return; }
                    }


                    else { if (fleet.GetComponent<Fleet>().fuel > 0) { fleet.GetComponent<Fleet>().UpdateResources(0, -1, 0, 0, 0); } else { return; } }
                }

                GameObject clone = Instantiate(myResource, whoUsedMe.transform.position, myResource.transform.rotation) as GameObject;
                whoUsedMe.GetComponent<PlayerCharacter>().carriedObject = clone;
                whoUsedMe.GetComponent<PhotonView>().RPC("PickedUpObject", PhotonTargets.AllBufferedViaServer);

                clone.active = false;
                clone.transform.parent = whoUsedMe.transform;
            }
            GetComponent<PhotonView>().RPC("UpdateTotal", PhotonTargets.AllBufferedViaServer, -1);
        }
    }
    [PunRPC]
    public void UpdateTotal(int change)
    {

        resourceInMachine += change;
    }
}
