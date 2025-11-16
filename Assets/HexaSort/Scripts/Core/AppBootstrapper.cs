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
        public void Start()
        {
            StartAsync().Forget();
        }

        private async UniTask StartAsync()
        {
            var container = new Container();
            await RegisterServicesAsync(container);

            var app = container.Resolve<Application>();
            await app.InitializeAsync();

            app.Play();
        }

        private async UniTask RegisterServicesAsync(Container container)
        {
            // Resources
            container.RegisterFactory<IResourceProvider>(c => new AddressableResourceProvider());
            container.RegisterFactory(c => new ResourceCache(c.Resolve<IResourceProvider>()));

            // Configuration
            container.RegisterFactory(c => new ConfigurationLoader(container.Resolve<IResourceProvider>()));
            container.RegisterInstance(await container.Resolve<ConfigurationLoader>().LoadAsync());

            // Audio
            container.RegisterFactory(c => new UnityAudioPlayer(c.Resolve<GameConfiguration>()));
            container.RegisterInstance<IAudioPlayer>(container.Resolve<UnityAudioPlayer>());

            // Storage
            container.RegisterFactory<ISaveDataStorage>(c => new PlayerPrefsStorage());

            // Analytics
            container.RegisterFactory(c => new EventBatcher(c.Resolve<GameConfiguration>()));
            container.RegisterFactory(c => new AnalyticsReporter(c.Resolve<EventBatcher>()));

            // Localization
            container.RegisterFactory(c => new LocalizationProvider());

            // Game
            container.RegisterFactory(c => new HexGrid(c.Resolve<GameConfiguration>()));
            container.RegisterFactory(c => new LevelLoader(c.Resolve<IResourceProvider>()));

            // Game logic
            container.RegisterFactory(c => new HexGrid(c.Resolve<GameConfiguration>()));
            container.RegisterFactory(c => new ScoreCalculator(c.Resolve<GameConfiguration>()));

            // UI
            container.RegisterFactory(c => new UITransition(c.Resolve<Tweener>()));
            container.RegisterFactory(c => new ViewBinder());
            container.RegisterFactory(c => new ViewPresenter(
                c.Resolve<UITransition>(),
                c.Resolve<ViewBinder>(),
                c.Resolve<GameConfiguration>(),
                c.Resolve<IResourceProvider>()));

            // Game states
            container.RegisterFactory(c => new LobbyState());
            container.RegisterFactory(c => new GameplayState(c.Resolve<HexGrid>(), c.Resolve<ScoreCalculator>()));
            container.RegisterFactory(c => new PauseState());

            // State machine
            container.RegisterFactory(c =>
                new GameStateMachine(
                    c.Resolve<LobbyState>(),
                    c.Resolve<GameplayState>(),
                    c.Resolve<PauseState>()));

            // Main application
            container.RegisterFactory(c =>
                new Application(
                    c.Resolve<GameStateMachine>(),
                    c.Resolve<AnalyticsReporter>(),
                    c.Resolve<LocalizationProvider>()
                ));
        }
    }
}