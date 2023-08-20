using System;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityNetcode.Commons.Systems.LobbySystems;
using UnityNetcode.Commons.Systems.SceneSystems;

namespace UnityNetcode.Commons.Init
{
    public class GameInitializer : MonoBehaviour
    {
        [SerializeField] private SceneSystemParameters sceneSystemParameters;
        [SerializeField] private LobbySystemParameters lobbySystemParameters;

        private GameSystems gameSystems;
        private Dictionary<int, ISystemParameters> gameSystemArgs;

        private void Awake()
        {
            SetupGameParameters();
        }

        private void OnEnable()
        {
            gameSystems = new GameSystems();
            gameSystems.Initialize(gameSystemArgs);
        }

        private void OnApplicationQuit()
        {
            gameSystems.OnApplicationQuit();
        }

        private void SetupGameParameters()
        {
            gameSystemArgs = new Dictionary<int, ISystemParameters>
            {
                {GameSystemArgs.SceneSystemParam, sceneSystemParameters},
                {GameSystemArgs.LobbySystemParam, lobbySystemParameters}
            };
        }

        private async void Start()
        {
            await UnityServices.InitializeAsync();

            if (UnityServices.State != ServicesInitializationState.Initialized)
            {
                return;
            }

            await AuthenticationService.Instance.SignInAnonymouslyAsync();

            if (AuthenticationService.Instance.IsSignedIn)
            {
                Debug.Log("Signed in.");
                Debug.Log($"Player {AuthenticationService.Instance.PlayerName}");


                GameSystems.SceneSystem.LoadMainScene();
            }
            else
            {
                Debug.Log($"Sign in failed");
            }
        }
    }
}