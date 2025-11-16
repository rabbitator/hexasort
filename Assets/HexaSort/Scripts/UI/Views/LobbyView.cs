using UnityEngine;
using UnityEngine.UI;

namespace HexaSort.UI.Views
{
    public class LobbyView : MonoBehaviour
    {
        [SerializeField]
        private Button _playButton;

        [SerializeField]
        private RectTransform _logoRoot;

        public Button PlayButton => _playButton;
        public RectTransform PlayButtonRoot => _playButton.transform as RectTransform;
        public RectTransform LogoRoot => _logoRoot;
    }
}