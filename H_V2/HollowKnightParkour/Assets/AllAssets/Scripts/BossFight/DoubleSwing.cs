using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using UnityEngine;

public class L3_DoubleSwing : BoosAction
{
    public float moveSpeed = 2.0f;
    public string animationTriggerName;
    public float swingDuration;
    public Transform player;

    private Tween swingTween;
    private bool hasCompleted;
    private int facingDirection;
    private float startTime;

    public override void OnStart()
    {
        startTime = Time.time;
        facingDirection = (player.position.x > transform.position.x) ? 1 : -1;
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

        if (Time.time - startTime >= 2.5f)
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