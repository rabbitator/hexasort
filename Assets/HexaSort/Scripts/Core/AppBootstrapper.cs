using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using HexaSort.Analytics;
using HexaSort.Audio;
using HexaSort.Configuration;
using HexaSort.DependencyInjection;
using HexaSort.Game.GameFlow;
using HexaSort.Game.Gameplay;
using HexaSort.Game.Level;
using HexaSort.GameResources;
using HexaSort.Localization;
using HexaSort.Storage;
using HexaSort.Tweening;
using HexaSort.UI;

namespace HexaSort.Core
{
    public class AppBootstrapper
    {
        private CancellationTokenSource _cts = new();
        private List<IInitializableAsync> _asyncInitializables = new();
        private List<IInitializable> _initializables = new();

        public void Start()
        {
            _cts.Cancel();
            _cts = new CancellationTokenSource();

            StartAsync(_cts.Token).Forget();
        }

        private async UniTask StartAsync(CancellationToken ct)
        {
            var container = new Container();
            RegisterResourceProvider(container);
            await RegisterConfig(container, ct);
            RegisterDependencies(container);
            await InitializeEverything(ct);

            var app = container.Resolve<GameApplication>();
            await app.StartAsync(ct);
        }

        private async UniTask InitializeEverything(CancellationToken ct)
        {
            _initializables.ForEach(i => i.Initialize());
            await UniTask.WhenAll(_asyncInitializables.Select(i => i.InitializeAsync(ct)));
        }

        private void RegisterResourceProvider(Container container)
        {
            // Resources
            container.RegisterFactory<IResourceProvider>(c => new AddressableResourceProvider());
            container.RegisterFactory(c => new ResourceCache(c.Resolve<IResourceProvider>()));
        }

        private async UniTask RegisterConfig(Container container, CancellationToken ct)
        {
            // Configuration
            container.RegisterFactory(c => new ConfigurationLoader(container.Resolve<IResourceProvider>()));
            var configurationLoader = container.Resolve<ConfigurationLoader>();
            await configurationLoader.InitializeAsync(ct);
            container.RegisterInstance(configurationLoader.Config);
        }

        private void RegisterDependencies(Container container)
        {
            // Audio
            RegisterDependency(container, c => new UnityAudioPlayer(c.Resolve<GameConfiguration>()));
            container.RegisterInstance<IAudioPlayer>(container.Resolve<UnityAudioPlayer>());

            // Storage
            RegisterDependency(container, c => new PlayerPrefsStorage());
            container.RegisterInstance<ISaveDataStorage>(container.Resolve<PlayerPrefsStorage>());

            // Analytics
            RegisterDependency(container, c => new EventBatcher(c.Resolve<GameConfiguration>()));
            RegisterDependency(container, c => new AnalyticsReporter(c.Resolve<EventBatcher>()));

            // Localization
            RegisterDependency(container, c => new LocalizationProvider());

            // Animations
            RegisterDependency(container, c => new Tweener(c.Resolve<GameConfiguration>()));

            // Game
            RegisterDependency(container, c => new HexGrid(c.Resolve<GameConfiguration>()));
            RegisterDependency(container, c => new LevelLoader(c.Resolve<GameConfiguration>(), c.Resolve<IResourceProvider>()));
            RegisterDependency(container, c => new ScoreCalculator(c.Resolve<GameConfiguration>()));

            // UI
            RegisterDependency(container, c => new UITransition(c.Resolve<Tweener>()));
            RegisterDependency(container, c => new ViewBinder());
            RegisterDependency(container, c => new ViewPresenter(
                c.Resolve<UITransition>(),
                c.Resolve<ViewBinder>(),
                c.Resolve<GameConfiguration>(),
                c.Resolve<IResourceProvider>()));

            // UI state machine
            RegisterDependency(container, c => new UIStateMachine());

            // Game states
            RegisterDependency(container, c => new LobbyState(c.Resolve<GameConfiguration>(), c.Resolve<LevelLoader>()));
            RegisterDependency(container, c => new GameplayState(
                c.Resolve<GameConfiguration>(),
                c.Resolve<LevelLoader>(),
                c.Resolve<ISaveDataStorage>(),
                c.Resolve<ScoreCalculator>()));
            RegisterDependency(container, c => new PauseState());

            // Game state machine
            RegisterDependency(container, c => new GameStateMachine(c.Resolve<LobbyState>(), c.Resolve<GameplayState>(), c.Resolve<PauseState>()));

            // Main application
            RegisterDependency(container, c =>
                new GameApplication(
                    c.Resolve<GameConfiguration>(),
                    c.Resolve<GameStateMachine>(),
                    c.Resolve<ViewPresenter>()
                ));
        }

        private void RegisterDependency<T>(Container container, Func<Container, T> factory) where T : class
        {
            container.RegisterFactory(factory);
            var instance = container.Resolve<T>();

            if (instance is IInitializableAsync initializableAsync)
            {
                _asyncInitializables.Add(initializableAsync);
            }

            if (instance is IInitializable initializable)
            {
                _initializables.Add(initializable);
            }
        }
    }
}