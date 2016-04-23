using UnityEngine;
using System.Collections;
using System;

public class GenericAnimation : MonoBehaviour
{
    private CharacterState characterState;
    private Animator animator;
    private Attack attack;

    private CharacterState.State previousState;
    public Grabber grabber;

    void Start()
    {
        animator = GetComponent<Animator>();
        characterState = GetComponent<CharacterState>();
        attack = GetComponent<Attack>();
        grabber = GetComponent<Grabber>();
    }

    void Update()
    {
        CharacterState.State state = characterState.state;
        if (previousState != state)
        {
            previousState = state;
            UpdateState();
        }
    }

    public void UpdateState()
    {
        CharacterState.State state = characterState.state;
        if (animator.runtimeAnimatorController.name == "Bear")
        {
            if (state == CharacterState.State.Idle) animator.Play("Idle Bear", 0.1f);
            if (state == CharacterState.State.Movement) animator.Play("Walk Bear", 0.1f);
        }
        if (grabber && grabber.state == Grabber.State.Hold)
        {
            if (state == CharacterState.State.Idle) animator.Play("Grab Idle", 0.1f);
            if (state == CharacterState.State.Movement) animator.Play("Grab Walk", 0.1f);
        }
        else if (attack.weapon.attackType == Weapon.AttackType.Swing)
        {
            if (state == CharacterState.State.Idle) animator.Play("Idle Axe", 0.1f);
            if (state == CharacterState.State.Movement) animator.Play("Walk Axe", 0.1f);
        }
        else if (attack.weapon.attackType == Weapon.AttackType.Shoot)
        {
            if (state == CharacterState.State.Idle) animator.Play("Idle Musket", 0.1f);
            if (state == CharacterState.State.Movement) animator.Play("Walk Musket", 0.1f);
        }
        else
        {
            if (tag == "Player")
            {
                if (state == CharacterState.State.Idle) animator.Play("Abe Idle", 0.1f);
                if (state == CharacterState.State.Movement) animator.Play("Abe Walk", 0.1f);
            }
            else
            {
                if (state == CharacterState.State.Idle) animator.Play("Idle", 0.1f);
                if (state == CharacterState.State.Movement) animator.Play("Walk", 0.1f);
            }
        }
        if (state == CharacterState.State.Dead) animator.Play("Dead", 0.1f);
    }
}
