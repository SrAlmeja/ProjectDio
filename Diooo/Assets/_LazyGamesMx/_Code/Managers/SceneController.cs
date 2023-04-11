using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;
    
    void Start()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
    }

    void Update()
    {
        
    }
    
    public void LoadScene(string sceneKey)
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneKey);
    }
}
