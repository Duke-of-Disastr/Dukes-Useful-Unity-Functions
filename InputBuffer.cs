using System.Collections;
using UnityEngine;

//Written by Clayton Ives

/*
Quick example of declaration because the requirements field is not a commonly used type

private InputBuffer jumpBuffer = new InputBuffer(Jump, 0.30f, () => isGrounded, () => isWalking, () => health > 0);

Update{
if (input.jump == true) {
    jumpBuffer.StartBuffer();
  }
 }
  private void Jump() {}
*/

public class InputBuffer
{
    public delegate void InputBufferEvent();
    public float bufferTime;

    private bool bufferStarted; // flag to indicate if the buffer has started
    private float timer; // timer for buffer time
  
    private InputBufferEvent bufferEvent; // function to be executed once buffer is complete
    private System.Func<bool>[] requirements;

    
    
    public InputBuffer(InputBufferEvent eventToExecute, float bufferDuration, params System.Func<bool>[] _requirements)
    {
        bufferEvent = eventToExecute;
        requirements = _requirements;
        bufferTime = bufferDuration;
        bufferStarted = false;
        timer = 0f;
    }
    
    public StartBuffer() 
    {
        bufferStarted = true;
        timer = 0f;
        StartCoroutine(CheckBufferRequirements(requirements));
    }
    // Coroutine to check the buffer requirements
    private IEnumerator CheckBufferRequirements(params System.Func<bool>[] requirements)
    {
        while (timer < bufferTime)
        {
            timer += Time.deltaTime;

            // Check if all requirements are met
            bool requirementsMet = true;
            foreach (System.Func<bool> requirement in requirements)
            {
                if (!requirement())
                {
                    requirementsMet = false;
                    break;
                }
            }

            // If all requirements are met, execute the buffer event
            if (requirementsMet)
            {
                bufferEvent();
                break;
            }
            yield return null;
        }
        bufferStarted = false;
    }
}

