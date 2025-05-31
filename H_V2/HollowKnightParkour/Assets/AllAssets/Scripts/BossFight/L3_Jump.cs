using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using Unity.VisualScripting.Antlr3.Runtime;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace Core.AI
{
    public class L3_Jump : BoosAction
    {
        public float horizontalForce = 5.0f;
        public float jumpForce = 10.0f;

        public float buildupTime;
        public float jumpTime;

        public string animationTriggerName;
        public bool shakeCameraOnLanding;

        private bool hasLanded;
        private int facingDirection;
        private Tween buildupTween;
        private Tween jumpTween;

        public UnityEngine.Transform player;
        public override void OnStart()
        {
            buildupTween = DOVirtual.DelayedCall(buildupTime, StartJump, false);
            animator.SetTrigger(animationTriggerName);
        }
        private void StartJump()
        {
            facingDirection = (player.position.x > transform.position.x) ? 1 : -1;
            var direction = facingDirection;

            body.AddForce(new Vector2(horizontalForce * direction, jumpForce), ForceMode2D.Impulse);
            jumpTween = DOVirtual.DelayedCall(jumpTime, () =>
            {
                hasLanded = true;

            }, false);
        }

        public override TaskStatus OnUpdate()
        {
            return hasLanded ? TaskStatus.Success : TaskStatus.Running;
        }
        public override void OnEnd()
        {
            buildupTween?.Kill();
            jumpTween?.Kill();
            hasLanded = false;
        }
    }
}