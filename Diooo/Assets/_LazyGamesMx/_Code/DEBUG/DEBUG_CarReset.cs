using UnityEngine;
using UnityEngine.SceneManagement;
public class DEBUG_CarReset : MonoBehaviour
{
    private Vector3 startPos;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!Input.GetKeyDown(KeyCode.R))return;
        transform.position = startPos;
    }
}
