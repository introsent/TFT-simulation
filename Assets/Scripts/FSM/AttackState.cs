using UnityEngine;

namespace FSM
{
    public class AttackState : UnitState
    {
        private Transform _target;
        private bool      _waitingForHit;
        private bool      _hasFiredThisSwing;
        private float     _hitTimeNorm = 0.5f;

        public AttackState(Unit unit) : base(unit)
        {
            // Constructor left empty; swing flags reset in OnEnter
        }
        
        public override void OnEnter()
        {
            // Reset flags when entering attack state
            _waitingForHit     = false;
            _hasFiredThisSwing = false;
        }

        public override void Execute()
        {
            // 1) Check for lost target or death
            if (_target == null || _target.GetComponent<Unit>().Health <= 0)
            {
                _unit.FSM.TransitionToState(new SeekState(_unit));
                return;
            }

            // 2) Rotate to face the target
            Vector3 direction = (_target.position - _unit.transform.position).normalized;
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                _unit.transform.rotation = Quaternion.Slerp(
                    _unit.transform.rotation,
                    lookRotation,
                    _unit.RotationSpeed * Time.deltaTime
                );
            }

            // 3) Check range
            if (Vector3.Distance(_unit.transform.position, _target.position) > _unit.Range)
            {
                _unit.FSM.TransitionToState(new SeekState(_unit));
                return;
            }

            // 4) If mid-swing, wait for hit moment
            if (_waitingForHit)
            {
                Animator animator = _unit.GetComponent<Animator>();
                var info = animator.GetCurrentAnimatorStateInfo(0);

                if (!_hasFiredThisSwing && info.IsName("Attack") && info.normalizedTime >= _hitTimeNorm)
                {
                    _hasFiredThisSwing = true;

                    // Apply damage and set cooldown
                    _target.GetComponent<Unit>().TakeDamage(_unit.Damage);
                    _unit.nextAttackTime = Time.time + _unit.AttackCooldown;

                    // Swing complete; leave mid-swing
                    _waitingForHit = false;
                }

                return;
            }

            // 5) If not mid-swing, check if cooldown elapsed to start new swing
            if (Time.time >= _unit.nextAttackTime)
            {
                _unit.GetComponent<UnitAnimator>().TriggerAttack();
                // Prepare for new swing
                _waitingForHit = true;
                _hasFiredThisSwing = false;
            }
        }

        public override void OnExit()
        {
            var animator = _unit.GetComponent<UnitAnimator>();
            animator.ResetAttackTriggers();
            _unit.GetComponent<UnitAnimator>().TriggerBackToWalking();
        }

        public override UnitState CheckTransitions()
        {
            // Transitions handled inline in Execute
            return null;
        }

        // Set by SeekState when transitioning in
        public void SetTarget(Transform target) => _target = target;
    }
}