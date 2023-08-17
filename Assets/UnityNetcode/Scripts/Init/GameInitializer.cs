using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityNetcode.Init
{
    public class GameInitializer : MonoBehaviour
    {
        private async void Start()
        {
            await UnityServices.InitializeAsync();

            if (UnityServices.State != ServicesInitializationState.Initialized)
            {
                return;
            }

            await AuthenticationService.Instance.SignInAnonymouslyAsync();

            if (AuthenticationService.Instance.IsSignedIn)
            {
                Debug.Log("Signed in.");
                Debug.Log($"Player {AuthenticationService.Instance.PlayerName}");

                SceneManager.LoadSceneAsync("Main", LoadSceneMode.Additive);
            }
            else
            {
                Debug.Log($"Sign in failed");
            }
        }
    }
}