using UnityEngine;
using System.Collections;

public class Item {
    public int Id { get; private set; }
    public string Name { get; private set; }

    public int Cost { get; private set; }
    public GameObject Prefab { get; private set; }

    public Item(ItemAsset asset) {
        this.Id = asset.Id;
        this.Name = asset.Name;
        this.Cost = asset.Cost;
        this.Prefab = asset.Prefab;
    }
}
