using UnityEngine;

[RequireComponent(typeof(Enemy))]
[DisallowMultipleComponent]
public class AnimateEnemy : MonoBehaviour
{
    private Enemy enemy;

    private void Awake()
    {
         enemy = GetComponent<Enemy>();
    }

    private void OnEnable()
    {
         enemy.movementToPositionEvent.OnMovementToPosition += MovementToPositionEvent_OnMovementToPosition;

         enemy.idleEvent.OnIdle += IdleEvent_OnIdle;
    }

    private void OnDisable()
    {
         enemy.movementToPositionEvent.OnMovementToPosition -= MovementToPositionEvent_OnMovementToPosition;

         enemy.idleEvent.OnIdle -= IdleEvent_OnIdle;
    }

    private void MovementToPositionEvent_OnMovementToPosition(MovementToPositionEvent movementToPositionEvent, MovementToPositionArgs movementToPositionArgs)
    {
        if (enemy.transform.position.x < GameManager.Instance.GetPlayer().transform.position.x)
        {
            SetAimWeaponAnimationParameters(AimDirection.Right);
        }
        else
        {
            SetAimWeaponAnimationParameters(AimDirection.Left);    
        }

        SetMovementAnimationParameters();

    }
    private void IdleEvent_OnIdle(IdleEvent idleEvent)
    {
        SetIdleAnimationParameters();
    }

    private void InitialiseAimAnimationParameters()
    {
        enemy.animator.SetBool(GameSettings.aimRight, false);
        enemy.animator.SetBool(GameSettings.aimLeft, false);
    }

    private void SetMovementAnimationParameters()
    {
        enemy.animator.SetBool(GameSettings.isIdle, false);
        enemy.animator.SetBool(GameSettings.isMoving, true);
    }

    private void SetIdleAnimationParameters()
    {
        enemy.animator.SetBool(GameSettings.isIdle, true);
        enemy.animator.SetBool(GameSettings.isMoving, false);
    }

    private void SetAimWeaponAnimationParameters(AimDirection aimDirection)
    {
        InitialiseAimAnimationParameters();

        switch (aimDirection)
        {
            case AimDirection.Right:
                enemy.animator.SetBool(GameSettings.aimRight, true);
                break;
            
            case AimDirection.Left:
                enemy.animator.SetBool(GameSettings.aimLeft, true);
                break;
        }
    }

}
