using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dradis : MonoBehaviour {
    public float tick;
    public float timer;
    public List<GameObject> dradisList = new List<GameObject>();
    public GameObject placeHolder;
    public GameObject emptyObject;
	// Use this for initialization
	void Start () {
        emptyObject = Instantiate(placeHolder, transform.position, transform.rotation) as GameObject;
        emptyObject.transform.parent = this.transform;
    }
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if (timer > 1f) {
            Destroy(emptyObject);
             emptyObject = Instantiate(placeHolder, transform.position, transform.rotation) as GameObject;
            emptyObject.transform.parent = this.transform;
            tick = 0.02f; timer = 0; }
        if (tick > 0) {
            tick = 0;
            spawnDradis(); 
            //tick -= Time.deltaTime;
        }
	}
    public void spawnDradis() {
        for (var i = dradisList.Count - 1; i > -1; i--)
        {
            if (dradisList[i] != null) {
                GameObject clone = Instantiate(placeHolder, ((dradisList[i].transform.position - transform.position) * 0.01f), dradisList[i].transform.rotation) as GameObject;
                clone.transform.parent = emptyObject.transform;
                //clone.transform.name = dradisList[i].transform.name + "dradis";
            } else { dradisList.RemoveAt(i); }
                
        }
        //foreach (GameObject obj in dradisList)
        //{
        //    //Debug.Log(obj.transform.name);
        //   if (obj != null)
        //    {
                
        //    }
        //   // else { dradisList.Remove(obj); }
        //}
       }
        public void OnTriggerExit(Collider col2) {
        if (dradisList.Contains(col2.gameObject) == true) { dradisList.Remove(col2.gameObject); }
            

    }
    public void OnTriggerEnter(Collider col)
    {
        if (!dradisList.Contains(col.gameObject))
        {
            dradisList.Add(col.gameObject);
            //GameObject clone = Instantiate(placeHolder, ((col.transform.position - transform.position) * 0.1f), col.transform.rotation) as GameObject;
            //clone.transform.parent = emptyObject.transform;
            //clone.transform.name = col.transform.name;
        }

    }
}
