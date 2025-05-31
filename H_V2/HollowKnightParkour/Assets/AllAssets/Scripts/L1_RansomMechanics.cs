using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L1_RandomMechanics : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        StartRandomMechanic(animator);
    }

    private void StartRandomMechanic(Animator animator)
    {
        int randomMechanic = Random.Range(1, 5); 
        Debug.Log(randomMechanic);
        animator.SetInteger("MechanicType", randomMechanic);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<MonoBehaviour>().StartCoroutine(WaitBeforeNextMechanic(animator));
    }

    IEnumerator WaitBeforeNextMechanic(Animator animator)
    {
        yield return new WaitForSeconds(1f); 
        StartRandomMechanic(animator); 
    }
}