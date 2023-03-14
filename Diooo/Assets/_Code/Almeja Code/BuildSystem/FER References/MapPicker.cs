using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPicker : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private MapManager _mapManager;

    private GameObject _mapPiece;
    private int _rotation;

    private void Awake()
    {
        SelectMapPiece();
        SelectMapOrientation();
        InstantiateMap();
    }

    public enum SelectedPiece
    {
        Center,
        Middle,
        Corner
    }
    public SelectedPiece selectedPiece;

    public enum SelectedOrientation
    {
        Down,
        Left,
        Up,
        Right,
        Random
    }
    public SelectedOrientation selectedOrientation;

    public void SelectMapPiece()
    {
        switch (selectedPiece)
        {
            case SelectedPiece.Center:
                _mapPiece = _mapManager.GenerateCenterPiece();
                break;

            case SelectedPiece.Middle:
                _mapPiece = _mapManager.GenerateMiddlePiece();
                break;

            case SelectedPiece.Corner:
                _mapPiece = _mapManager.GenerateCornerPiece();
                break;
        }
    }

    public void SelectMapOrientation()
    {
        switch (selectedOrientation)
        {
            case SelectedOrientation.Down:
                _rotation = 0;
                break;

            case SelectedOrientation.Left:
                _rotation = 90;
                break;

            case SelectedOrientation.Up:
                _rotation = 180;
                break;

            case SelectedOrientation.Right:
                _rotation = 270;
                break;

            case SelectedOrientation.Random:
                _rotation = 90 * Random.Range(0,4);
                break;
        }
    }

    public void InstantiateMap()
    {
        Instantiate(_mapPiece, this.gameObject.transform.position, Quaternion.Euler(0,_rotation,0));
    }
}
