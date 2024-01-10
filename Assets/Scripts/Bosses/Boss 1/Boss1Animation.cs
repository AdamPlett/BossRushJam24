using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Animation : MonoBehaviour
{
    public Animator bossAnimator;

    [Header("Animation Hashes")]
    public readonly int IdleHash = Animator.StringToHash("Idle");
    public readonly int WalkHash = Animator.StringToHash("Walk");
    public readonly int WalkHashL = Animator.StringToHash("WalkLeft");
    public readonly int WalkHashR = Animator.StringToHash("WalkRight");
    public readonly int SwingHash = Animator.StringToHash("Swing");
    public readonly int ShootHash = Animator.StringToHash("Shoot");
    public readonly int MeleeHash1 = Animator.StringToHash("Melee1");
    public readonly int MeleeHash2 = Animator.StringToHash("Melee2");
    public readonly int MeleeHash3 = Animator.StringToHash("Melee3");
    public readonly int MeleeHash360 = Animator.StringToHash("Melee360");
    public readonly int ComboHash = Animator.StringToHash("MeleeCombo");
    public readonly int ChargeHash = Animator.StringToHash("MeleeCharge");

    public void SwitchAnimation(int animHash)
    {
        bossAnimator.CrossFadeInFixedTime(animHash, 0.25f);
    }
}
