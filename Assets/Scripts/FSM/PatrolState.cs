using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public class PatrolState : UnitState
    {
        private Vector3 _centerPosition;
        private bool _reachedCenter = false;
        private GameObject _targetEnemy;

        public PatrolState(Unit unit) : base(unit) 
        {
            _centerPosition = new Vector3(5, 0, 6); // Adjust to match your map center
        }

        public override void Execute()
        {
            Vector3 targetPos;

            if (!_reachedCenter)
            {
                targetPos = _centerPosition;
                float distanceToCenter = Vector3.Distance(_unit.transform.position, _centerPosition);
                if (distanceToCenter <= 0.1f)
                {
                    _reachedCenter = true;
                }
            }
            else
            {
                if (_targetEnemy == null)
                {
                    _targetEnemy = FindClosestEnemy();
                }

                if (_targetEnemy != null)
                {
                    targetPos = _targetEnemy.transform.position;
                }
                else
                {
                    return; // No target to move toward
                }
            }

            // Move toward target position
            Vector3 direction = (targetPos - _unit.transform.position).normalized;
            _unit.transform.position += direction * _unit.Speed * Time.deltaTime;
        }

        public override UnitState CheckTransitions()
        {
            if (EnemyInSight(out GameObject visibleEnemy))
            {
                return new SeekState(_unit, visibleEnemy.transform);
            }
            return null;
        }

        private bool EnemyInSight(out GameObject enemyFound)
        {
            List<GameObject> enemies = new List<GameObject>();
            Unit[] allUnits = Object.FindObjectsByType<Unit>(FindObjectsSortMode.None);
            foreach (Unit unit in allUnits)
            {
                if (unit.Side != _unit.Side)
                {
                    enemies.Add(unit.gameObject);
                }
            }
            foreach (var enemy in enemies)
            {
                if (Vector3.Distance(_unit.transform.position, enemy.transform.position) <= _unit.DetectionRange)
                {
                    enemyFound = enemy;
                    return true;
                }
            }
            enemyFound = null;
            return false;
        }

        private GameObject FindClosestEnemy()
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            GameObject closest = null;
            float minDistance = float.MaxValue;

            foreach (var enemy in enemies)
            {
                float distance = Vector3.Distance(_unit.transform.position, enemy.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = enemy;
                }
            }

            return closest;
        }
    }
}