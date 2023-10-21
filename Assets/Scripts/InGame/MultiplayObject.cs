using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;

public class MultiplayObject : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] GameObject Bike;

    private VideoPlayer Video;
    private NetworkManager NetworkManager;
    private PlayerController PlayerController;

    // 내 변수
    private string myName;
    private float myDistance;       // 대략적 거리
    private float myCrossLocate;    // x축 위치 

    //클론이 통신을 받는 변수
    private string targetName;
    private float targetDistance;
    private float targetCrossLocate;

    // private List<MeshRenderer> RenderComponent;
    private MeshRenderer[] RenderComponent;
    private SkinnedMeshRenderer[] SkinMeshComponent;
    private bool isRended = true;

    private void Start()
    {
        Video = GameObject.FindWithTag("Video").GetComponent<VideoPlayer>();
        PlayerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        NetworkManager = GameObject.FindWithTag("GameManager").GetComponent<NetworkManager>();
        myName = photonView.Owner.NickName;

        // 초반에 active 안된상태로 생성되는 현상 대응 
        gameObject.SetActive(true);

        if (photonView.IsMine)
        {
            // 오브젝트의 모든 Mesh Renderer 탐색
            RenderComponent = Bike.GetComponentsInChildren<MeshRenderer>();
            SkinMeshComponent = Bike.GetComponentsInChildren<SkinnedMeshRenderer>();
            SetObjectRender(false);
        }

        // TODO : 시작하는 타이밍 맞추기

    }


    private void FixedUpdate()
    {
        // 내 캐릭터일때
        if (photonView.IsMine)
        {
            myDistance = (float)Video.time;
            myCrossLocate = PlayerController.GetCrossLocation();
        }
        // 다른 캐릭터일때
        else
        {
            if (myName.Equals(targetName))
            {
                myDistance = targetDistance;
                myCrossLocate = targetCrossLocate;
            }

            // 플레이어 가시성 역할
            float distanceGap = myDistance - NetworkManager.GetMasterDistance();
            Debug.Log(myDistance + " | " + distanceGap);


            if (-1f <= distanceGap && distanceGap <= 20f)
            {
                SetObjectRender(true);
                // SetObjectAlpha(distanceGap <= 10 ? 1 : 2f - distanceGap * 0.1f);
                float targetX = (myCrossLocate * ((20f - distanceGap) / 20f)) - gameObject.transform.position.x;
                float targetZ = (15 * distanceGap + 12) - gameObject.transform.position.z;
                // y : to 4
                // gameObject.transform.Translate(new Vector3(targetX, gameObject.transform.position.y, targetZ) * Time.deltaTime * 10); // 12 ~ 312
                gameObject.transform.Translate(new Vector3(targetX, 0, targetZ) * Time.deltaTime * 10); // 12 ~ 312
            }
            else
            {
                SetObjectRender(false);
            }
        }

        NetworkManager.SetDistance(myName, myDistance);
    }

    public bool GetIsMaster()
    {
        return photonView.IsMine;
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //통신을 보내는 
        if (stream.IsWriting)
        {
            stream.SendNext(myName);
            stream.SendNext(myDistance);
            stream.SendNext(myCrossLocate);
            //stream.SendNext(myIngameStatus);
        }
        /* myIngameStatus
         */

        //클론이 통신을 받는 
        else
        {
            targetName = (string)stream.ReceiveNext();
            targetDistance = (float)stream.ReceiveNext();
            targetCrossLocate = (float)stream.ReceiveNext();
            //targetIngameStatus = (int)stream.ReceiveNext();
        }
    }

    private void SetObjectRender(bool flag)
    {
        if (isRended == flag) return;
        isRended = flag;

        foreach (SkinnedMeshRenderer mesh in SkinMeshComponent)
        {
            mesh.enabled = flag;
        }
        foreach (MeshRenderer mesh in RenderComponent)
        {
            mesh.enabled = flag;
        }
    }
    private void SetObjectAlpha(float a)
    {
        foreach (SkinnedMeshRenderer mesh in SkinMeshComponent)
        {
            Color data = mesh.material.color;
            Color addAlpha = new Color(data.r, data.g, data.b, a);
            mesh.material.color = addAlpha;
        }
        foreach (MeshRenderer mesh in RenderComponent)
        {
            Color data = mesh.material.color;
            Color addAlpha = new Color(data.r, data.g, data.b, a);
            mesh.material.color = addAlpha;
        }
    }
}