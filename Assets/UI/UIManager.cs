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

        private void OnDisable()
        {
            var root = _uiDocument.rootVisualElement;
            root.UnregisterCallback<ClickEvent>(OnButtonClicked);
        }
    
        private void OnButtonClicked(ClickEvent evt)
        {
            // Check which button was clicked
            if (evt.target is Button button)
            {
                switch (button.name)
                {
                    case "TankButton":
                        Debug.Log("Button tank clicked");
                        break;
                    case "SniperButton":
                        Debug.Log("Button sniper clicked");
                        break;
                    case "MeleeButton":
                        Debug.Log("Button button clicked");
                        break;
                }
            }
        }
    }
}
