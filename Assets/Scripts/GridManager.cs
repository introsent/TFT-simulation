using UnityEngine;
using UnityEngine.Serialization;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }
    
    [FormerlySerializedAs("width")] [Header("Grid Settings")]
    public int _width = 4;
    [FormerlySerializedAs("height")] public int _height = 3; // For player side (blue tiles)
    [FormerlySerializedAs("tileSize")] public float _tileSize = 2f;
    [FormerlySerializedAs("tilePrefab")] public GameObject _tilePrefab;

    private Tile[,] _grid;

    private void Awake()
    {
        Instance = this;
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        _grid = new Tile[_width, _height];
        
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Vector3 position = new Vector3(x * _tileSize, 0, y * _tileSize);
                GameObject tileGO = Instantiate(_tilePrefab, position, Quaternion.identity, transform);
                tileGO.name = $"Tile_{x}_{y}";
                
                Tile tile = tileGO.GetComponent<Tile>();
                tile.Initialize(true); // All blue tiles for player
                _grid[x, y] = tile;
            }
        }
    }

    public bool TryGetTile(Vector3 worldPosition, out Tile tile)
    {
        int x = Mathf.FloorToInt(worldPosition.x / _tileSize);
        int y = Mathf.FloorToInt(worldPosition.z / _tileSize);
        
        if(x >= 0 && x < _width && y >= 0 && y < _height)
        {
            tile = _grid[x, y];
            return true;
        }
        
        tile = null;
        return false;
    }
}
