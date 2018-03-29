using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float gravity = 0.98f;
    public float speedRun = 8.0f;
    public float speedHorizontal = 5.0f;
    public float speedJump = 8.0f;
    private float curSpeedJumpUp;

    private Transform playerTf;
    private Rigidbody playerRb;
    private CapsuleCollider capsuleCollider;
    private readonly float RoadWith = 2f;
    private float defaultCapsuleColliderCenterY = 1.13f;
    private float Offset4SlideCollider = 0.3f;
    private AudioSource audio_source;
    private readonly float Offset4BackWhenSlide = 0.3f;
    private readonly float speed4BackWhenSlide = 1.6f;
    private Vector3 posBeforeSlide;

    private float bodyInitHeght;
    private Transform body;

    private Animator m_anim;

    void Awake()
    {
        Debug.Log("Awake:" + this);
    }
    // Use this for initialization
    void Start () {
        playerTf = transform;
        playerRb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        //defaultCapsuleColliderCenterY = capsuleCollider.center.y;
        body = transform.Find("polySurface1");
        bodyInitHeght = body.transform.position.y;

        m_anim = GetComponent<Animator>();
        m_anim.SetTrigger(Constants.Animation_Start);

        audio_source = GetComponent<AudioSource>();
        audio_source.loop = true;

        PlayLoopAudio();
    }
	
	// Update is called once per frame
	void Update () {
        //playerTf.position += new Vector3(0,0,1)*speed*Time.deltaTime;

        //Input.GetAxisRaw 按键一次会播放两次动画 多次SetTrigger
        
    }
   
    public void Relife()
    {
        playerTf.position = new Vector3(0, deathPos.y, deathPos.z);

        m_anim.SetBool(Constants.Animation_Grounded, true);
        m_anim.SetTrigger(Constants.Animation_Start);
        PlayLoopAudio();
        capsuleCollider.enabled = true;
    }

    Vector3 deathPos;
    public void Death()
    {
        m_anim.SetBool(Constants.Animation_Grounded, false);
        deathPos = playerTf.position;
        PlayDeathAudio();

        float h = Input.GetAxisRaw(Constants.Input_Horizontal);
        playerRb.MovePosition(deathPos + Vector3.right*h);
        capsuleCollider.enabled = false;
    }

    void FixedUpdate()
    {
        if (GameManager.Instance.GameState == Constants.GameState.Dead)
        {
            return;
        }

        //if (!IsAnimationSliding())
        //{
        //    posBeforeSlide = playerRb.transform.position;
        //}
        //else
        //{
        //    posBeforeSlide = playerRb.transform.position + Vector3.down * Offset4SlideCollider;
        //}

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            //if (PlatformManager.Instance.IsOnGround)
            //{
                if (curSpeedJumpUp<= 0 && !IsAnimationJump())
                {
                    m_anim.SetTrigger(Constants.Animation_Jump);
                    //playerRb.AddForce(Vector3.up * speedJump);
                    curSpeedJumpUp = speedJump;
                }
            //}
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (!IsAnimationSliding())
            {
                m_anim.SetTrigger(Constants.Animation_Slide);
            }
        }

        float h = Input.GetAxisRaw(Constants.Input_Horizontal);
        float v = Input.GetAxis(Constants.Input_Vertical);
        float vRawv = Input.GetAxisRaw(Constants.Input_Vertical);
        //if (IsSliding())
        //{
        //    cpCollider.enabled = false;//会掉落地板
        //}
        //else
        //{
        //    cpCollider.enabled = true;
        //}

        //方式一  Player的RigidBodys属性 Is Kinematic = false
        Vector3 movement = new Vector3(h*speedHorizontal, curSpeedJumpUp, speedRun) * Time.deltaTime;
        curSpeedJumpUp -= gravity * Time.deltaTime;
        curSpeedJumpUp = Mathf.Max(0, curSpeedJumpUp);

        ////Player的RigidBodys属性  Is Kinematic = true
        //if (playerRb.isKinematic)//方式二
        //{
        //    PlatformView platformView = PlatformManager.Instance.CurPlatformView4InPlatform;
        //    float runSpeed_cos = platformView.GetOffsetForwardCos(speedRun);
        //    float runSpeed_sin = platformView.GetOffsetForwardSin(speedRun);
        //    //Debug.Log("runSpeed_cos:" + runSpeed_cos + ", runSpeed_sin:" + runSpeed_sin);
        //    movement.z = runSpeed_cos;
        //    movement.y -= runSpeed_sin;
        //    if (!PlatformManager.Instance.IsOnGround)
        //    {
        //        movement.y -= gravity;
        //    }
        //    movement = movement * Time.deltaTime;
        //}

        playerRb.useGravity = true;
        bool canMoveForward = true;
        bool needMoveBack = false;
        if (PlatformManager.Instance.hitBarrier != null)
        {
            canMoveForward = false;
            if (!PlatformManager.Instance.hitBarrier.isUpPos)
            {
                if (IsAnimationJump())
                {
                    //if ((playerTf.position.y + movement.y > 0.10f))
                    //{
                        canMoveForward = true;
                    //}
                }
                else if (IsAnimationSliding())
                {
                    needMoveBack = true;
                }
            }
            if (PlatformManager.Instance.hitBarrier.isUpPos)
            {
                if (IsAnimationSliding())
                {
                    canMoveForward = true;
                }
                //else if (IsAnimationJump())
                //{
                //    movement = Vector3.zero;
                //}
            }
        }
        if (!canMoveForward)
        {
            //movement.z -= Offset4BackWhenSlide * Time.deltaTime;
            movement.z = 0;
        }
        if (needMoveBack)
        {
            movement.z -= speed4BackWhenSlide * Time.deltaTime;
        }

        //Debug.Log("bodyInitHeght:"+ bodyInitHeght + ", body.transform.position.y:" + body.transform.position.y);
        //movement.y += (bodyInitHeght-body.transform.position.y)*Time.deltaTime;

        //float offsetBody = bodyInitHeght - body.transform.position.y;
        //if (IsAnimationJump())
        //{
        //    capsuleCollider.center = new Vector3(capsuleCollider.center.x, defaultCapsuleColliderCenterY - offsetBody, capsuleCollider.center.z);
        //}
        //else 
        if (IsAnimationSliding())
        {
            //movement.y += Offset4SlideCollider * Time.deltaTime;
            //movement.z没有变; 但是会改变capsuleCollider 慢慢磨动往前
            capsuleCollider.center = new Vector3(capsuleCollider.center.x, defaultCapsuleColliderCenterY - Offset4SlideCollider, capsuleCollider.center.z);
        }
        else
        {
            capsuleCollider.center = new Vector3(capsuleCollider.center.x, defaultCapsuleColliderCenterY, capsuleCollider.center.z);
        }


        movement = playerTf.position + movement;

        //if (IsAnimationSliding())
        //{
        //    movement.y = posBeforeSlide.y + Offset4SlideCollider;
        //    //playerRb.useGravity = false;
        //}

        if (Mathf.Abs(movement.x) >= RoadWith)
        {
            GameManager.Instance.DeathHandle();
            return;
        }

        playerRb.MovePosition(movement);
    }

    public bool IsAnimationStart()
    {
        return GameUtil.CheckAnimatorState(m_anim, Constants.Animation_Start);
    }
    public bool IsAnimationSliding()
    {
        return GameUtil.CheckAnimatorState(m_anim, Constants.Animation_Slide);
    }
    public bool IsAnimationJump()
    {
        return GameUtil.CheckAnimatorState(m_anim, Constants.Animation_Jump);
    }

    void PlayLoopAudio()
    {
        audio_source.loop = true;
        GameUtil.PlayAudio(audio_source, Constants.AudioFile_Loop);
    }

    void PlayDeathAudio()
    {
        audio_source.loop = false;
        GameUtil.PlayAudio(audio_source, Constants.AudioFile_Death);
    }
    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("playerMove-------------OnCollisionEnter:" + collision.gameObject.name);
    }
    private void OnCollisionExit(Collision collision)
    {
        //Debug.Log("playerMove-------------OnCollisionExit:" + collision.gameObject.name);//Platform
    }
}
