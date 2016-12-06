using UnityEngine;
using System.Collections;
using UtilitySystems;

/// <summary>
/// Example class that contains an instance of a Database that can
/// be used from other scripts.
/// </summary>
public class DatabaseManager : Singleton<DatabaseManager> {
    private ItemDatabase _itemDatabase;

    static public ItemDatabase ItemAssets {
        get {
            if (Instance != null) {
                if (Instance._itemDatabase == null) {
                    Instance._itemDatabase = new ItemDatabase();
                    Instance._itemDatabase.LoadDatabase();
                }
                return Instance._itemDatabase;
            }
            return null;
        }
    }

    public void Awake() {
        if (_itemDatabase == null) {
            _itemDatabase = new ItemDatabase();
            _itemDatabase.LoadDatabase();
        }
    }

    public void Start() {
        ItemAsset asset = null;
        if (DatabaseManager.ItemAssets.TryGet(100, out asset)) {
            var item = asset.CreateInstance();
            // Use Item instance
        }

    }
}
