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
                text = "¿Þ¼Õ µé¾î";
                break;
            case 2:
                text = "¿À¸¥¼Õ µé¾î";
                break;
            case 3:
                text = "¿Þ ÆÈ²ÞÄ¡·Î ÃÄ";
                break;
            case 4:
                text = "¿À¸¥ ÆÈ²ÞÄ¡·Î ÃÄ";
                break;
            case 5:
                text = "¾ç ÆÈ²ÞÄ¡·Î ÃÄ";
                break;
            case 6:
                text = "¿ÞÆÈ ¿À¸¥ÂÊ À§·Î";
                break;
            case 7:
                text = "¿À¸¥ÆÈ ¿ÞÂÊ À§·Î";
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
