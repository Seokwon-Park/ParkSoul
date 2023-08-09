using UnityEngine;
using Unity.Netcode;

namespace PS
{
    public class TitleScreenManager : MonoBehaviour
    {
        public void StartNetworkAsHost()
        {
            NetworkManager.Singleton.StartHost();
        }
    }
}
