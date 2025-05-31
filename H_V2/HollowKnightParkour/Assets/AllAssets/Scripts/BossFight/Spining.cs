using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using Unity.VisualScripting.Antlr3.Runtime;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;
public class Spining : BoosAction
{
    public float moveSpeed = 2.0f;
    public string animationTriggerName;
    public float swingDuration;
    public UnityEngine.Transform player;

    private Tween swingTween;
    private bool hasCompleted;
    private int facingDirection;
    private float startTime;
    public override void OnStart()
    {
        facingDirection = (player.position.x > transform.position.x) ? 1 : -1;
        startTime = Time.time;
        animator.SetTrigger(animationTriggerName);
        swingTween = DOVirtual.DelayedCall(swingDuration, () =>
        {
            hasCompleted = true;
        }, false);
    }
    public override TaskStatus OnUpdate()
    {
        if (!hasCompleted)
        {
            body.velocity = new Vector2(facingDirection * moveSpeed, body.velocity.y);
        }

        if (Time.time - startTime >= 2f)
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }
    public override void OnEnd()
    {
        swingTween?.Kill();
        hasCompleted = false;
        body.velocity = Vector2.zero;
    }
}
