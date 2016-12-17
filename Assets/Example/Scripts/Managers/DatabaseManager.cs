using UnityEngine;
using System.Collections;
using UtilitySystems;
using System;

/// <summary>
/// Example class that contains an instance of a Database that can
/// be used from other scripts.
/// </summary>
public class DatabaseManager : Singleton<DatabaseManager> {
    private ItemDatabase _itemDatabase;

    static public ItemDatabase ItemAssets {
        get {
            if (Instance != null) {
                Instance.SetupItemDatabase();
                return Instance._itemDatabase;
            }
            return null;
        }
    }

    public void Awake() {
        SetupItemDatabase();
    }

    private void SetupItemDatabase() {
        if (_itemDatabase == null) {
            _itemDatabase = new ItemDatabase();
            _itemDatabase.LoadDatabase();
        }
    }
}
