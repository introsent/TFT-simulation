using UnityEngine;

namespace FSM
{
    public class SeekState : UnitState
    {
        private Transform _target;

        public SeekState(Unit unit) : base(unit) { }

        public override void OnEnter()
        {
            // Initialize seek behavior
        }

        public override void Execute()
        {
            // Implement seek behavior
            FindTarget();
            if (_target != null)
            {
                MoveTowardsTarget();
            }
        }

        public override void OnExit()
        {
            // Clean up seek behavior
        }

        private void FindTarget()
        {
            // Find the nearest target based on attack priority
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            float closestDistance = Mathf.Infinity;
            GameObject closestEnemy = null;

            foreach (GameObject enemy in enemies)
            {
                Unit enemyUnit = enemy.GetComponent<Unit>();
                if (enemyUnit != null)
                {
                    float distance = Vector3.Distance(_unit.transform.position, enemy.transform.position);
                    if (distance < closestDistance && distance <= _unit.Range)
                    {
                        if (_unit.AttackPriority == 0 || (_unit.AttackPriority == 1 && enemyUnit.Type == UnitType.Tank) || (_unit.AttackPriority == 2 && enemyUnit.Type == UnitType.Melee))
                        {
                            closestDistance = distance;
                            closestEnemy = enemy;
                        }
                    }
                }
            }

            _target = closestEnemy != null ? closestEnemy.transform : null;
        }

        private void MoveTowardsTarget()
        {
            // Move towards the target
            _unit.transform.position = Vector3.MoveTowards(_unit.transform.position, _target.position, _unit.Speed * Time.deltaTime);
        }
    }
}
