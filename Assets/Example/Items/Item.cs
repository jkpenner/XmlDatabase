using UnityEngine;
using System.Collections;

public class Item {
    public int Cost { get; private set; }
    public GameObject Prefab { get; private set; }

    public Item(ItemAsset asset) {
        this.Cost = asset.Cost;
        this.Prefab = asset.Prefab;
    }
}
