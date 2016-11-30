using UnityEngine;
using System.Collections;
using System;
using System.Xml;


namespace UtilitySystem.XmlDatabase {
    public abstract class XmlDatabaseAsset : IXmlDatabaseAsset {
        public int Id { get; set; }
        public virtual string Name { get; set; }

        public string PerferredTypeString { get; set; }

        public XmlDatabaseAsset() {
            Initialize();
        }

        public XmlDatabaseAsset(int id) {
            Initialize();
            Id = id;
        }

        public XmlDatabaseAsset(int id, string name) {
            Initialize();
            Id = id;
            Name = name;
        }

        public virtual void Initialize() {
            Id = -1;
            Name = string.Empty;
        }

        public abstract void OnSaveAsset(XmlWriter writer);
        public abstract void OnLoadAsset(XmlReader reader);
    }
}
