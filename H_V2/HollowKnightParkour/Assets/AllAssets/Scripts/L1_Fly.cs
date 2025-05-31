using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L1_Fly : StateMachineBehaviour
{
    flippingTheBoss flippingTheBoss;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        flippingTheBoss = animator.GetComponent<flippingTheBoss>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<MonoBehaviour>().StartCoroutine(WaitBeforeNextMechanic(animator));
    }

    IEnumerator WaitBeforeNextMechanic(Animator animator)
    {
        yield return new WaitForSeconds(0.1f);

        int randomMechanic = Random.Range(1, 5); 
        Debug.Log(randomMechanic+"fly");

        animator.SetInteger("MechanicType", randomMechanic);

    }
}
