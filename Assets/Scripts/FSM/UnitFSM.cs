using UnityEngine;

namespace FSM
{
    public class UnitFSM
    {
        private Unit _unit;
        private UnitState _currentState;

        public UnitFSM(Unit unit)
        {
            _unit = unit;
            // Start with the Patrol state
            TransitionToState(new PatrolState(_unit));
        }

        public void Update()
        {
            if (_currentState != null)
            {
                _currentState.Execute();
            }
        }

        public void TransitionToState(UnitState newState)
        {
            if (_currentState != null)
            {
                _currentState.OnExit();
            }

            _currentState = newState;
            _currentState.OnEnter();
        }

        public void SetTarget(Transform target)
        {
            if (_currentState is AttackState attackState)
            {
                attackState.SetTarget(target);
            }
        }
    }
}

