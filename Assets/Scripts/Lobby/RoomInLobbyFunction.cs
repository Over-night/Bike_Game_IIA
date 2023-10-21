using UnityEngine;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using TMPro;

public class RoomInLobbyFunction : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI roomTitle;
    [SerializeField] TextMeshProUGUI roomInfo;

    private string roomName = "";       // 방 이름
    private string roomUUID;              // 방 식별을 위한 UUID
    private bool roomIsProtected;
    private string roomPassword;
    private int roomNowPerson = 1;      // 현재 방 인원
    private int roomMaxPerson = 12;     // 최대 방 인원
    private LobbyManager manager;

    private void Start()
    {
        manager = GameObject.FindWithTag("LobbyManager").GetComponent<LobbyManager>();
    }

    // Start is called before the first frame update
    public void RoomSettingInit(RoomInfo roomOption)
    {
        roomName = (string)roomOption.CustomProperties["title"];
        roomUUID = (string)roomOption.Name;
        roomIsProtected = (string)roomOption.CustomProperties["isProtected"] == "T" ? true : false;
        roomPassword = (string)roomOption.CustomProperties["pw"];
        roomMaxPerson = roomOption.MaxPlayers;

        // 이름 설정
        roomTitle.text = roomName;
        roomInfo.text = roomNowPerson.ToString() + " / " + roomMaxPerson.ToString();
    }


    public void Onclick_EnterRoom()
    {
        manager.Onclick_JoinRoom(roomUUID);
    }
}
