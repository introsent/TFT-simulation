using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;


[System.Serializable]
public struct UnitPrefabMapping
{
    public UnitType type;
    public GameObject prefab;
}

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; private set; }

    // Existing variables
    public List<UnitPrefabMapping> _unitPrefabs = new List<UnitPrefabMapping>();
    private Dictionary<UnitType, GameObject> _prefabMap;
    public GameObject LastClickedUnit { get; private set; }

    private void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;

        // Existing initialization
        _prefabMap = new Dictionary<UnitType, GameObject>();
        foreach (var mapping in _unitPrefabs)
        {
            if (mapping.prefab != null)
            {
                _prefabMap[mapping.type] = mapping.prefab;
            }
        }
    }

    // Keep existing methods as instance methods
    public void SetLastClickedUnit(UnitType unitType)
    {
        LastClickedUnit = _prefabMap[unitType];
    }
    
    
    public void TrySpawnUnit(Vector3 position)
    {
        if(LastClickedUnit == null) return;

        if(Physics.Raycast(position + Vector3.up * 10f, Vector3.down, out RaycastHit hit))
        {
            if(hit.collider.TryGetComponent<Tile>(out Tile tile))
            {
                if(tile.IsBlueTeamTile && !tile.IsOccupied)
                {
                    SpawnUnit(tile.transform.position);
                    tile.IsOccupied = true;
                }
            }
        }
    }

    private void SpawnUnit(Vector3 position)
    {
        Instantiate(LastClickedUnit, position, Quaternion.identity);
    }
    
    public void SpawnAIUnits(List<GameManager.UnitPosition> unitPositions)
    {
        // Spawn AI units on specified grid positions
        Tile[,] grid = GridManager.Instance.GetGrid();
        int playerHeight = GridManager.Instance._playerHeight;

        foreach (GameManager.UnitPosition unitPos in unitPositions)
        {
            Vector2Int gridPos = unitPos.position;
            if (gridPos.x >= 0 && gridPos.x < grid.GetLength(0) && gridPos.y >= (playerHeight - 1) && gridPos.y < grid.GetLength(1))
            {
                Tile tile = grid[gridPos.x, gridPos.y];
                if (!tile.IsOccupied)
                {
                    GameObject unitPrefab = _prefabMap[unitPos.type];
                    Instantiate(unitPrefab, tile.transform.position, Quaternion.Euler(0, 180, 0));
                    tile.IsOccupied = true;
                }
            }
        }
    }
}
