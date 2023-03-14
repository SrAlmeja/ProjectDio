using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MapManager : MonoBehaviour
{
    public GameObject[] CenterPieces = new GameObject[6];
    public GameObject[] MiddlePieces = new GameObject[6];
    public GameObject[] CornerPieces = new GameObject[6];

    public GameObject GenerateCenterPiece()
    {
        return CenterPieces[Random.Range(0, 6)];
    }

    public GameObject GenerateMiddlePiece()
    {
        return MiddlePieces[Random.Range(0, 6)];
    }

    public GameObject GenerateCornerPiece()
    {
        return CornerPieces[Random.Range(0, 6)];
    }
}
