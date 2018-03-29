using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierView : MonoBehaviour {
    public bool isUpPos;//是否悬空

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
            PlatformManager.Instance.hitBarrier =this;
            //Debug.Log("set Barrier");
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Constants.Tag_Player))
        {
            PlatformManager.Instance.hitBarrier = null;
            //Debug.Log("reset Barrier");
        }
    }
}
