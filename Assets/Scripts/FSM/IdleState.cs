using UI;
using UnityEngine;

namespace FSM
{
    public class IdleState : UnitState
    {
        public IdleState(Unit unit) : base(unit)
        {
        }

        public override void OnEnter()
        {
            _unit.GetComponent<UnitAnimator>().SetIdleState();
            UIManager.Instance.ShowUI();
        }

        public override void Execute()
        {
        }

        public override UnitState CheckTransitions()
        {
            return null;
        }

        public override void OnExit()
        {
        }
    }
}