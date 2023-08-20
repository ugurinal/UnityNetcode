// Copyright © 2023 no-pact

using System.Collections.Generic;
using UnityNetcode.Commons.Systems.LobbySystems;
using UnityNetcode.Commons.Systems.SceneSystems;

namespace UnityNetcode.Commons.Init
{
    public class GameSystems
    {
        public static SceneSystem SceneSystem { get; private set; }
        public static LobbySystem LobbySystem { get; private set; }

        public void Initialize(Dictionary<int, ISystemParameters> gameSystemArgs)
        {
            SceneSystem = SceneSystem.Create(gameSystemArgs[GameSystemArgs.SceneSystemParam]);
            LobbySystem = LobbySystem.Create(gameSystemArgs[GameSystemArgs.LobbySystemParam]);
        }

        public void OnApplicationQuit()
        {
            LobbySystem.OnApplicationQuit();
        }
    }
}