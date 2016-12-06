﻿using UnityEngine;
using System.Collections;
using UtilitySystems.XmlDatabase.Editor;
using System;
using UtilitySystems.XmlDatabase;
using UnityEditor;

public class ItemWindowComplexExt : XmlDatabaseWindowComplexExt<ItemAsset> {
    private ItemDatabase _itemDatabase;

    [MenuItem("Window/RPGSystems/Item/Item Editor Complex Ext")]
    static public void ShowWindow() {
        var wnd = GetWindow<ItemWindowComplexExt>();
        wnd.titleContent.text = "Complex Ext";
        wnd.Show();
    }

    protected override AbstractXmlDatabase<ItemAsset> GetDatabaseInstance() {
        if (_itemDatabase == null) {
            _itemDatabase = new ItemDatabase();
            _itemDatabase.LoadDatabase();
        }
        return _itemDatabase;
    }

    protected override void OnAddNewAssetClick() {
        XmlDatabaseEditorUtility.GetGenericMenu(GetDatabaseInstance().GetListOfAssetTypes(), 
            (selectedIndex) => {
                var newAsset = GetDatabaseInstance().CreateAssetOfType(GetDatabaseInstance().GetListOfAssetTypes()[selectedIndex]);
                newAsset.Id = GetDatabaseInstance().GetNextHighestId();
                SelectedAssetId = newAsset.Id;
                GetDatabaseInstance().Add(newAsset);
        }).ShowAsContext();
    }

    protected override IEditorExtension[] GetExtensions() {
        return new IEditorExtension[] {
            new ItemExtension(),
            new WeaponExtension(),
        };
    }
}
