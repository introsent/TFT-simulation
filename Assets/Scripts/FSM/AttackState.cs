using UnityEngine;

namespace FSM
{
    public class AttackState : UnitState
    {
        private Transform _target;

        public AttackState(Unit unit) : base(unit) { }

        public override void OnEnter()
        {
            // Initialize attack behavior
        }

        public override void Execute()
        {
            // Implement attack behavior
            if (_target != null && Vector3.Distance(_unit.transform.position, _target.position) <= _unit.Range)
            {
                _target.GetComponent<Unit>().TakeDamage(_unit.Damage);
            }
        }

        public override void OnExit()
        {
            // Clean up attack behavior
        }

        public void SetTarget(Transform target)
        {
            _target = target;
        }
    }
}
