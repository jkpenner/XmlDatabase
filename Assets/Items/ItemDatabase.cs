using UnityEngine;
using System.Collections;
using UtilitySystem.XmlDatabase;

public class ItemDatabase : AbstractXmlDatabase<ItemAsset> {
    static private ItemDatabase _instance = null;
    static public ItemDatabase Instance {
        get {
            if (_instance == null) {
                _instance = new ItemDatabase();
            }
            return _instance;
        }
    }

    public override string DatabaseName { get { return @"ItemDatabase.xml"; } }
    public override string DatabasePath { get { return @"Databases/Item/"; } }

    protected override ItemAsset CreateAssetOfType(string type) {
        if (type == typeof(ItemAsset).Name) {
            return new ItemAsset(this.GetNextId());
        }
        return null;
    }
}
