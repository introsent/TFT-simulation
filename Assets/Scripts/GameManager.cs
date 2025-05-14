using System.Collections.Generic;
using UI;
using UnityEngine;



public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public List<UnitPosition> _aiUnitPositions; // List of positions to place AI units
    private bool hasStarted = false;
    
    [System.Serializable]
    public struct UnitPosition
    {
        public UnitType type;
        public Vector2Int position;
    }
    private void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        // Initialize the game
        InitializeGame();

    }

    private void InitializeGame()
    {
        _aiUnitPositions = new List<UnitPosition>
        {
            new() { type = UnitType.Melee, position = new Vector2Int(1, 3) },
            new() { type = UnitType.Melee, position = new Vector2Int(2, 3) },
            new() { type = UnitType.Tank, position = new Vector2Int(2, 4) },
            new() { type = UnitType.Sniper, position = new Vector2Int(3, 5) },
            new() { type = UnitType.Sniper, position = new Vector2Int(0, 5) },
            // Add more unit positions as needed
        };
        SpawnManager.Instance.SpawnAIUnits(_aiUnitPositions);
    }

    public void Run()
    {
        hasStarted = true;
        UIManager.Instance.HideUI();
    }
}