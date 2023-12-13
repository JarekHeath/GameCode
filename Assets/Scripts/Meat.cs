using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Meat : MonoBehaviour, ICollectible
{
    public static event HandleMeatCollected OnMeatCollected;
    public static event HandleMeatCollected OnEat;
    public delegate void HandleMeatCollected(ItemData itemData);
    public ItemData meatData;

    public void Collect()
    {
        Destroy(gameObject);
        OnMeatCollected?.Invoke(meatData);
    }




}
