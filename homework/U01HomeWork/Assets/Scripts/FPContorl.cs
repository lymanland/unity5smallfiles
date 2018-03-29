using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPContorl : MonoBehaviour {
    //Windows->Lighting窗口中Scene页签打开
    public int speed = 5;

    private Transform cameraTf;
    private Vector3 offset = new Vector3(0, 4, -10);//相机相对于玩家的位置

    public Transform car;
    private Rigidbody rbCar;
    private Vector3 pos;

    void Awake()
    {
        cameraTf = transform;
        rbCar = car.GetComponent<Rigidbody>();
    }
    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        float H = Input.GetAxis("Horizontal");
        float V = Input.GetAxis("Vertical");
        //tf.Translate(H, 0, V);
        //car.Rotate(Vector3.up * H);

        //if (Input.GetKey(KeyCode.W))
        //{
        //    car.Translate(Vector3.forward * Time.deltaTime * speed);

        //}
        //else if (Input.GetKey(KeyCode.S))
        //{
        //    //后退
        //    car.Translate(Vector3.forward * Time.deltaTime * -speed);
        //}

        //if (Input.GetKey(KeyCode.A))
        //{
        //    car.Translate(Vector3.right * Time.deltaTime * -speed);

        //}
        //else if (Input.GetKey(KeyCode.D))
        //{
        //    //后右
        //    car.Translate(Vector3.right * Time.deltaTime * speed);
        //}

        Vector3 movement = new Vector3(H * speed, 0, V * speed);
        rbCar.velocity = movement;

        pos = car.position + offset;
        cameraTf.position = Vector3.Lerp(cameraTf.position, pos, speed * Time.deltaTime);//调整相机与玩家之间的距离
        //Quaternion angel = Quaternion.LookRotation(car.position - cameraTf.position);//获取旋转角度
        //cameraTf.rotation = Quaternion.Slerp(cameraTf.rotation, angel, speed * Time.deltaTime);
    }

}
