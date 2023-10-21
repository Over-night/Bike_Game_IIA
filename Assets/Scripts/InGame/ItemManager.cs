using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class ItemManager : MonoBehaviour
{
    [SerializeField] GameObject Model;

    private ItemCollideCheck itemCollideCheck;
    private int itemType;
    private VideoPlayer Video;
    private float spinSpeed = 100f;

    // Start is called before the first frame update
    void Start()
    {
        /* Item List
         * 1. �ν�Ʈ ȿ��
         * 2. ���ӵ� ����(�� ��Ƶ� ������)
         * 3. �ڼ� ����
         */

        itemCollideCheck = Model.GetComponent<ItemCollideCheck>();

        itemType = Random.Range(1, 3);
        Video = GameObject.FindWithTag("Video").GetComponent<VideoPlayer>();
    }
    void FixedUpdate()
    {
        // �������� �����̴� ����
        gameObject.transform.position -= transform.forward * Video.playbackSpeed * 0.5f;
        // ������ ȸ��
        Model.transform.Rotate(new Vector3(-spinSpeed * Time.deltaTime, spinSpeed * Time.deltaTime, 0f));
        // ���� Ȯ��
        if(itemCollideCheck.GetIsDestroy())
        {
            if(itemCollideCheck.GetIsPlayer())
            {
                Collider collision = itemCollideCheck.GetCollideObject();
                collision.gameObject.GetComponent<PlayerController>().GetItem(itemType);
            }
            Destroy(gameObject);
        }
    }
    //private void OnTriggerEnter(Collider collision)
    //{
    //    if (collision.tag == "Player")
    //    {
    //        collision.gameObject.GetComponent<PlayerController>().GetItem(itemType);
    //        Destroy(gameObject);
    //    }
    //    if (collision.tag == "Destroyer")
    //    {
    //        Destroy(gameObject);
    //    }
    //}

    //// Update is called once per frame

}
