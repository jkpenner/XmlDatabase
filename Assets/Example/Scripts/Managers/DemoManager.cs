using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;

public class DemoManager : MonoBehaviour {
    public Transform spawnPoint;
    private GameObject activeItemInstance;

    public Text objectName;
    public Text objectCost;

    public GameObject weaponGroup;
    public Text objectRange;
    public Text objectDamage;

    private Item activeItem = null;
    public Item ActiveItem {
        get { return activeItem; }
        set {
            if (activeItem != value) {
                activeItem = value;
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
        var asset = DatabaseManager.ItemAssets.Get(itemId);
        if (asset != null) {
            ActiveItem = asset.CreateInstance();
        }
    }

    private void UpdateAcitveItemUI() {
        if (ActiveItem != null) {
            objectName.text = string.Format("Id {0}, Name: {1}", ActiveItem.Id, ActiveItem.Name);
            objectCost.text = string.Format("Cost: {0}", ActiveItem.Cost);

            var activeWeapon = ActiveItem as Weapon;
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

        if (activeItemInstance != null) {
            Destroy(activeItemInstance);
        }

        if (ActiveItem != null) {
            activeItemInstance = Instantiate(ActiveItem.Prefab, spawnPoint, false) as GameObject;
        }
    }
}
