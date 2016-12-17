using System.Xml;

namespace UtilitySystems.XmlDatabase {
    public interface IXmlDatabaseAsset : IXmlOnSaveAsset, IXmlOnLoadAsset {
        int Id { get; set; }
        string Name { get; set; }

        string PerferredTypeString { get; set; }
    }
}
