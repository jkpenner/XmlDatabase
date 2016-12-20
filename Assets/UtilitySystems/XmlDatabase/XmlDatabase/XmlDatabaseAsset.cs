using UnityEngine;
using System.Collections;
using System;
using System.Xml;
using System.Xml.Serialization;

namespace UtilitySystems.XmlDatabase {
    public abstract class XmlDatabaseAsset : IXmlDatabaseAsset {
        public int Id { get; set; }

        public virtual string Name { get; set; }

        public string PerferredTypeString { get { return string.Empty; } }

        public XmlDatabaseAsset() {
            Initialize();
        }

        public XmlDatabaseAsset(int id) {
            Initialize();
            Id = id;
        }

        public virtual void Initialize() {
            Id = 0;
            Name = string.Empty;
        }

        public abstract void OnSaveAsset(XmlDatabaseWriter writer);
        public abstract void OnLoadAsset(XmlDatabaseReader reader);
    }
}
