using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PANEL_LIST
{
    Main, GameList, Shop, Guild, Farm, FarmShop
}

public class PanManager : MonoBehaviour
{

    [Header("Panels")]
    public List<GameObject> List_Panels;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Set_PanelEnable(int Panel_Num)
    {
        int i = 0;
        foreach(GameObject cur in List_Panels)
        {
            if (i == Panel_Num)
            {
                cur.SetActive(true);
            }
            else
            {
                cur.SetActive(false);
            }
            i++;
        }
    }
}
