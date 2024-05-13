using UnityEngine;

public class GameInput : MonoBehaviour
{
    public bool GetInputDash()
    {
        return Input.GetKeyDown(KeyCode.E);
    }
    
    public bool GetInputJump()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }
    
    public float GetInputMovement()
    {
        float inputVector = 0;
        
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.Q))
        {
            inputVector -= 1;
        }

        if (Input.GetKey(KeyCode.D))
        {
            inputVector += 1;
        }

        return inputVector;
    }

    public bool GetInputMeleeAttack()
    {
        return Input.GetMouseButtonDown(0);
    }
}
