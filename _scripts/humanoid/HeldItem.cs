using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeldItem : MonoBehaviour {
    //Items held by humanoid characters that are used: guns, wrenches, hands[to access computers]
    //Items have a primary use mouse0 and a secondary mouse1

    public int weaponType; //0 gun, 1 wrench, 2 hands, 3 bombs
    public bool isGun; //TODO use vector 3 for dmg,cooldown,ammo tracking
    public bool isWrench;//TODO use vector 4 for type,dmg,cooldown,ammo tracking
    public bool isHands;
    public GameObject gun;
    public GameObject bullet;
    public int ammo;
    public int range;
    public float coolDown;
    public float wrenchCoolDown;
    public float gunCoolDown;
    public GameObject wrenchImpact;
    // Use this for initialization
    void Start () {
        ammo = 10;
	}
	
	// Update is called once per frame
	void Update () {
        if (coolDown > 0) { coolDown -= Time.deltaTime; }
	}
    public void UsePrimary()
    {
        if (isGun == true)
        { ShootGun(); }
        else if (isWrench == true) { Repair(); }
        else { }
    }
    public void UseSecondary()
    {
        if (isGun == true)
        { ShootGun(); }
        else if (isWrench == true) { Sabotage(); }
        else { }
    }
    public bool ShootGun()
    {
        if (ammo > 0 && coolDown <= 0)
        {
            coolDown = gunCoolDown;
            Instantiate(bullet, transform.position, transform.rotation);
            ammo--;
            return true;
        }
        else { return false; }
    }
    public void Repair()
    {
        if (coolDown <= 0)
        {
            GetComponent<Animator>().Play("FPSswingwrench");
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.forward, out hit, 2.0f) && hit.transform.tag == "Interactable")
            {

                coolDown = wrenchCoolDown;
                Instantiate(wrenchImpact, hit.point, transform.rotation);
               // hit.transform.gameObject.SendMessage("Repair", this.gameObject);

            }
        }
    }
    public void Sabotage()
    {
        if (coolDown <= 0)
        {
            GetComponent<Animator>().Play("FPSswingwrench");
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.forward, out hit, 2.0f) && hit.transform.tag == "Interactable")
            {
                Instantiate(wrenchImpact, hit.point, transform.rotation);
                coolDown = wrenchCoolDown;
                //hit.transform.gameObject.SendMessage("Sabotage", this.gameObject);

            }
        }

    }


    public void ChangeType(int newType)
    {
        isGun = false;
        isWrench = false;
        isHands = false;
        switch (newType)
        {
            case 1:
                isGun = true;
                break;
            case 2:
                isWrench = true;
                break;
            default:
                isHands = true;
                break;
        }

    }
}
