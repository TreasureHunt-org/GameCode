using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class FlippingTheBoss : BoosAction
{
    public Transform player; 

    public override TaskStatus OnUpdate()
    {
        if (player == null)
        {
            Debug.LogWarning("Player reference is missing!");
            return TaskStatus.Failure;
        }

        var scale = transform.localScale;
        scale.x = (player.position.x > transform.position.x) ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
        transform.localScale = scale;

        return TaskStatus.Success;
    }
}