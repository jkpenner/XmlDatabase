using UnityEngine;
using UnityEditor;
using UtilitySystems.XmlDatabase.Editor;
using System;

public class WeaponExtension : EditorExtension {
    public override bool CanHandleType(Type type) {
        return typeof(WeaponAsset).IsAssignableFrom(type);
    }

    public override void OnGUI(object asset) {
        var weapon = asset as WeaponAsset;
        if(weapon == null) return;

        GUILayout.Label("Weapon");

        weapon.Range = EditorGUILayout.FloatField("Range", weapon.Range);
        weapon.Damage = EditorGUILayout.FloatField("Damage", weapon.Damage);
    }
}
