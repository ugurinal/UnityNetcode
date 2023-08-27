// Copyright © 2023 no-pact

using System;
using UnityEngine;
using UnityNetcode.Commons.Init;
using UnityNetcode.Commons.Systems.LobbySystems;

namespace UnityNetcode.Gameplay.LobbyScene
{
    public class LobbyPlayerSpawner : MonoBehaviour
    {
        private void OnEnable()
        {
            LobbyEvents.OnLobbyUpdated += OnLobbyUpdated;
        }

        private void OnDisable()
        {
            LobbyEvents.OnLobbyUpdated -= OnLobbyUpdated;
        }

        private void Start()
        {
            Debug.Log(GameSystems.LobbySystem.LobbyPlayerData.Count);
        }

        private void OnLobbyUpdated()
        {
            Debug.Log(GameSystems.LobbySystem.LobbyPlayerData.Count);
        }
    }
}