using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class SingleUIUpdator : MonoBehaviour
{
    [SerializeField] VideoPlayer Video;
    [SerializeField] TextMeshProUGUI speedText;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI rankText;
    [SerializeField] TextMeshProUGUI successText;
    [SerializeField] Image processBar;
    [SerializeField] Image itemScreen;
    [SerializeField] Sprite[] ItemImage;

    private string isSuccess;
    private float curTime;
    private int rank = 1;
    private float EndLine;

    // Start is called before the first frame update
    void Start()
    {
        VideoController VideoController = GameObject.FindWithTag("GameManager").GetComponent<VideoController>();
        EndLine = VideoController.GetEndPoint();

        StartCoroutine("RankUpdate");
        StartCoroutine("SpeedUpdate");
        StartCoroutine("ProcessUpdate");
        StartCoroutine("TimeUpdate");
    }

    // Update is called once per frame

    IEnumerator RankUpdate()
    {
        while (true)
        {
            string text = rank.ToString();
            switch (rank % 10)
            {
                case 1:
                    text += "ST";
                    break;
                case 2:
                    text += "ND";
                    break;
                case 3:
                    text += "RD";
                    break;
                default:
                    text += "TH";
                    break;
            }
            rankText.text = text;
            yield return null;
        }
    }
    IEnumerator SpeedUpdate()
    {
        while (true)
        {
            float showSpeed = Mathf.Round(Video.playbackSpeed * 15000) / 1000;
            speedText.text = showSpeed.ToString();
            yield return null;
        }
    }
    IEnumerator ProcessUpdate()
    {
        while (true)
        {
            processBar.fillAmount = (float)Video.time / EndLine;
            yield return null;
        }
    }
 
    IEnumerator TimeUpdate()
    {
        while (true)
        {
            curTime += Time.deltaTime;
            timeText.text = string.Format("{0:00}:{1:00.00}", (int)(curTime / 60 % 60), curTime % 60);
            yield return null;
        }
    }

    public void CallCoroutineSuccess(string suc)
    {
        isSuccess = suc;
        StopCoroutine("SuccessUpdate");
        StartCoroutine("SuccessUpdate");
    }

    IEnumerator SuccessUpdate()
    {
        successText.text = isSuccess;
        yield return new WaitForSeconds(4.0f);
        successText.text = "";
    }

    public void ItemImageUpdate(int code)
    {
        itemScreen.sprite = ItemImage[code];
    }
}
