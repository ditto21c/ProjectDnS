using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CInventory : Object
{
    
    public void AddGold(int InGold)
    {
        Gold += InGold;
    }
    public int GetGold() { return Gold; }

    int Gold = 0;
}
