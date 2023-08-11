using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PS
{
    public class WorldSaveGameManager : MonoBehaviour
    {
        public static WorldSaveGameManager instance;

        [SerializeField] int worldSceneIndex = 1;
        private void Awake()
        {
            // 1개의 인스턴스만 존재할 수 있으므로 다른것이 존재하는 경우 파괴.
            if(instance ==null)
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

        public IEnumerator LoadNewGame()
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);

            yield return null;
        }

        public int GetWorldSceneIndex()
        {
            return worldSceneIndex;
        }
    }
}
