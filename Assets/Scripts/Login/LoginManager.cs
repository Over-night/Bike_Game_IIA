using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class LoginManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField NamePlate;

    private string nickname;

    public void OnClick_Login()
    {
        nickname = NamePlate.text;

        if (string.IsNullOrEmpty(nickname))
        {
            Debug.LogError("Player Name is null or empty");
            return;
        }

        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.GameVersion = "1";
        PhotonNetwork.NickName = nickname;
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        PhotonNetwork.LoadLevel("Lobby");
    }
}
