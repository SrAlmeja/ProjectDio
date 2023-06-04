using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[InitializeOnLoad]
#endif
public class RemoveColliders : MonoBehaviour
{
#if UNITY_EDITOR
    static RemoveColliders()
    {
        EditorApplication.hierarchyChanged += OnHierarchyChanged;
    }

    private static void OnHierarchyChanged()
    {
        RemoveColliders[] removerComponents = GameObject.FindObjectsOfType<RemoveColliders>();
        foreach (RemoveColliders remover in removerComponents)
        {
            if (!remover.HasCollidersRemoved())
            {
                remover.RemoveCollidersRecursive(remover.transform);
                DestroyImmediate(remover);
            }
        }
    }

    private bool HasCollidersRemoved()
    {
        return (GetComponents<Collider>().Length == 0);
    }
#endif

    private void RemoveCollidersRecursive(Transform parent)
    {
        foreach (Collider collider in parent.GetComponents<Collider>())
        {
#if UNITY_EDITOR
            DestroyImmediate(collider, true);
#else
            Destroy(collider.gameObject);
#endif
        }

        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            RemoveCollidersRecursive(child);
        }
    }
}
