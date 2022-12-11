using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
[DisallowMultipleComponent]
public class PlayerControl : MonoBehaviour
{
    [SerializeField] private MovementDetailsSO movementDetails;
    
    private Player player;
    private bool leftMouseDownPreviousFrame = false;
    private int currentWeaponIndex = 1;
    private float moveSpeed;
    private Coroutine playerDashCoroutine;
    private WaitForFixedUpdate waitForFixedUpdate;
    [HideInInspector] public bool isPlayerDashing = false;
    private float playerDashCooldownTimer = 0f;

    private void Awake()
    {
        player = GetComponent<Player>();
        moveSpeed = movementDetails.GetMoveSpeed();
    }

    private void Start()
    {
        //Set Starting weapon
        SetStartingWeapon();

        //Set player animation speed
        
    }

    private void SetStartingWeapon()
    {
        int index = 1;

        foreach (Weapon weapon in player.weaponList)
        {
            if(weapon.weaponDetails == player.playerDetails.startingWeapon)
            {
                SetWeaponByIndex(index);
                break;
            }
            index++;
            
        }
    }

    private void Update()
    {
        //if player is rolling then return
        if (isPlayerDashing) return;

        //process player movement input
        MovementInput();

        //process player weapon input
        WeaponInput();

        //Player roll cooldown timer
        PlayerDashCooldownTimer();
    }

    private void MovementInput()
    {
        //Get Movement Input
        float horizontalMovement = Input.GetAxisRaw("Horizontal");
        float verticalMovement = Input.GetAxisRaw("Vertical");

        bool rightMouseButonDown = Input.GetMouseButtonDown(1);

        //Create a direction vector based on the input
        Vector2 direction = new Vector2(horizontalMovement, verticalMovement);

        //Adjust distance for diagonal movement
        if(horizontalMovement != 0f && verticalMovement != 0f) 
        {
            direction *= 0.7f;   
        }

        //If there is movement
        if(direction != Vector2.zero)
        {
            if(!rightMouseButonDown)
            {
                //trigger movement event
                player.movementByVelocityEvent.CallMovementByVelocityEvent(direction, moveSpeed); 
            }

            //Else player Dash if not cooling down 
            else if(playerDashCooldownTimer <= 0f)
            {
                PlayerDash((Vector3)direction);
            }

            //trigger particle system
            //player.PlayDustParticle();
        }

        else
        {
            player.idleEvent.CallIdleEvent();
            //player.StopDustParticle();
        }
    }

    private void PlayerDash(Vector3 direction)
    {
        playerDashCoroutine = StartCoroutine(PlayerDashRoutine(direction));
    }

    private IEnumerator PlayerDashRoutine(Vector3 direction)
    {
        //min Distance used to decide when to exit coroutine loop
        float minDistance = 0.2f;

        isPlayerDashing = true;

        Vector3 targetPosition = player.transform.position + (Vector3)direction * movementDetails.dashDistance;

        while (Vector3.Distance(player.transform.position, targetPosition) > minDistance)
        {
            player.movementToPositionEvent.CallMovementToPositionEvent(targetPosition, player.transform.position, movementDetails.dashSpeed, direction, isPlayerDashing);

            //Yield return and wait for fixed update
            yield return waitForFixedUpdate;
        }

        isPlayerDashing = false;

        //Set cooldown timer
        playerDashCooldownTimer = movementDetails.dashCooldownTime;

        player.transform.position = targetPosition;


    }

    private void PlayerDashCooldownTimer()
    {
        if(playerDashCooldownTimer >= 0f)
        {
            playerDashCooldownTimer -= Time.deltaTime;
        }
    }

    private void WeaponInput()
    {
        Vector3 weaponDirection;
        float weaponAngleDegrees, playerAngleDegrees;
        AimDirection playerAimDirection;

        AimWeaponInput(out weaponDirection, out weaponAngleDegrees, out playerAngleDegrees, out playerAimDirection);

        FireWeaponInput(weaponDirection, weaponAngleDegrees, playerAngleDegrees, playerAimDirection);

        ReloadWeaponInput();
    }

    private void AimWeaponInput(out Vector3 weaponDirection, out float weaponAngleDegrees, out float playerAngleDegrees, out AimDirection playerAimDirection)
    {
        //Get mouse world position
        Vector3 mouseWorldPosition = HelperUtilities.GetMouseWorldPosition();

        //Calculate direction vector of mouse cursor from weapon shoot position
        weaponDirection = (mouseWorldPosition - player.activeWeapon.GetShootPosition());

        //Calculate directon vector of mouse cursor from player transform position
        Vector3 playerDirection = (mouseWorldPosition - transform.position);

        //Get weapon to cursor angle
        weaponAngleDegrees = HelperUtilities.GetAngleFromVector(weaponDirection);

        //Get player to cursor angle
        playerAngleDegrees = HelperUtilities.GetAngleFromVector(playerDirection);

        //Set player aim direction
        playerAimDirection = HelperUtilities.GetAimDirection(playerAngleDegrees);

        //Trigger weapon aim event
        player.aimWeaponEvent.CallAimWeaponEvent(playerAimDirection, playerAngleDegrees, weaponAngleDegrees, weaponDirection);


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if collided with something stop player dash coroutine
        StopPlayerDashRoutine();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //if in collision with something stop player dash coroutine
        StopPlayerDashRoutine();
    }

    private void StopPlayerDashRoutine()
    {
        if (playerDashCoroutine != null)
        {
            StopCoroutine(playerDashCoroutine);

            isPlayerDashing = false;
        }
    }

    private void FireWeaponInput(Vector3 weaponDirection, float weaponAngleDegrees, float playerAngleDegrees, AimDirection playerAimDirection)
    {
        if(Input.GetMouseButton(0)) //0 = Left Click
        {
            //Trigger fire weapon event
            player.fireWeaponEvent.CallFireWeaponEvent(true, leftMouseDownPreviousFrame, playerAimDirection, playerAngleDegrees, weaponAngleDegrees, weaponDirection);

            leftMouseDownPreviousFrame = true;
        }

        else
        {
            leftMouseDownPreviousFrame = false;
        }

    }

    private void SetWeaponByIndex(int weaponIndex)
    {
        if(weaponIndex - 1 < player.weaponList.Count)
        {
            currentWeaponIndex = weaponIndex;
            player.setActiveWeaponEvent.CallSetActiveWeaponEvent(player.weaponList[weaponIndex - 1]);
        }
    }

    private void ReloadWeaponInput()
    {
        Weapon currentWeapon = player.activeWeapon.GetCurrentWeapon();

        if(currentWeapon.isWeaponReloading) return;

        if(currentWeapon.weaponRemainingAmmo < currentWeapon.weaponDetails.weaponClipAmmoCapacity && !currentWeapon.weaponDetails.hasInfiniteAmmo)
            return;
        
        if(currentWeapon.weaponClipRemainingAmmo == currentWeapon.weaponDetails.weaponClipAmmoCapacity) return;

        if(Input.GetKeyDown(KeyCode.R))
        {
            //Call reload weapon event
            player.reloadWeaponEvent.CallReloadWeaponEvent(currentWeapon, 0);
        }
    }

    #region Validation
#if UNITY_EDITOR

    private void OnValidate()
    {
        HelperUtilities.ValidateCheckNullValue(this, nameof(movementDetails), movementDetails);
    }

#endif
    #endregion
}
