using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using UtilitySystems.XmlDatabase;

public class ItemAsset : XmlDatabaseAsset {
    public int Cost { get; set; }
    public GameObject Prefab { get; set; }

    public ItemAsset() { }
    public ItemAsset(int id) : base(id) { }

    public override void OnLoadAsset(XmlReader reader) {
        if(reader.Name == "ItemValues") {
            Cost = reader.GetAttrInt("Cost", 0);
            Prefab = reader.GetAttrGameObject("Prefab");
        }
    }

    public override void OnSaveAsset(XmlWriter writer) {
        writer.WriteStartElement("ItemValues");
        writer.SetAttr("Cost", Cost);
        writer.SetAttr("Prefab", Prefab);
        writer.WriteEndElement();
    }

    public virtual Item CreateInstance() {
        return new Item(this);
    }
}
