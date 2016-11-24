using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

namespace UtilitySystem.XmlDatabase.Editor {
    abstract public class XmlDatabaseWindowSimple<DatabaseAssetType> : EditorWindow where DatabaseAssetType : class, IXmlDatabaseAsset, new() {
        protected abstract AbstractXmlDatabase<DatabaseAssetType> GetDatabaseInstance();
        protected abstract DatabaseAssetType CreateNewDatabaseAsset();

        private Vector2 selectorScroll = Vector2.zero;

        private Queue<Action> actionQueue = null;
        public Queue<Action> ActionQueue {
            get {
                if (actionQueue == null) {
                    actionQueue = new Queue<Action>();
                }
                return actionQueue;
            }
        }

        private int _selectorIndex = -1;
        public int SelectedAssetId {
            get {
                return _selectorIndex;
            }
            set {
                if (_selectorIndex != value) {
                    _selectorIndex = value;
                    EditorGUI.FocusTextInControl(string.Empty);
                }
            }
        }

        protected abstract void DisplayAssetGUI(DatabaseAssetType asset);

        private GUIStyle toggleButtonStyle;
        public virtual GUIStyle ToggleButtonStyle {
            get {
                if (toggleButtonStyle == null) {
                    // Create custom style for stat buttons
                    toggleButtonStyle = new GUIStyle(EditorStyles.toolbarButton);
                    toggleButtonStyle.alignment = TextAnchor.MiddleLeft;
                }
                return toggleButtonStyle;
            }
        }

        public void OnEnable() {
            GetDatabaseInstance().LoadAssets();
        }

        public void OnGUI() {
            GUILayout.BeginVertical();

            // Scroll view for the listed assets
            selectorScroll = GUILayout.BeginScrollView(selectorScroll, false, true);

            // List all the assets in the database
            GUILayout.BeginVertical("Box", GUILayout.ExpandWidth(true));

            var database = GetDatabaseInstance();
            foreach (var asset in database.GetAssets()) {
                if (asset != null) {
                    DisplayAssetHeaderGUI(asset.Id, asset);

                    if (SelectedAssetId == asset.Id) {
                        GUILayout.BeginVertical("Box");
                        DisplayAssetGUI(asset);
                        GUILayout.EndVertical();
                    }
                }
            }

            if (database.GetCount() == 0) {
                GUILayout.Label("No assets in database.\nClick 'Add New' to create an asset.", EditorStyles.centeredGreyMiniLabel);
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();

            GUILayout.EndScrollView();

            DisplayGUIFooter();

            GUILayout.Space(2);
            GUILayout.EndVertical();

            // Invoke any actions
            while (ActionQueue.Count > 0) {
                ActionQueue.Dequeue().Invoke();
            }
        }

        private void DisplayAssetHeaderGUI(int id, DatabaseAssetType asset) {
            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            GUILayout.Label(string.Format("Id: {0}", asset.Id.ToString("D3")), GUILayout.Width(60));

            var clicked = GUILayout.Toggle(asset.Id == SelectedAssetId, asset.Name, ToggleButtonStyle);
            if (clicked != (asset.Id == SelectedAssetId)) {
                if (clicked) {
                    SelectedAssetId = asset.Id;
                } else {
                    SelectedAssetId = -1;
                }
            }

            if (GUILayout.Button("-", EditorStyles.toolbarButton, GUILayout.Width(30))) {
                int idToRemove = id;
                ActionQueue.Enqueue(() => {
                    GetDatabaseInstance().Remove(idToRemove);
                    EditorGUI.FocusTextInControl(string.Empty);
                });
            }
            GUILayout.EndHorizontal();
        }

        public virtual void DisplayGUIFooter() {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Add New", EditorStyles.toolbarButton)) {
                var newAsset = new DatabaseAssetType();
                newAsset.Id = GetDatabaseInstance().GetNextId();
                SelectedAssetId = newAsset.Id;

                GetDatabaseInstance().Add(newAsset);

                EditorGUI.FocusTextInControl(string.Empty);
            }

            if (GUILayout.Button("Save", EditorStyles.toolbarButton, GUILayout.Width(60))) {
                if (EditorUtility.DisplayDialog("Save Database", "Save current data to the XML document?", "Save", "Cancel")) {
                    GetDatabaseInstance().SaveAssets();
                }
            }

            if (GUILayout.Button("Load", EditorStyles.toolbarButton, GUILayout.Width(60))) {
                if (EditorUtility.DisplayDialog("Load Database", "Loading values from the database will override all unsaved changes in the editor.", "Load", "Cancel")) {
                    GetDatabaseInstance().LoadAssets();
                }
            }
            GUILayout.EndHorizontal();
        }
    }
}
