// Copyright © 2023 no-pact

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityNetcode.Commons.Init;
using UnityNetcode.Gameplay.Player;

namespace UnityNetcode.Commons.Systems.LobbySystems
{
    public class LobbySystem
    {
        private LobbySystemParameters parameters;
        private Lobby lobby;
        private float time;

        private LobbyState lobbyState;
        private JoinState joinState;

        private CancellationTokenSource heartbeatToken;
        private CancellationTokenSource refreshToken;

        public List<LobbyPlayerData> LobbyPlayerData { get; private set; }

        public string GetLobbyCode()
        {
            return lobby?.LobbyCode;
        }

        public string GetLobbyName()
        {
            return lobby?.Name;
        }

        public static LobbySystem Create(ISystemParameters parameters)
        {
            var instance = new LobbySystem();
            instance.Initialize((LobbySystemParameters)parameters);

            return instance;
        }

        private void Initialize(LobbySystemParameters parameters)
        {
            this.parameters = parameters;
            LobbyPlayerData = new List<LobbyPlayerData>();

            lobbyState = LobbyState.Idle;
            joinState = JoinState.Idle;
        }

        public async Task<bool> CreateLobby(string lobbyName, int maxPlayers, CreateLobbyOptions options)
        {
            if (lobbyState is not LobbyState.Idle)
            {
                return false;
            }

            lobbyState = LobbyState.TryingToCreate;

            try
            {
                lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);
            }
            catch (Exception e)
            {
                lobbyState = LobbyState.Idle;
                Console.WriteLine(e);

                return false;
            }

            lobbyState = LobbyState.Created;
            Debug.Log("Lobby created.");

            RunHeartbeatPingLobbyTask();
            RunRefreshLobbyTask();


            return true;
        }

        public async Task<bool> JoinLobbyByCode(string lobbyCode, JoinLobbyByCodeOptions options)
        {
            if (joinState is not JoinState.Idle)
            {
                return false;
            }

            joinState = JoinState.TryingToJoin;

            try
            {
                lobby = await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode, options);
            }
            catch (Exception e)
            {
                joinState = JoinState.Idle;

                Console.WriteLine(e);

                return false;
            }

            joinState = JoinState.Joined;

            RunRefreshLobbyTask();

            return true;
        }

        private void RunHeartbeatPingLobbyTask()
        {
            heartbeatToken = new CancellationTokenSource();
            TryToSendHeartbeatPing(heartbeatToken);
        }

        private void RunRefreshLobbyTask()
        {
            refreshToken = new CancellationTokenSource();
            RefreshLobby(refreshToken);
        }

        private async void TryToSendHeartbeatPing(CancellationTokenSource token)
        {
            while (true)
            {
                await Task.Delay(parameters.HeartbeatRateMs);

                if (token.IsCancellationRequested)
                {
                    return;
                }

                Debug.Log("Heartbeat...");
                await LobbyService.Instance.SendHeartbeatPingAsync(lobby.Id);
            }
        }

        private async void RefreshLobby(CancellationTokenSource token)
        {
            while (true)
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }

                Debug.Log("Refreshing...");
                var newLobby = await LobbyService.Instance.GetLobbyAsync(lobby.Id);

                Debug.Log($"New lobby {newLobby.LastUpdated}");
                Debug.Log($"Old lobby {lobby.LastUpdated}");

                if (newLobby.LastUpdated > lobby.LastUpdated)
                {
                    Debug.Log("Lobby updated.");
                    lobby = newLobby;
                    UpdateLobbyPlayerData();
                    LobbyEvents.TriggerLobbyUpdated();
                }

                await Task.Delay(parameters.RefreshRateMs);
            }
        }

        private void UpdateLobbyPlayerData()
        {
            LobbyPlayerData.Clear();

            foreach (var player in lobby.Players)
            {
                var lobbyPlayerData = new LobbyPlayerData(player.Data);
                LobbyPlayerData.Add(lobbyPlayerData);
            }
        }

        public void OnApplicationQuit()
        {
            if (lobby == null)
            {
                return;
            }

            refreshToken.Cancel();

            if (lobby.HostId != AuthenticationService.Instance.PlayerId)
            {
                return;
            }

            heartbeatToken.Cancel();
            LobbyService.Instance.DeleteLobbyAsync(lobby.Id);
        }

        private enum LobbyState
        {
            Idle = 0,
            TryingToCreate = 1,
            Created = 2
        }

        private enum JoinState
        {
            Idle = 0,
            TryingToJoin = 1,
            Joined = 2
        }
    }
}