using UnityEngine;
using UnityEditor;
using UtilitySystems.XmlDatabase;
using UtilitySystems.XmlDatabase.Editor;

public class ItemWindowSimple : XmlDatabaseWindowSimple<ItemAsset> {
    private ItemDatabase _itemDatabase;

    [MenuItem("Window/RPGSystems/Item/Item Editor Simple")]
    static public void ShowWindow() {
        var wnd = GetWindow<ItemWindowSimple>();
        wnd.titleContent.text = "Item Editor Simple";
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
