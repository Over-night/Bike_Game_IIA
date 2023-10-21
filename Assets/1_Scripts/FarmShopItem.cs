using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FarmShopItem : MonoBehaviour
{
    [Header("Manager")]
    public PlayerManager playerManager;

    [Header("UI")]
    public Button BTN_Prize;
    public Text TXT_ItemCount;
    public Text TXT_ItemPrize;


    public void BTN_SellItem()
    {
        BTN_Prize.interactable = false;
        int itemcount;
        bool success = int.TryParse(TXT_ItemCount.text, out itemcount);
        if (!success) return;

        if (itemcount > 0)
        {
            int prize = int.Parse(TXT_ItemPrize.text);
            
            itemcount--; // 아이템 감소
            TXT_ItemCount.text = itemcount.ToString();

            playerManager.IncreaseGold(prize); // 돈올리기
        }
        BTN_Prize.interactable = true;
    }
}
