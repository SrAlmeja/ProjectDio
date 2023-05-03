//Dino 03/05/2023 Change script to a scriptable object to manage inputs players
using UnityEngine;
using UnityEngine.Serialization;

namespace com.LazyGames.Dio
{
    [CreateAssetMenu(menuName = "ScriptableObject/Events/Enable Inputs Player")]
    public class EnableInputsPlayer : ScriptableObject
    {
        
        [SerializeField] private BoolEventChannelSO _playerInputs;

        private void OnEnable()
        {
            _playerInputs.BoolEvent += HandleInputsPlayer;
        ;}

        private void OnDisable()
        {
            _playerInputs.BoolEvent -= HandleInputsPlayer;
        }

        void HandleInputsPlayer(bool active)
        {
            Debug.Log("EL juego inicio: " + active);
        }
    }
}

