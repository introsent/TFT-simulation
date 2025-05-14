using FSM;
using UnityEngine;

public enum UnitType
{
    Tank,
    Melee,
    Sniper
}

public class Unit : MonoBehaviour
{
    public UnitType Type;
    public int Health;
    public int Damage;
    public float Speed;
    public int Range;
    public float DetectionRange = 5f;
    public int AttackPriority; // 0: Nearest, 1: Melee, 2: Tank

    private UnitFSM _fsm;
    public UnitFSM FSM => _fsm;
    private void Start()
    {
        // Initialize unit attributes based on type
        InitializeAttributes();
        _fsm = new UnitFSM(this);
    }

    public void TransitionToState(UnitState state)
    {
        _fsm.TransitionToState(state);
    }
    private void Update()
    {
        if (GameManager.Instance.HasStarted())
        {
            _fsm.Update();
        }
    }

    private void InitializeAttributes()
    {
        switch (Type)
        {
            case UnitType.Tank:
                Health = 150;
                Damage = 20;
                Speed = 2f;
                Range = 1;
                AttackPriority = 0; // Tank attacks the nearest enemy
                break;
            case UnitType.Melee:
                Health = 100;
                Damage = 30;
                Speed = 3f;
                Range = 1;
                AttackPriority = 1; // Melee fighter targets tanks
                break;
            case UnitType.Sniper:
                Health = 50;
                Damage = 40;
                Speed = 1f;
                Range = 3;
                AttackPriority = 2; // Sniper targets melee fighters
                break;
        }
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            _fsm.TransitionToState(new DieState(this));
        }
    }
}

