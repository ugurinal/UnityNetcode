using System;
using Unity.Netcode;
using UnityEngine;

namespace UnityNetcode.Utilities
{
    public class NetworkModeSelector : MonoBehaviour
    {
        private bool styleInitialized;

        private GUIStyle buttonStyle;
        private GUIStyle labelStyle;

        private void OnGUI()
        {
            InitStyles();

            GUILayout.BeginArea(new Rect(20, 20, 300, 300));


            if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
            {
                StartButtons();
            }
            else
            {
                StatusLabels();
            }

            GUILayout.EndArea();
        }

        private void InitStyles()
        {
            if (styleInitialized)
            {
                return;
            }

            buttonStyle = new GUIStyle(GUI.skin.button)
            {
                fixedHeight = 35,
                fontSize = 20,
                alignment = TextAnchor.MiddleCenter
            };

            labelStyle = new GUIStyle(GUI.skin.label)
            {
                fixedHeight = 35,
                fontSize = 20,
                alignment = TextAnchor.MiddleCenter
            };

            styleInitialized = true;
        }

        private void StartButtons()
        {
            if (GUILayout.Button("Host", buttonStyle))
            {
                NetworkManager.Singleton.StartHost();
            }

            if (GUILayout.Button("Client", buttonStyle))
            {
                NetworkManager.Singleton.StartClient();
            }

            if (GUILayout.Button("Server", buttonStyle))
            {
                NetworkManager.Singleton.StartServer();
            }
        }

        private void StatusLabels()
        {
            var mode = NetworkManager.Singleton.IsHost ? "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";

            GUILayout.Label($"Transport: {NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name}", labelStyle);
            GUILayout.Label($"Mode: {mode}", labelStyle);
        }
    }
}