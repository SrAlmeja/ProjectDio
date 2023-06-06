using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextSceneHighScore : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(BackToMenu());
    }

    IEnumerator BackToMenu()
    {
        yield return new WaitForSeconds(15);
        SceneManager.LoadScene("02_LevelSelection");
    }
}
