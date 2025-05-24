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
            // First, check global transitions
            var globalTransition = _currentState.CheckGlobalTransitions();
            if (globalTransition != null)
            {
                TransitionToState(globalTransition);
                return;
            }

            // Then, execute logic
            _currentState.Execute();

            // Then, check state-specific transitions
            var newState = _currentState.CheckTransitions();
            if (newState != null)
            {
                TransitionToState(newState);
            }
        }

        private void CheckStateTransition()
        {
            var nextState = _currentState.CheckTransitions();
            if (nextState != null)
            {
                TransitionToState(nextState);
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

