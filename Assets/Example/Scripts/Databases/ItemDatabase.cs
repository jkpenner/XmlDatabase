using UnityEngine;
using System.Collections;
using UtilitySystems.XmlDatabase;

public class ItemDatabase : AbstractXmlDatabase<ItemAsset> {
    public override string DatabaseName { get { return @"ItemDatabase.xml"; } }
    public override string DatabasePath { get { return @"Databases/Item/"; } }

    public override ItemAsset CreateAssetOfType(string type) {
        if (type == typeof(ItemAsset).Name || type == "Item") {
            return new ItemAsset(GetNextHighestId());
        } else if (type == typeof(WeaponAsset).Name || type == "Weapon") {
            return new WeaponAsset(GetNextHighestId());
        }
        return null;
    }

    public override string[] GetListOfAssetTypes() {
        return new string[] {
            "Item", // typeof(ItemAsset).Name,
            "Weapon", //typeof(WeaponAsset).Name,
            
            
        };
    }
}
