﻿using UnityEngine;
using System.Collections;
using UnityEditor;

namespace UtilitySystems.XmlDatabase.Editor {
    static public class XmlDatabaseEditorUtility {
        public delegate void SelectEvent<T>(T asset);

        static public void ShowContext<T>(AbstractXmlDatabase<T> database, SelectEvent<T> callback) where T : class, IXmlDatabaseAsset {
            GetGenericMenu(database, callback, null).ShowAsContext();
        }

        static public void ShowContext<T>(AbstractXmlDatabase<T> database, SelectEvent<T> callback, System.Type createNewWindow) where T : class, IXmlDatabaseAsset {
            GetGenericMenu(database, callback, createNewWindow).ShowAsContext();
        }

        static public void ShowContext<T>(AbstractXmlDatabase<T> database, SelectEvent<T> callback, Rect position) where T : class, IXmlDatabaseAsset {
            GetGenericMenu(database, callback, null).DropDown(position);
        }

        static public void ShowContext<T>(AbstractXmlDatabase<T> database, SelectEvent<T> callback, Rect position, System.Type createNewWindow) where T : class, IXmlDatabaseAsset {
            GetGenericMenu(database, callback, createNewWindow).DropDown(position);
        }

        static public GenericMenu GetGenericMenu(string[] assets, SelectEvent<int> callback) {
            GenericMenu menu = new GenericMenu();
            for (int i = 0; i < assets.Length; i++) {
                menu.AddItem(new GUIContent(assets[i]), false,
                    (index) => { callback((int)index); }, i);
            }

            return menu;
        }

        static public GenericMenu GetGenericMenu<T>(AbstractXmlDatabase<T> database, SelectEvent<T> callback, System.Type createNewWindow) where T : class, IXmlDatabaseAsset {
            GenericMenu menu = new GenericMenu();
            if (createNewWindow != null) {
                menu.AddItem(new GUIContent("Create New"), false, () => {
                    EditorWindow.GetWindow(createNewWindow).Show();
                });

                menu.AddSeparator(null);
            }

            foreach (var asset in database.GetAssets()) {
                int selectedAssetId = asset.Id;
                menu.AddItem(new GUIContent(asset.Name), false,
                    (assetId) => { callback(database.Get((int)assetId)); }, selectedAssetId);
            }

            return menu;
        }


        public delegate void DatabaseIdFieldCallback(int newId);
        static public int DatabaseIdField<T>(AbstractXmlDatabase<T> database, string label, int assetId, DatabaseIdFieldCallback callback, System.Type createNewWindow) where T : class, IXmlDatabaseAsset {
            int value = 0;

            GUILayout.BeginHorizontal();
            var asset = database.LoadAsset(assetId);
            GUILayout.Label(label, GUILayout.Width(EditorGUIUtility.labelWidth - 4));
            if (GUILayout.Button(asset != null ? asset.Name : "Not Set", EditorStyles.toolbarDropDown)) {

                XmlDatabaseEditorUtility.ShowContext(database, (selectedAsset) => {
                    if (callback != null) {
                        callback.Invoke(selectedAsset.Id);
                    }
                }, createNewWindow);

            }
            GUILayout.EndHorizontal();

            return value;
        }
    }
}