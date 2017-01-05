using UnityEngine;
using System.Collections;
using UtilitySystems.XmlDatabase;

public class ItemDatabase : AbstractXmlDatabase<ItemAsset> {
    /// <summary>
    /// The name of the database file
    /// </summary>
    public override string DatabaseName { get { return @"ItemDatabase.xml"; } }
    /// <summary>
    /// The path within the StreamingAsset folder the database file is located in
    /// </summary>
    public override string DatabasePath { get { return @"Databases/Item/"; } }

    /// <summary>
    /// Defines all the assets that can be created from database
    /// </summary>
    public override ItemAsset CreateAssetOfType(string type) {
        if (type == typeof(ItemAsset).Name || type == "Item") {
            return new ItemAsset(GetNextHighestId());
        } else if (type == typeof(WeaponAsset).Name || type == "Weapon") {
            return new WeaponAsset(GetNextHighestId());
        }
        return null;
    }

    /// <summary>
    /// Used to create simpler names for use in editor and other places
    /// </summary>
    public override string[] GetListOfAssetTypes() {
        return new string[] {
            "Item", // typeof(ItemAsset).Name,
            "Weapon", //typeof(WeaponAsset).Name,
            
            
        };
    }
}
