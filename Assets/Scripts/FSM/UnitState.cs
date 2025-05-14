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
}