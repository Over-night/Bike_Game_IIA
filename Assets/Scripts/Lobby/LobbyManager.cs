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
    [SerializeField] TextMeshProUGUI Nickname;      // �г���
    [SerializeField] GameObject GameRoom;           // ���� �� ������
    [SerializeField] GameObject Panel_CreateRoom;   // �� ���� �г�
    [SerializeField] GameObject ScrollviewContent;
    [SerializeField] TMP_InputField RoomTitle;
    [SerializeField] TMP_InputField RoomPassword;

    private GameObject[] BaseButtonList;            // ��ư Ȱ��ȭ ��� ���� 
    private List<RoomInfo> roomInLobby;             // �� ����Ʈ ����
    private Transform[] contentChildObject;         // �� ����Ʈ ����
    private int defaultMaxPlayer = 12;              // �⺻ �ִ� �ο�

    #region Start
    void Start()
    {
        // ���� ������ ������ ���� �г��� ��ư ��Ȱ��ȭ
        BaseButtonList = GameObject.FindGameObjectsWithTag("BaseButton");

        Nickname.text = "Hello, " + PhotonNetwork.NickName;     // �г��� ǥ��
        
        OnClick_RefreshRoom();                  // �� ����Ʈ ����
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

        // ������ ��� ��� ����
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
            GameObject roomObject = Instantiate(GameRoom, new Vector3(0, 0, 0), Quaternion.identity);   // ��ü ����
            roomObject.transform.SetParent(ScrollviewContent.transform, false);                         // ������ ��ü�� �θ� ����

            RoomInLobbyFunction roomSet = roomObject.GetComponent<RoomInLobbyFunction>();                             // �ҽ� ����
         
            roomSet.RoomSettingInit(roomInLobby[i]);

        }
    }
    public void OnClick_CreateRoom()
    {
        // �� ���� ������ Ȱ��ȭ
        for (int i = 0; i < BaseButtonList.Length; i++) 
            BaseButtonList[i].GetComponent<Button>().interactable = false;
        Panel_CreateRoom.SetActive(true);
    }
    public void OnClick_CreateRoom_Cancel()
    {
        // �� ���� ������ ��Ȱ��ȭ
        for (int i = 0; i < BaseButtonList.Length; i++)
            BaseButtonList[i].GetComponent<Button>().interactable = true;
        Panel_CreateRoom.SetActive(false);
    }
    public void OnClick_CreateRoom_Create()
    {
        // �� ���� ����
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 12;
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        roomOptions.CustomRoomProperties = new Hashtable() { 
            { "title", RoomTitle.text},                                                          // ���
            { "isProtected", String.IsNullOrEmpty(RoomPassword.text) ? "F" : "T" },             // ��й�ȣ ����
            { "pw", String.IsNullOrEmpty(RoomPassword.text) ? "" : RoomPassword.text},    // ��й�ȣ
        };
        roomOptions.CustomRoomPropertiesForLobby = System.Array.ConvertAll(
            roomOptions.CustomRoomProperties.Keys.ToArray(), 
            x=>x.ToString()
        );
        string code = Guid.NewGuid().ToString();

        PhotonNetwork.CreateRoom(code, roomOptions, null); // �� ����
    }
    
    public void Onclick_JoinRoom(string uuid)
    {
        PhotonNetwork.JoinRoom(uuid);
    }

    #endregion
    #region check
    #endregion

}
