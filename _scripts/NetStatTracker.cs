using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetStatTracker : Photon.PunBehaviour
{
    public int stat;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
           stream.SendNext(stat);

        }
        else
        {
            stat = (int)stream.ReceiveNext();

        }
    }
}
