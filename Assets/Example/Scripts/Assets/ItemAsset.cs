using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using UtilitySystems.XmlDatabase;

public class ItemAsset : XmlDatabaseAsset {
    private const string elementItem = "ItemValues";

    private const string attrCost = "Cost";
    public int Cost { get; set; }

    private const string attrPrefab = "Prefab";
    public GameObject Prefab { get; set; }

    public ItemAsset() { }
    public ItemAsset(int id) : base(id) { }

    public override void OnLoadAsset(XmlDatabaseReader reader) {
        if(reader.IsStartElement(elementItem)) {
            Cost = reader.GetAttrInt(attrCost);
            Prefab = reader.GetAttrResource<GameObject>(attrPrefab);
        }
    }

    public override void OnSaveAsset(XmlDatabaseWriter writer) {
        writer.StartElement(elementItem);
        writer.SetAttr(attrCost, Cost);
        writer.SetAttr(attrPrefab, Prefab);
        writer.EndElement();
    }

    public virtual Item CreateInstance() {
        return new Item(this);
    }
}
