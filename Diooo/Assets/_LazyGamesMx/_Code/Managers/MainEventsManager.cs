//Fernando Cossio 02/05/2023

using UnityEngine;

namespace com.LazyGames.Dio
{
    public class MainEventsManager : MonoBehaviour
    {
        [Header("SO Channel Dependencies")]
        [SerializeField] private VoidEventChannelSO _raceStarting;
        [SerializeField] private VoidEventChannelSO _raceStarted;
        [SerializeField] private BoolEventChannelSO _racePause;
        [SerializeField] private VoidEventChannelSO _raceEnding;
        [SerializeField] private VoidEventChannelSO _raceFinished;

        private bool _racePaused;

        public void PrepareRace()
        {
            _raceStarting.VoidEvent.Invoke();
        }

        public void StartRace()
        {
            _raceStarted.VoidEvent.Invoke();
            UnpauseRace();
        }

        public void PauseRace()
        {
            _racePause.BoolEvent.Invoke(true);
            _racePaused = true;
        }

        public void UnpauseRace()
        {
            _racePause.BoolEvent.Invoke(false);
            _racePaused = false;
        }

        public void StopRace()
        {
            _raceEnding.VoidEvent.Invoke();
        }

        public void FinishRace()
        {
            PauseRace();
            _raceFinished.VoidEvent.Invoke();
        }
    }
}
