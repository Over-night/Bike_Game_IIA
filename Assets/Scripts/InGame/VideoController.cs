using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class VideoController : MonoBehaviourPunCallbacks
{
    [SerializeField] VideoPlayer Video;
    [SerializeField] GameObject startLine; // 출발선 오브젝트
    [SerializeField] GameObject endLine; // 도착선 오브젝트
    [SerializeField] GameObject txtFinish; // 도착 오브젝트

    private float endPointAppear;
    private float endPointLocate;
    private float endTimer;

    private void Start()
    {
        // Video.Play();

        endPointAppear = (float)Video.length - 15f;         // 도착점 등장
        endPointLocate = endPointAppear + 6.5f;             // 도착
    }

    // Keyboard
    private void FixedUpdate()
    {
        // 출발선을 자연스럽게 지나가도록 구현 & 특정 z축에서 오브젝트 비활성화
        if (startLine.transform.position.z > 50)
        {
            startLine.transform.position -= transform.forward * Video.playbackSpeed;
            if (startLine.transform.position.z <= 50)
                startLine.SetActive(false);
        }

        // 도착지점 지정
        if (Video.time >= endPointAppear)
        {
            // 도착 전 일정 거리에서 오브젝트 활성화
            endLine.SetActive(true);
            endLine.transform.position -= transform.forward * Video.playbackSpeed;


            // 도착 시 이벤트 처리
            if (endLine.transform.position.z <= 13)
            {
                endLine.SetActive(false);
                txtFinish.SetActive(true);
                MoveText();
                endTimer += Time.deltaTime;
                if (endTimer > 20)
                    SceneManager.LoadScene("Single_Start");
            }
        }
    }
    private void MoveText()
    {
        Vector3 pos = txtFinish.transform.localPosition;
        pos.y += 1 * Mathf.Sin(Time.time * 3);
        txtFinish.transform.localPosition = pos;
    }

    public float GetEndPoint()
    {
        return endPointLocate;
    }
}