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
         * 1. 부스트 효과
         * 2. 가속도 증진(덜 밟아도 가지게)
         * 3. 자세 지정
         */

        itemCollideCheck = Model.GetComponent<ItemCollideCheck>();

        itemType = Random.Range(1, 3);
        Video = GameObject.FindWithTag("Video").GetComponent<VideoPlayer>();
    }
    void FixedUpdate()
    {
        // 아이템을 움직이는 정도
        gameObject.transform.position -= transform.forward * Video.playbackSpeed * 0.5f;
        // 아이템 회전
        Model.transform.Rotate(new Vector3(-spinSpeed * Time.deltaTime, spinSpeed * Time.deltaTime, 0f));
        // 상태 확인
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
