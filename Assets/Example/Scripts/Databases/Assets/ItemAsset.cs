using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using UtilitySystems.XmlDatabase;

/// <summary>
/// Example Item class inheriting from the XmlDatabaseAsset
/// </summary>
public class ItemAsset : XmlDatabaseAsset {
    // Name of the Item Element tag
    private const string elementItem = "ItemValues";

    // Name of the Cost Attribute tag
    private const string attrCost = "Cost";
    // Example: How much the item costs to purchase
    public int Cost { get; set; }

    // Name of the Cost Attribute tag
    private const string attrPrefab = "Prefab";
    // Example: GameObject used within the game
    public GameObject Prefab { get; set; }

    /// <summary>
    /// Basic Constructer
    /// </summary>
    public ItemAsset() { }
    /// <summary>
    /// Constructer that assigns the asset's Id
    /// </summary>
    public ItemAsset(int id) : base(id) { }

    /// <summary>
    /// Reads the asset's values from the passed xml reader
    /// </summary>
    public override void OnLoadAsset(XmlDatabaseReader reader) {
        if(reader.IsStartElement(elementItem)) {
            Cost = reader.GetAttrInt(attrCost);
            Prefab = reader.GetAttrResource<GameObject>(attrPrefab);
        }
    }

    /// <summary>
    /// Writes the asset's values to the passed xml writer
    /// </summary>
    /// <param name="writer"></param>
    public override void OnSaveAsset(XmlDatabaseWriter writer) {
        writer.StartElement(elementItem);
        writer.SetAttr(attrCost, Cost);
        writer.SetAttr(attrPrefab, Prefab);
        writer.EndElement();
    }

    /// <summary>
    /// Creates an instance of ItemData based off this asset
    /// </summary>
    public virtual ItemData CreateInstance() {
        return new ItemData(this);
    }

    /// <summary>
    /// Creates a GameObject with a component of type T which inherits
    /// from an ItemObject. Then assigns the ItemObject component's
    /// Data property to an instance of this asset.
    /// </summary>
    protected T Internal_CreateObjectInstance<T>() where T : ItemObject {
        var gameObject = GameObject.Instantiate(Prefab);

        T itemObject = gameObject.GetComponent<T>();
        if (itemObject == null) {
            itemObject = gameObject.AddComponent<T>();
        }
        itemObject.Initialize(CreateInstance());
        return itemObject;
    }

    /// <summary>
    /// Creates a GameObject with an ItemObject component with an
    /// instance of this asset as its data value.
    /// </summary>
    public virtual ItemObject CreateObjectInstance() {
        return Internal_CreateObjectInstance<ItemObject>();
    }
}
