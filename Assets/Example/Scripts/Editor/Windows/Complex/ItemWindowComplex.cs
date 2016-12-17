using UnityEngine;
using UnityEditor;
using UtilitySystems.XmlDatabase;
using UtilitySystems.XmlDatabase.Editor;

public class ItemWindowComplex : XmlDatabaseWindowComplex<ItemAsset> {
    private ItemDatabase _itemDatabase = null;

    [MenuItem("Window/RPGSystems/Item/Item Editor Complex")]
    static public void ShowWindow() {
        var wnd = GetWindow<ItemWindowComplex>();
        wnd.titleContent.text = "Item Editor Complex";
        wnd.Show();
    }

    protected override AbstractXmlDatabase<ItemAsset> GetDatabaseInstance() {
        if (_itemDatabase == null) {
            _itemDatabase = new ItemDatabase();
            _itemDatabase.LoadDatabase();
        }
        return _itemDatabase;
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
