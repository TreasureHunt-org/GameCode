using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class JumpWaiting : BoosAction
{
    public float waitTime = 2f;  
    private float timer;
    public override void OnStart()
    {
        timer = Time.time;
        animator.SetTrigger("StartIdle");
    }
    public override TaskStatus OnUpdate()
    {
        if (Time.time - timer >= waitTime)
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }
}
