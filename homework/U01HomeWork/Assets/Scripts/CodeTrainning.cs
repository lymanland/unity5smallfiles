using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeTrainning : MonoBehaviour {
    public float speed4Cube = 2f;
    public float speed4Sphere = 2f;
    public float speed4Cylinder = 2f;
    private readonly float TimeSec4HideQuad = 5f;

    private Transform myCube;
    private Transform mySphere;
    private Transform myCylinder;
    private GameObject myQuad;

	void Awake () {
		//Transform root = Transform.Find("Root");
		//Debug.Log(root);

		myCube = transform.Find("3DObjects/MyCube");//脚本本身依附Root，错误方式Find("Root/3DObjects/MyCube");
		//Debug.Log(myCube);
		mySphere = transform.Find("3DObjects/MySphere");
		myCylinder = transform.Find("3DObjects/MyCylinder");
		myQuad = transform.Find("MyQuad").gameObject;

		TraverseTarget(transform);
	}

	void TraverseTarget(Transform tfTarget, string prefix = "") {
		PrintTransformName(tfTarget, prefix);

		//Debug.Log(tfTarget.name + " childCount:"+tfTarget.childCount);
		for (int j = 0; j < tfTarget.childCount;j++ )
           {
               Transform tfChild = tfTarget.GetChild(j); //死循环transform.GetChild(j); unity崩溃
				TraverseTarget(tfChild, prefix+"#");
           }
	}

	void PrintTransformName(Transform tf, string prefix) {
		 //Debug.Log(prefix + tf.name);
		 print(prefix + tf.name);
	}

 	void Start()
    {
      	StartCoroutine(DoHideQuad());//开启协程  //错误方式直接调用DoHideQuad()；
		//Destroy(myQuad, 5);
    }

	void Update() {
		myCube.Translate(speed4Cube * Time.deltaTime, 0, 0);//每帧????朝Z轴方向移动 2单位----太快 所以*Time.deltaTime
		mySphere.Translate(0, speed4Sphere * Time.deltaTime, 0);
		myCylinder.Translate(0, 0, speed4Cylinder * Time.deltaTime);
	}

	IEnumerator DoHideQuad() {
		yield return new WaitForSeconds(TimeSec4HideQuad);
		myQuad.SetActive(false);//场景中停用该物体 

		//Destroy(myQuad);

		//Renderer rend = myQuad.GetComponent<Renderer>();
		//rend.enabled = false;//而物体实际还是存在的 只是想当于隐身 而物体本身的碰撞体还依然存在的  
		//
		//myQuad.renderer.enabled = false;//错误写法 
	}
}
