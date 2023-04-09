//Daniel Navarrete 08/04/2023 UI listener for example

using UnityEngine;

namespace com.LazyGames.Dio
{
    public class UIListener : MonoBehaviour
    {
        [SerializeField] private VoidEventChannelSO UpUIEvent;
        [SerializeField] private VoidEventChannelSO DownUIEvent;
        [SerializeField] private VoidEventChannelSO LeftUIEvent;
        [SerializeField] private VoidEventChannelSO RightUIEvent;
        [SerializeField] private VoidEventChannelSO SelectUIEvent;
        [SerializeField] private VoidEventChannelSO BackUIEvent;

        private void OnEnable()
        {
            UpUIEvent.VoidEvent += UpUI;
            DownUIEvent.VoidEvent += DownUI;
            LeftUIEvent.VoidEvent += LeftUI;
            RightUIEvent.VoidEvent += RightUI;
            SelectUIEvent.VoidEvent += SelectUI;
            BackUIEvent.VoidEvent += BackUI;
        }

        private void OnDisable()
        {
            UpUIEvent.VoidEvent -= UpUI;
            DownUIEvent.VoidEvent -= DownUI;
            LeftUIEvent.VoidEvent -= LeftUI;
            RightUIEvent.VoidEvent -= RightUI;
            SelectUIEvent.VoidEvent -= SelectUI;
            BackUIEvent.VoidEvent -= BackUI;
        }

        void UpUI()
        {
            Debug.Log("Up");
        }

        void DownUI() 
        {
            Debug.Log("Down");
        }

        void LeftUI()
        {
            Debug.Log("Left");
        }

        void RightUI() 
        {
            Debug.Log("Right");
        }

        void SelectUI()
        {
            Debug.Log("Select");
        }

        void BackUI()
        {
            Debug.Log("Back");
        }
    }
}