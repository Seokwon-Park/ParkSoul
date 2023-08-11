using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Data;

namespace PS
{
    public class PlayerUIManager : MonoBehaviour
    {
        public static PlayerUIManager instance;

        [Header("NETWORK JOIN")]
        [SerializeField] bool startGameAsClient;

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if(startGameAsClient)
            {
                startGameAsClient= false;
                // 클라이언트로 시작하려면 호스트로서의 네트워크를 종료
                NetworkManager.Singleton.Shutdown();
                // 클라이언트로 재시작
                NetworkManager.Singleton.StartClient();
            }

        }
    }
}
