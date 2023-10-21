using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System.Linq;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] TextMeshProUGUI Nickname;      // 닉네임
    [SerializeField] GameObject GameRoom;           // 게임 방 프리셋
    [SerializeField] GameObject Panel_CreateRoom;   // 방 생성 패널
    [SerializeField] GameObject ScrollviewContent;
    [SerializeField] TMP_InputField RoomTitle;
    [SerializeField] TMP_InputField RoomPassword;

    private GameObject[] BaseButtonList;            // 버튼 활성화 토글 관련 
    private List<RoomInfo> roomInLobby;             // 방 리스트 저장
    private Transform[] contentChildObject;         // 방 리스트 관련
    private int defaultMaxPlayer = 12;              // 기본 최대 인원

    #region Start
    void Start()
    {
        // 개별 윈도우 생성시 기존 패널의 버튼 비활성화
        BaseButtonList = GameObject.FindGameObjectsWithTag("BaseButton");

        Nickname.text = "Hello, " + PhotonNetwork.NickName;     // 닉네임 표시
        
        OnClick_RefreshRoom();                  // 방 리스트 갱신
    }
    private void Update()
    {
        Debug.Log(PhotonNetwork.NetworkClientState.ToString());
    }


    #endregion

    #region Photon PUN2 Callback

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        roomInLobby = roomList;
        roomInLobby.RemoveAll(x => x.MaxPlayers == 0 || x.PlayerCount == 0);
    }
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Room");
    }
    #endregion

    #region Onclick
    public void OnClick_RefreshRoom()
    {
        if (!PhotonNetwork.InLobby) PhotonNetwork.JoinLobby();

        // 이전의 목록 모두 제거
        contentChildObject = ScrollviewContent.GetComponentsInChildren<Transform>();
        if(contentChildObject != null)
        {
            for(int i=1; i<contentChildObject.Length; i++)
            {
                if(contentChildObject[i] != transform)
                    Destroy(contentChildObject[i].gameObject);
            }
        }

        if (roomInLobby == null) return;
        Debug.Log("Total Room : " + roomInLobby.Count);

        for (int i=0; i< roomInLobby.Count; i++)
        {
            GameObject roomObject = Instantiate(GameRoom, new Vector3(0, 0, 0), Quaternion.identity);   // 객체 생성
            roomObject.transform.SetParent(ScrollviewContent.transform, false);                         // 생성한 객체의 부모 설정

            RoomInLobbyFunction roomSet = roomObject.GetComponent<RoomInLobbyFunction>();                             // 소스 지정
         
            roomSet.RoomSettingInit(roomInLobby[i]);

        }
    }
    public void OnClick_CreateRoom()
    {
        // 방 생성 윈도우 활성화
        for (int i = 0; i < BaseButtonList.Length; i++) 
            BaseButtonList[i].GetComponent<Button>().interactable = false;
        Panel_CreateRoom.SetActive(true);
    }
    public void OnClick_CreateRoom_Cancel()
    {
        // 방 생성 윈도우 비활성화
        for (int i = 0; i < BaseButtonList.Length; i++)
            BaseButtonList[i].GetComponent<Button>().interactable = true;
        Panel_CreateRoom.SetActive(false);
    }
    public void OnClick_CreateRoom_Create()
    {
        // 룸 설정 규정
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 12;
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        roomOptions.CustomRoomProperties = new Hashtable() { 
            { "title", RoomTitle.text},                                                          // 방명
            { "isProtected", String.IsNullOrEmpty(RoomPassword.text) ? "F" : "T" },             // 비밀번호 여부
            { "pw", String.IsNullOrEmpty(RoomPassword.text) ? "" : RoomPassword.text},    // 비밀번호
        };
        roomOptions.CustomRoomPropertiesForLobby = System.Array.ConvertAll(
            roomOptions.CustomRoomProperties.Keys.ToArray(), 
            x=>x.ToString()
        );
        string code = Guid.NewGuid().ToString();

        PhotonNetwork.CreateRoom(code, roomOptions, null); // 방 생성
    }
    
    public void Onclick_JoinRoom(string uuid)
    {
        PhotonNetwork.JoinRoom(uuid);
    }

    #endregion
    #region check
    #endregion

}
