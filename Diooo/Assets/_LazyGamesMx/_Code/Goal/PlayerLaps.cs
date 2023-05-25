using com.LazyGames.Dio;
using UnityEngine;

public class PlayerLaps : MonoBehaviour
{
    [SerializeField] private VoidEventChannelSO _actualLap;
    [SerializeField] private BoolEventChannelSO _isReverse;
    private bool _reverse = false;
    public int _currentLap = 0;

    private void OnEnable()
    {
        _isReverse.BoolEvent += UpdateBool;
        _actualLap.VoidEvent += LapsEvent;
    }

    private void OnDisable()
    {
        _isReverse.BoolEvent -= UpdateBool;
        _actualLap.VoidEvent -= LapsEvent;
    }

    void Start()
    {
        _currentLap = 0;
    }

    void LapsEvent()
    {
        if(!_reverse)
        {
            _currentLap++;
            if (_currentLap >= 3)
            {
                Debug.Log("push to db");
            }
        }
    }

    void UpdateBool(bool b) 
    { 
        _reverse = b;
        Debug.Log("car in reverse");
    }
}
