using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public AmmoStats ammoStats;
    public int count;
    public bool isActive = true;

    public Item(AmmoStats ammoStats, int count)
    {
        this.ammoStats = ammoStats;
        AddAmmo(count);
        isActive = true;
    }

    public int AddAmmo(int newCount)
    {
        int currentCount = count + newCount;
        if (currentCount <= ammoStats.maxCount)
        {
            count = currentCount;
            return 0;
        }
        else
        {
            count = ammoStats.maxCount;
            return currentCount - ammoStats.maxCount;
        }
    }
}