using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool IsOccupied;
    public bool IsBlueTeamTile { get; private set; } // Player's side
    private Material _originalMaterial;
    
    [Header("Visuals")]
    [SerializeField] private Material _blueTileMaterial;
    [SerializeField] private Material _redTileMaterial;
    [SerializeField] private Material _hoverMaterial;

    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _originalMaterial = _meshRenderer.material;
    }

    public void Initialize(bool isBlueTeam)
    {
        IsBlueTeamTile = isBlueTeam;
        _originalMaterial = isBlueTeam ? _blueTileMaterial : 
            _redTileMaterial;
        _meshRenderer.material = _originalMaterial;
    }

    private void OnMouseEnter()
    {
        if(!IsOccupied && IsBlueTeamTile) 
            _meshRenderer.material = _hoverMaterial;
    }

    private void OnMouseExit()
    {
        _meshRenderer.material = _originalMaterial;
    }
}
