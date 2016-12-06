using UnityEngine;
using System.Collections;
using UtilitySystems.XmlDatabase.Editor;
using System;
using UnityEditor;

public class ItemExtension : EditorExtension {
    public override bool CanHandleType(Type type) {
        return typeof(ItemAsset).IsAssignableFrom(type);
    }

    public override void OnGUI(object asset) {
        var item = asset as ItemAsset;
        if (item == null) return;

        GUILayout.Label("Item");

        item.Cost = EditorGUILayout.IntField("Cost", item.Cost);
        item.Prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", item.Prefab, typeof(GameObject), false);
    }
}
