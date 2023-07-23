// Copyright © 2023 no-pact

using Unity.Netcode.Components;
using UnityEngine;

namespace UnityNetcode.Core
{
    [DisallowMultipleComponent]
    public class ClientNetworkTransform : NetworkTransform
    {
        protected override bool OnIsServerAuthoritative()
        {
            return false;
        }
    }
}