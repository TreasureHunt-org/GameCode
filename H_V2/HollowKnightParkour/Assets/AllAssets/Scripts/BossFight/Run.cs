using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class Run : BoosAction
{
    public float moveSpeed = 3.0f;
    public float stopDistance = 2.0f;
    public float verticalStopDistance = 1.5f;
    public Transform player;
    public string animationTriggerName;
    private SpriteRenderer spriteRenderer;
    public static bool isRunning;

    public override void OnStart()
    {
        animator.SetTrigger(animationTriggerName);
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the sprite renderer
    }

    public override TaskStatus OnUpdate()
    {
        float horizontalDistance = Mathf.Abs(player.position.x - transform.position.x);
        float verticalDistance = Mathf.Abs(player.position.y - transform.position.y);

        if (horizontalDistance > stopDistance || verticalDistance > verticalStopDistance)
        {
            int facingDirection = (player.position.x > transform.position.x) ? 1 : -1;
            body.velocity = new Vector2(facingDirection * moveSpeed, body.velocity.y);
            isRunning = true;

            //FlipSpriteBasedOnPlayerPosition(); // Flip sprite dynamically
            return TaskStatus.Running;
        }
        else
        {
            isRunning = false;
            body.velocity = Vector2.zero;
            return TaskStatus.Failure;
        }
    }

    private void FlipSpriteBasedOnPlayerPosition()
    {
        if (player == null || spriteRenderer == null)
        {
            Debug.LogWarning("Player or SpriteRenderer reference is missing!");
            return;
        }

        // Flip sprite based on player's position relative to the boss
        spriteRenderer.flipX = (player.position.x > transform.position.x);
    }
}