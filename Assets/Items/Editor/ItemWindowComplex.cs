using UnityEngine;
using UnityEditor;
using UtilitySystem.XmlDatabase;
using UtilitySystem.XmlDatabase.Editor;

public class ItemWindowComplex : XmlDatabaseWindowComplex<ItemAsset> {
    [MenuItem("Window/RPGSystems/Item/Item Editor Complex")]
    static public void ShowWindow() {
        var wnd = GetWindow<ItemWindowComplex>();
        wnd.titleContent.text = "Item Editor Complex";
        wnd.Show();
    }

    protected override AbstractXmlDatabase<ItemAsset> GetDatabaseInstance() {
        return ItemDatabase.Instance;
    }

    protected override ItemAsset CreateNewDatabaseAsset() {
        return new ItemAsset(GetDatabaseInstance().GetNextId());
    }

    protected override void DisplayAssetGUI(ItemAsset asset) {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Name", GUILayout.Width(80));
        asset.Name = EditorGUILayout.TextField(asset.Name);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Prefab", GUILayout.Width(80));
        asset.Prefab = (GameObject)EditorGUILayout.ObjectField(asset.Prefab, typeof(GameObject), false);
        GUILayout.EndHorizontal();
    }
}
