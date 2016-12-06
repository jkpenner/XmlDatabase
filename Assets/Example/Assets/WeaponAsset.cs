using UnityEngine;
using System.Collections;
using System.Xml;
using UtilitySystems.XmlDatabase;
using System.Xml.Serialization;

public class WeaponAsset : ItemAsset {
    public float Range { get; set; }
    public float Damage { get; set; }

    public WeaponAsset() {}
    public WeaponAsset(int id) : base(id) {}

    public override void OnSaveAsset(XmlWriter writer) {
        base.OnSaveAsset(writer);

        writer.WriteStartElement("WeaponValues");
        writer.SetAttr("Range", Range);
        writer.SetAttr("Damage", Damage);
        writer.WriteEndElement();
    }

    public override void OnLoadAsset(XmlReader reader) {
        base.OnLoadAsset(reader);

        if (reader.Name == "WeaponValues") {
            Range = reader.GetAttrFloat("Range", 0f);
            Damage = reader.GetAttrFloat("Damage", 0f);
        }
    }

    public override Item CreateInstance() {
        return new Weapon(this);
    }
}
