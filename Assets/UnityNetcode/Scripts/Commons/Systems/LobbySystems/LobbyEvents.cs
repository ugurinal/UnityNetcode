// Copyright © 2023 no-pact

using System;

namespace UnityNetcode.Commons.Systems.LobbySystems
{
    public static class LobbyEvents
    {
        public static event Action OnLobbyUpdated;

        public static void TriggerLobbyUpdated()
        {
            OnLobbyUpdated?.Invoke();
        }
    }
}