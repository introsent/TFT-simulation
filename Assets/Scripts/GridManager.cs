using UnityEngine;
using UnityEngine.Serialization;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }
    

    [Header("Grid Settings")]
    public int _width = 4;
    public int _playerHeight = 3; // For player side (blue tiles)
    public int _enemyHeight = 3;  // For enemy side (red tiles)
    public float _tileSize = 2f;
    public GameObject _tilePrefab;

    private Tile[,] _grid;

    public Tile[,] GetGrid()
    {
        return _grid;
    }
    private void Awake()
    {
        Instance = this;
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        int totalHeight = _playerHeight + _enemyHeight;
        _grid = new Tile[_width, totalHeight];

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < totalHeight; y++)
            {
                Vector3 position = new Vector3(x * _tileSize, 0, y * _tileSize);
                GameObject tileGO = Instantiate(_tilePrefab, position, Quaternion.identity, transform);
                tileGO.name = $"Tile_{x}_{y}";

                Tile tile = tileGO.GetComponent<Tile>();
                bool isBlueTeam = y < _playerHeight; // Player tiles are blue, enemy tiles are red
                tile.Initialize(isBlueTeam);
                _grid[x, y] = tile;
            }
        }
    }

    public bool TryGetTile(Vector3 worldPosition, out Tile tile)
    {
        int x = Mathf.FloorToInt(worldPosition.x / _tileSize);
        int y = Mathf.FloorToInt(worldPosition.z / _tileSize);

        if (x >= 0 && x < _width && y >= 0 && y < _grid.GetLength(1))
        {
            tile = _grid[x, y];
            return true;
        }

        tile = null;
        return false;
    }
}
