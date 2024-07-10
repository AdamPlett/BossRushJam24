using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerController", menuName = "ScriptableObjects/InputControllers/Player")]
public class PlayerController : InputController
{
    private PlayerInputManager inputManager = null;
    
    public override Vector2 RetrieveMoveInput()
    {
        Debug.Log(PlayerInputManager.Instance);

        return PlayerInputManager.Instance.MoveInput;
    }

    public override Vector2 RetrieveLookInput()
    {
        Debug.Log(PlayerInputManager.Instance);
        
        return PlayerInputManager.Instance.LookInput;
    }

    public override float RetrieveSprintInput()
    {
        return inputManager.SprintValue;
    }

    public override bool RetrieveJumpInput()
    {
        return inputManager != null ? inputManager.JumpTriggered : false;
    }

    public override bool RetrieveDashInput()
    {
        return inputManager.dashTriggered;
    }

    public override bool RetrieveCrouchInput()
    {
        return inputManager.crouchTriggered;
    }

    public override bool RetrievePrimaryAttack()
    {
        return inputManager.primaryAttackTriggered;
    }

    public override bool RetrieveSecondaryAttack()
    {
        return inputManager.secondaryAttackTriggered;
    }

    public override bool RetrieveRightSkill()
    {
        return inputManager.rightSkillTriggered;
    }

    public override bool RetrieveLeftSkill()
    {
        return inputManager.leftSkillTriggered;
    }
}
