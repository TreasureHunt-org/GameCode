using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using DG.Tweening;
using UnityEngine;

public class flippingTheBoss : MonoBehaviour
{

    public Transform player;
    private int storedFacingDirection;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D body;
    public AudioSource DoubleSwingAudio,RunAudio,ThrowAudio,SpinSwingAudio,JumpAudio,SpawnAudio;
    public AudioClip DoubleSwing,Run,Throw,Throw2,SpinSwing,Spin,Jump,Falling,HittingTheGround,Spawn;

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnIntroAnimationComplete()
    {
        animator.SetBool("IntroFinished", true);
    }
    public void detectDirection()
    {
        storedFacingDirection = (player.position.x > transform.position.x) ? 1 : -1;
    }
    public void StartSwing(float stepDistance)
    {

        Invoke(nameof(FirstMove), 0.01f); 
        Invoke(nameof(SecondMove), 0.9f); 
    }
    private void FirstMove()
    {
        MoveForwardDuringSwing(5f); 
    }
    private void SecondMove()
    {
        MoveForwardDuringSwing(3f); 
    }
    private void MoveForwardDuringSwing(float stepDistance)
    {
        transform.position += new Vector3(storedFacingDirection * stepDistance, 0, 0);
    }
    public void StartSpining()
    {

        Invoke(nameof(FirstMoveSpin), 0.01f); 
        Invoke(nameof(SecondMoveSpin), 0.3f); 
    }
    private void FirstMoveSpin()
    {
        MoveForwardDuringSwing(5f);
    }
    private void SecondMoveSpin()
    {
        Vector3 targetPosition = transform.position + new Vector3(storedFacingDirection * 2.0f, 0, 0);
        transform.DOMove(targetPosition, 0.5f).SetEase(Ease.OutQuad);
    }
    public void FlipSpriteAtRunStart()
    {
        if (player == null)
        {
            Debug.LogWarning("Player reference is missing!");
        }

        var scale = transform.localScale;
        scale.x = (player.position.x > transform.position.x) ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
        transform.localScale = scale;
    }
    public void DoubleSwingSound()
    {
        DoubleSwingAudio.PlayOneShot(DoubleSwing);
    }
    public void RunSound()
    {
        RunAudio.PlayOneShot(Run);
    }
    public void ThrowSound()
    {
        ThrowAudio.PlayOneShot(Throw);
    }
    public void SecondThrowSound()
    {
        ThrowAudio.PlayOneShot(Throw2);
    }
    public void SpinSwingSound()
    {
        SpinSwingAudio.PlayOneShot(SpinSwing);
    }
    public void SpinSound()
    {
        SpinSwingAudio.PlayOneShot(Spin);
    }
    public void JumpSoumd()
    {
        JumpAudio.PlayOneShot(Jump);
    }
    public void FallingSound()
    {
        JumpAudio.PlayOneShot(Falling);
    }
    public void HittingTheGroundSound()
    {
        JumpAudio.PlayOneShot(HittingTheGround);
    }
    public void  SpawnSound()
    {
        SpawnAudio.PlayOneShot(Spawn);
    }
}
