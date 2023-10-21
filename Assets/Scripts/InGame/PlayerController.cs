using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Video;
using Photon.Pun;
using Photon.Realtime;
using static UnityEngine.Mathf;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float crossSpeed;
    [SerializeField] VideoPlayer Video;
    [SerializeField] float defaultMaxSpeed;      // �ִ� �ӵ�
    [SerializeField] float defaultAccel;        // ���ӵ�
    [SerializeField] float defaultFriction;        // ����
    [SerializeField] float boostSpeed;
    [SerializeField] float boostAccel;
    [SerializeField] float boostFriction;
    [SerializeField] int pedalTerm = 15;

    private float addMaxSpeed;      // �߰� �ӵ�
    private float addAccel;         // �߰� ���ӵ�
    private float addFriction;      // �߰� ����
    private float playerSpeed;      // ���� �ӵ�
    private float timerFriction;    // ���� ���� Ÿ�̸�
    private float haveItem;         // ������ ����

    // �÷��̾� �� ����ȭ ����
    // private UIUpdator uiUpdator;
    private UIUpdator uiUpdator;
    private PoseEstimator poseEstimator;
    private MultiplayObject myMultiplayObject;

    private List<bool> myActType;

    private float y = 0.0f;
    private float gravity = 0.0f;
    private int direction = 0; // 0:��������, 1:������, 2:�ٿ���

    private int pedalValue = 0;

    const float jump_speed = 0.04f; // �����ӵ�
    const float jump_accell = 0.0014f; // ��������
    const float y_base = -4;

    private bool isCoroutine_ActDisturb = false;

    void Start()
    {
        // uiUpdator = GameObject.FindGameObjectWithTag("GameManager").GetComponent<UIUpdator>();
        uiUpdator = GameObject.FindGameObjectWithTag("GameManager").GetComponent<UIUpdator>();
        poseEstimator = GameObject.FindGameObjectWithTag("GameManager").GetComponent<PoseEstimator>();

        y = y_base;

        //GameObject[] multiObjectList = GameObject.FindGameObjectsWithTag("VarContainer");
        //for (int i = 0; i < multiObjectList.Length; i++)
        //{
        //    if(multiObjectList[i].GetComponent<MultiplayObject>().GetIsMaster())
        //    {
        //        myMultiplayObject = multiObjectList[i].GetComponent<MultiplayObject>();
        //        break;
        //    }
        //}

        StartCoroutine("UpdateActType");
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        JumpProcess();
        MoveProcess();
        ItemProcess();
        if (Input.GetKeyUp(KeyCode.Space))
        {
            DoJump();
        }
        Vector3 pos = transform.position;
        pos.y = y;
        transform.position = pos;
    }

    private void DoJump()
    {
        direction = 1;
        gravity = jump_speed;
    }
    private void JumpProcess()
    {
        switch (direction)
        {
            case 0:
                {
                    if (y > y_base)
                    {
                        if (y >= jump_accell)
                        {
                            y -= gravity;
                        }
                        else
                        {
                            y = y_base;
                        }
                    }
                    break;
                }
            case 1:
                {
                    y += gravity;
                    if (gravity <= 0.0f)
                    {
                        direction = 2;
                    }
                    else
                    {
                        gravity -= jump_accell;
                    }
                    break;
                }
            case 2:
                {
                    y -= gravity;
                    if (y > y_base)
                    {
                        gravity += jump_accell;
                    }
                    else
                    {
                        direction = 0;
                        y = y_base;
                    }
                    break;
                }
        }
    }
    private void MoveProcess()
    {
        // Video.time : �ð���ȯ | Video.length : ��ü �ð�
        Video.playbackSpeed = playerSpeed;

        if (Input.GetKey(KeyCode.Tab)) pedalValue = pedalTerm;

        if (pedalValue-- > 0)
        {
            playerSpeed += SumValue(defaultAccel, addAccel) * Time.deltaTime;

            // �÷��̾� �ӵ� ����
            float limitSpeed = SumValue(defaultMaxSpeed, addMaxSpeed);
            playerSpeed = LimitValueUpper(playerSpeed, limitSpeed); 

            Video.playbackSpeed = playerSpeed;
        }

        // Debug.Log("Term : " + pedalValue + "\n" + Video.playbackSpeed + " | " + playerSpeed);

        timerFriction += Time.deltaTime;
        if (timerFriction >= 1)
        {
            playerSpeed -= SumValue(defaultFriction, addFriction);
            playerSpeed = LimitValueLower(playerSpeed, 0);
            timerFriction = 0;
        }
    }
    public void PoseProcess(int getActType)
    {
        // Act���̸� ������ Act ���� ��
        if (myActType[0] && myActType[getActType])
        {
            uiUpdator.CallCoroutineSuccess("Success!");
            ItemCoroutineBoost();
        }
        // �ϴ��� ��ġ�� ��� �ν��ͷ� ���� 
    }

    private void Move()
    {
        // float moveDirX = Input.GetAxisRaw("Horizontal");
        float moveDirX = myActType[9] ? -1 : (myActType[10] ? 1 : 0);
        float moveDirY = Input.GetAxisRaw("Vertical");
        var curPos = transform.position;

        curPos += new Vector3(moveDirX, moveDirY, 0) * crossSpeed * Time.deltaTime;
        curPos.x = Clamp(curPos.x, -1.5f, 1.5f);
        curPos.y = Clamp(curPos.y, -4, -3);
        transform.position = curPos;
    }

    #region Item
    private void ItemProcess()
    {
        if (myActType[8])
        {
            if (haveItem == 0) return;

            switch (haveItem)
            {
                case 1:
                    ItemCoroutineBoost();
                    break;
                case 2:
                    ItemCoroutineBreak();
                    break;
                case 3:
                    ItemCoroutineActDisturb();
                    break;
                default:
                    break;
            }
            UseItem();
        }
    }
    public void UseItem()
    {
        uiUpdator.ItemImageUpdate(0);
        haveItem = 0;
    }
    #endregion

    #region Getter
    public List<bool> GetActType()
    {
        return myActType;
    }
    public void GetItem(int code)
    {
        uiUpdator.ItemImageUpdate(code);
        haveItem = code;
    }
    public float GetCrossLocation()
    {
        return transform.position.x;
    }
    #endregion

    #region Trigger
    public void TriggerSetAddSpeed(float speed)
    {
        addMaxSpeed = speed;
    }
    public void TriggerSetAddAccel(float speed)
    {
        addAccel = speed;
    }
    public void TriggerSetAddFriction(float speed)
    {
        addFriction = speed;
    }
    #endregion

    #region Calculator
    private float SumValue(float valA, float valB)
    {
        // �ִ�ӵ�, ���ӵ�, ���� ����
        return valA + valB;
    }
    private float LimitValueUpper(float value, float limit)
    {
        // �� | �Ѱ谪
        return value > limit ? limit : value;
    }
    private float LimitValueLower(float value, float limit)
    {
        // �� | �Ѱ谪
        return value < limit ? limit : value;
    }

    #endregion

    #region ItemCoroutineSet
    private void ItemCoroutineBoost()
    {
        // 5�ʵ��� �ν�Ʈ
        StopCoroutine("ItemUse_Boost");
        StartCoroutine("ItemUse_Boost");
    }
    private void ItemCoroutineBreak()
    {
        // 10�ʵ��� ���ӵ����� �� ��������
        StopCoroutine("ItemUse_Break");
        StartCoroutine("ItemUse_Break");
    }

    private void ItemCoroutineActDisturb()
    { 
        StopCoroutine("ItemUse_ActDisturb");
        StartCoroutine("ItemUse_ActDisturb");
    }

    public void ItemCoroutineActDisturbAffect()
    {
        StopCoroutine("ItemAffect_ActDisturb");
        StartCoroutine("ItemAffect_ActDisturb");
    }

    private void ItemCoroutineFalldown()
    {
        // 5�ʵ��� �극��ũ
        StopCoroutine("Effect_Falldown");
        StartCoroutine("Effect_Falldown");
    }
    #endregion

    #region Coroutine
    IEnumerator UpdateActType()
    {
        while (true)
        {
            myActType = poseEstimator.GetMotionType();
            yield return null;
        }
    }
    IEnumerator ItemUse_Boost()
    {
        float maxSpeed = boostSpeed;
        float decreaseSpeed = boostSpeed / 20;


        gameObject.GetComponent<PlayerController>().TriggerSetAddSpeed(maxSpeed);
        yield return new WaitForSeconds(5.0f);
        
        while (maxSpeed > 0)
        {
            gameObject.GetComponent<PlayerController>().TriggerSetAddSpeed(maxSpeed);
            maxSpeed -= decreaseSpeed;
            yield return null;
        }

        gameObject.GetComponent<PlayerController>().TriggerSetAddSpeed(0);
    }
    IEnumerator ItemUse_Break()
    {
        // 
        gameObject.GetComponent<PlayerController>().TriggerSetAddAccel(boostAccel);
        gameObject.GetComponent<PlayerController>().TriggerSetAddFriction(boostFriction);
        yield return new WaitForSeconds(10.0f);
        gameObject.GetComponent<PlayerController>().TriggerSetAddAccel(0);
        gameObject.GetComponent<PlayerController>().TriggerSetAddFriction(0);
    }
    IEnumerator ItemUse_ActDisturb()
    {
        //while (isCoroutine_ActDisturb)
        //{
        //    yield return null;
        //}
        //isCoroutine_ActDisturb = true;
        ////////////////////////////////////////////////////////
        //myMultiplayObject.SetIngameStatus(1);
        //yield return new WaitForSeconds(1.0f);
        //myMultiplayObject.SetIngameStatus(0);
        //yield return new WaitForSeconds(3.0f);
        ////////////////////////////////////////////////////////
        //isCoroutine_ActDisturb = false;
        yield return null;
    }
    IEnumerator ItemAffect_ActDisturb()
    {
        //while (isCoroutine_ActDisturb)
        //{
        //    yield return null;
        //}
        //isCoroutine_ActDisturb = true;
        ////////////////////////////////////////////////////////
        //int randPose = Random.Range(1, 10);      // ���� ���� ����
        //bool isPass = false;

        //float timer = 3f;
        //while (timer > 0)
        //{
        //    timer -= Time.deltaTime;
        //    if (myActType[randPose]) isPass = true;
        //    yield return null;
        //}

        //if(!isPass)
        //{
        //    StartCoroutine("Effect_Falldown");
        //}
        //yield return new WaitForSeconds(1.0f);
        ////////////////////////////////////////////////////////
        //isCoroutine_ActDisturb = false;
        yield return null;
    }
    IEnumerator Effect_Falldown()
    {
        //gameObject.GetComponent<PlayerController>().TriggerSetAddFriction(defaultFriction * 5);
        //yield return new WaitForSeconds(5.0f);
        //gameObject.GetComponent<PlayerController>().TriggerSetAddFriction(0);
        yield return null;
    }
    #endregion
}
