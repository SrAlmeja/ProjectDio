using UnityEngine;
using com.LazyGames.Dio;


[CreateAssetMenu(fileName = "SceneLoader", menuName = "ScriptableObjects/SceneLoader")]
public class SceneLoader : ScriptableObject
{
    
    public void LoadScene(SceneKeySO sceneKeySo)
    {
        SceneController.Instance.LoadScene(sceneKeySo.MyKey);
    }

    public void LoadSceneNetwork(SceneKeySO sceneKeySo)
    {
        SceneController.Instance.LoadSceneNetwork(sceneKeySo.MyKey);
    }
    
}
