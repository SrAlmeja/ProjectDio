using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

public class CreateColliderBetweenObjects : MonoBehaviour
{
    [SerializeField]
    private GameObject[] objectsToConnect;

    [SerializeField]
    private float _height;

    private bool ignoreHeightDifference;

    private void OnEnable()
    {
        ignoreHeightDifference = EditorPrefs.GetBool("IgnoreHeightDifference", true);
    }

    private void OnDisable()
    {
        EditorPrefs.SetBool("IgnoreHeightDifference", ignoreHeightDifference);
    }

    private void CreateCollider()
    {
        int objectCount = objectsToConnect.Length;
        if (objectCount < 2)
        {
            Debug.LogWarning("Insufficient objects to connect. At least 2 objects are required.");
            return;
        }

        for (int i = 0; i < objectCount - 1; i++)
        {
            GameObject firstObject = objectsToConnect[i];
            GameObject secondObject = objectsToConnect[i + 1];

            if (firstObject != null && secondObject != null)
            {
                GameObject colliderObject = new GameObject("WorldBorderBarrier (" + i + ")");
                colliderObject.transform.SetParent(transform);

                Vector3 center = (firstObject.transform.position + secondObject.transform.position) / 2f;
                Vector3 direction = secondObject.transform.position - firstObject.transform.position;
                float distance = direction.magnitude;

                colliderObject.transform.position = center;

                Quaternion rotation;
                if (ignoreHeightDifference)
                {
                    rotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
                    rotation.eulerAngles = new Vector3(0f, rotation.eulerAngles.y, 0f);
                }
                else
                {
                    rotation = Quaternion.LookRotation(direction, transform.up);
                }
                colliderObject.transform.rotation = rotation;

                BoxCollider boxCollider = colliderObject.AddComponent<BoxCollider>();
                boxCollider.size = new Vector3(1f, _height, distance);
            }
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(CreateColliderBetweenObjects))]
    public class CreateColliderBetweenObjectsEditor : Editor
    {
        private SerializedProperty objectsToConnectProp;
        private SerializedProperty heightProp;

        private void OnEnable()
        {
            objectsToConnectProp = serializedObject.FindProperty("objectsToConnect");
            heightProp = serializedObject.FindProperty("_height");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(objectsToConnectProp, true);
            EditorGUILayout.PropertyField(heightProp);

            CreateColliderBetweenObjects script = (CreateColliderBetweenObjects)target;
            if (GUILayout.Button("Create Collider"))
            {
                script.CreateCollider();
            }

            EditorGUILayout.Space();

            script.ignoreHeightDifference = EditorGUILayout.Toggle("Ignore Height Difference", script.ignoreHeightDifference);

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}
#endif
