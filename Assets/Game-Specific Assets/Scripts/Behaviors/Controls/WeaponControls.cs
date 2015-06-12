using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponControls : DebuggableBehavior, ISuspendable 
{
    #region Variables / Properties

    public bool CanFire = true;

    public GameObject Muzzle;
    public GameObject Projectile;
    public Lockout WeaponLockout;

    public Action<GameObject> OnFireWeapon;

    #endregion Variables / Properties

    #region Hooks

    public void Start()
    {
        Muzzle = transform.FindChild("Muzzle").gameObject;
    }

    public void Suspend()
    {
        CanFire = false;
    }

    public void Resume()
    {
        CanFire = true;
    }

    #endregion Hooks

    #region Methods

    public void Fire()
    {
        if (!CanFire)
            return;

        if (!WeaponLockout.CanAttempt())
            return;

        FireProjectile();
        WeaponLockout.NoteLastOccurrence();
    }

    private void FireProjectile()
    {
        Vector3 spawnAt = Muzzle.transform.position;
        Quaternion spawnRotation = Muzzle.transform.rotation;

        GameObject projectile = (GameObject) GameObject.Instantiate(Projectile, spawnAt, spawnRotation);

        if(OnFireWeapon != null)
            OnFireWeapon(projectile);
    }

    #endregion Methods
}
