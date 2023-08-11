
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PS
{
    public class PlayerManager : CharacterManager
    {
        [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
        [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
        protected override void Awake()
        {
            base.Awake();

            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            playerAnimatorManager= GetComponent<PlayerAnimatorManager>();
        }

        protected override void Update()
        {
            base.Update();

            // if we do not own this gameobj, we dont control or edit it
            if (!IsOwner)
                return;

            // 이동 처리
            playerLocomotionManager.HandleAllMovement();

        }

        protected override void LateUpdate()
        {
            if (!IsOwner)
                return;

            base.LateUpdate();

            PlayerCamera.instance.HandleAllCameraActions();
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            // if this is player object owned by this client
            if (IsOwner)
            {
                PlayerCamera.instance.player = this;
                PlayerInputManager.instance.player = this;
            }
        }
    }
}