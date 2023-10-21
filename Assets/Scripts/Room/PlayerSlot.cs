using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSlot : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI TextField;

    private Image background;
    private bool isExists = false;  // 플레이어 체크
    private bool isEnabled = true;  // 활성화 및 비활성화 체크
    private string playerNickname;
    private string[] cond = { "Empty", "Disabled" };

    private void Start()
    {
        background = gameObject.GetComponent<Image>();
    }

    public void SlotPlayer(string name)
    {
        isExists = true;
        isEnabled = true;
        playerNickname = name;
        TextField.text = playerNickname;
        background.color = new Color32(124, 255, 255, 255);
    }
    public void SlotEmpty()
    {
        isExists = false;
        isEnabled = true;
        playerNickname = string.Empty;
        TextField.text = cond[0];
        background.color = new Color32(159, 204, 204, 255);
    }

    public void SlotDisabled()
    {
        isExists = false;
        isEnabled = false;
        playerNickname = string.Empty;
        TextField.text = cond[1];
        background.color = new Color32(83, 116, 116, 255);
    }

    public string OnClick()
    {
        if (isEnabled || !isExists) return "";
        return playerNickname;
    }
}
