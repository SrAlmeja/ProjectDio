using System.Collections;
using System.Collections.Generic;
using com.LazyGames.Dio;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "SceneKey", menuName = "ScriptableObjects/SceneKey")]
public class SceneKeySO : ScriptableObject
{
    private enum Keys
    {
        none,
        LOBBY_SCENE,
        GAME_SCENE,
        MENU_SCENE    
    }
    
   [SerializeField] private Keys myKey;

    public string MyKey
    {
        get
        {
            switch (myKey)
            {
                case Keys.LOBBY_SCENE:
                   return  SceneKeys.LOBBY_SCENE;
                case Keys.GAME_SCENE:
                    return SceneKeys.GAME_SCENE;
                case Keys.MENU_SCENE:
                    return SceneKeys.MAIN_MENU_SCENE;
                break;
            }
            return Keys.none.ToString();
        }
    }
    
}
