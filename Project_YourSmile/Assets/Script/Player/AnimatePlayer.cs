using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
[DisallowMultipleComponent]

public class AnimatePlayer : MonoBehaviour
{
    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void OnEnable()
    {
        //Subscribe to Movement by velocity event
        player.movementByVelocityEvent.OnMovementByVelocity += MovementByVelocityEvent_OnMovementByVelocity;

        //Subscribe to idle event
        player.idleEvent.OnIdle += IdleEvent_OnIdle;

        //Subscribe to weapon aim event
        player.aimWeaponEvent.OnWeaponAim += AimWeaponEvent_OnWeaponAim;
    }

    private void OnDisable()
    {
        //Unsubscribe to Movement by velocity event
        player.movementByVelocityEvent.OnMovementByVelocity -= MovementByVelocityEvent_OnMovementByVelocity;

        //Unsubscribe to idle event
        player.idleEvent.OnIdle -= IdleEvent_OnIdle;

        //Unsubscribe to weapon aim event
        player.aimWeaponEvent.OnWeaponAim -= AimWeaponEvent_OnWeaponAim;
    }

    private void MovementByVelocityEvent_OnMovementByVelocity(MovementByVelocityEvent movementByVelocityEvent, MovementByVelocityArgs movementByVelocityArgs)
    {
        SetMovementAnimationParameters();
    }

    private void IdleEvent_OnIdle(IdleEvent idleEvent)
    {
        SetIdleAnimationParameters();
    }

    private void AimWeaponEvent_OnWeaponAim(AimWeaponEvent aimWeaponEvent, AimWeaponEventArgs aimWeaponEventArgs)
    {
        InitializeAimAnimationParameters();

        SetAimWeaponAnimationParameters(aimWeaponEventArgs.aimDirection);
    }

    //Set Movement animation parameters
    private void SetIdleAnimationParameters()
    {
        player.animator.SetBool(GameSettings.isMoving, false);
        player.animator.SetBool(GameSettings.isIdle,true);
    }
    
    //Initialize aim Animation Parameters
    private void InitializeAimAnimationParameters()
    {
        player.animator.SetBool(GameSettings.aimRight,false);
        player.animator.SetBool(GameSettings.aimLeft,false);
        /*
        player.animator.SetBool(GameSettings.aimUp,false);
        player.animator.SetBool(GameSettings.aimUpRight,false);
        player.animator.SetBool(GameSettings.aimUpLeft,false);
        player.animator.SetBool(GameSettings.aimDown,false);
        */
    }

    private void SetMovementAnimationParameters()
    {
        player.animator.SetBool(GameSettings.isMoving, true);
        player.animator.SetBool(GameSettings.isIdle, false);
    }
    
    //Set aim Animation Parameters
    private void SetAimWeaponAnimationParameters(AimDirection aimDirection)
    {
        switch (aimDirection)
        {   
            case AimDirection.Right:
                player.animator.SetBool(GameSettings.aimRight,true);
                break;
            
            case AimDirection.Left:
                player.animator.SetBool(GameSettings.aimLeft,true);
                break;

            /*
            case AimDirection.Up:
                player.animator.SetBool(GameSettings.aimUp,true);
                break;

            case AimDirection.UpRight:
                player.animator.SetBool(GameSettings.aimUpRight,true);
                break;

            case AimDirection.UpLeft:
                player.animator.SetBool(GameSettings.aimUpLeft,true);
                break;

            case AimDirection.Down:
                player.animator.SetBool(GameSettings.aimDown,true);
                break;
            */
        }

    }
    
}
