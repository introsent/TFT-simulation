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
            _attackCooldown = 1.5f; // Adjust based on unit type
        }

        public override void Execute()
        {
            if (_target == null || _target.GetComponent<Unit>().Health <= 0)
            {
                _unit.FSM.TransitionToState(new SeekState(_unit));
                return;
            }

            if (Vector3.Distance(_unit.transform.position, _target.position) > _unit.Range)
            {
                _unit.FSM.TransitionToState(new SeekState(_unit));
                return;
            }

            if (Time.time - _lastAttackTime >= _attackCooldown)
            {
                _target.GetComponent<Unit>().TakeDamage(_unit.Damage);
                _lastAttackTime = Time.time;
            }
        }

        public override UnitState CheckTransitions()
        {
            throw new System.NotImplementedException();
        }

        public void SetTarget(Transform target) => _target = target;
    }
}
