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
    public GameObject dradisModel;
    public GameObject transparentDradisModel;
    public float radarSize = 0.01f; // for scaling the relative distance between the radar and what it hits
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
        }
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
                    clone.transform.localScale *= reScale;
                    clone.transform.localPosition = ((dradisList[i].transform.position - transform.position) * radarSize);
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
        if (dradisList.Contains(col2.gameObject) == true)
        {
            if (dontDestroyOld == true)
            {
                int index = dradisList.IndexOf(col2.gameObject);

                dradisList.Remove(col2.gameObject);
                Destroy(activeDradisModelList[index]);
                activeDradisModelList.RemoveAt(index);

                //Destroy(emptyObject);
                //emptyObject = Instantiate(placeHolder, displayLocation.transform.position, displayLocation.transform.rotation) as GameObject;
                //emptyObject.transform.parent = displayLocation.transform;
            }
            else { dradisList.Remove(col2.gameObject); }
        }
            

    }
    public void OnTriggerEnter(Collider col)
    {
        if (isRadar == true)
        {
            if (col.gameObject.GetComponent<Dradis>() != null)
            {
                if (!dradisList.Contains(col.gameObject) && dradisValue <= dradisPower)
                {
                    dradisList.Add(col.gameObject);
                    
                    if (dontDestroyOld == true)
                    {
                        GameObject clone = null;
                        if (fullScaleRadarImages == true)
                        {
                            clone = Instantiate(col.gameObject.GetComponent<Dradis>().dradisModel, Vector3.zero, col.gameObject.transform.rotation) as GameObject;
                        }
                        else
                        {
                            clone = Instantiate(col.gameObject.GetComponent<Dradis>().transparentDradisModel, Vector3.zero, col.gameObject.transform.rotation) as GameObject;
                        }

                        clone.transform.parent = emptyObject.transform;
                        clone.transform.localScale *= reScale;
                        clone.transform.localPosition = ((col.gameObject.transform.position - transform.position) * radarSize);
                        activeDradisModelList.Add(clone);
                    }
                }
            }
        }
    }
}
