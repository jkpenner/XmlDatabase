using UnityEngine;
using System.Collections;
using System;
using System.Xml;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UtilitySystem.XmlDatabase {
    public abstract class XmlDatabaseAsset : IXmlDatabaseAsset {
        public int Id { get; set; }
        public virtual string Name { get; set; }

        public XmlDatabaseAsset() {
            Id = -1;
            Name = string.Empty;
        }

        public XmlDatabaseAsset(int id) {
            Id = id;
            Name = string.Empty;
        }

        public XmlDatabaseAsset(int id, string name) {
            Id = id;
            Name = name;
        }

        public abstract void OnSaveAsset(XmlWriter writer);
        public abstract void OnLoadAsset(XmlReader reader);

        public string GetAssetResourcePath(UnityEngine.Object asset) {
#if UNITY_EDITOR
            if (asset == null) { return string.Empty; }

            string dir = AssetDatabase.GetAssetPath(asset);

            // Remove the extension of the asset
            dir = dir.Split(new char[] { '.' })[0];

            bool foundResources = false;
            string newDir = "";
            var folders = dir.Split(new char[] { '\\', '/' });
            for (int i = 0; i < folders.Length; i++) {
                if (foundResources == true) {
                    newDir += folders[i];
                    if (i != folders.Length - 1) {
                        newDir += "/";
                    }
                }

                if (folders[i] == "Resources") {
                    foundResources = true;
                }
            }

            if (foundResources == false) {
                Debug.LogErrorFormat("Asset {0} is not placed into a resources folder will not load correctly", asset.name);
            }

            return newDir;
#else
            return string.Empty;
#endif
        }
    }
}
