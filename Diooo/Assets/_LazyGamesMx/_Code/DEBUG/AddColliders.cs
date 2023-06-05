using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[InitializeOnLoad]
#endif
public class AddColliders : MonoBehaviour
{
#if UNITY_EDITOR
    static AddColliders()
    {
        EditorApplication.hierarchyChanged += OnHierarchyChanged;
    }

    private static void OnHierarchyChanged()
    {
        AddColliders[] adderComponents = GameObject.FindObjectsOfType<AddColliders>();
        foreach (AddColliders adder in adderComponents)
        {
            if (!adder.HasCollidersAdded())
            {
                adder.AddCollidersRecursive(adder.transform);
                DestroyImmediate(adder);
            }
        }
    }

    private bool HasCollidersAdded()
    {
        return (GetComponents<Collider>().Length > 0);
    }
#endif

    private void AddCollidersRecursive(Transform parent)
    {
        parent.gameObject.AddComponent<BoxCollider>();

        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            AddCollidersRecursive(child);
        }
    }
}

