using UnityEngine;
using System.Collections;
using System.Xml;
using UtilitySystems.XmlDatabase;
using System.Xml.Serialization;

public class WeaponAsset : ItemAsset {
    private const string elementWeapon = "WeaponValues";

    private const string attrRange = "Range";
    public float Range { get; set; }

    private const string attrDamage = "Damage";
    public float Damage { get; set; }

    public WeaponAsset() {}
    public WeaponAsset(int id) : base(id) {}

    public override void OnSaveAsset(XmlDatabaseWriter writer) {
        base.OnSaveAsset(writer);

        writer.StartElement(elementWeapon);
        writer.SetAttr(attrRange, Range);
        writer.SetAttr(attrDamage, Damage);
        writer.EndElement();
    }

    public override void OnLoadAsset(XmlDatabaseReader reader) {
        base.OnLoadAsset(reader);

        if (reader.IsStartElement(elementWeapon)) {
            Range = reader.GetAttrFloat(attrRange);
            Damage = reader.GetAttrFloat(attrDamage);
        }
    }

    public override Item CreateInstance() {
        return new Weapon(this);
    }
}
