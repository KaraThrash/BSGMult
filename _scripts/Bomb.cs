using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {
    public float timeIncrement;
    public float timeUntilExplode;

    public GameObject myBattleStation;
    public GameObject crisisManager;
    
    public float timeToDefuse;
    public GameObject visualTimerAnimation;
    public bool timesUp;
    public bool defused;
    public int target;
    public GameObject explosion;
	// Use this for initialization
	void Start () {
        timeIncrement = 360 / timeUntilExplode;

    }
    void Awake()
    {
       
            DontDestroyOnLoad(this.gameObject);
      
    }
    // Update is called once per frame
    void Update () {
        if (timesUp == false && defused ==  false)
        {
            if (GetComponent<BattleStation>().on == true) { timeToDefuse -= Time.deltaTime; }

            visualTimerAnimation.transform.localEulerAngles = new Vector3(0, timeUntilExplode * timeIncrement, 0);

            timeUntilExplode -= Time.deltaTime;
            if (timeUntilExplode <= 0)
            {
                Instantiate(explosion,transform.position,transform.rotation);
                timesUp = true;
                if (crisisManager != null) { crisisManager.GetComponent<PhotonView>().RPC("NukeDetonated", PhotonTargets.AllViaServer,target); }
                this.gameObject.active = false;
            }
            if (timeToDefuse <= 0)
            {
                defused = true;
                if (crisisManager != null) { crisisManager.GetComponent<PhotonView>().RPC("NukeDefused", PhotonTargets.AllViaServer, target); }
                this.gameObject.active = false;
            }
        }
    }
    public void Manned()
    {
       
      
    }
    public void NotManned()
    {

    }


}
