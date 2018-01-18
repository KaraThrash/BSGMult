using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dradis : MonoBehaviour {
    public float tick;
    public float timer;
    public List<GameObject> dradisList = new List<GameObject>();
    public List<GameObject> activeDradisModelList = new List<GameObject>();
    public GameObject placeHolder;
    public GameObject displayLocation; // to have the dradis on the ship and display it elsewhere
    public GameObject emptyObject;
    public int dradisValue; //object size fighter/capital ship
    public int dradisPower; //what size object this can detect
    public bool isRadar; //or target 
    public bool canZoom; 
    public GameObject dradisModel;
    public GameObject transparentDradisModel;

    //for ship to ship targets/locking on
    public GameObject myShip;
    public int faction;
    public bool isTargetable;

    public float radarSize = 0.01f; // for scaling the relative distance between the radar and what it hits
    public float zoom = 0;

    public float reScale = 1;// interior dradis targets can be displayed larger
    public float radarUpdateTime;
    public bool fullScaleRadarImages;
    public bool dontDestroyOld;
    // Use this for initialization
    void Start () {
        if (isRadar == true)
        {
            emptyObject = Instantiate(placeHolder, displayLocation.transform.position, transform.rotation) as GameObject;
            emptyObject.transform.parent = displayLocation.transform;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (isRadar == true)
        {
            transform.eulerAngles = Vector3.zero;
            //displayLocation.transform.localEulerAngles = transform.eulerAngles;
            timer += Time.deltaTime;
            if (timer > radarUpdateTime)
            {
                if (dontDestroyOld == false)
                {
                    Destroy(emptyObject);
                    emptyObject = Instantiate(placeHolder, displayLocation.transform.position, displayLocation.transform.rotation) as GameObject;
                    emptyObject.transform.parent = displayLocation.transform;
                }
                tick = 0.02f; timer = 0;
            }
            if (tick > 0)
            {
                tick = 0;
                if (dontDestroyOld == false)
                {
                    spawnDradis();
                }
                else {
                    UpdateDradisLocation();
                }
                //tick -= Time.deltaTime;
            }

            if (Input.GetKeyDown(KeyCode.LeftControl) && canZoom == true)
            { ChangeZoom(); }
        }
	}

    public void ChangeZoom()
    {
        timer = 0;
        Destroy(emptyObject);
        emptyObject = Instantiate(placeHolder, displayLocation.transform.position, displayLocation.transform.rotation) as GameObject;
        emptyObject.transform.parent = displayLocation.transform;
        if (zoom == 0.03f) { zoom = 0; } else { zoom += 0.01f; }
        GetComponent<SphereCollider>().radius = 1000 - (30000 * zoom);
        spawnDradis();
    }

    public void spawnDradis() {

        for (var i = dradisList.Count - 1; i > -1; i--)
        {
            if (dradisList[i] != null) {
                if (dradisList[i].GetComponent<Dradis>() != null && dradisList[i].active != false)
                {
                    GameObject clone = null;
                    if (fullScaleRadarImages == true) {
                        clone = Instantiate(dradisList[i].GetComponent<Dradis>().dradisModel, Vector3.zero, dradisList[i].transform.rotation) as GameObject;
                    } else {
                        clone = Instantiate(dradisList[i].GetComponent<Dradis>().transparentDradisModel, Vector3.zero, dradisList[i].transform.rotation) as GameObject;
                    }
                     
                    clone.transform.parent = emptyObject.transform;
                    clone.transform.localScale *= (reScale + (30 * zoom));
                    clone.transform.localPosition = ((dradisList[i].transform.position - transform.position) * (radarSize + zoom));
                    //clone.transform.name = dradisList[i].transform.name + "dradis";
                }
            } else { dradisList.RemoveAt(i); }
                
        }

       }

    public void UpdateDradisLocation()
    {
        for (var i = dradisList.Count - 1; i > -1; i--)
        {
            if (dradisList[i] != null && dradisList[i].active != false)
            {
                activeDradisModelList[i].transform.localPosition = ((dradisList[i].transform.position - transform.position) * radarSize);
                activeDradisModelList[i].transform.rotation = dradisList[i].transform.rotation;
                
                
            }
            else {
                dradisList.RemoveAt(i);
                Destroy(activeDradisModelList[i]);
                activeDradisModelList.RemoveAt(i);

            }

        }

    }

    public void OnTriggerExit(Collider col2)
    {

        if (isRadar == false && col2.gameObject.GetComponent<Dradis>() != null)
        {

            if (col2.gameObject.GetComponent<Dradis>().isRadar == true)

            {
                col2.gameObject.GetComponent<Dradis>().LostDradisContact(this.gameObject);


            }

        }

    }
    public void OnTriggerEnter(Collider col)
    {
        if (isRadar == false && col.gameObject.GetComponent<Dradis>() != null)
        {
            
                if(col.gameObject.GetComponent<Dradis>().isRadar == true)
                
                {
                col.gameObject.GetComponent<Dradis>().NewDradisContact(this.gameObject);


                }
            
        }
    }

    public void LostDradisContact(GameObject oldContact)
    {
        if (dradisList.Contains(oldContact) == true)
        {
            if (dontDestroyOld == true)
            {
                int index = dradisList.IndexOf(oldContact);

                dradisList.Remove(oldContact);
                Destroy(activeDradisModelList[index]);
                activeDradisModelList.RemoveAt(index);

                //Destroy(emptyObject);
                //emptyObject = Instantiate(placeHolder, displayLocation.transform.position, displayLocation.transform.rotation) as GameObject;
                //emptyObject.transform.parent = displayLocation.transform;
            }
            else { dradisList.Remove(oldContact); }
        }

    }

    public void NewDradisContact(GameObject newContact)
    {
        if (!GetComponent<Dradis>().dradisList.Contains(newContact) && dradisValue <= dradisPower)
        {
            dradisList.Add(newContact);

            if (dontDestroyOld == true)
            {
                GameObject clone = null;
                if (fullScaleRadarImages == true)
                {
                    clone = Instantiate(newContact.GetComponent<Dradis>().dradisModel, Vector3.zero, newContact.transform.rotation) as GameObject;
                }
                else
                {
                    clone = Instantiate(newContact.GetComponent<Dradis>().transparentDradisModel, Vector3.zero, newContact.transform.rotation) as GameObject;
                }

                clone.transform.parent = emptyObject.transform;
                clone.transform.localScale *= reScale;
                clone.transform.localPosition = ((newContact.transform.position - transform.position) * radarSize);
                activeDradisModelList.Add(clone);
            }
        }

    }
}
