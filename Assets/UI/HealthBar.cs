using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image _fillImage;
    [SerializeField] private Canvas _canvas;
    private Camera _mainCamera;
    private void Start()
    {
        _mainCamera = Camera.main;
        _canvas.worldCamera = _mainCamera;
        _fillImage.fillAmount = 1;
        _canvas.enabled = false; // Start hidden
    }

    public void UpdateHealth(float current, float max)
    {
        // Force immediate UI update
        _fillImage.fillAmount = Mathf.Clamp01(current / max);
        _canvas.enabled = _fillImage.fillAmount < 1f;
        
        // Debug test
        Debug.Log($"Health Updated: {current}/{max} = {_fillImage.fillAmount}");
    }

    private void LateUpdate()
    {
        if (!_canvas.enabled) return;
        
        // Face camera while maintaining world position
        _canvas.transform.LookAt(_mainCamera.transform);
        _canvas.transform.Rotate(0, 180, 0); // Fix mirroring
    }
    public void Hide() => _canvas.enabled = false;
}