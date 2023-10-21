using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Linq;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public GameObject VarContainerPrefab;

    private Dictionary<string, float> playerDistance;

    private string myName;


    void Start()
    {
        //QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = 120;

        myName = PhotonNetwork.LocalPlayer.NickName;

        // 플레이어마다 VarContainer 생성 
        PhotonNetwork.Instantiate(this.VarContainerPrefab.name, new Vector3(0, -4, 12), Quaternion.identity, 0);

        Invoke("StartInvoke", 2f);
    }

    private void StartInvoke()
    {
        playerDistance = new Dictionary<string, float>();
        for (int i = 1; i <= PhotonNetwork.CurrentRoom.Players.Count; i++)
        {
            playerDistance.Add(PhotonNetwork.CurrentRoom.Players[i].NickName, 0f);
        }
    }


    // Update is called once per frame
    private void FixedUpdate()
    {
        // Application.targetFrameRate = 300;
    }

    public void SetDistance(string name, float distance)
    {
        playerDistance[name] = distance;
    }

    public int GetRank()
    {
        playerDistance = playerDistance.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        int rank = 1;
        foreach(KeyValuePair<string, float> dic in playerDistance)
        {
            if (dic.Key.Equals(myName)) break;
            rank++;
        }
        return rank;
    }

    public float GetMasterDistance()
    {
        return playerDistance[myName];  
    }
}