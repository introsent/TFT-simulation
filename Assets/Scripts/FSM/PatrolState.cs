using UnityEngine;

namespace FSM
{
    public class PatrolState : UnitState
    {
        public PatrolState(Unit unit) : base(unit) { }

        public override void OnEnter()
        {
            // Initialize patrol behavior
        }

        public override void Execute()
        {
            // Implement patrol behavior
            Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
            _unit.transform.position += randomDirection * _unit.Speed * Time.deltaTime;
        }

        public override void OnExit()
        {
            // Clean up patrol behavior
        }
    }
}