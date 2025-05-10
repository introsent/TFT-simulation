using UnityEngine;

public class TileInputHandler : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if(Physics.Raycast(ray, out RaycastHit hit))
            {
                if(hit.collider.CompareTag("Tile"))
                {
                    SpawnManager.Instance.TrySpawnUnit(hit.point);
                }
            }
        }
    }
}
