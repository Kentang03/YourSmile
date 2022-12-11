using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ActiveWeapon))]
[RequireComponent(typeof(FireWeaponEvent))]
[RequireComponent(typeof(WeaponFiredEvent))]
[RequireComponent(typeof(AnimateWeapon))]

[DisallowMultipleComponent]
public class FireWeapon : MonoBehaviour
{
    private float firePrechargeTimer = 0f;
    private float fireRateCoolDownTimer = 0f;
    private ActiveWeapon activeWeapon;
    private FireWeaponEvent fireWeaponEvent;
    private ReloadWeaponEvent reloadWeaponEvent;
    private WeaponFiredEvent weaponFiredEvent;
    private AnimateWeapon animateWeapon;

    private void Awake()
    {
        activeWeapon = GetComponent<ActiveWeapon>();
        fireWeaponEvent = GetComponent<FireWeaponEvent>();
        reloadWeaponEvent = GetComponent<ReloadWeaponEvent>();
        weaponFiredEvent = GetComponent<WeaponFiredEvent>();
        animateWeapon = GetComponent<AnimateWeapon>();
    }

    private void OnEnable()
    {
        fireWeaponEvent.OnFireWeapon += FireWeaponEvent_OnFireWeapon;      
    }
    
    private void OnDisable()
    {
        fireWeaponEvent.OnFireWeapon -= FireWeaponEvent_OnFireWeapon;
    }

    private void Update()
    {
        fireRateCoolDownTimer -= Time.deltaTime;
    }

    private void FireWeaponEvent_OnFireWeapon(FireWeaponEvent fireWeaponEvent, FireWeaponEventArgs fireWeaponEventArgs)
    {
        WeaponFire(fireWeaponEventArgs);
    }

    private void WeaponFire(FireWeaponEventArgs fireWeaponEventArgs)
    {
        //Handle weapon precharge timer.
        WeaponPreCharge(fireWeaponEventArgs);
        //Weapon fire.
        if(fireWeaponEventArgs.fire)
        {
            animateWeapon.OpenBook();
            //Test if weapon ready to fire
            if(IsWeaponReadyToFire())
            {
                FireAmmo(fireWeaponEventArgs.aimAngle, fireWeaponEventArgs.weaponAimAngle, fireWeaponEventArgs.weaponAimDirectionVector);

                ResetCoolDownTimer();

                ResetPrechargeTimer();
            }
        }

        else if (!fireWeaponEventArgs.fire && animateWeapon.GetCountdown() == 0)
        {
            animateWeapon.CloseBook();
        }
    }

    private void WeaponPreCharge(FireWeaponEventArgs fireWeaponEventArgs)
    {
        //Weapon precharge
        if (fireWeaponEventArgs.firePreviousFrame)
        {
            //Decrease precharge timer if fire button held previous frame.
            firePrechargeTimer -= Time.deltaTime;
        }

        else
        {
            ResetPrechargeTimer();
        }
    }

    private bool IsWeaponReadyToFire()
    {
        //if there is no ammo and weapon doesnt have infinite ammo the return false.
        if(activeWeapon.GetCurrentWeapon().weaponRemainingAmmo <= 0 && !activeWeapon.GetCurrentWeapon().weaponDetails.hasInfiniteAmmo)
            return false;

        //if weapon reloading return false
        if(activeWeapon.GetCurrentWeapon().isWeaponReloading)
            return false;

        //If the weapon isn't precharged or is cooling down then return false.
        if(firePrechargeTimer > 0f || fireRateCoolDownTimer > 0f)
            return false;

        //If no ammo in the clip and the weapon doesnt  have infinite clip capacity then return false.
        if(activeWeapon.GetCurrentWeapon().weaponClipRemainingAmmo <= 0 && !activeWeapon.GetCurrentWeapon().weaponDetails.hasInfiniteClipCapacity)
        {
            reloadWeaponEvent.CallReloadWeaponEvent(activeWeapon.GetCurrentWeapon(),0);

            return false;
        }

        //weapon is ready to fire - return true
        return true;
    }

    private void FireAmmo(float aimAngle, float weaponAimAngle, Vector3 weaponAimDirectionVector)
    {
        AmmoDetailsSO currentAmmo = activeWeapon.GetCurrentAmmo();

        if(currentAmmo != null)
        {
            //Fire ammo routine.
            StartCoroutine(FireAmmoRoutine(currentAmmo, aimAngle, weaponAimAngle, weaponAimDirectionVector));
        }
    }

    private IEnumerator FireAmmoRoutine(AmmoDetailsSO currentAmmo, float aimAngle, float weaponAimAngle, Vector3 weaponAimDirectionVector)
    {
        int ammoCounter = 0;

        //Get random ammo per shot
        int ammoPerShot = Random.Range(currentAmmo.ammoSpawnAmountMin, currentAmmo.ammoSpawnAmountMax + 1);

        //Get random interval between ammo
        float ammoSpawnInterval;

        if (ammoPerShot > 1)
        {
            ammoSpawnInterval = Random.Range(currentAmmo.ammoSpawnIntervalMin, currentAmmo.ammoSpawnIntervalMax);
        }
        else
        {
            ammoSpawnInterval = 0f;
        }

        while (ammoCounter < ammoPerShot)
        {
            ammoCounter++;

            //Get ammo prefabs from the array
            GameObject ammoPrefab = currentAmmo.ammoPrefabArray[Random.Range(0, currentAmmo.ammoPrefabArray.Length)];

            //Get random speed value
            float ammoSpeed = Random.Range(currentAmmo.ammoSpeedMin, currentAmmo.ammoSpeedMax);

            //Get GameObject from IFireable component
            IFireable ammo = (IFireable)PoolManager.Instance.ReuseComponent(ammoPrefab, activeWeapon.GetShootPosition(), Quaternion.identity);

            //Initialise ammo
            ammo.InitialiseAmmo(currentAmmo, aimAngle, weaponAimAngle, ammoSpeed, weaponAimDirectionVector);

            //Wait for ammo per shot timegap
            yield return new WaitForSeconds(ammoSpawnInterval);

        }

        //Reduce ammo clip count if not infinite clip capacity
        if(!activeWeapon.GetCurrentWeapon().weaponDetails.hasInfiniteClipCapacity)
        {
            activeWeapon.GetCurrentWeapon().weaponClipRemainingAmmo--;
            activeWeapon.GetCurrentWeapon().weaponRemainingAmmo--;
        }

        //Call weapon fired event
        weaponFiredEvent.CallWeaponFiredEvent(activeWeapon.GetCurrentWeapon());
    }

    private void ResetCoolDownTimer()
    {
        fireRateCoolDownTimer = activeWeapon.GetCurrentWeapon().weaponDetails.weaponFireRate;
    }

    private void ResetPrechargeTimer()
    {
        firePrechargeTimer = activeWeapon.GetCurrentWeapon().weaponDetails.weaponPrechargeTime;
    }
}
