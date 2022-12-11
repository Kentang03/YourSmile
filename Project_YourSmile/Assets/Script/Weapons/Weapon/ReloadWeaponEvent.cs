using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[DisallowMultipleComponent]
public class ReloadWeaponEvent : MonoBehaviour
{
    public event Action<ReloadWeaponEvent, ReloadWeaponEventArgs> OnReloadWeapon;
    
    public void CallReloadWeaponEvent(Weapon weapon, int topupAmmoPercent)
    {
        OnReloadWeapon?.Invoke(this, new ReloadWeaponEventArgs()
        {
            weapon = weapon,
            topupAmmoPercent = topupAmmoPercent
        });
    }
}

public class ReloadWeaponEventArgs : EventArgs
{
    public Weapon weapon;
    public int topupAmmoPercent;
}
