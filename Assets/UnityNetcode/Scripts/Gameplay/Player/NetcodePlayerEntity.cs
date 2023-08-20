using Unity.Netcode;
using UnityEngine;

namespace UnityNetcode.Gameplay.Player
{
    public class NetcodePlayerEntity : NetworkBehaviour
    {
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float rotationSpeed = 10f;
        [SerializeField] private GameObject vCam;
        [SerializeField] private CharacterController controller;

        private float accumulatedRotation;
        private Transform xform;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            xform = GetComponent<Transform>();

            if (!IsOwner)
            {
                vCam.SetActive(false);
            }
        }

        private void Update()
        {
            if (IsServer)
            {
                // UpdateServer();
            }

            if (IsClient && IsOwner)
            {
                UpdateClient();
            }
        }

        private void UpdateClient()
        {
            var moveDir = GetMoveInput();
            var mouseDelta = GetMouseDelta();

            MoveServerRPC(moveDir);
            RotateServerRPC(mouseDelta);
        }

        private void Move(Vector2 input)
        {
            var calcMove = input.x * xform.right + input.y * xform.forward;
            var move = calcMove * (moveSpeed * Time.deltaTime);

            controller.Move(move);
        }

        private void Rotate(Vector2 input)
        {
            if (input == Vector2.zero)
            {
                return;
            }

            xform.Rotate(new Vector3(0f, input.x * rotationSpeed, 0f));
        }

        [ServerRpc]
        private void MoveServerRPC(Vector3 input)
        {
            Move(input);
        }

        [ServerRpc]
        private void RotateServerRPC(Vector2 input)
        {
            Rotate(input);
        }

        private Vector2 GetMoveInput()
        {
            var input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;

            return input;
        }

        private Vector2 GetMouseDelta()
        {
            if (!Input.GetMouseButton(1))
            {
                return Vector2.zero;
            }

            var input = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

            return input;
        }
    }
}