using UnityEngine;

namespace FSM
{
    public class DieState : UnitState
    {
        public DieState(Unit unit) : base(unit) { }

        public override void OnEnter()
        {
            // Initialize die behavior
        }

        public override void Execute()
        {
            // Implement die behavior
            Object.Destroy(_unit.gameObject);
        }

        public override UnitState CheckTransitions()
        {
            return null; 
        }

        public override void OnExit()
        {
            // Clean up die behavior
        }
    }
}