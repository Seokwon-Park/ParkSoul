using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PS
{
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager instance;

        // 단계별 목표
        // 1. 조이스틱 값을 읽을 방법
        // 2. 해당 값에 기반한 캐릭터 움직임

        PlayerControls playerControls;

        [SerializeField] Vector2 movementInput;

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(instance);
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);

            SceneManager.activeSceneChanged += OnSceneChange;

            instance.enabled = false;


        }

        private void OnSceneChange(Scene oldScene, Scene newScene)
        {
            if(newScene.buildIndex == WorldSaveGameManager.instance.GetWorldSceneIndex())
            {
                instance.enabled = true;
            }
            else
            {
                instance.enabled = false;
            }
        }

        private void OnEnable()
        {
            if(playerControls == null)
            {
                playerControls= new PlayerControls();

                playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            }

            playerControls.Enable();
        }

        private void OnDestroy()
        {
            SceneManager.activeSceneChanged -= OnSceneChange;
        }
    }
}