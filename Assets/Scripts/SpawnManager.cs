using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;

public enum UnitType
{
    Tank,
    Melee,
    Sniper
}

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
}
