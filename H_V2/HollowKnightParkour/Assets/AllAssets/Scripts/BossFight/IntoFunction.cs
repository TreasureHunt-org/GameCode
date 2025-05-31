using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntoFunction : MonoBehaviour
{
    public Animator animator;
    public void OnIntroAnimationComplete()
    {
        animator.SetBool("IntroFinished", true); // This ensures BD knows when the intro is done
    }
}
