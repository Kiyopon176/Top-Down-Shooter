using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimations : MonoBehaviour
{
    private Animator animator;
    private bool isDead = false;
    public float enemyAnimLength, playerAnimLength;

    private void Start()
    {
        animator = GetComponent<Animator>();
        GetAnimLength();
    }

    public void PlayDeathAnimation(string animName)
    {
        if (animator != null && !isDead)
        {
            animator.SetBool(animName, true);
            isDead = true;
        }
        else
        {
            print("Animator is null or already dead");
        }
    }

    void GetAnimLength()
    {
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach(AnimationClip clip in clips)
        {
            switch(clip.name)
            {
                case "Death":
                    enemyAnimLength = clip.length;
                    break;
                case "PlayerDeath":
                    playerAnimLength = clip.length;
                    break;
            }
        }
    }
}
