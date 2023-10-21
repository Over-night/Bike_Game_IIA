using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Chat;
using Photon.Realtime;
using PhotonPlayer = Photon.Realtime.Player;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using TMPro;
using System;

public class RoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField] TextMeshProUGUI Title;
    [SerializeField] GameObject[] PlayerSlot; // 플레이어 칸
    [SerializeField] Button StartButton;

    private string roomUUID;            // 해당 방의 uuid
    private int defaultMaxPlayer = 12;  // 기본 최대 인원

    private string[] cond = { "Empty", "Disabled" };
    

    void Start()
    {
        RoomInitation();
        UpdateSlot();
    }

    private void RoomInitation()
    {
        Hashtable customProp = PhotonNetwork.CurrentRoom.CustomProperties;

        
        roomUUID = PhotonNetwork.CurrentRoom.Name;  // UUID 설정
        Title.SetText((string)customProp["title"]); // 제목 설정
        if(!PhotonNetwork.IsMasterClient)
        {
            StartButton.enabled = false;
        }

    }

    #region callback
    public override void OnPlayerEnteredRoom(PhotonPlayer player)
    {
        UpdateSlot();
    }

    public override void OnPlayerLeftRoom(PhotonPlayer player)
    {
        UpdateSlot();
    }

    #endregion
    #region func
    private void UpdateSlot()
    {
        int cnt = 0;

        Debug.Log(DateTime.Now.ToString("HHmmss") + 
            " || " + PhotonNetwork.CurrentRoom.Players.Count +
            " || " + PhotonNetwork.CurrentRoom.Players);

        // 닉네임
        for(int i=1; i<=PhotonNetwork.CurrentRoom.Players.Count; i++)
        {
            PlayerSlot[cnt++].GetComponent<PlayerSlot>().SlotPlayer(PhotonNetwork.CurrentRoom.Players[i].NickName);
        }
        for(int i=cnt; i< PhotonNetwork.CurrentRoom.MaxPlayers; i++)
        {
            PlayerSlot[cnt++].GetComponent<PlayerSlot>().SlotEmpty();
        }
        for (int i = PhotonNetwork.CurrentRoom.MaxPlayers; i < defaultMaxPlayer; i++)
        {
            PlayerSlot[cnt++].GetComponent<PlayerSlot>().SlotDisabled();
        }

    }
    #endregion
    #region OnClick
    public void OnClick_Exit()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("Lobby");
    }
    public void OnClick_Start()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        
        PhotonNetwork.LoadLevel("Game_Bike");
    }
    #endregion
}
