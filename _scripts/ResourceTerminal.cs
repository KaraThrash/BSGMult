using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceTerminal : Photon.PunBehaviour
{
    public GameObject fleet;
    public bool foodTerminal;
    public bool fuelTerminal;
    public bool ammoTerminal;
    public GameObject myResource;
    public bool countedForFleetStats;
    public int resourceInMachine;
    public Transform storageDisplay;
    public int resourceInList;
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
                //resourceInMachine += col.gameObject.GetComponent<SupplyCrate>().quantity;
                if (resourceInMachine < 5) { storageDisplay.GetChild(resourceInMachine).gameObject.active = true; }
                Destroy(col.gameObject);
                return;
            }

            if (fuelTerminal == true && col.gameObject.GetComponent<SupplyCrate>().fuel == true)
            { fleet.GetComponent<Fleet>().UpdateResources(0, col.gameObject.GetComponent<SupplyCrate>().quantity, 0, 0, 0);
                GetComponent<PhotonView>().RPC("UpdateTotal", PhotonTargets.AllBufferedViaServer, col.gameObject.GetComponent<SupplyCrate>().quantity);
                //resourceInMachine += col.gameObject.GetComponent<SupplyCrate>().quantity;
                if (resourceInMachine < 5) { storageDisplay.GetChild(resourceInMachine).gameObject.active = true; }
                Destroy(col.gameObject);
                return;
            }
            if (ammoTerminal == true && col.gameObject.GetComponent<SupplyCrate>().ammo == true)
            {
                GetComponent<PhotonView>().RPC("UpdateTotal", PhotonTargets.AllBufferedViaServer, col.gameObject.GetComponent<SupplyCrate>().quantity);
                if (resourceInMachine < 5) { storageDisplay.GetChild(resourceInMachine).gameObject.active = true; }
                Destroy(col.gameObject);
                return;
            }
        }

    }
    public void Interact(GameObject whoUsedMe)
    {
        if (resourceInMachine > 0)
        {
            if (ammoTerminal == true )
            {
                if (whoUsedMe.GetComponent<PlayerCharacter>().ammo < 20)
                {
                    whoUsedMe.GetComponent<PhotonView>().RPC("ChangeAmmo", PhotonTargets.AllBufferedViaServer, 25);
                    GetComponent<PhotonView>().RPC("UpdateTotal", PhotonTargets.AllBufferedViaServer, -1);
                }
            }
            else
            {
                if (whoUsedMe.GetComponent<PlayerCharacter>().carriedObject == -1)
                {
                    if (countedForFleetStats == true)
                    {
                        if (foodTerminal == true)
                        {
                            if (fleet.GetComponent<Fleet>().food > 0) { fleet.GetComponent<Fleet>().UpdateResources(-1, 0, 0, 0, 0);
                                //whoUsedMe.GetComponent<PlayerCharacter>().carriedObject = 0;
                                GetComponent<PhotonView>().RPC("UpdateTotal", PhotonTargets.AllBufferedViaServer, -1);
                                whoUsedMe.GetComponent<PhotonView>().RPC("PickedUpObject", PhotonTargets.AllBufferedViaServer, 0);
                            }
                            
                            else { return; }
                        }
                        if (fuelTerminal == true)
                        {
                            if (fleet.GetComponent<Fleet>().fuel > 0) { fleet.GetComponent<Fleet>().UpdateResources(0, -1, 0, 0, 0);
                                //whoUsedMe.GetComponent<PlayerCharacter>().carriedObject = 1;
                                GetComponent<PhotonView>().RPC("UpdateTotal", PhotonTargets.AllBufferedViaServer, -1);
                                whoUsedMe.GetComponent<PhotonView>().RPC("PickedUpObject", PhotonTargets.AllBufferedViaServer, 1);
                            }
                            else { return; }
                        }

                    }
                    
                    
             
                    //GetComponent<PhotonView>().RPC("UpdateTotal", PhotonTargets.AllBufferedViaServer, -1);
                }
            }

            
           
            
        }

    }
    [PunRPC]
    public void UpdateTotal(int change)
    {

        resourceInMachine += change;
        if (change > 0 && resourceInMachine < 5) { storageDisplay.GetChild(resourceInMachine - 1).gameObject.active = true; }
        if (change < 0 && resourceInMachine >= 0 && resourceInMachine < 5) { storageDisplay.GetChild(resourceInMachine).gameObject.active = false; }
    }
}
