// Copyright © 2023 no-pact

using UnityEngine;
using UnityNetcode.Commons.Init;

namespace UnityNetcode.Commons.Systems.LobbySystems
{
    [CreateAssetMenu]
    public class LobbySystemParameters : ScriptableObject, ISystemParameters
    {
        [SerializeField] private int heartbeatRateMs = 6000;
        [SerializeField] private int refreshRateMs = 1000;

        public int HeartbeatRateMs => heartbeatRateMs;
        public int RefreshRateMs => refreshRateMs;
    }
}