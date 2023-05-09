using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Recording/Recorded Object Data")]
public class RecordData : ScriptableObject
{
    public List<Record> records = new List<Record>();

    public void AddRecord(Vector3 position, Quaternion rotation, float time)
    {
        Record record = new Record();
        record.position = position;
        record.rotation = rotation;
        record.time = time;
        records.Add(record);
    }

    [System.Serializable]
    public class Record
    {
        public Vector3 position;
        public Quaternion rotation;
        public float time;
    }
}
