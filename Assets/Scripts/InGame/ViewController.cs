using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewController : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] Button Btn_viewchange;
    [SerializeField] Camera TPP_Camera;
    [SerializeField] Camera FPP_Camera;
    private int mode_state = 0; // 0: TPP_Mode, 1:FPP_Mode

    // Start is called before the first frame update
    void FixedUpdate()
    {
        if(mode_state == 0)
            Btn_viewchange.onClick.AddListener(FPP_Mode);
        else if(mode_state == 1)
            Btn_viewchange.onClick.AddListener(TPP_Mode);

    }

    // Update is called once per frame
    void FPP_Mode()
    {
        canvas.worldCamera = FPP_Camera;
        TPP_Camera.targetDisplay = 1;
        FPP_Camera.targetDisplay = 0;
        TPP_Camera.GetComponent<AudioListener>().enabled = false;
        FPP_Camera.GetComponent<AudioListener>().enabled = true;
        mode_state = 1;
    }
    void TPP_Mode()
    {
        canvas.worldCamera = TPP_Camera;
        TPP_Camera.targetDisplay = 0;
        FPP_Camera.targetDisplay = 1;
        TPP_Camera.GetComponent<AudioListener>().enabled = true;
        FPP_Camera.GetComponent<AudioListener>().enabled = false;
        mode_state = 0;
    }
}
