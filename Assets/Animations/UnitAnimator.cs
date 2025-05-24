using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    protected Animator _animator;
    
    private void Awake() => _animator = GetComponent<Animator>();
    
    public virtual void TriggerAttack() => 
        _animator.SetTrigger("Attack");
    
    public virtual void TriggerMove() => 
        _animator.SetTrigger("Move");

    public virtual void TriggerBackToIdle() => 
        _animator.SetTrigger("BackToIdle");
    
    public virtual void TriggerBackToWalking() => 
        _animator.SetTrigger("BackToWalking");

    public virtual void TriggerAttackAgain() =>
        _animator.SetTrigger("AttackAgain");

    public virtual void SetIdleState() =>
        _animator.Play("Idle");
}
