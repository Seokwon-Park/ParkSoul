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
                // Ŭ���̾�Ʈ�� �����Ϸ��� ȣ��Ʈ�μ��� ��Ʈ��ũ�� ����
                NetworkManager.Singleton.Shutdown();
                // Ŭ���̾�Ʈ�� �����
                NetworkManager.Singleton.StartClient();
            }

        }
    }
}
