using TMPro;
using UnityEngine;
using UnityNetcode.Commons.Init;

namespace UnityNetcode.UI
{
    public class LobbyUIController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI lobbyCode;
        [SerializeField] private TextMeshProUGUI lobbyName;

        private void Start()
        {
            lobbyCode.text = GameSystems.LobbySystem.GetLobbyCode();
            lobbyName.text = GameSystems.LobbySystem.GetLobbyName();
        }
    }
}