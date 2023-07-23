using Unity.Netcode;
using UnityEngine;

namespace UnityNetcode.Player
{
    public class NetcodePlayerEntity : NetworkBehaviour
    {
        [SerializeField] private float moveSpeed;

        private Vector2 oldInput;

        [SerializeField] private NetworkVariable<float> xInput = new NetworkVariable<float>();
        [SerializeField] private NetworkVariable<float> zInput = new NetworkVariable<float>();

        private float oldXInput;
        private float oldZInput;

        private NetworkVariable<Vector2> moveInput = new NetworkVariable<Vector2>();

        private void Update()
        {
            if (IsServer)
            {
                UpdateServer();
            }

            if (IsClient && IsOwner)
            {
                UpdateClient();
            }
        }

        private void UpdateServer()
        {
            ApplyMoveInput();
        }

        private void UpdateClient()
        {
            GetInput();
        }

        private void GetInput()
        {
            var input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            if (input == oldInput)
            {
                return;
            }

            oldInput = input;

            UpdateInputVariableServerRpc(input);
        }

        private void ApplyMoveInput()
        {
            var moveDir = new Vector3(moveInput.Value.x, 0f, moveInput.Value.y).normalized;

            transform.Translate(moveDir * (Time.deltaTime * moveSpeed));
        }

        [ServerRpc]
        private void UpdateInputVariableServerRpc(Vector2 inputNetwork)
        {
            moveInput.Value = inputNetwork;
        }
    }
}