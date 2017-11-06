using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dradis : MonoBehaviour {
    public float tick;
    public float timer;
    public List<GameObject> dradisList = new List<GameObject>();
    public GameObject placeHolder;
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
    // Use this for initialization
    void Start () {
        if (isRadar == true)
        {
            emptyObject = Instantiate(placeHolder, transform.position, transform.rotation) as GameObject;
            emptyObject.transform.parent = this.transform;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (isRadar == true)
        {
            transform.eulerAngles = Vector3.zero;
            timer += Time.deltaTime;
            if (timer > radarUpdateTime)
            {
                Destroy(emptyObject);
                emptyObject = Instantiate(placeHolder, transform.position, transform.rotation) as GameObject;
                emptyObject.transform.parent = this.transform;
                tick = 0.02f; timer = 0;
            }
            if (tick > 0)
            {
                tick = 0;
                spawnDradis();
                //tick -= Time.deltaTime;
            }
        }
	}
    public void spawnDradis() {
        for (var i = dradisList.Count - 1; i > -1; i--)
        {
            if (dradisList[i] != null) {
                if (dradisList[i].GetComponent<Dradis>() != null)
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

    public void OnTriggerExit(Collider col2)
    {
        if (dradisList.Contains(col2.gameObject) == true)
        { dradisList.Remove(col2.gameObject); }
            

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
                    //GameObject clone = Instantiate(placeHolder, ((col.transform.position - transform.position) * 0.1f), col.transform.rotation) as GameObject;
                    //clone.transform.parent = emptyObject.transform;
                    //clone.transform.name = col.transform.name;
                }
            }
        }
    }
}
