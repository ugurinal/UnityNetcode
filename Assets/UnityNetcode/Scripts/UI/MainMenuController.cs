// Copyright © 2023 no-pact

using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;
using UnityNetcode.Commons.Init;

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
        [SerializeField] private TMP_InputField ipAddress;

        private ActivePanel activePanel;

        private void Awake()
        {
            activePanel = ActivePanel.Initial;

            initialPanel.SetActive(true);
            hostPanel.SetActive(false);
            joinPanel.SetActive(false);
        }

        public async void CreateLobbyButtonClick()
        {
            var lobbyOptions = new CreateLobbyOptions
            {
                IsPrivate = isPrivate.isOn,
                Player = new Player(AuthenticationService.Instance.PlayerId)
            };

            var isSuccess = await GameSystems.LobbySystem.CreateLobby(lobbyName.text, maxPlayerDropdown.value + 1, lobbyOptions);

            if (!isSuccess)
            {
                return;
            }

            GameSystems.SceneSystem.LoadLobbyScene();
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

            // var created =GameSystems.LobbySystem.CreateLobby();
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
    }
}