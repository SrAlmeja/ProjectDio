using UnityEngine;

namespace com.LazyGames.Dio
{
    public class StartGameListener : MonoBehaviour
    {
        [SerializeField] private BoolEventChannelSO _playerReady;

        private void OnEnable()
        {
            _playerReady.BoolEvent += InitialzeGame;
        ;}

        private void OnDisable()
        {
            _playerReady.BoolEvent -= InitialzeGame;
        }

        void InitialzeGame(bool b)
        {
            Debug.Log("EL juego inicio: " + b);
        }
    }
}

