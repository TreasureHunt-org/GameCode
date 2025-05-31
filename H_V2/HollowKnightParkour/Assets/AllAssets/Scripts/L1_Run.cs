using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class L1_Run : StateMachineBehaviour
{
    Transform player;
    Rigidbody2D rigidbody2;
    public float minDashSpeed = 10f;  
    public float maxDashSpeed = 20f; 
    public float dashFactor = 3f;    
    private bool hasDashed = false;  
    flippingTheBoss flippingTheBoss;
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        flippingTheBoss = animator.GetComponent<flippingTheBoss>();
        if (!hasDashed) 
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            rigidbody2 = animator.GetComponent<Rigidbody2D>();

            PerformDash(animator);
            hasDashed = true; 
        }
    }
   
    void PerformDash(Animator animator)
    {
        if (player == null || rigidbody2 == null) return;

      
        float distanceToPlayer = Mathf.Abs(player.position.x - rigidbody2.position.x);

        float dashSpeed = Mathf.Clamp(distanceToPlayer * dashFactor, minDashSpeed, maxDashSpeed);

        rigidbody2.velocity = new Vector2(Mathf.Sign(player.position.x - rigidbody2.position.x) * dashSpeed, rigidbody2.velocity.y);

        animator.GetComponent<MonoBehaviour>().StartCoroutine(StopDash());
    }

    IEnumerator StopDash()
    {
        yield return new WaitForSeconds(0.2f); 
        if (rigidbody2 != null)
        {
            rigidbody2.velocity = Vector2.zero; 
        }
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<MonoBehaviour>().StartCoroutine(WaitBeforeNextMechanic(animator));
        if (rigidbody2 != null)
        {
            rigidbody2.velocity = Vector2.zero; 
        }

        hasDashed = false; 
    }
    IEnumerator WaitBeforeNextMechanic(Animator animator)
    {
        yield return new WaitForSeconds(0.1f); 

        int randomMechanic = Random.Range(1, 5); 
        Debug.Log(randomMechanic+"run");
        animator.SetInteger("MechanicType", randomMechanic);

    }
}