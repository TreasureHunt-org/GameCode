using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using Unity.VisualScripting.Antlr3.Runtime;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;
public class Throwing : BoosAction
{
    private float startTime;

    public override void OnStart()
    {
        startTime = Time.time; 
        animator.SetTrigger("Throwing");
    }

    public override TaskStatus OnUpdate()
    {
        if (Time.time - startTime >= 1f)
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }
}