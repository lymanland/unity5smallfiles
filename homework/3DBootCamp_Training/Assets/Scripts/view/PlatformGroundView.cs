using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGroundView : MonoBehaviour {
    public bool isOnGround;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.Tag_Player))
        {
            isOnGround = true;
            //Debug.Log("[PlatformGroundView] OnTriggerEnter:" + gameObject.name);
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(Constants.Tag_Player))
        {
            isOnGround = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Constants.Tag_Player))
        {
            isOnGround = false;
            //Debug.Log("[PlatformGroundView] OnTriggerExit:" + gameObject.name);
        }
    }
}
