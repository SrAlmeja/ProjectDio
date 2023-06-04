using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[InitializeOnLoad]
#endif
public class CreateColliderBetweenObjects : MonoBehaviour
{
#if UNITY_EDITOR
    private static GameObject firstSelectedObject;
    private static GameObject secondSelectedObject;

    static CreateColliderBetweenObjects()
    {
        EditorApplication.hierarchyChanged += OnHierarchyChanged;
    }

    private static void OnHierarchyChanged()
    {
        GameObject[] selectedObjects = Selection.gameObjects;

        if (selectedObjects.Length == 2 && firstSelectedObject == null && secondSelectedObject == null)
        {
            firstSelectedObject = selectedObjects[0];
            secondSelectedObject = selectedObjects[1];
            CreateCollider();
        }
        else if (selectedObjects.Length != 2)
        {
            firstSelectedObject = null;
            secondSelectedObject = null;
        }
    }

    private static void CreateCollider()
    {
        if (firstSelectedObject != null && secondSelectedObject != null)
        {
            GameObject colliderObject = new GameObject("WorldBorderBarrier");
            colliderObject.transform.SetParent(firstSelectedObject.transform);

            Vector3 center = (firstSelectedObject.transform.position + secondSelectedObject.transform.position) / 2f;
            Vector3 direction = secondSelectedObject.transform.position - firstSelectedObject.transform.position;
            float distance = direction.magnitude;

            colliderObject.transform.position = center;

            Quaternion rotation = Quaternion.LookRotation(direction, firstSelectedObject.transform.up);
            colliderObject.transform.rotation = rotation;

            BoxCollider boxCollider = colliderObject.AddComponent<BoxCollider>();
            boxCollider.size = new Vector3(1f, distance, distance);

            UnityEditor.EditorApplication.delayCall += () =>
            {
                DestroyImmediate(colliderObject.GetComponent<CreateColliderBetweenObjects>());
            };
        }
    }

    private void OnDestroy()
    {
        UnityEditor.EditorApplication.delayCall -= () =>
        {
            DestroyImmediate(this);
        };
    }
#endif
}
