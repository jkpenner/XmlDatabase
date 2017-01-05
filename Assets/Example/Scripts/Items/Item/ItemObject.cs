using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour {
    public ItemData Data { get; private set; }

    public T GetDataAs<T>() where T : ItemData {
        return Data as T;
    }

    public void Initialize(ItemData data) {
        Data = data;
        OnInitialize();
    }

    protected virtual void OnInitialize() { }
    protected virtual void OnDisable() { }
}
