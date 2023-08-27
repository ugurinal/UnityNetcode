// Copyright © 2023 no-pact

using System.Collections.Generic;
using Unity.Services.Lobbies.Models;

namespace UnityNetcode.Gameplay.Player
{
    public class LobbyPlayerData
    {
        private string id;
        private string username;
        private bool isReady;

        public string ID => id;
        public string Username => username;
        public bool IsReady => isReady;

        public LobbyPlayerData(string id, string username)
        {
            this.id = id;
            this.username = username;
            isReady = false;
        }

        public LobbyPlayerData(Dictionary<string, PlayerDataObject> playerData)
        {
            UpdateData(playerData);
        }

        private void UpdateData(Dictionary<string, PlayerDataObject> playerData)
        {
            if (playerData.TryGetValue("ID", out var id))
            {
                this.id = id.Value;
            }

            if (playerData.TryGetValue("USERNAME", out var username))
            {
                this.username = username.Value;
            }

            if (playerData.TryGetValue("IsReady", out var isReady))
            {
                this.isReady = isReady.Value == "True";
            }
        }

        public Dictionary<string, string> Serialize()
        {
            var data = new Dictionary<string, string>()
            {
                {"ID", id},
                {"USERNAME", username},
                {"IsReady", isReady.ToString()}
            };

            return data;
        }
    }
}