using UnityEngine;
using System.Collections;
using System.Xml;
using UtilitySystems.XmlDatabase;

/// <summary>
/// Wrapper class to simplify the XmlWriter
/// </summary>
public class XmlDatabaseWriter {
    public XmlWriter Writer { get; private set; }

    public XmlDatabaseWriter(XmlWriter writer) {
        this.Writer = writer;
    }

    public void StartElement(string name) {
        Writer.WriteStartElement(name);
    }

    public void SetElementValue(string value) {
        Writer.WriteString(value);
    }

    public void EndElement() {
        Writer.WriteEndElement();
    }

    public void SetAttr(string name, int value) {
        Writer.WriteAttributeString(name, value.ToString());
    }

    public void SetAttr(string name, float value) {
        Writer.WriteAttributeString(name, value.ToString());
    }

    public void SetAttr(string name, bool value) {
        Writer.WriteAttributeString(name, value.ToString());
    }

    public void SetAttr(string name, string value) {
        Writer.WriteAttributeString(name, value);
    }

    public void SetAttr<T>(string name, T value) where T : UnityEngine.Object {
        Writer.WriteAttributeString(name, XmlDatabaseUtility.GetAssetResourcePath(value));
    }
}
