// Copyright © 2023 no-pact

using UnityEngine;
using UnityNetcode.Commons.Init;

namespace UnityNetcode.Commons.Systems.SceneSystems
{
    [CreateAssetMenu]
    public class SceneSystemParameters : ScriptableObject, ISystemParameters
    {
        [SerializeField] private string mainSceneName = "Main";
        [SerializeField] private string lobbySceneName = "Lobby";

        public string MainSceneName => mainSceneName;
        public string LobbySceneName => lobbySceneName;
    }
}