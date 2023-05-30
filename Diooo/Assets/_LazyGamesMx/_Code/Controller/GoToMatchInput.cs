using UnityEngine;
using UnityEngine.InputSystem;

namespace com.LazyGames.Dio
{
    public class GoToMatchInput : MonoBehaviour
    {
        [SerializeField] private InputActionAsset inputActions;
        InputActionMap gameplayActionMap;
        InputAction startGameInputAction;

        
        void OnGoToMatch(InputAction.CallbackContext context)
        {
            SceneController.Instance.LoadSceneNetwork(SceneKeys.GAME_SCENE);
        }

        public void PrepareInputs()
        {
            gameplayActionMap = inputActions.FindActionMap("Start");
            startGameInputAction = gameplayActionMap.FindAction("GoToMatch");
            startGameInputAction.Enable();

            startGameInputAction.performed += OnGoToMatch;
            startGameInputAction.canceled += OnGoToMatch;
        }
    }
}