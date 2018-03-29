using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor; // 这个文件在手机上没有，需要使用条件编译
#endif

public class PlatformManager : MonoBehaviour {
    public GameObject dollarPrefab;
    public GameObject barrierPrefab;

    public PlatformView p1View;
    public PlatformView p2View;
    private float offsetBetweenPlatform;
    private float halfLengthPlatform;
    public float HalfLengthPlatform
    {
        get { return halfLengthPlatform; }
    }


    private readonly float[] ArrHeights4Dollar = new float[] { 0.6f, 1.6f, 2.6f };
    private List<GameObject> dollarList;

    private readonly float Rate4GenBarrier = 18f;
    private readonly float[] ArrHeights4Barrier = new float[] { 0.26f, 1.52f };
    private List<GameObject> barrierList;
    public BarrierView hitBarrier;

    private readonly float GenOffset4RoadEnd = 2.5f;//两端边缘不添加物体
    private readonly float GenOffset4RoadSide = 1.5f;
    private readonly float[] GenOffsets4RoadForward = new float[] { 4f, 9f };

    public bool IsOnGround
    {
        get
        {
            return p1View.groundView.isOnGround || p2View.groundView.isOnGround;
        }
    }

    public PlatformView CurPlatformView4OnGround
    {
        get
        {
            return p1View.groundView.isOnGround ? p1View : p2View;
        }
    }

    public PlatformView CurPlatformView4InPlatform
    {
        get
        {
            return p1View.isInPlatform ? p1View : p2View;
        }
    }

    

    private static PlatformManager instance;
    public static PlatformManager Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        instance = this;

        Debug.Log("Awake:" + this);
    }

    // Use this for initialization
    void Start () {
        dollarList = new List<GameObject>();
        barrierList = new List<GameObject>();

        offsetBetweenPlatform = p2View.transform.position.z - p1View.transform.position.z;
        Debug.Log("平台间距:"+ offsetBetweenPlatform);
        halfLengthPlatform = offsetBetweenPlatform / 2;

        InitPlatform(p1View);
        InitPlatform(p2View);

        //////////////////////测试代码 ---- 测试旋转拼接 ////////////////

        //float angleX = -15f;//向上
        //Quaternion targetRotation = Quaternion.Euler(angleX, 0f, 0f);
        ////p1View.transform.rotation = targetRotation; // 直接设置旋转角度 

        //angleX = 10f;//向下
        //targetRotation = Quaternion.Euler(angleX, 0f, 0f);
        //p2View.transform.rotation = targetRotation; // 直接设置旋转角度 

        ////PlatformView preView = GetOtherPlatformView();
        //float of1cos = p1View.GetOffsetForwardCos(offsetForward/2);
        //float of1sin = p1View.GetOffsetForwardSin(offsetForward/2);
        //float of2cos = p2View.GetOffsetForwardCos(offsetForward/2);
        //float of2sin = p2View.GetOffsetForwardSin(offsetForward/2);

        //Debug.Log("of1cos:" + of1cos + ", of1sin:" + of1sin + ", of2cos:" + of2cos + ", of2sin:" + of2sin);
        //p2.transform.position = p1.transform.position + new Vector3(0, 0-(of1sin+ of2sin), of1cos + of2cos);
    }

    public PlatformView GetOtherPlatformView(PlatformView view)
    {
        return view == p1View ? p2View : p1View;
    }

    public void ExitPlatform(PlatformView view)
    {
        if (GameManager.Instance.GameState == Constants.GameState.Dead)
        {
            return;
        }
        PlatformView otherView = PlatformManager.Instance.GetOtherPlatformView(view);
        if (!otherView.isInPlatform)
        {
            GameManager.Instance.DeathHandle();
            return;
        }

        MoveForwardPlatform(view);

    }
    private void MoveForwardPlatform(PlatformView view)
    {
        view.isWaitMoveForward = true;
        StartCoroutine(MoveForwardPlatformHandle(view));//延迟消失，不然会看到人物后面底板突然消失
    }

    private int rotationedCount = 0;
    IEnumerator MoveForwardPlatformHandle(PlatformView view)
    {
        yield return new WaitForSeconds(1.5f);

        view.isWaitMoveForward = false;
        //if (GameManager.Instance.GameState != Constants.GameState.Dead)
        //{
            view.ResetRotaion();
            InitPlatform(view);
            //view.transform.position += Vector3.forward * offsetForward * 2;

            rotationedCount++;
            bool isDown = rotationedCount % 2 == 0;//上下交替
            view.SetRandomRotaion(isDown);
            //view.SetRandomRotaion();//随机上下

            view.MoveForwardPostion(halfLengthPlatform);
        //}
    }

    void InitPlatform(PlatformView view)
    {
        float maxPosZ = view.transform.position.z + halfLengthPlatform - GenOffset4RoadEnd;
        float startPozZ = view.transform.position.z - halfLengthPlatform + GenOffset4RoadEnd;

        Vector3 pos = new Vector3(0, 0, startPozZ);
        while (pos.z < maxPosZ)
        {
            pos.x = Random.Range(-GenOffset4RoadSide, GenOffset4RoadSide);
            if (GameUtil.GetRandomRate() < Rate4GenBarrier)
            {
                GenBarrier(pos, view);
            }
            else
            {
                GenDollar(pos, view);
            }
            pos.z += Random.Range(GenOffsets4RoadForward[0], GenOffsets4RoadForward[1]);
        }
    }

    void GenDollar(Vector3 pos, PlatformView platformView)
    {
        int ran = Random.Range(0, ArrHeights4Dollar.Length);
        pos.y = ArrHeights4Dollar[ran];

        GameObject dollar = platformView.AddGameObject2Platform(dollarPrefab, pos);
        dollarList.Add(dollar);

        dollar.AddComponent<DolloarView>();
    }

    void GenBarrier(Vector3 pos, PlatformView platformView)
    {
        pos.x = 0;//障碍物不用左右偏移
        int ran = Random.Range(0, ArrHeights4Barrier.Length);
        pos.y = ArrHeights4Barrier[ran];

        GameObject barrier = platformView.AddGameObject2Platform(barrierPrefab, pos);
        barrierList.Add(barrier);

        BarrierView barrierView = barrier.GetComponent<BarrierView>();
        bool isUpPos = pos.y == ArrHeights4Barrier[ArrHeights4Barrier.Length - 1];
        barrierView.isUpPos = isUpPos;
    }

    public void AddScore(DolloarView view)
    {
        GameManager.Instance.AddScore(view);

        int findIndex = dollarList.IndexOf(view.gameObject);
        if (findIndex>=0)
        {
            dollarList.RemoveAt(findIndex);
            Destroy(view.gameObject);
        }
      
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.GameState == Constants.GameState.Dead)
        {
            return;
        }

        int di = 0;//每次只检测最前面一个
        //for (int di = dollarList.Count-1; di >=0; di--)
        //{
        if (di < dollarList.Count)
        {
            GameObject doolarView = dollarList[di];
            if (doolarView.transform.position.z < GameManager.Instance.cameraTf.position.z)
            {
                dollarList.RemoveAt(di);
                Destroy(doolarView);
            }
        }

        if (di < barrierList.Count)
        {
            GameObject barrierView = barrierList[di];
            if (barrierView.transform.position.z < GameManager.Instance.cameraTf.position.z)
            {
                barrierList.RemoveAt(di);
                Destroy(barrierView);
            }
        }
        //}
    }
}
