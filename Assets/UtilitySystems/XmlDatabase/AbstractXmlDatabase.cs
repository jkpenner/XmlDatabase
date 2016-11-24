using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using System.Xml;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace UtilitySystem.XmlDatabase {
    /// <summary>
    /// Abstract Xml Database with that can create and save a database to an xml file.
    /// </summary>
    public abstract class AbstractXmlDatabase<T> where T : class, IXmlDatabaseAsset {
        /// <summary>
        /// Creates a instance of an asset given a string of the asset type
        /// </summary>
        protected abstract T CreateAssetOfType(string type);

        /// <summary>
        /// The path to the database file within the StreamingAssets folder.
        /// Example: Folder1/DatabaseFolder/
        /// </summary>
        public abstract string DatabasePath { get; }

        /// <summary>
        /// The name of the database file. Example: database.xml
        /// </summary>
        public abstract string DatabaseName { get; }

        /// <summary>
        /// Dictionary containing all loaded assets with their ids as keys
        /// </summary>
        protected Dictionary<int, T> AssetDict { get; private set; }

        public Dictionary<int, T>.ValueCollection GetAssets() {
            return AssetDict.Values;
        }

        public AbstractXmlDatabase() {
            AssetDict = new Dictionary<int, T>();
        }

        /// <summary>
        /// Add an asset to the database
        /// </summary>
        public void Add(T obj) {
            if (obj != null) {
                if (AssetDict.ContainsKey(obj.Id) == false) {
                    AssetDict.Add(obj.Id, obj);
                } else {
                    Debug.LogWarningFormat("[{0}]: Attempting to add asset {1} with Id {2}, but id is already assigned to {3}. " +
                        "Asset will not be added to the database.", DatabaseName, obj.Name, obj.Id, AssetDict[obj.Id]);
                }
            }
        }

        public T GetWithId(int id) {
            return GetWithId(id, false, false);
        }

        /// <summary>
        /// Get the asset with the given id
        /// </summary>
        public T GetWithId(int id, bool tryToLoadAsset, bool addAssetToDatabaseOnLoad = false) {
            T asset;
            if (TryGetWithId(id, out asset, tryToLoadAsset, addAssetToDatabaseOnLoad)) {
                return asset;
            }
            return default(T);
        }

        /// <summary>
        /// Checks the Instance of the database for an asset with the given Id. Also can
        /// try to load the asset from the database file if no asset is initially found.
        /// </summary>
        /// <param name="id">Id of asset</param>
        /// <param name="asset">Assigned to if asset is found</param>
        /// <param name="tryToLoadAsset">Try to load asset from file if not currently loaded</param>
        /// <returns>If an asset with the passed Id was found</returns>
        public bool TryGetWithId(int id, out T asset, bool tryToLoadAsset = false, bool addAssetToDatabaseOnLoad = false) {
            // Check if asset is already in the database
            if (AssetDict.ContainsKey(id)) {
                asset = AssetDict[id];
                return true;
            }

            // Attempt to load the asset, if not in database
            if (tryToLoadAsset == true) {
                asset = LoadById(id);
                if (asset != null && addAssetToDatabaseOnLoad) {
                    Add(asset);
                }
            } else {
                asset = null;
            }

            return asset != null;
        }

        public int GetNextId() {
            int maxId = 0;
            foreach (var asset in AssetDict.Values) {
                if (asset.Id > maxId) {
                    maxId = asset.Id;
                }
            }
            return maxId + 1;
        }

        public int GetFirstAvailableId() {
            if (GetCount() <= 0) {
                return 1;
            } else {
                int targetId = 1;
                bool foundUsableId = false;
                while (!foundUsableId) {
                    foundUsableId = true;
                    foreach (var asset in AssetDict.Values) {
                        if (asset.Id == targetId) {
                            foundUsableId = false;
                            targetId++;
                            break;
                        }
                    }
                }
                return targetId;
            }
        }

        /// <summary>
        /// Get the number of assets in the database
        /// </summary>
        /// <returns></returns>
        public int GetCount() {
            return AssetDict.Count;
        }

        /// <summary>
        /// Remove the asset with the given id
        /// </summary>
        public void Remove(int id) {
            if (AssetDict.ContainsKey(id)) {
                AssetDict.Remove(id);
            }
        }

        /// <summary>
        /// Replaces the asset at the given index with a different asset
        /// </summary>
        public void Replace(int id, T obj) {
            if (AssetDict.ContainsKey(id)) {
                AssetDict[id] = obj;
                obj.Id = id;
            }
        }

        /// <summary>
        /// Reads a single asset from the xml reader
        /// </summary>
        private T ReadAsset(XmlReader reader) {
            // Get the basic values for the asset
            int id = int.Parse(reader.GetAttribute("Id"));
            string name = reader.GetAttribute("Name");
            string type = reader.GetAttribute("Type");

            // Create a new instance of the read type
            var asset = CreateAssetOfType(type);
            if (asset != null) {
                // Assign the read values
                asset.Id = id;
                asset.Name = name;

                // Reached end of asset, return created asset
                if (reader.IsStartElement() && reader.IsEmptyElement) return asset;

                // read the asset until we reach the end element
                while (reader.Read()) {
                    if (reader.NodeType == XmlNodeType.EndElement) {
                        if (reader.Name == "Asset") {
                            break;
                        }
                    }

                    // Check if we are reading an element of the asset
                    if (reader.NodeType == XmlNodeType.Element) {
                        // Make the object load it's data
                        asset.OnLoadAsset(reader);
                    }
                }
            } else {
                Debug.LogFormat("[{0}]: Unhandled asset type of {1} assigned to asset with id {2}.", DatabaseName, type, id);
            }

            // Return the asset as the database's asset type
            return asset as T;
        }

        /// <summary>
        /// Finds the asset with the given id in the database's xml file,
        /// then loads and returns the asset if found.
        /// </summary>
        public T LoadById(int id) {
            CreateDatabaseIfMissing();

            // Get the database file stream
            using (Stream stream = GetFileStreamToLoad()) {

                // If no file is found return the default value
                if (stream == null) {
                    Debug.LogFormat("[{0}]: Could not load asset. No load stream found.", DatabaseName);
                    return default(T);
                }

                // ToDo: Decrypt the database file if needed

                // Get a xml reader from the file stream
                using (XmlReader reader = XmlReader.Create(stream)) {
                    // Read through the xml file till we find an asset element
                    while (reader.Read()) {
                        if (reader.NodeType == XmlNodeType.Element) {
                            if (reader.Name == "Asset") {
                                // If the asset element has a matching id, read the asset
                                if (int.Parse(reader.GetAttribute("Id")) == id) {
                                    return ReadAsset(reader);
                                }
                            }
                        }
                    }
                }
            }

            // No asset was found in the database
            return default(T);
        }

        /// <summary>
        /// Loads all asset into the database.
        /// Clears any assets already in the database.
        /// </summary>
        public void LoadAssets() {
            LoadAssets(true);
        }

        /// <summary>
        /// Loads all assets into the database. Has 
        /// option to clear assets already in the database.
        /// </summary>
        public void LoadAssets(bool clear) {
            CreateDatabaseIfMissing();

            // Remove previous asset from database if clear is true
            if (clear == true) {
                AssetDict.Clear();
            }

            // Get the database file stream
            using (Stream stream = GetFileStreamToLoad()) {
                if (stream == null) {
                    Debug.LogFormat("[{0}]: Could not load assets. No load stream found.", DatabaseName);
                    return;
                }

                // ToDo: Decrypt the database file if needed

                // Get a xml reader from the file stream
                using (XmlReader reader = XmlReader.Create(stream)) {
                    // Read through the xml file till we find an asset element
                    while (reader.Read()) {
                        if (reader.NodeType == XmlNodeType.Element) {
                            if (reader.Name == "Asset") {
                                // Read the asset and add it to the database
                                var asset = ReadAsset(reader);
                                if (asset != null) {
                                    if (AssetDict.ContainsKey(asset.Id)) {
                                        Debug.LogWarningFormat("[{0}]: Asset {1} has Id {2}, but id is already assigned to {3}",
                                            DatabaseName, asset.Name, asset.Id, AssetDict[asset.Id]);
                                    } else {
                                        AssetDict.Add(asset.Id, asset);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Saves all assets in the database to the database xml file
        /// </summary>
        public void SaveAssets() {
            CreateDatabaseIfMissing();

            // Create xml writer settings
            var settings = new XmlWriterSettings();
            settings.Indent = true;

            // Create an xml file at the database path
            using (var stream = File.Create(GetDatabaseFullPath())) {
                using (XmlWriter writer = XmlWriter.Create(stream, settings)) {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("Assets");

                    // Write All Assets in the database to file
                    foreach (var asset in AssetDict.Values) {
                        if (asset == null) {
                            Debug.LogWarningFormat("[{0}]: Asset is null. Skipping", DatabaseName);
                            continue;
                        }

                        // Write the start Asset element and attributes
                        writer.WriteStartElement("Asset");
                        writer.WriteAttributeString("Id", asset.Id.ToString());
                        writer.WriteAttributeString("Name", asset.Name);
                        writer.WriteAttributeString("Type", asset.GetType().Name);

                        // Make the object save it's values
                        asset.OnSaveAsset(writer);

                        // Write the end Asset Element
                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }
            }

            // ToDo: Encrypt the database file if needed

#if UNITY_EDITOR
            // Editor only: Save the asset and refresh the editor
            // to display the newly created asset
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
#endif
        }

        /// <summary>
        /// Get the database path based off the Streaming Assets folder in the Application's database 
        /// </summary>
        public string GetDatabaseFullPath() {
            return string.Format(@"{0}/StreamingAssets/{1}{2}", Application.dataPath, DatabasePath, DatabaseName);
        }

        /// <summary>
        /// Get the file stream to read the xml from
        /// </summary>
        public Stream GetFileStreamToLoad() {
            return File.OpenRead(GetDatabaseFullPath());
        }

        /// <summary>
        /// Check if the database exists. If the database is missing
        /// create a new xml file with default values.
        /// </summary>
        public void CreateDatabaseIfMissing() {
            if (!File.Exists(GetDatabaseFullPath())) {

                Debug.LogFormat(@"[{0}]: No database found at {1}. Creating a new empty database file.", DatabaseName, GetDatabaseFullPath());
                Directory.CreateDirectory(string.Format("{0}/StreamingAssets/{1}", Application.dataPath, DatabasePath));

                // Write the default xml file
                var settings = new XmlWriterSettings();
                settings.Indent = true;
                using (Stream newFileStream = File.Create(GetDatabaseFullPath())) {
                    //using (var stringWriter = new StringWriter()) {
                    using (XmlWriter writer = XmlWriter.Create(newFileStream, settings)) {
                        writer.WriteStartDocument();
                        writer.WriteStartElement("Assets");

                        writer.WriteEndElement();
                        writer.WriteEndDocument();
                    }
                    //File.WriteAllText(GetDatabaseFullPath(), stringWriter.ToString());
                }

#if UNITY_EDITOR
                // Editor only: Save the asset and refresh the editor
                // to display the newly created asset
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
#endif
            }
        }
    }
}