using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Leaf : MonoBehaviour, ICollectible
{
    public static event HandleLeafCollected OnLeafCollected;
    public delegate void HandleLeafCollected(ItemData itemData);
    public ItemData leafData;

    public void Collect()
    {
        Destroy(gameObject);
        OnLeafCollected?.Invoke(leafData);
    }


}
