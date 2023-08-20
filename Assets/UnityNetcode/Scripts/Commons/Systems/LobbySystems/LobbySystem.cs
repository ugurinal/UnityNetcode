// Copyright © 2023 no-pact

using System;
using System.Threading;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityNetcode.Commons.Init;

namespace UnityNetcode.Commons.Systems.LobbySystems
{
    public class LobbySystem
    {
        private LobbySystemParameters parameters;
        private Lobby lobby;
        private float time;
        private State lobbyState;

        private CancellationTokenSource heartbeatToken;
        private CancellationTokenSource refreshToken;

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
            lobbyState = State.Idle;
        }

        public async Task<bool> CreateLobby(string lobbyName, int maxPlayers, CreateLobbyOptions options)
        {
            if (lobbyState is not State.Idle)
            {
                return false;
            }

            lobbyState = State.TryingToCreate;

            try
            {
                lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);
            }
            catch (Exception e)
            {
                lobbyState = State.Idle;
                Console.WriteLine(e);

                return false;
            }

            lobbyState = State.Created;
            Debug.Log("Lobby created.");

            heartbeatToken = new CancellationTokenSource();
            refreshToken = new CancellationTokenSource();

            TryToSendHeartbeatPing(heartbeatToken);
            RefreshLobby(refreshToken);

            return true;
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

                if (newLobby.LastUpdated > lobby.LastUpdated)
                {
                    lobby = newLobby;
                }

                await Task.Delay(parameters.RefreshRateMs);
            }
        }

        public void OnApplicationQuit()
        {
            if (lobby == null)
            {
                return;
            }

            if (lobby.HostId != AuthenticationService.Instance.PlayerId)
            {
                return;
            }

            heartbeatToken.Cancel();
            refreshToken.Cancel();
            LobbyService.Instance.DeleteLobbyAsync(lobby.Id);
        }

        private enum State
        {
            Idle = 0,
            TryingToCreate = 1,
            Created = 2
        }
    }
}