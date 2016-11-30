﻿using UnityEngine;
using UnityEditor;

namespace UtilitySystem.XmlDatabase.Editor {
    public abstract class XmlDatabaseWindowSimpleExt<DatabaseAssetType> : XmlDatabaseWindowSimple<DatabaseAssetType>
    where DatabaseAssetType : class, IXmlDatabaseAsset, new() {
        private Vector2 extensionScroll;

        private IEditorExtension[] extensions;
        protected abstract IEditorExtension[] GetExtensions();

        protected override void OnEnable() {
            base.OnEnable();

            extensions = GetExtensions();
            foreach (var extension in extensions) {
                extension.OnEnable();
            }
        }

        protected override void OnDisable() {
            foreach (var extension in extensions) {
                extension.OnDisable();
            }
            extensions = null;
        }

        protected override void DisplayAssetGUI(DatabaseAssetType asset) {
            GUILayout.Label(asset.Name, EditorStyles.toolbarButton);

            extensionScroll = EditorGUILayout.BeginScrollView(extensionScroll);

            foreach (var extension in GetExtensions()) {
                if (extension.CanHandleType(asset.GetType())) {
                    extension.OnGUI(asset);
                }
            }

            EditorGUILayout.EndScrollView();
        }
    }
}
