using UnityEditor;
using UnityEngine;

namespace com.LazyGames.Dio
{
    [CustomEditor(typeof(ObjectRecorder))]
    public class DioRecorder : Editor
    {
        public override void OnInspectorGUI()
        {
            ObjectRecorder objectRecorder = (ObjectRecorder)target;

            DrawDefaultInspector();

            // Add a custom button
            if (GUILayout.Button("Custom Button"))
            {
                Debug.Log("Custom Editor test");
            }
        }
    }
}

