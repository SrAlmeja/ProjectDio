using UnityEngine;

public class MapSelection : MonoBehaviour
{
    [SerializeField] private GameObject _pivot;

    private void Start()
    {
        ChangeSelection(true);
    }

    private void ChangeSelection(bool isForward)
    {
        if (isForward) iTween.RotateAdd(_pivot, new Vector3(0, 45f, 0f), 2f);
        else iTween.RotateAdd(_pivot, new Vector3(0,-45f,0f), 2f);
    }
}
