using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ReloadWeaponEvent))]
[RequireComponent(typeof(WeaponReloadedEvent))]
[RequireComponent(typeof(SetActiveWeaponEvent))]

[DisallowMultipleComponent]
public class ReloadWeapon : MonoBehaviour
{
    private ReloadWeaponEvent reloadWeaponEvent;
    private WeaponReloadedEvent weaponReloadedEvent;
    private SetActiveWeaponEvent setActiveWeaponEvent;
    private Coroutine reloadWeaponCoroutine;

    private void Awake() 
    {
        reloadWeaponEvent = GetComponent<ReloadWeaponEvent>();
        weaponReloadedEvent = GetComponent<WeaponReloadedEvent>();
        setActiveWeaponEvent = GetComponent<SetActiveWeaponEvent>();
    }

    private void OnEnable()
    {
        //Subs to reload weapon event
        reloadWeaponEvent.OnReloadWeapon += ReloadWeaponEvent_OnReloadWeapon;
        
        //Subs to set active weapon event
        setActiveWeaponEvent.OnSetActiveWeapon += SetActiveWeaponEvent_OnSetActiveWeapon;
    }
    
    private void OnDisable()
    {
        //Subs to reload weapon event
        reloadWeaponEvent.OnReloadWeapon -= ReloadWeaponEvent_OnReloadWeapon;
        
        //Subs to set active weapon event
        setActiveWeaponEvent.OnSetActiveWeapon -= SetActiveWeaponEvent_OnSetActiveWeapon;
    }

    private void ReloadWeaponEvent_OnReloadWeapon(ReloadWeaponEvent reloadWeaponEvent, ReloadWeaponEventArgs reloadWeaponEventArgs)
    {
        StartReloadWeapon(reloadWeaponEventArgs);
    }

    private void StartReloadWeapon(ReloadWeaponEventArgs reloadWeaponEventArgs)
    {
        if(reloadWeaponCoroutine != null)
        {
            StopCoroutine(reloadWeaponCoroutine);
        }

        reloadWeaponCoroutine = StartCoroutine(ReloadWeaponRoutine(reloadWeaponEventArgs.weapon, reloadWeaponEventArgs.topupAmmoPercent));
    }

    private IEnumerator ReloadWeaponRoutine(Weapon weapon, int topupAmmoPercent)
    {
        //Set weapon as reloading
        weapon.isWeaponReloading = true;

        //update reload progress timer
        while(weapon.weaponReloadTimer < weapon.weaponDetails.weaponReloadTime)
        {
            weapon.weaponReloadTimer += Time.deltaTime;
            yield return null;
        }

        //If total ammo is to be increased then update
        if(topupAmmoPercent != 0)
        {
            int ammoIncrease = Mathf.RoundToInt((weapon.weaponDetails.weaponAmmoCapacity * topupAmmoPercent) / 100f);

            int totalAmmo = weapon.weaponRemainingAmmo + ammoIncrease;

            if(totalAmmo > weapon.weaponDetails.weaponAmmoCapacity)
            {
                weapon.weaponRemainingAmmo = weapon.weaponDetails.weaponAmmoCapacity;
            }
            else
            {
                weapon.weaponRemainingAmmo = totalAmmo;
            }
            
        }

        //If weapon has infinite ammo then just refill clip
        if(weapon.weaponDetails.hasInfiniteAmmo)
        {
            weapon.weaponClipRemainingAmmo = weapon.weaponDetails.weaponClipAmmoCapacity;
        }
        else if(weapon.weaponRemainingAmmo >= weapon.weaponDetails.weaponAmmoCapacity)
        {
            weapon.weaponClipRemainingAmmo = weapon.weaponDetails.weaponClipAmmoCapacity;
        }
        else
        {
            weapon.weaponClipRemainingAmmo = weapon.weaponRemainingAmmo;
        }

        weapon.weaponReloadTimer = 0f;
        weapon.isWeaponReloading = false;
        weaponReloadedEvent.CallWeaponReloadedEvent(weapon);

    }

    private void SetActiveWeaponEvent_OnSetActiveWeapon(SetActiveWeaponEvent setActiveWeaponEvent, SetActiveWeaponEventArgs setActiveWeaponEventArgs)
    {
        if(setActiveWeaponEventArgs.weapon.isWeaponReloading)
        {
            if (reloadWeaponCoroutine != null)
            {
                StopCoroutine(reloadWeaponCoroutine);
            }

            reloadWeaponCoroutine = StartCoroutine(ReloadWeaponRoutine(setActiveWeaponEventArgs.weapon, 0));
        }
    }


}
