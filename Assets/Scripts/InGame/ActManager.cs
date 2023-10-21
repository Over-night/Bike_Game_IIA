using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using TMPro;

public class ActManager : MonoBehaviour
{
    [SerializeField] GameObject textPlane;
    [SerializeField] TextMeshPro textContainer;
    [SerializeField] Material[] matList;

    private int actType;
    private VideoPlayer Video;
    

    // Start is called before the first frame update
    void Start()
    {
        actType = Random.Range(1, 8);
        Video = GameObject.FindWithTag("Video").GetComponent<VideoPlayer>();

        string text;
        switch(actType)
        {
            case 1:
                text = "�޼� ���";
                break;
            case 2:
                text = "������ ���";
                break;
            case 3:
                text = "�� �Ȳ�ġ�� ��";
                break;
            case 4:
                text = "���� �Ȳ�ġ�� ��";
                break;
            case 5:
                text = "�� �Ȳ�ġ�� ��";
                break;
            case 6:
                text = "���� ������ ����";
                break;
            case 7:
                text = "������ ���� ����";
                break;
            default:
                text = "";
                break;
        }

        textContainer.text = text;
        textPlane.GetComponent<MeshRenderer>().material = matList[actType - 1];
    }
    void FixedUpdate()
    {
        gameObject.transform.position -= transform.forward * Video.playbackSpeed * 0.5f;
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
        {
            List<bool> getActType = collision.gameObject.GetComponent<PlayerController>().GetActType();
            if (getActType[0])
            {
                collision.gameObject.GetComponent<PlayerController>().PoseProcess(actType);
                Destroy(gameObject);
            }
        }
        if (collision.tag == "Destroyer")
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame

}
