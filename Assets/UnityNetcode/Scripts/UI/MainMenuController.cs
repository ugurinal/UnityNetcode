// Copyright © 2023 no-pact

using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;
using UnityNetcode.Commons.Init;
using UnityNetcode.Gameplay.Player;
using UnityNetcode.Utilities;

namespace UnityNetcode.UI
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private GameObject initialPanel;
        [SerializeField] private GameObject hostPanel;
        [SerializeField] private GameObject joinPanel;

        [Header("Host Panel References")]
        [SerializeField] private TMP_InputField lobbyName;
        [SerializeField] private TMP_Dropdown maxPlayerDropdown;
        [SerializeField] private Toggle isPrivate;

        [Header("Join Panel References")]
        [SerializeField] private TMP_InputField lobbyCodeField;

        private ActivePanel activePanel;

        public async void CreateLobbyButtonClick()
        {
            var playerData = new LobbyPlayerData(AuthenticationService.Instance.PlayerId, "HOST");
            var player = new Player(playerData.ID, null, DataUtilities.SerializePlayerData(playerData.Serialize()));

            var lobbyOptions = new CreateLobbyOptions
            {
                IsPrivate = isPrivate.isOn,
                Player = player
            };

            var isSuccess = await GameSystems.LobbySystem.CreateLobby(lobbyName.text, maxPlayerDropdown.value + 1, lobbyOptions);

            if (!isSuccess)
            {
                return;
            }

            GameSystems.SceneSystem.LoadLobbyScene();
        }

        public async void JoinLobbyByCodeClick()
        {
            var playerData = new LobbyPlayerData(AuthenticationService.Instance.PlayerId, "CLIENT");
            var player = new Player(playerData.ID, null, DataUtilities.SerializePlayerData(playerData.Serialize()));

            var lobbyOptions = new JoinLobbyByCodeOptions()
            {
                Player = player
            };


            var isSuccess = await GameSystems.LobbySystem.JoinLobbyByCode(lobbyCodeField.text, lobbyOptions);

            if (!isSuccess)
            {
                return;
            }

            GameSystems.SceneSystem.LoadLobbyScene();
        }

        #region UNITY_METHODS
        private void Awake()
        {
            activePanel = ActivePanel.Initial;

            initialPanel.SetActive(true);
            hostPanel.SetActive(false);
            joinPanel.SetActive(false);
        }


        public void BackButtonClick()
        {
            activePanel = ActivePanel.Initial;
            OnPanelStateChanged();
        }

        public void HostButtonClick()
        {
            activePanel = ActivePanel.Host;
            OnPanelStateChanged();
        }

        public void JoinButtonClick()
        {
            activePanel = ActivePanel.Join;
            OnPanelStateChanged();
        }

        private void OnPanelStateChanged()
        {
            switch (activePanel)
            {
                case ActivePanel.Initial:
                    initialPanel.SetActive(true);
                    hostPanel.SetActive(false);
                    joinPanel.SetActive(false);

                    break;
                case ActivePanel.Host:
                    hostPanel.SetActive(true);
                    initialPanel.SetActive(false);
                    joinPanel.SetActive(false);

                    return;

                case ActivePanel.Join:
                    joinPanel.SetActive(true);
                    initialPanel.SetActive(false);
                    hostPanel.SetActive(false);

                    return;
            }
        }

        private enum ActivePanel
        {
            Initial,
            Host,
            Join
        }
        #endregion
    }
}