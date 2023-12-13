using NUnit.Framework.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static event Action<List<InventoryItem>> OnInventoryChange;
    public static event Action OnUsedMeat;
    public static event Action OnMakeBoat;
    public static event Action<ItemData> OnAddedItem;
    public static event Action<ItemData> OnRemovedItem;

    public List<InventoryItem> inventory = new List<InventoryItem>();
    public Dictionary<ItemData, InventoryItem> itemDictionary = new Dictionary<ItemData, InventoryItem>();

    private void OnEnable()
    {
        Wood.OnWoodCollected += Add;
        Leaf.OnLeafCollected += Add;
        Meat.OnMeatCollected += Add;
        PlayerMovement2.OnEat += Use1;
        PlayerMovement2.OnPlace += Use2;
        PlayerMovement2.OnWoodsCollected += Add;
        PlayerMovement2.OnLeafsCollected += Add;
        PlayerMovement2.OnMeatsCollected += Add;


    }

    private void OnDisable()
    {
        Wood.OnWoodCollected -= Add;
        Leaf.OnLeafCollected -= Add;
        Meat.OnMeatCollected -= Add;
        PlayerMovement2.OnEat -= Use1;
        PlayerMovement2.OnPlace -= Use2;
        PlayerMovement2.OnWoodsCollected -= Add;
        PlayerMovement2.OnLeafsCollected -= Add;
        PlayerMovement2.OnMeatsCollected -= Add;
    }


    public void Add(ItemData itemData)
    {
        if(itemDictionary.TryGetValue(itemData, out InventoryItem item))
        {
            item.AddToStack();
            OnInventoryChange?.Invoke(inventory);
            OnAddedItem?.Invoke(itemData);

        }
        else
        {
            InventoryItem newItem = new InventoryItem(itemData);
            inventory.Add(newItem);
            itemDictionary.Add(itemData, newItem);
            OnInventoryChange?.Invoke(inventory);
            OnAddedItem?.Invoke(itemData);
        }
    }

    public void Remove(ItemData itemData)
    {
        if (itemDictionary.TryGetValue(itemData,out InventoryItem item))
        {
            item.RemoveFromStack();
            if(item.stackSize == 0)
            {
                inventory.Remove(item);
                itemDictionary.Remove(itemData);
            }
            OnInventoryChange?.Invoke(inventory);
            OnRemovedItem?.Invoke(itemData);
        }
    }

    public void Use1(ItemData itemData, int amount)
    {
        if (itemDictionary.TryGetValue(itemData, out InventoryItem item))
        {
            if (item.stackSize < amount)
            {
                return;
            }
            else
            {
                item.RemoveFromStack();
                if (item.stackSize == 0)
                {
                    inventory.Remove(item);
                    itemDictionary.Remove(itemData);
                }
                OnInventoryChange?.Invoke(inventory);
                OnUsedMeat?.Invoke();
            }
        }
        else
        {
            return;
        }
    }

    public void Use2(ItemData itemData, int amount, ItemData itemData2, int amount2)
    {
        if (itemDictionary.TryGetValue(itemData, out InventoryItem item))
        {
            if (item.stackSize < amount)
            {
                return;
            }
            else
            {
                if (itemDictionary.TryGetValue(itemData2, out InventoryItem item2))
                {
                    if (item2.stackSize < amount2)
                    {
                        return;
                    }
                    else
                    {
                        for (int i = 0; i < amount; i++)
                        {
                            item.RemoveFromStack();
                        }
                        for (int i = 0; i < amount2; i++)
                        {
                            item2.RemoveFromStack();
                        }
                        if (item.stackSize == 0)
                        {
                            inventory.Remove(item);
                            itemDictionary.Remove(itemData);
                        }
                        if (item2.stackSize == 0)
                        {
                            inventory.Remove(item2);
                            itemDictionary.Remove(itemData2);
                        }
                        OnInventoryChange?.Invoke(inventory);
                        OnMakeBoat?.Invoke();
                    }
                }
            }
        }
        else
        {
            return;
        }
    }

}
