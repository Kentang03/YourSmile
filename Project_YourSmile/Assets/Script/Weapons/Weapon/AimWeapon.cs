using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AimWeaponEvent))]
[DisallowMultipleComponent]

public class AimWeapon : MonoBehaviour
{
   [SerializeField] private Transform weaponRotationPointTransform;
   //[SerializeField] private Transform arrowRotationPointTransform;
   private AimWeaponEvent aimWeaponEvent;
   
   private void Awake() 
   {
        //Load Components
        aimWeaponEvent = GetComponent<AimWeaponEvent>();
   }

   private void OnEnable()
   {
        //Subscribe to aim weapon event
        aimWeaponEvent.OnWeaponAim += AimWeaponEvent_OnWeaponAim;
   }

   private void OnDisable()
   {
        //Unsubscribe to aim weapon event
        aimWeaponEvent.OnWeaponAim -= AimWeaponEvent_OnWeaponAim;
   }

    //Aim weapon event handler
   private void AimWeaponEvent_OnWeaponAim(AimWeaponEvent aimWeaponEvent, AimWeaponEventArgs aimWeaponEventArgs)
   {
        Aim(aimWeaponEventArgs.aimDirection, aimWeaponEventArgs.aimAngle);
   }
    
    //Aim the weapon
   private void Aim(AimDirection aimDirection, float aimAngle)
   {
        //set angle of the weapon transform
        weaponRotationPointTransform.eulerAngles = new Vector3(0f,0f,aimAngle);

        //arrowRotationPointTransform.eulerAngles = new Vector3(0f,0f,aimAngle);

        //Flip weapon transform based on player direction
        switch (aimDirection)
        {
          case AimDirection.Left:
          case AimDirection.UpLeft:
               weaponRotationPointTransform.localScale = new Vector3(1f,-1f,0f);
               break;
          case AimDirection.Up:
          case AimDirection.UpRight:
          case AimDirection.Right:
          case AimDirection.Down:
               weaponRotationPointTransform.localScale = new Vector3(1f,1f,0f);
               break;
               
        }
   }
    
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckNullValue(this, nameof(weaponRotationPointTransform),weaponRotationPointTransform);
    }
    
   
}
