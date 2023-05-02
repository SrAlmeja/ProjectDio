using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using Color = UnityEngine.Color;

public class BuildPreviewSystem : MonoBehaviour
{
    [SerializeField] private float previewYOffset = 0.06f;

    [SerializeField]
    private GameObject cellIndicator;
    private GameObject _previewObject;

    [SerializeField]
    private Material previewMaterialsPrefab;
    private Material previewMaterialsInstance;

    private Renderer cellIndicatorRender;

    private void Start()
    {
        previewMaterialsInstance = new Material(previewMaterialsPrefab);
        cellIndicator.SetActive(false);
    }

    public void startShowingPlacementPreview(GameObject prefab, Vector2Int size)
    {
        _previewObject = Instantiate(prefab);
        preparePreview(_previewObject);
        prepareCursor(size);
        cellIndicator.SetActive(true);
    }

    private void prepareCursor(Vector2Int size)
    {
        if (size.x > 0 || size.y > 0)
        {
            cellIndicator.transform.localScale = new Vector3(size.x, 1, size.y);
            cellIndicator.GetComponentInChildren<Renderer>().material = previewMaterialsInstance;
        }
    }
 
    private void preparePreview(GameObject previewObject)
    {
        Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            Material[] materials = renderer.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = previewMaterialsInstance;
            }

            renderer.materials = materials;
        }
    }
    public void StopShowingPreview()
    {
        cellIndicator.SetActive(false);
        Destroy(_previewObject);
    }

    public void UpdatePosition(Vector3 position, bool validity)
    {
        MovePreview(position);
        MoveCursor(position);
        ApplyFeedback(validity);
    }

    private void ApplyFeedback(bool validity)
    {
        Color c = validity ? Color.white : Color.red;
        cellIndicatorRender.material.color = c;
        c.a = 0.5f;
        previewMaterialsInstance.color = c;
    }

    private void MoveCursor(Vector3 position)
    {
        cellIndicator.transform.position = position;
    }

    private void MovePreview(Vector3 position)
    {
        _previewObject.transform.position = new Vector3(
            position.x, position.y + previewYOffset, position.z);
    }
}
