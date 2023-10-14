using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : Singleton<InventoryController>
{
    [SerializeField]
    private List<Item> currentAmmoList;
    [SerializeField]
    private int itemCountInInventory = 8;
    [SerializeField]
    private Item nullItem;

    public int activeSlotIndex = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddNewAmmoToInventory(AmmoStats ammoStats, int count)
    {
        int firstSlot = GetSlotByType(ammoStats.fruitType);
        int emptySlot = GetSlotByType(AmmoStats.FruitType.Null);

        Debug.Log(firstSlot + " " + emptySlot);

        if (firstSlot >= 0)
        {
            //Check stack item here with existed item
            int leftOverAmmo = currentAmmoList[firstSlot].AddAmmo(count);
            Debug.Log("Add MORE" + count + ammoStats.fruitType + " and left over" + leftOverAmmo);
        }
        else if (emptySlot != -1)
        {
            //Add new iteam which not existed in inventory
            currentAmmoList[emptySlot] = new Item(ammoStats, count);
            Debug.Log("Add NEW" + count + ammoStats.fruitType);
        }
        else
        {
            Debug.Log("No more slot");
            //New item but don't enough slot
            return;
        }
    }

    public int GetSlotByType(AmmoStats.FruitType fruitType)
    {
        for (int i = 0; i < itemCountInInventory; i++)
        {
            if (currentAmmoList[i].ammoStats.fruitType == fruitType) return i;
        }
        return -1;
    }

    public Item GetItem(AmmoStats ammoStats)
    {
        return currentAmmoList[GetSlotByType(ammoStats.fruitType)];
    }

    public Item GetCurrentItem()
    {
        return currentAmmoList[activeSlotIndex];
    }
}
