using UnityEngine;
using UnityEngine.UI;

namespace HexaSort.UI.Views
{
    public class GameplayView : MonoBehaviour
    {
        [SerializeField]
        private Button _settingsButton;

        [SerializeField]
        private Button _menuButton;

        public Button SettingsButton => _settingsButton;
        public Button MenuButton => _menuButton;

        public RectTransform SettingsButtonRoot => _settingsButton.transform as RectTransform;
        public RectTransform MenuButtonRoot => _menuButton.transform as RectTransform;
    }
}