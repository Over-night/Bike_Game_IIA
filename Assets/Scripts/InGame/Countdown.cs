using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using TMPro;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
    [SerializeField] VideoPlayer Video;
    [SerializeField] float txtMoveMax;          // 카운트 다운 글씨 움직임 폭
    [SerializeField] float txtMovespeed;        // 카운트 다운 글씨 움직임 속도
    [SerializeField] GameObject StartLine;      // 출발선
    [SerializeField] TextMeshProUGUI Counter;   // 카운터 텍스트 개체
    [SerializeField] float countTime;           // 카운트 시간

    private void Start()
    {
        
        Video.Play();
        Video.time = 1.5;
        Video.playbackSpeed = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CountDown();
        MoveText();

        if (countTime <= 0)
        {
            gameObject.GetComponent<VideoController>().enabled = true;
            gameObject.GetComponent<UIUpdator>().enabled = true;
            // gameObject.GetComponent<UIUpdator>().enabled = true;
            GameObject.FindWithTag("Player").GetComponent<PlayerController>().enabled = true;
            // GameObject.FindWithTag("Player").GetComponent<BicycleCollider>().enabled = true;
            GameObject[] toEnableText = GameObject.FindGameObjectsWithTag("StartEnableText");
            for(int i=0; i< toEnableText.Length; i++)
                toEnableText[i].GetComponent<TextMeshProUGUI>().enabled = true;
            GameObject[] toEnableImage = GameObject.FindGameObjectsWithTag("StartEnableImage");
            for (int i = 0; i < toEnableImage.Length; i++)
                toEnableImage[i].GetComponent<Image>().enabled = true;
            Invoke("Destroyer", 1.5f);
        }

    }
    private void CountDown()
    {
        countTime -= Time.deltaTime;
        int CeilTime = (int)Mathf.Ceil(countTime);      // 올림연산 
        if (CeilTime > 5)
            Counter.text = "Get Ready";

        if (CeilTime > 0 && CeilTime <= 5)
        {
            Counter.text = CeilTime.ToString();
            return;
        }

        if (CeilTime <= 0)
            Counter.text = "Start!!";
    }
    private void MoveText()
    {
        Vector3 pos = Counter.transform.localPosition;
        pos.y += txtMoveMax * Mathf.Sin(Time.time * txtMovespeed);
        Counter.transform.localPosition = pos;
    }
    
    private void Destroyer()
    {
        Counter.text = "";
        gameObject.GetComponent<Countdown>().enabled = false;
    }
}
