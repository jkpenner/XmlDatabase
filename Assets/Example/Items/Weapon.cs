using UnityEngine;
using System.Collections;

public class Weapon : Item {
    public float Range { get; private set; }
    public float Damage { get; private set; }

    public Weapon(WeaponAsset asset) : base(asset) {
        this.Range = asset.Range;
        this.Damage = asset.Damage;
    }
}
