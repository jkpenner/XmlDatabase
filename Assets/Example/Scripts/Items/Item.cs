using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    static public Item Create(int itemId) {
        return Create<Item>(itemId);
    }

    static public T Create<T>(int id) where T : Item{
        if (DatabaseManager.ItemAssets != null) {
            var asset = DatabaseManager.ItemAssets.Get(id);
            if (asset != null) {
                return asset.CreateInstance() as T;
            } else {
                // No asset with given id in database
            }
        } else {
            // No instance of a ItemDatabase available
        }
        return null;
    }
}
