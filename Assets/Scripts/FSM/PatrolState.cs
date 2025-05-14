using UnityEngine;

namespace FSM
{
    public class PatrolState : UnitState
    {
        private Vector3 _centerPosition;

        public PatrolState(Unit unit) : base(unit) 
        {
            // Define the center of the map (adjust based on your grid setup)
            _centerPosition = new Vector3(5, 0, 6); // Example center for a 4-column grid
        }

        public override void Execute()
        {
            Vector3 direction = (_centerPosition - _unit.transform.position).normalized;
            _unit.transform.position += direction * _unit.Speed * Time.deltaTime;
        }

        public override UnitState CheckTransitions()
        {
            if (EnemyInSight())
            {
                return new SeekState(_unit);
            }
            return null;
        }

        private bool EnemyInSight()
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (var enemy in enemies)
            {
                if (Vector3.Distance(_unit.transform.position, enemy.transform.position) <= _unit.DetectionRange)
                {
                    return true;
                }
            }
            return false;
        }
    }
}