using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.LazyGames.Dio
{
    public class NextScene_SinglePlayer : MonoBehaviour
    {
        [SerializeField] private float delayTime = 2;
        [SerializeField] private SceneLoader sceneLoader;
        [SerializeField] SceneKeySO sceneKeySo;
        [SerializeField] private CanvasGroup canvasGroup;
 
        void Start()
        {
            DioGameManagerSingleplayer.Instance.OnGameStateChange += HandleGameStateChange;
        }

        void Update()
        {

        }
        
        
        private void LoadNextScene()
        {
            sceneLoader.LoadScene(sceneKeySo);
        }
        
        private IEnumerator DelayLoad()
        {
            yield return new WaitForSeconds(delayTime);
            LoadNextScene();
        }
        
        private void HandleGameStateChange(DioGameManagerSingleplayer.GameStatesSingleplayer state)
        {
            if (state == DioGameManagerSingleplayer.GameStatesSingleplayer.GameOver)
            {
                StartCoroutine(DelayLoad());
            }
            
        }

    }
}