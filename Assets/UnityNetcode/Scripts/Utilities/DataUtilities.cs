// Copyright © 2023 no-pact

using System.Collections.Generic;
using Unity.Services.Lobbies.Models;

namespace UnityNetcode.Utilities
{
    public static class DataUtilities
    {
        public static Dictionary<string, PlayerDataObject> SerializePlayerData(Dictionary<string, string> data)
        {
            var playerData = new Dictionary<string, PlayerDataObject>();

            foreach (var (key, value) in data)
            {
                playerData.Add(key, new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, value));
            }

            return playerData;
        }
    }
}