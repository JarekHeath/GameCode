using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Wood : MonoBehaviour, ICollectible
{
    public static event HandleWoodCollected OnWoodCollected;
    public static event HandleWoodCollected OnPlace;
    public delegate void HandleWoodCollected(ItemData itemData);
    public ItemData woodData;

    public void Collect()
    {
        Destroy(gameObject);
        OnWoodCollected?.Invoke(woodData);
    }


}
