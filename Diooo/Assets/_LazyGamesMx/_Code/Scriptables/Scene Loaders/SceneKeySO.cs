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
        MENU_SCENE,
        GAME_SINGLEPLAYER,
        TUTORIAL,
        LEVEL_1,
        LEVEL_2,
        LEVEL_3,
        LEVEL_4,
        HIGHSCORE,
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
                case Keys.GAME_SINGLEPLAYER:
                    return SceneKeys.GAME_SCENE_SINGLEPLAYER;
                case Keys.TUTORIAL:
                    return SceneKeys.GAME_TUTORIAL;
                case Keys.LEVEL_1:
                    return SceneKeys.GAME_LEVEL_1;
                case Keys.LEVEL_2:
                    return SceneKeys.GAME_LEVEL_2;
                case Keys.LEVEL_3:
                    return SceneKeys.GAME_LEVEL_3;
                case Keys.LEVEL_4:
                    return SceneKeys.GAME_LEVEL_4;
                case Keys.HIGHSCORE:
                    return SceneKeys.HIGHSCORE;
                    break;
            }
            return Keys.none.ToString();
        }
    }
    
}
