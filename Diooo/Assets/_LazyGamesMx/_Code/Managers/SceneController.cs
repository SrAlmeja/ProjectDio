// Dino 11/04/2023 Creation of the script
//Control the scenes and the network scenes
using UnityEngine;

public class SceneController : MonoBehaviour
{
    #region public variables
    
    public static SceneController Instance;
    
    
    
    #endregion

    #region private variables

    private static string _targetScene;

    #endregion
    
    
    #region unity methods
    
    void Start()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
    }

    void Update()
    {
        
    }
    #endregion

    #region public methods

    

    public void LoadScene(string sceneKey)
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneKey);
    }
    #endregion

}
