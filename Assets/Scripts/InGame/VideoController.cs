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
    [SerializeField] GameObject startLine; // ��߼� ������Ʈ
    [SerializeField] GameObject endLine; // ������ ������Ʈ
    [SerializeField] GameObject txtFinish; // ���� ������Ʈ

    private float endPointAppear;
    private float endPointLocate;
    private float endTimer;

    private void Start()
    {
        // Video.Play();

        endPointAppear = (float)Video.length - 15f;         // ������ ����
        endPointLocate = endPointAppear + 6.5f;             // ����
    }

    // Keyboard
    private void FixedUpdate()
    {
        // ��߼��� �ڿ������� ���������� ���� & Ư�� z�࿡�� ������Ʈ ��Ȱ��ȭ
        if (startLine.transform.position.z > 50)
        {
            startLine.transform.position -= transform.forward * Video.playbackSpeed;
            if (startLine.transform.position.z <= 50)
                startLine.SetActive(false);
        }

        // �������� ����
        if (Video.time >= endPointAppear)
        {
            // ���� �� ���� �Ÿ����� ������Ʈ Ȱ��ȭ
            endLine.SetActive(true);
            endLine.transform.position -= transform.forward * Video.playbackSpeed;


            // ���� �� �̺�Ʈ ó��
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