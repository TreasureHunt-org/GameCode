using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class FinishingIntro : BoosAction
{
    public override TaskStatus OnUpdate()
    {
        if (animator.GetBool("IntroFinished"))
        {
            return TaskStatus.Failure;
        }

        return TaskStatus.Running;
    }
}