using FSM;
using UnityEngine;

public enum UnitType
{
    Tank,
    Melee,
    Sniper
}

public enum Faction
{
    Player,
    Enemy
}

public class Unit : MonoBehaviour
{
    public Faction Side;
    
    public UnitType Type;
    public int Health;
    public int Damage;
    public float Speed;
    public int Range;
    public float DetectionRange = 5f;
    public int AttackPriority; // 0: Nearest, 1: Melee, 2: Tank
    public float RotationSpeed = 5f;
    public float AttackCooldown;
    public float nextAttackTime = 0f;
    
    private UnitFSM _fsm;
    public UnitFSM FSM => _fsm;
    
    [SerializeField] private HealthBar _healthBar;
    
    [Header("Utility System")]
    public UtilityConfig utilityConfig;
    public UnitType preferredTargetType;
    
    [HideInInspector] public float maxHealth;
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
        maxHealth = Health;
        _healthBar.UpdateHealth(maxHealth, maxHealth); 
    }
    
    public float HealthPercentage => Health / maxHealth;
    public void TakeDamage(int damage)
    {
        Health -= damage;
        _healthBar.UpdateHealth(Health, maxHealth);
        
        if (Health <= 0)
        {
            _healthBar.Hide();
            _fsm.TransitionToState(new DieState(this));
        }
        
    }
}

