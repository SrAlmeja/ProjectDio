using UnityEngine;

public class RotatorObj : MonoBehaviour
{
    public GameObject pilar;
    public float rotationSpeed = -90f; // Velocidad de rotación en grados por segundo
    private bool isRotating = false;
    private float targetRotation = 90f;

    private void Update()
    {
        if (isRotating)
        {
            float step = rotationSpeed * Time.deltaTime;
            pilar.transform.rotation = Quaternion.RotateTowards(pilar.transform.rotation, Quaternion.Euler(0f, 0f, targetRotation), step);

            // Verificar si alcanzó la rotación objetivo
            if (Quaternion.Angle(pilar.transform.rotation, Quaternion.Euler(0f, 0f, targetRotation)) < 0.1f)
            {
                isRotating = false;
                this.gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isRotating)
        {
            targetRotation = pilar.transform.rotation.eulerAngles.z + -90f;
            isRotating = true;
        }
    }
}