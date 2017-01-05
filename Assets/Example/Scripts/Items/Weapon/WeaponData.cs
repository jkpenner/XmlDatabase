using UnityEngine;
using System.Collections;

public class WeaponData : ItemData {
    public float Range { get; private set; }
    public float Damage { get; private set; }
    public GameObject Projectile { get; private set; }

    public WeaponData(WeaponAsset asset) : base(asset) {
        this.Range = asset.Range;
        this.Damage = asset.Damage;
        this.Projectile = asset.Projectile;
    }
}
