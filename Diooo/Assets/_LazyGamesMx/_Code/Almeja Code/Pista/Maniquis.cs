using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maniquis : MonoBehaviour
{
    [Header("Parametros de movimiento")]
    [SerializeField] private float speed;
    [SerializeField] private float distance;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool movingRight = true;

    [Header("Activador")]
    public bool isInRange;

    void Start()
    {
        startPosition = transform.position;
        targetPosition = startPosition + new Vector3(distance, 0f, 0f);
        isInRange = false;
    }

    void Update()
    {
        if (isInRange)
        {
            Movement();
        }
    }

    void Movement()
    {
        if (movingRight)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                movingRight = false;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, startPosition, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, startPosition) < 0.01f)
            {
                movingRight = true;
            }
        }
    }
}
