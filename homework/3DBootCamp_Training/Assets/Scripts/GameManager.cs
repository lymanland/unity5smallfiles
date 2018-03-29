using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor; // 这个文件在手机上没有，需要使用条件编译
#endif

public class GameManager : MonoBehaviour {
    public GameObject burstPrefab;

    public GameUi gameUI;

    private Constants.GameState gameState = Constants.GameState.Playing;
    public Constants.GameState GameState
    {
        get { return gameState; }
    }
    public PlayerController playerController;
    public Transform cameraTf;
    public Transform playerTf;

    private static GameManager instance;
    public static GameManager Instance
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

    void Start()
    {
        cameraTf = GameObject.FindGameObjectWithTag(Constants.Tag_MainCamera).transform;
    }

    public void AddScore(DolloarView view)
    {
        gameUI.AddScore();

        GameObject burst = Instantiate(burstPrefab, view.transform.position, Quaternion.identity);
        Destroy(burst, Constants.TimeSec_CoinBurst_Destory);
    }

    public void DeathHandle()
    {
        Debug.Log("player death");
        gameState = Constants.GameState.Dead;
        playerController.Death();
      

        StartCoroutine(RelifeHandle());
    }

    IEnumerator RelifeHandle()
    {
        yield return new WaitForSeconds(Constants.TimeSec_Reflife);

        Debug.Log("player relife");
        gameState = Constants.GameState.Playing;
        playerController.Relife();
        
    }
}
