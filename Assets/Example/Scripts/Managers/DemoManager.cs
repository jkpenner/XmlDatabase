using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;

public class DemoManager : MonoBehaviour {
    public Transform spawnPoint;

    public Text objectName;
    public Text objectCost;

    public GameObject weaponGroup;
    public Text objectRange;
    public Text objectDamage;

    private ItemObject activeItemObject = null;
    public ItemObject ActiveItemObject {
        get { return activeItemObject; }
        set {
            if (activeItemObject != value) {

                if (activeItemObject != null) {
                    Destroy(activeItemObject.gameObject);
                } 
                
                activeItemObject = value;
                activeItemObject.transform.SetParent(spawnPoint);
                activeItemObject.transform.localPosition = Vector3.zero;

                var weaponObject = activeItemObject as WeaponObject;
                if (weaponObject != null) {
                    weaponObject.FireWeapon();
                }
                UpdateAcitveItemUI();
            }
        }
    }

    private int[] assetIds = null;
    private int index = 0;


    public void Start() {
        assetIds = DatabaseManager.ItemAssets.GetIds().ToArray();
        if (assetIds.Length == 0) {
            Debug.Log("No assets are in the item database");
        } else {
            Debug.Log("Asset count is " + assetIds.Length);
            SetActiveItem(assetIds[index]);
        }
    }

    public void ModifyAcitveIndex(int value) {

        index += value;
        if (index < 0) {
            index = assetIds.Length - 1;
        }

        if (index >= assetIds.Length) {
            index = 0;
        }

        SetActiveItem(assetIds[index]);
    }

    private void SetActiveItem(int itemId) {
        var itemAsset = DatabaseManager.ItemAssets.Get(itemId);
        if (itemAsset != null) {
            ActiveItemObject = itemAsset.CreateObjectInstance();
        } else {
            ActiveItemObject = null;
        }

        //var asset = DatabaseManager.ItemAssets.Get(itemId);
        //if (asset != null) {
        //    ActiveItem = asset.CreateInstance();
        //}
    }

    private void UpdateAcitveItemUI() {
        if (ActiveItemObject != null) {
            objectName.text = string.Format("Id {0}, Name: {1}", ActiveItemObject.Data.Id, ActiveItemObject.Data.Name);
            objectCost.text = string.Format("Cost: {0}", ActiveItemObject.Data.Cost);

            var activeWeapon = ActiveItemObject.GetDataAs<WeaponData>();
            if (activeWeapon != null) {
                weaponGroup.SetActive(true);
                objectDamage.text = string.Format("Damage: {0}", activeWeapon.Damage);
                objectRange.text = string.Format("Range: {0}", activeWeapon.Range);
            } else {
                weaponGroup.SetActive(false);
            }
        } else {
            objectName.text = "Active Item Not Set";
            objectCost.text = "";
        }
    }
}
