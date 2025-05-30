using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public class SeekState : UnitState
{
    private float _lastPatrolTime;
    private Transform _target;

    public SeekState(Unit unit) : base(unit) { }

    public SeekState(Unit unit, Transform target) : base(unit)
    {
        _target = target;
    }
    
    public override void OnEnter()
    {
        // Trigger move animation when entering this state
        _unit.GetComponent<UnitAnimator>().TriggerMove();
    }

    public override void Execute()
    {
        // 1. Check target validity
        if (_target == null || !TargetIsValid())
        {
            FindBestTarget();
        }

        // 2. Act based on target availability
        if (_target != null)
        {
            MoveTowardsTarget();
        }
    }
    
    private bool TargetIsValid()
    {
        return _target != null && 
               _target.GetComponent<Unit>().Health > 0 &&
               Vector3.Distance(_unit.transform.position, _target.position) <= _unit.DetectionRange;
    }
    
    private void FindBestTarget()
    {
        Unit bestTarget = null;
        float highestScore = Mathf.NegativeInfinity;

        foreach (Unit enemy in GetAllEnemies())
        {
            float score = CalculateUtility(enemy);
            if (score > highestScore)
            {
                highestScore = score;
                bestTarget = enemy;
            }
        }

        _target = bestTarget?.transform;
    }
    
    private float CalculateUtility(Unit enemy)
    {
        // Distance factor
        float distance = Vector3.Distance(_unit.transform.position, enemy.transform.position);
        float distanceScore = 1 / (distance + 0.1f);

        // Health factor
        float healthScore = 1 - enemy.HealthPercentage;

        // Threat factor
        float threatScore = enemy.Damage / (distance + 0.1f);

        // Type factors
        float typeScore = 0;
        if (enemy.Type == _unit.preferredTargetType) 
            typeScore += _unit.utilityConfig.preferredTypeBonus;
        if (enemy.Type == _unit.Type) 
            typeScore += _unit.utilityConfig.sameTypePenalty;

        return (distanceScore * _unit.utilityConfig.distanceWeight) +
               (healthScore * _unit.utilityConfig.healthWeight) +
               (threatScore * _unit.utilityConfig.threatWeight) +
               typeScore;
    }


    public override UnitState CheckTransitions()
    {
        // if current target died or out of detect‐range → patrol
        if (_target == null || 
            _target.GetComponent<Unit>().Health <= 0 ||
            Vector3.Distance(_unit.transform.position, _target.position) > _unit.DetectionRange)
        {
            return new PatrolState(_unit);
        }
    
        // if target not set but there _are_ valid enemies → stay in Seek so Execute() will FindBestTarget()
        if (_target == null && GetAllEnemies().Count > 0)
            return this; 

        // if we’ve got a valid target in attack-range → go Attack
        if (_target != null && Vector3.Distance(_unit.transform.position, _target.position) <= _unit.Range)
        {
            var atk = new AttackState(_unit);
            atk.SetTarget(_target);
            return atk;
        }

        return null; // stay in Seek
    }
    
    private List<Unit> GetAllEnemies()
    {
        List<Unit> enemies = new List<Unit>();
        Unit[] allUnits = Object.FindObjectsByType<Unit>(FindObjectsSortMode.None);
        foreach (Unit unit in allUnits)
        {
            if (unit.Side != _unit.Side && unit.Health > 0)
            {
                enemies.Add(unit);
            }
        }
        return enemies;
    }

    private void FindTarget()
    {
        // Modified to find targets regardless of current distance
        List<GameObject> enemies = new List<GameObject>();
        Unit[] allUnits = Object.FindObjectsByType<Unit>(FindObjectsSortMode.None);
        foreach (Unit unit in allUnits)
        {
            if (unit.Side != _unit.Side)
            {
                enemies.Add(unit.gameObject);
            }
        }
        
        GameObject bestTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            Unit enemyUnit = enemy.GetComponent<Unit>();
            if (enemyUnit == null) continue;

            float distance = Vector3.Distance(_unit.transform.position, enemy.transform.position);
            if (IsPriorityTarget(enemyUnit) && distance < closestDistance)
            {
                closestDistance = distance;
                bestTarget = enemy;
            }
        }

        _target = bestTarget?.transform;
    }

    private bool IsPriorityTarget(Unit enemy)
    {
        return _unit.AttackPriority switch
        {
            0 => true, // Nearest target
            1 => enemy.Type == UnitType.Tank,
            2 => enemy.Type == UnitType.Melee,
            _ => false
        };
    }

    private void MoveTowardsTarget()
    {
        Vector3 direction = (_target.position - _unit.transform.position).normalized;

        // Rotate towards the target
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            _unit.transform.rotation = Quaternion.Slerp(
                _unit.transform.rotation,
                lookRotation,
                _unit.RotationSpeed * Time.deltaTime
            );
        }

        // Move towards the target
        _unit.transform.position = Vector3.MoveTowards(
            _unit.transform.position,
            _target.position,
            _unit.Speed * Time.deltaTime
        );
    }

    private bool IsTargetInRange()
    {
        return Vector3.Distance(_unit.transform.position, _target.position) <= _unit.Range;
    }
}
}
