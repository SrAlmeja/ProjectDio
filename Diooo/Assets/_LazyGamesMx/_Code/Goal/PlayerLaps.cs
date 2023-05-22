using com.LazyGames.Dio;
using UnityEngine;

public class PlayerLaps : MonoBehaviour
{
    [SerializeField] private VoidEventChannelSO _actualLap;
    [SerializeField] private BoolEventChannelSO _isReverse;
    private bool _reverse;
    private int _currentLap;

    private void OnEnable()
    {
        _isReverse.BoolEvent += UpdateBool;
    }

    private void OnDisable()
    {
        _isReverse.BoolEvent -= UpdateBool;
    }

    void Start()
    {
        _currentLap = 0;
    }

    void LapsEvent(int i)
    {
        if(!_isReverse)
        {
            _currentLap++;
            if (_currentLap >= 3)
            {
                Debug.Log("push to db");
            }
        }
        if (_isReverse)
        {

        }
    }

    void UpdateBool(bool b) 
    { 
        _reverse = b;
    }
}
