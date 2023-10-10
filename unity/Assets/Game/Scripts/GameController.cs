using Cubeshift;
using Frictionless;
using SimpleJSON;
using UnityEngine;

namespace Game.Scripts
{
    public class GameController : MonoBehaviour
    {
        public delegate void onGameStart();
        public onGameStart OnGameStart;
        
        public bool IsGameOver;
        public bool IsGameWon;
        public bool IsGameRunning;

        public void Awake()
        {
            ServiceFactory.RegisterSingleton(this);
        }

        public void Start()
        {
            ServiceFactory.Resolve<GameshiftService>().OnLogin += OnLogin;
            ServiceFactory.Resolve<GameshiftService>().OnAssetsLoaded += OnAssetLoaded;
        }

        private void OnAssetLoaded(AssetsResult allAssets)
        {
            ServiceFactory.Resolve<StartGameScreen>().UpdateAssets(allAssets);
        }

        private void OnLogin()
        {
            ServiceFactory.Resolve<StartGameScreen>().Open();
        }

        public void StartGame(CharacterId characterId)
        {
            IsGameRunning = true;
            ServiceFactory.Resolve<CharacterCustomizationController>().SpawnCharacter(characterId);
            ServiceFactory.Resolve<EnemySpawner>().StartSpawning();
            ServiceFactory.Resolve<EnemyManager>().SpawnStartXpBlobs();
            ServiceFactory.Resolve<MovementUi>().gameObject.SetActive(true);
            OnGameStart.Invoke();
        }

        public void GameOver(bool won)
        {
            IsGameRunning = false;
            ServiceFactory.Resolve<MovementUi>().gameObject.SetActive(false);
            ServiceFactory.Resolve<GameOverScreen>().Init(won);
            ServiceFactory.Resolve<EnemySpawner>().StopSpawning();
            IsGameOver = true;
            IsGameWon = won;
        }

        public void ResetGame()
        {
            // TODO: Reset player, kill all enemies and colletables etc 
        }
    }
}