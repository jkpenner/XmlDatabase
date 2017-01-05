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

        GUILayout.BeginHorizontal(EditorStyles.toolbar);
        GUILayout.Label("Weapon", EditorStyles.centeredGreyMiniLabel);
        GUILayout.EndHorizontal();

        GUILayout.BeginVertical("Box");
        weapon.Range = EditorGUILayout.FloatField("Range", weapon.Range);
        weapon.Damage = EditorGUILayout.FloatField("Damage", weapon.Damage);
        weapon.Projectile = (GameObject)EditorGUILayout.ObjectField("Projectile", weapon.Projectile, typeof(GameObject), false);
        GUILayout.EndVertical();
    }
}
