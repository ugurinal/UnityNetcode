// Copyright © 2023 no-pact

using UnityEngine.SceneManagement;
using UnityNetcode.Commons.Init;

namespace UnityNetcode.Commons.Systems.SceneSystems
{
    public class SceneSystem
    {
        private SceneSystemParameters parameters;

        public static SceneSystem Create(ISystemParameters systemParameters)
        {
            var instance = new SceneSystem();
            instance.Initialize(systemParameters);

            return instance;
        }

        private void Initialize(ISystemParameters parameters)
        {
            this.parameters = (SceneSystemParameters)parameters;

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SceneManager.SetActiveScene(scene);
        }

        public void LoadMainScene()
        {
            SceneManager.LoadSceneAsync(parameters.MainSceneName, LoadSceneMode.Additive);
        }

        public void LoadLobbyScene()
        {
            var task = SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(parameters.MainSceneName));
            task.completed += _ => { SceneManager.LoadSceneAsync(parameters.LobbySceneName, LoadSceneMode.Additive); };
        }
    }
}