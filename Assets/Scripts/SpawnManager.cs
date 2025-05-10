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
    public List<UnitPrefabMapping> _unitPrefabs = new List<UnitPrefabMapping>();
    private Dictionary<UnitType, GameObject> _prefabMap;

    private void Awake()
    {
        _prefabMap = new Dictionary<UnitType, GameObject>();
        foreach (var mapping in _unitPrefabs)
        {
            if (mapping.prefab != null)
            {
                _prefabMap[mapping.type] = mapping.prefab;
            }
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
