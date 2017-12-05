using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HospitalBed : MonoBehaviour {
    public GameObject personInBed;
    public bool bedOpen;
    public GameObject myMedbay;
    public int healModifier;
    public int maxHeal = 10;

    public float healTimer;
    public float healCost;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (healTimer >= healCost && personInBed != null)
        {
            healTimer = 0;
            if (personInBed.GetComponent<PlayerCharacter>().hp < maxHeal)
            { personInBed.GetComponent<PlayerCharacter>().Heal(1 + healModifier); }
            
        }
    }
    public void PlaceWoundedPerson(GameObject wounded)
    {
        bedOpen = false;
        healTimer = 0;
        personInBed = wounded;
    }
    public void OnTriggerExit(Collider col2)
    {
        if (col2.gameObject == personInBed)
        {
            bedOpen = true;

            personInBed = null;

        }
    }
    public void OnTriggerEnter(Collider col3)
    {
        if (col3.gameObject.GetComponent<PlayerCharacter>() != null)
        {
            bedOpen = false;

            personInBed = col3.gameObject;

        }
    }
    public void OnTriggerStay(Collider col)
    {
        if (col.gameObject == personInBed)
        { healTimer += Time.deltaTime; }
        

    }

}