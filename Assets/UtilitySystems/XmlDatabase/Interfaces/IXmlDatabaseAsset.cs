using System.Xml;

namespace UtilitySystem.XmlDatabase {
    public interface IXmlDatabaseAsset {
        int Id { get; set; }
        string Name { get; set; }

        void OnSaveAsset(XmlWriter writer);
        void OnLoadAsset(XmlReader reader);
    }
}
