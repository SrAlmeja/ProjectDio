using System.Collections.Generic;
using UnityEngine;

namespace com.LazyGames.Dio
{
    [CreateAssetMenu(menuName = "ScriptableObject/Recording/Recorded Object Data")]
    public class RecordData : ScriptableObject
    {
        public List<Record> Records = new List<Record>();
        public float StartTime = 0f;

        public void AddRecord(Vector3 position, Quaternion rotation, float time)
        {
            Record record = new Record();
            record.position = position;
            record.rotation = rotation;
            record.time = time;
            Records.Add(record);
        }

        public void ClearData()
        {
            if (Records.Count > 0) Records.Clear();
        }

        [System.Serializable]
        public class Record
        {
            public Vector3 position;
            public Quaternion rotation;
            public float time;
        }
    }
}

