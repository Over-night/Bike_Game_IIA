using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Canvas_Fixed : MonoBehaviour
{
    int i_before_Width = 0;
    int i_before_Height = 0;
    public Image BG;


    private void Start()
    {
        SetResolution(); // 초기에 게임 해상도 고정
    }

    public void Update()
    {
        
        SetResolution();

    }

    /* 해상도 설정하는 함수 */
    public void SetResolution()
    {
        if (i_before_Width == Screen.width && i_before_Height == Screen.height)
        {
            return;
        }
        RectTransform rect = BG.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(Screen.width, Screen.height);

        Debug.Log("해상도 세팅");

        int setWidth = 1920; // 사용자 설정 너비
        int setHeight = 1080; // 사용자 설정 높이
                
        int deviceWidth = Screen.width; // 기기 너비 저장
        int deviceHeight = Screen.height; // 기기 높이 저장

        i_before_Width = deviceWidth;
        i_before_Height = deviceHeight;

        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true); // SetResolution 함수 제대로 사용하기

        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight) // 기기의 해상도 비가 더 큰 경우
        {
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); // 새로운 너비
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // 새로운 Rect 적용
        }
        else // 게임의 해상도 비가 더 큰 경우
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight); // 새로운 높이
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // 새로운 Rect 적용
        }
    }
}
