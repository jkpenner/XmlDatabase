using UnityEngine;
using System.Collections;
using System;
using System.Xml;
using System.Xml.Serialization;

namespace UtilitySystems.XmlDatabase {
    /// <summary>
    /// Base class for assets contained within an abstractXmlDatabase
    /// </summary>
    public abstract class XmlDatabaseAsset : IXmlDatabaseAsset {
        /// <summary>
        /// The Id of the asset
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the asset
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// The prefered string that is used as the type in the xml
        /// </summary>
        public string PerferredTypeString { get { return string.Empty; } }

        /// <summary>
        /// Basic constructor
        /// </summary>
        public XmlDatabaseAsset() {
            Initialize();
        }

        /// <summary>
        /// Constructor that sets the id
        /// </summary>
        public XmlDatabaseAsset(int id) {
            Initialize();
            Id = id;
        }

        /// <summary>
        /// Initializes all values of the asset
        /// </summary>
        public virtual void Initialize() {
            Id = 0;
            Name = string.Empty;
        }

        /// <summary>
        /// Defines how the asset is writen to the xml
        /// </summary>
        public abstract void OnSaveAsset(XmlDatabaseWriter writer);

        /// <summary>
        /// Defines how the asset is read from the xml
        /// </summary>
        public abstract void OnLoadAsset(XmlDatabaseReader reader);
    }
}
