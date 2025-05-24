using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UI;
using UnityEngine;



public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public List<UnitPosition> _aiUnitPositions; // List of positions to place AI units
    private bool hasStarted = false;
    
    private List<List<UnitPosition>> _levels = new List<List<UnitPosition>>();
    private int _currentLevel = 0;
    
    [System.Serializable]
    public struct UnitPosition
    {
        public UnitType type;
        public Vector2Int position;
    }
    private void Awake()
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
        _levels.Add(new List<UnitPosition>
        {
            new() { type = UnitType.Melee,  position = new Vector2Int(1, 3) },
            new() { type = UnitType.Melee,  position = new Vector2Int(2, 3) },
            new() { type = UnitType.Tank,   position = new Vector2Int(2, 4) },
            new() { type = UnitType.Sniper, position = new Vector2Int(3, 5) },
            new() { type = UnitType.Sniper, position = new Vector2Int(0, 5) }
        });

        _levels.Add(new List<UnitPosition>
        {
            new() { type = UnitType.Tank,   position = new Vector2Int(1, 3) },
            new() { type = UnitType.Tank,   position = new Vector2Int(2, 3) },
            new() { type = UnitType.Melee,  position = new Vector2Int(1, 4) },
            new() { type = UnitType.Melee,  position = new Vector2Int(2, 4) },
            new() { type = UnitType.Sniper, position = new Vector2Int(3, 5) }
        });
        
        _levels.Add(new List<UnitPosition>
        {
            new() { type = UnitType.Tank,   position = new Vector2Int(1, 3) },
            new() { type = UnitType.Tank,   position = new Vector2Int(2, 3) },
            new() { type = UnitType.Sniper, position = new Vector2Int(3, 5) },
            new() { type = UnitType.Sniper, position = new Vector2Int(1, 5) },
            new() { type = UnitType.Sniper, position = new Vector2Int(0, 5) }
        });
        //SpawnManager.Instance.SpawnAIUnits(_aiUnitPositions);
    }
    
    private void Start()
    {
        LoadLevel(0);
    }
    
    private void ClearAllUnits()
    {
        // 1) Destroy every existing Unit
        foreach (var u in Object.FindObjectsByType<Unit>(FindObjectsSortMode.None))
            Destroy(u.gameObject);

        // 2) Reset every tile so its free again
        var grid = GridManager.Instance.GetGrid();
        int width  = grid.GetLength(0);
        int height = grid.GetLength(1);
        for (int x = 0; x < width;  x++)
        for (int y = 0; y < height; y++)
            grid[x, y].IsOccupied = false;
    }
    
    private void LoadLevel(int levelIndex)
    {
        Debug.Log($"[GameManager] Loading level {levelIndex} of {_levels.Count}");
        _currentLevel = levelIndex;
        ClearAllUnits();
        SpawnManager.Instance.SpawnAIUnits(_levels[_currentLevel]);
        UIManager.Instance.ShowUI();
        hasStarted = false;
    }
    public void Run()
    {
        hasStarted = true;
        UIManager.Instance.HideUI();
        Unit[] allUnits = Object.FindObjectsByType<Unit>(FindObjectsSortMode.None);
        foreach (Unit unit in allUnits)
        {
            if (unit.gameObject.CompareTag("Enemy"))
            {
                unit.Side = Faction.Enemy;
            }
            else
            {
                unit.Side = Faction.Player;
            }
        }
    }
    public void NextLevel()
    {
        if (_currentLevel < _levels.Count - 1)
        {
            LoadLevel(_currentLevel + 1);
        }
    }

    public void PrevLevel()
    {
        if (_currentLevel > 0)
        {
            LoadLevel(_currentLevel - 1);
        }
    }
    

    public bool HasStarted()
    {
        return hasStarted;
    }
}