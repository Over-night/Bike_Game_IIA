using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int player_Gold = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void IncreaseGold(int amount) // 돈증가
    {
        player_Gold += amount;
        
    }

    public bool DecreaseGold(int amount) // 돈감소
    {
        if(player_Gold - amount > 0)
        {
            player_Gold -= amount;
            return true;
        }
        else
        {
            return false;
        }
    }
}
