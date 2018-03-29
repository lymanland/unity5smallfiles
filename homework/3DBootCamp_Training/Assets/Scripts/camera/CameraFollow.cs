using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public float speed = 5f;
    private Vector3 offset = new Vector3(0, 4, -10);//相机相对于玩家的位置
    public Transform player;
    private Transform cameraTf;

    // Use this for initialization
    void Start () {
        cameraTf = transform;
        offset = player.position - cameraTf.position;

    }
	
	// Update is called once per frame
	void Update () {
        Vector3 pos = player.position - offset;
        pos.Set(pos.x, pos.y, pos.z);
        //pos.Set(cameraTf.position.x, cameraTf.position.y, pos.z);//固定上下左右
        cameraTf.position = Vector3.Lerp(cameraTf.position, pos, speed * Time.deltaTime);//调整相机与玩家之间的距离
    }
}
