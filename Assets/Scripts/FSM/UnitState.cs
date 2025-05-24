using FSM;
using UnityEngine;

public abstract class UnitState
{
    protected Unit _unit;

    public UnitState(Unit unit)
    {
        _unit = unit;
    }

    public virtual void OnEnter() { }
    public virtual void OnExit() { }
    public abstract void Execute();
    public abstract UnitState CheckTransitions();
    
    // Add this method for global checks
    public virtual UnitState CheckGlobalTransitions()
    {
        if (AllEnemiesDead())
        {
            return new IdleState(_unit);
        }

        return null;
    }

    private bool AllEnemiesDead()
    {
        var allUnits = Object.FindObjectsByType<Unit>(FindObjectsSortMode.None);
        foreach (var unit in allUnits)
        {
            if (unit.Side != _unit.Side && unit.Health > 0)
                return false;
        }

        return true;
    }
}