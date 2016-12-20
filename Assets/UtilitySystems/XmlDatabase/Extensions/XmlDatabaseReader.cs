using UnityEngine;
using System.Collections;
using System.Xml;

public class XmlDatabaseReader {
    public XmlReader Reader { get; private set; }

    public XmlDatabaseReader(XmlReader reader) {
        this.Reader = reader;
    }

    public bool IsStartElement(string name) {
        return Reader.IsStartElement(name);
    }

    public string GetAttrString(string name) {
        return GetAttrString(name, string.Empty);
    }

    public string GetAttrString(string name, string defaultValue) {
        string value = Reader.GetAttribute(name);
        if (value == string.Empty) {
            return defaultValue;
        }
        return value;
    }

    public int GetAttrInt(string name) {
        return GetAttrInt(name, 0);
    }

    public int GetAttrInt(string name, int defaultValue) {
        string strValue = Reader.GetAttribute(name);
        if (strValue == string.Empty) {
            return defaultValue;
        }

        int value;
        if (int.TryParse(strValue, out value)) {
            return value;
        }

        return defaultValue;
    }

    public float GetAttrFloat(string name) {
        return GetAttrFloat(name, 0f);
    }

    public float GetAttrFloat(string name, float defaultValue) {
        string strValue = Reader.GetAttribute(name);
        if (strValue == string.Empty) {
            return defaultValue;
        }

        float value;
        if (float.TryParse(strValue, out value)) {
            return value;
        }

        return defaultValue;
    }

    public bool GetAttrBool(string name) {
        return GetAttrBool(name, false);
    }

    public bool GetAttrBool(string name, bool defaultValue) {
        string strValue = Reader.GetAttribute(name);
        if (strValue == string.Empty) {
            return defaultValue;
        }

        bool value;
        if (bool.TryParse(strValue, out value)) {
            return value;
        }

        return defaultValue;
    }

    public EnumType GetAttrEnum<EnumType>(string name) {
        return GetAttrEnum(name, default(EnumType));
    }

    public EnumType GetAttrEnum<EnumType>(string name, EnumType defaultValue) {
        string strValue = Reader.GetAttribute(name);
        if (strValue == string.Empty) {
            return defaultValue;
        }

        try {
            var parseValue = System.Enum.Parse(typeof(EnumType), strValue);
            if (System.Enum.IsDefined(typeof(EnumType), parseValue)) {
                return (EnumType)parseValue;
            }
            return defaultValue;
        } catch {
            Debug.Log("Error occured whil parsing enum value. Returning default value.");
            return defaultValue;
        }
    }

    public T GetAttrResource<T>(string name) where T : UnityEngine.Object {
        return GetAttrResource<T>(name, null);
    }

    public T GetAttrResource<T>(string name, T value) where T : UnityEngine.Object {
        string path = GetAttrString(name, "");
        if (!string.IsNullOrEmpty(path)) {
            T t = Resources.Load<T>(path);
            if (t != null) {
                return t;
            } else {
                Debug.LogWarning("No " + t.GetType().Name + " found at path: " + path);
            }
        }

        return value;
    }
}
