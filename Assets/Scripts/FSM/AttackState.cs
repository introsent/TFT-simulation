using UnityEngine;

namespace FSM
{
    public class AttackState : UnitState
    {
        private Transform _target;
        private float _attackCooldown;
        private float _lastAttackTime;

        public AttackState(Unit unit) : base(unit) 
        {
            _attackCooldown = 2.5f; // Adjust based on unit type
        }
        
        public override void OnEnter()
        {
            // Trigger move animation when entering this state
            _unit.GetComponent<UnitAnimator>().TriggerAttack();
        }

        public override void OnExit()
        {
            _unit.GetComponent<UnitAnimator>().TriggerBackToWalking();
        }
        public override void Execute()
        {
            if (_target == null || _target.GetComponent<Unit>().Health <= 0)
            {
                _unit.FSM.TransitionToState(new SeekState(_unit));
                return;
            }

            // Add distance check
            if (Vector3.Distance(_unit.transform.position, _target.position) > _unit.Range)
            {
                _unit.FSM.TransitionToState(new SeekState(_unit));
                return;
            }

            // Existing attack logic
            if (Time.time - _lastAttackTime >= _attackCooldown)
            {
                _target.GetComponent<Unit>().TakeDamage(_unit.Damage);
                _lastAttackTime = Time.time;
            }
        }

        public override UnitState CheckTransitions()
        {
            return null; 
        }

        public void SetTarget(Transform target) => _target = target;
    }
}
