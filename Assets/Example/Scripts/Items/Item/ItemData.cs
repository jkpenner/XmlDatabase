using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemData {
    public ItemAsset Asset { get; private set; }

    public int Id { get; private set; }
    public string Name { get; private set; }

    public int Cost { get; private set; }
    public GameObject Prefab { get; private set; }

    public ItemData(ItemAsset asset) {
        this.Asset = asset;

        this.Id = asset.Id;
        this.Name = asset.Name;
        this.Cost = asset.Cost;
        this.Prefab = asset.Prefab;
    }

    static public ItemData Create(int itemId) {
        return Create<ItemData>(itemId);
    }

    static public T Create<T>(int id) where T : ItemData{
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
