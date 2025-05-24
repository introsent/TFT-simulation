using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }
        [SerializeField] private UIDocument _uiDocument;
        
        private void Start()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            Instance = this;
        }
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
                    case "RunButton":
                        GameManager.Instance.Run();
                        break;
                    case "NextButton":
                        GameManager.Instance.NextLevel();
                        break;
                    case "PreviousButton":
                        GameManager.Instance.PrevLevel();
                        break;
                }
            }
        }

        public void HideUI()
        {
            _uiDocument.rootVisualElement.style.display = DisplayStyle.None;
        }

        public void ShowUI()
        {
            _uiDocument.rootVisualElement.style.display = DisplayStyle.Flex;
        }
    }
}
