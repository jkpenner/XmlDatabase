using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponObject : ItemObject {
    public WeaponData WeaponData {
        get { return Data as WeaponData; }
    }

    public Transform projectileSpawn;
    public float fireSpeed = 0.5f;
    private Coroutine fireCoroutine;

    protected override void OnInitialize() {
        fireCoroutine = StartCoroutine(FireWeaponContinous());
    }

    public void Initialize(WeaponData data) {
        base.Initialize(data);


    }

    protected override void OnDisable() {
        if (fireCoroutine != null) {
            StopCoroutine(fireCoroutine);
        }
    }

    public IEnumerator FireWeaponContinous() {
        while (true) {
            FireWeapon();
            yield return new WaitForSeconds(fireSpeed);
        }
    }

    public void FireWeapon() {
        if (WeaponData.Projectile != null && projectileSpawn != null) {
            var projectile = GameObject.Instantiate(WeaponData.Projectile);
            projectile.transform.position = projectileSpawn.position;
            projectile.transform.rotation = projectileSpawn.rotation;
        }
    }
}
