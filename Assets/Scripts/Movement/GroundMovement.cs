using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundMovement : MonoBehaviour
{
    public PlayerMovementStateMachine stateMachine;

    public void Move()
    {
        Vector3 moveDirection = CalculateMoveDirection();
        MoveOnGround(moveDirection);
    }

    public void MoveOnGround(Vector3 moveDirection)
    {

    }

    public Vector3 CalculateMoveDirection()
    {
        float horizontalInput = stateMachine.controller.input.RetrieveMoveInput().x;
        float verticalInput = stateMachine.controller.input.RetrieveMoveInput().y;

        Vector3 camForward = new Vector3(stateMachine.cm.cameraForward.x, 0, stateMachine.cm.cameraForward.z);
        Vector3 camRight = new Vector3(stateMachine.cm.cameraRight.x, 0, stateMachine.cm.cameraRight.z);

        return camForward.normalized * verticalInput + camRight.normalized * horizontalInput;
    }
}