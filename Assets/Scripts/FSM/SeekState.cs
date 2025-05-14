using UnityEngine;

namespace FSM
{
    public class SeekState : UnitState
{
    private Transform _target;

    public SeekState(Unit unit) : base(unit) { }

    public override void Execute()
    {
        FindTarget();
        if (_target != null)
        {
            MoveTowardsTarget();
            if (IsTargetInRange())
            {
                AttackState attackState = new AttackState(_unit);
                attackState.SetTarget(_target);
                _unit.TransitionToState(attackState);
            }
        }
        else
        {
            _unit.FSM.TransitionToState(new PatrolState(_unit));
        }
    }

    public override UnitState CheckTransitions()
    {
        return null;
    }

    private void FindTarget()
    {
        // Modified to find targets regardless of current distance
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
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
