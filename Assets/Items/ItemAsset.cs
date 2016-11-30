using UnityEngine;
using System.Xml;
using UtilitySystem.XmlDatabase;

public class ItemAsset : XmlDatabaseAsset {
    public GameObject Prefab { get; set; }

    public ItemAsset() { }
    public ItemAsset(int id) : base(id) { }
    public ItemAsset(int id, string name) : base(id, name) { }

    public override void OnLoadAsset(XmlReader reader) {
        switch (reader.Name) {
            case "AssetParams":
                Prefab = Resources.Load<GameObject>(reader.GetStringAttribute("Prefab", ""));
                break;
        }
    }

    public override void OnSaveAsset(XmlWriter writer) {
        writer.WriteStartElement("AssetParams");
        writer.WriteAttributeString("Prefab", XmlDatabaseUtility.GetAssetResourcePath(Prefab));
        writer.WriteEndElement();
    }
}
