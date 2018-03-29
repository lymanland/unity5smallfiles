using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformView : MonoBehaviour {
    private readonly float MaxRotaionAngle = 14f;
    private readonly float MinRotaionAngle = 7f;

    private PlatformManager platformManager;
    public bool isInPlatform;
    public bool isWaitMoveForward = false;

    public PlatformGroundView groundView;

    void Awake()
    {
        Debug.Log("Awake:" + this);
    }

    // Use this for initialization
    void Start () {
        platformManager = GameObject.FindGameObjectWithTag(Constants.Tag_GameManager).GetComponent<PlatformManager>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

  
    public GameObject AddGameObject2Platform(GameObject prefab, Vector3 pos)
    {
        GameObject obj = Instantiate(prefab, pos, Quaternion.identity, transform);
        //obj.transform.rotation = Quaternion.Euler(0f - transform.rotation.eulerAngles.x, 0f, 0f);//反旋转
        //barrier.transform.localPosition = posNew;
        //Debug.Log("[AddGameObject2Platform] localPosition:" + obj.transform.localPosition + ", position:" + obj.transform.position + ", pos:" + pos);
        obj.transform.localPosition = new Vector3(obj.transform.localPosition.x, pos.y, obj.transform.localPosition.z);
        return obj;

    }

    public void MoveForwardPostion(float halfLengthPlatform)
    {
        PlatformView otherView = PlatformManager.Instance.GetOtherPlatformView(this);
        float of1cos = otherView.GetOffsetForwardCos(halfLengthPlatform);
        float of1sin = otherView.GetOffsetForwardSin(halfLengthPlatform);
        float of2cos = this.GetOffsetForwardCos(halfLengthPlatform);
        float of2sin = this.GetOffsetForwardSin(halfLengthPlatform);

        //Debug.Log("of1cos:" + of1cos + ", of1sin:" + of1sin + ", of2cos:" + of2cos + ", of2sin:" + of2sin);
        this.transform.position = otherView.transform.position + new Vector3(0, 0 - (of1sin + of2sin), of1cos + of2cos);
    }

    //public void ReverseRotationGameObjects(List<GameObject> objList)
    //{
    //    for (int di = objList.Count - 1; di >= 0; di--)
    //    {
    //        GameObject obj = objList[di];
    //        obj.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 0f, 0f);//反旋转
    //        Debug.Log("transform.rotation.eulerAngles.x:"+ transform.rotation.eulerAngles.x);
    //    }
    //}

    public void SetRandomRotaion()
    {
        bool isDown = GameUtil.GetRandomRate() < 50;
       SetRandomRotaion(isDown);
    }
    public void SetRandomRotaion(bool isDown)
    {
        float angleX = isDown ? Random.Range(-MaxRotaionAngle, -MinRotaionAngle) : Random.Range(MinRotaionAngle, MaxRotaionAngle);//向下
        Quaternion targetRotation = Quaternion.Euler(angleX, 0f, 0f);
        transform.rotation = targetRotation; // 直接设置旋转角度 
    }

    public void ResetRotaion()
    {
        transform.rotation = Quaternion.identity;
    }

    public float GetOffsetForwardCos(float offset)
    {
        return GameUtil.CosEulerAngle(offset, transform.rotation.eulerAngles.x);
    }

    public float GetOffsetForwardSin(float offset)
    {
        return GameUtil.SinEulerAngle(offset, transform.rotation.eulerAngles.x) ;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.Tag_Player))
        {
            isInPlatform = true;
            //Debug.Log("OnTriggerEnter:" + gameObject.name);
        }
       
    }

    void OnTriggerExit(Collider other)
    {
        if (isWaitMoveForward) return;

        if (other.CompareTag(Constants.Tag_Player))
        {
            isInPlatform = false;
            //Debug.Log("OnTriggerExit:" + gameObject.name);
            float of2cos = this.GetOffsetForwardCos(PlatformManager.Instance.HalfLengthPlatform);
            if (of2cos+transform.position.z < GameManager.Instance.playerTf.position.z)
            {
                platformManager.ExitPlatform(this);
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.tag == Constants.Tag_Player)
        //{
        //Debug.Log("-------------OnCollisionEnter:" + collision.gameObject.name);
        //}
    }
    private void OnCollisionExit(Collision collision)
    {
        //if (collision.gameObject.tag == Constants.Tag_Player)
        //{
            //Debug.Log("-------------OnCollisionExit:" + collision.gameObject.name);
            //platformManager.MoveForwardPlatform(this);
        //}
    }
}
