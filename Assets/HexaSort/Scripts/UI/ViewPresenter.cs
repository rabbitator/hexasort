using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using HexaSort.Configuration;
using HexaSort.GameResources;
using HexaSort.UI.Views;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace HexaSort.UI
{
    public class ViewPresenter
    {
        private readonly UITransition _uiTransition;
        private readonly ViewBinder _viewBinder;
        private readonly GameConfiguration _config;
        private readonly IResourceProvider _resourceProvider;

        private LobbyView _lobbyView;
        private GameplayView _gameplayView;
        private SettingsView _settingsView;

        public ViewPresenter(
            UITransition uiTransition,
            ViewBinder viewBinder,
            GameConfiguration config,
            IResourceProvider resourceProvider)
        {
            _uiTransition = uiTransition;
            _viewBinder = viewBinder;
            _config = config;
            _resourceProvider = resourceProvider;
        }

        public async UniTask InitializeAsync(CancellationToken ct)
        {
            if (ct.IsCancellationRequested) return;

            var canvas = Object.FindObjectOfType<Canvas>();

            if (!canvas) throw new Exception("Canvas wasn't found!");

            if (!GetViews(canvas.gameObject, out _lobbyView, out _gameplayView, out _settingsView))
            {
                await InstantiatePrefab(canvas);
            }

            if (ct.IsCancellationRequested) return;

            ConnectViews();

            await _uiTransition.InitializeAsync(_lobbyView, _gameplayView, _settingsView);

            if (ct.IsCancellationRequested) return;

            _uiTransition.ShowLobby(true);
        }

        private void ConnectViews()
        {
            _lobbyView.PlayButton.onClick.AddListener(HandlePlayClick);
            _gameplayView.MenuButton.onClick.AddListener(HandleMenuClick);
            _gameplayView.SettingsButton.onClick.AddListener(HandleSettingsClick);
        }

        private void HandlePlayClick()
        {
            if (Random.value > 0.5f)
            {
                _uiTransition.ShakePlayButton();
            }
            else
            {
                _uiTransition.ShowGameplay();
            }
        }

        private void HandleMenuClick()
        {
            _uiTransition.ShowLobby();
        }

        private void HandleSettingsClick()
        {
            Debug.Log("Settings!");
        }

        private async UniTask InstantiatePrefab(Canvas canvas)
        {
            var handle = await _resourceProvider.GetResource<GameObject>(_config.Prefabs.UI.AssetGUID);
            var uiInstance = Object.Instantiate(handle.Result, canvas.transform);
            handle.Dispose();

            if (!GetViews(uiInstance, out _lobbyView, out _gameplayView, out _settingsView))
            {
                throw new Exception("UI Prefab isn't set up correctly!");
            }
        }

        private bool GetViews(GameObject root, out LobbyView lobbyView, out GameplayView gameplayView, out SettingsView settingsView)
        {
            lobbyView = root.GetComponentInChildren<LobbyView>();
            gameplayView = root.GetComponentInChildren<GameplayView>();
            settingsView = root.GetComponentInChildren<SettingsView>();

            return lobbyView && gameplayView && settingsView;
        }
    }
}