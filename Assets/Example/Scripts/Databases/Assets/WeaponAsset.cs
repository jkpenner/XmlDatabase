using UnityEngine;
using System.Collections;
using System.Xml;
using UtilitySystems.XmlDatabase;
using System.Xml.Serialization;

/// <summary>
/// Example of a Weapon Asset which inherits from the base
/// ItemAsset class
/// </summary>
public class WeaponAsset : ItemAsset {
    private const string elementWeapon = "WeaponValues";

    private const string attrRange = "Range";
    public float Range { get; set; }

    private const string attrDamage = "Damage";
    public float Damage { get; set; }

    private const string attrProjectile = "Projectile";
    public GameObject Projectile { get; set; }

    public WeaponAsset() {}
    public WeaponAsset(int id) : base(id) {}

    public override void OnSaveAsset(XmlDatabaseWriter writer) {
        base.OnSaveAsset(writer);

        writer.StartElement(elementWeapon);
        writer.SetAttr(attrRange, Range);
        writer.SetAttr(attrDamage, Damage);
        writer.SetAttr(attrProjectile, Projectile);
        writer.EndElement();
    }

    public override void OnLoadAsset(XmlDatabaseReader reader) {
        base.OnLoadAsset(reader);

        if (reader.IsStartElement(elementWeapon)) {
            Range = reader.GetAttrFloat(attrRange);
            Damage = reader.GetAttrFloat(attrDamage);
            Projectile = reader.GetAttrResource<GameObject>(attrProjectile);
        }
    }

    public override ItemData CreateInstance() {
        return new WeaponData(this);
    }

    public override ItemObject CreateObjectInstance() {
        return Internal_CreateObjectInstance<WeaponObject>();
    }
}
