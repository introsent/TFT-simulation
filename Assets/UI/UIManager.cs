using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
    
        [SerializeField] private UIDocument _uiDocument;
    
        private void OnEnable()
        {
            var root = _uiDocument.rootVisualElement;
            root.RegisterCallback<ClickEvent>(OnButtonClicked);
        }
    
        private void OnButtonClicked(ClickEvent evt)
        {
            if (evt.target is Button button)
            {
                switch (button.name)
                {
                    case "TankButton":
                        SpawnManager.Instance.SetLastClickedUnit(UnitType.Tank);
                        break;
                    case "SniperButton":
                        SpawnManager.Instance.SetLastClickedUnit(UnitType.Sniper);
                        break;
                    case "MeleeButton":
                        SpawnManager.Instance.SetLastClickedUnit(UnitType.Melee);
                        break;
                }
            }
        }
    }
}
