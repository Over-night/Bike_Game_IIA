using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BTN_Gold : MonoBehaviour
{
    public PlayerManager playerManager;
    public Button BTN_Button;
    public Text TXT_Button;
    // Start is called before the first frame update
    void Start()
    {
        BTN_Button = GetComponent<Button>();
        TXT_Button = BTN_Button.GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if(TXT_Button.text != playerManager.player_Gold.ToString())
        {
            TXT_Button.text = playerManager.player_Gold.ToString();
        }
    }
}
